#include <stdio.h>
#include <string.h>
#include <malloc.h>
#include <ogcsys.h>

#include "title.h"
#include "utils.h"
#include "video.h"
#include "wad.h"
#include "sha1.h"
#include "install.h"


/* 'WAD Header' structure */
typedef struct {
	/* Header length */
	u32 header_len;

	/* WAD type */
	u16 type;

	u16 padding;

	/* Data length */
	u32 certs_len;
	u32 crl_len;
	u32 tik_len;
	u32 tmd_len;
	u32 data_len;
	u32 footer_len;
} ATTRIBUTE_PACKED wadHeader;

/* Variables */
static u8 wadBuffer[BLOCK_SIZE] ATTRIBUTE_ALIGN(32);

void * startOfData;
void * endOfData;
void * internalPointer;

void mopen(void * memPointer) 
{
	u32 size = *((u32 * ) memPointer);
	startOfData = memPointer + 28;
	endOfData = startOfData + size;
	internalPointer = startOfData;
}


int mseek(u32 offset, int origin) 
{
	if (origin == SEEK_SET) 
	{
		internalPointer = startOfData + offset;
	} else if (origin == SEEK_CUR) 
	{
		internalPointer = internalPointer + offset;
	} else if (origin == SEEK_END) {
		internalPointer = endOfData - offset;
	} else 
	{
		return -2;
	}
	
	if ((internalPointer<startOfData) || (internalPointer> endOfData)) 
	{
		return -1;
	} else 
	{
		return 0;
	}
}

int mread(void * buf, int size, int count) 
{
	memcpy(buf, internalPointer, size*count);
	//DCFlushRange(buf, size*count);
	return 0;
}


//-------------------------- INSTALL FROM MEMORY -------------------------------

s32 __Wad_ReadFile(void *outbuf, u32 offset, u32 len)
{
	s32 ret;

	/* Seek to offset */
	mseek(offset, SEEK_SET);

	/* Read data */
	ret = mread(outbuf, len, 1);
	if (ret < 0)
		return ret;

	return 0;
}

s32 __Wad_ReadAlloc(void **outbuf, u32 offset, u32 len)
{
	void *buffer = NULL;
	s32   ret;

	/* Allocate memory */
	buffer = memalign(32, len);
	if (!buffer)
		return -1;

	/* Read file */
	ret = __Wad_ReadFile(buffer, offset, len);
	if (ret < 0) {
		free(buffer);
		return ret;
	}

	/* Set pointer */
	*outbuf = buffer;

	return 0;
}

s32 __Wad_GetTitleID(wadHeader *header, u64 *tid)
{
	signed_blob *p_tik    = NULL;
	tik         *tik_data = NULL;

	u32 offset = 0;
	s32 ret;

	/* Ticket offset */
	offset += round_up(header->header_len, 64);
	offset += round_up(header->certs_len,  64);
	offset += round_up(header->crl_len,    64);

	/* Read ticket */
	ret = __Wad_ReadAlloc((void *)&p_tik, offset, header->tik_len);
	if (ret < 0)
		goto out;

	/* Ticket data */
	tik_data = (tik *)SIGNATURE_PAYLOAD(p_tik);

	/* Copy title ID */
	*tid = tik_data->titleid;

out:
	/* Free memory */
	if (p_tik)
		free(p_tik);

	return ret;
}


s32 __Wad_Install()
{
	wadHeader   *header  = NULL;
	signed_blob *p_certs = NULL, *p_crl = NULL, *p_tik = NULL, *p_tmd = NULL;

	tmd *tmd_data  = NULL;

	u32 cnt, offset = 0;
	s32 ret;

	printf("\t\t>> Reading WAD data...");
	fflush(stdout);

	/* WAD header */
	ret = __Wad_ReadAlloc((void *)&header, offset, sizeof(wadHeader));
	if (ret < 0)
		goto err;
	else
		offset += round_up(header->header_len, 64);

	/* WAD certificates */
	ret = __Wad_ReadAlloc((void *)&p_certs, offset, header->certs_len);
	if (ret < 0)
		goto err;
	else
		offset += round_up(header->certs_len, 64);

	/* WAD crl */
	if (header->crl_len) {
		ret = __Wad_ReadAlloc((void *)&p_crl, offset, header->crl_len);
		if (ret < 0)
			goto err;
		else
			offset += round_up(header->crl_len, 64);
	}

	/* WAD ticket */
	ret = __Wad_ReadAlloc((void *)&p_tik, offset, header->tik_len);
	if (ret < 0)
		goto err;
	else
		offset += round_up(header->tik_len, 64);

	/* WAD TMD */
	ret = __Wad_ReadAlloc((void *)&p_tmd, offset, header->tmd_len);
	if (ret < 0)
		goto err;
	else
		offset += round_up(header->tmd_len, 64);

	Con_ClearLine();

	printf("\t\t>> Installing ticket...");
	fflush(stdout);

	/* Install ticket */
	ret = ES_AddTicket(p_tik, header->tik_len, p_certs, header->certs_len, p_crl, header->crl_len);
	if (ret < 0)
		goto err;

	Con_ClearLine();

	printf("\r\t\t>> Installing title...");
	fflush(stdout);

	/* Install title */
	ret = ES_AddTitleStart(p_tmd, header->tmd_len, p_certs, header->certs_len, p_crl, header->crl_len);
	if (ret < 0)
		goto err;

	/* Get TMD info */
	tmd_data = (tmd *)SIGNATURE_PAYLOAD(p_tmd);

	/* Install contents */
	for (cnt = 0; cnt < tmd_data->num_contents; cnt++) {
		tmd_content *content = &tmd_data->contents[cnt];

		u32 idx = 0, len;
		s32 cfd;

		Con_ClearLine();

		printf("\r\t\t>> Installing content #%02d...", content->cid);
		fflush(stdout);

		/* Encrypted content size */
		len = round_up(content->size, 64);

		/* Install content */
		cfd = ES_AddContentStart(tmd_data->title_id, content->cid);
		if (cfd < 0) {
			ret = cfd;
			goto err;
		}

		/* Install content data */
		while (idx < len) {
			u32 size;

			/* Data length */
			size = (len - idx);
			if (size > BLOCK_SIZE)
				size = BLOCK_SIZE;

			/* Read data */
			ret = __Wad_ReadFile(&wadBuffer, offset, size);
			if (ret < 0)
				goto err;

			/* Install data */
			ret = ES_AddContentData(cfd, wadBuffer, size);
			if (ret < 0)
				goto err;

			/* Increase variables */
			idx    += size;
			offset += size;
		}

		/* Finish content installation */
		ret = ES_AddContentFinish(cfd);
		if (ret < 0)
			goto err;
	}

	Con_ClearLine();

	printf("\r\t\t>> Finishing installation...");
	fflush(stdout);

	/* Finish title install */
	ret = ES_AddTitleFinish();
	if (ret >= 0) {
		printf(" OK!\n");
		goto out;
	}

err:
	printf(" ERROR! (ret = %d)\n", ret);

	/* Cancel install */
	ES_AddTitleCancel();

out:
	/* Free memory */
	if (header)
		free(header);
	if (p_certs)
		free(p_certs);
	if (p_crl)
		free(p_crl);
	if (p_tik)
		free(p_tik);
	if (p_tmd)
		free(p_tmd);

	return ret;
}


void DumpHash(u8 * hash) 
{
	int i;
	for (i=0;i<20;i++) 
	{
		printf("%x", hash[i]);
	}
}

s32 CompareHashes(u8 * hash1, u8 * hash2) 
{

	printf("\nCalculated SHA1 Hash: "); DumpHash(hash1);
	printf("\nStored SHA1 Hash    : "); DumpHash(hash2);
	sleep(3);
	
	if (memcmp(hash1, hash2, 20)==0) 
	{
		return 1;
	} 
	else
	{
		return 0;
	}
}

s32 Wad_EnsureInjectedData() 
{
	u32 size = *((u32 * ) install);	
	u8 wadOffset = 28;
	
    u8 hash1[20];
	
    SHA1(((u8 *)install)+wadOffset, size , hash1); //Taking SHA of contents...
	
	return CompareHashes(hash1, ((u8 *)install)+4);
}

s32 Wad_InstallFromMemory() 
{
	/* Check integrity of the wad file using SHA */
	/* SHA digest of the installed wad will be from XX-XX region in the injected data */
	printf("\n");
	printf("\r\t\t>> Checking integrity of the contents...");
	if (Wad_EnsureInjectedData()) 
	{
		printf("\n\n\t\t>> Wad file integrity check succeeded\n\n");	
		mopen(install);	
		return __Wad_Install();
	} else 
	{
		printf("\n\n\t\t>> Wad file integrity check failed! Will not install the wad, possible corruption during transfer...");
	}
}

u8 Wad_SelectIOS() 
{
	u8 iosOffset = 24;
	return *(install+iosOffset);
	
}