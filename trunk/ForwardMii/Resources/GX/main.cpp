/*
 *      loadMii loader v0.3
 *      main.c
 *      http://code.google.com/p/loadmii
 *
 *      Copyright 2009 The Lemon Man
 *      Thanks to luccax, Wiipower, Aurelio and crediar
 *      GX added by dimok (thanks to Tantric)
 *      usbGecko powered by Nuke
 *		little modifications by Leathl
 *
 *      This program is free software; you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation; either version 2 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program; if not, write to the Free Software
 *      Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *      MA 02110-1301, USA.
 */

#include <gccore.h>
#include <malloc.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <ogc/machine/processor.h>

#include "pngu/pngu.h"
#include "video.h"
#include "filelist.h"
#include "dolloader.h"
#include "elfloader.h"
#include "fatmounter.h" 

/*edit these*/
#define PATH1 "---path1---"
#define PATH2 "---path2---"
#define PATH3 "---path3---"
#define PATH4 "---path4---"

#define PACK2 false

#define PATH5 "---path5---"
#define PATH6 "---path6---"
#define PATH7 "---path7---"
#define PATH8 "---path8---"

#define PACK3 false

#define PATH9 "---path9---"
#define PATH10 "---path10---"
#define PATH11 "---path11---"
#define PATH12 "---path12---"

#define PACK4 false

#define PATH13 "---path13---"
#define PATH14 "---path14---"
#define PATH15 "---path15---"
#define PATH16 "---path16---"

static PNGUPROP imgProp;
static IMGCTX ctx;


u8 * GetImageData(void) {

	u8 * data = NULL;

	int ret;

	if (CONF_GetAspectRatio()) {
        ctx = PNGU_SelectImageFromBuffer(background169_png);
        if (!ctx)
            return NULL;
    } else {
        ctx = PNGU_SelectImageFromBuffer(background_png);
        if (!ctx)
            return NULL;
    }

	ret = PNGU_GetImageProperties(ctx, &imgProp);
	if (ret != PNGU_OK)
		return NULL;

    int len = imgProp.imgWidth * imgProp.imgHeight * 4;

    if(len%32) len += (32-len%32);
    data = (u8 *)memalign (32, len);
    ret = PNGU_DecodeTo4x4RGBA8 (ctx, imgProp.imgWidth, imgProp.imgHeight, data, 255);
	DCFlushRange(data, len);

	PNGU_ReleaseImageContext(ctx);

    return data;
}

void Background_Show(int x, int y, int z, u8 * data, int angle, int scaleX, int scaleY, int alpha)
{
	/* Draw image */
	Menu_DrawImg(x, y, z, imgProp.imgWidth, imgProp.imgHeight, data, angle, scaleX, scaleY, alpha);
}


int main(int argc, char **argv)
{
	u32 cookie;
	FILE *exeFile = NULL;
	void *exeBuffer          = (void *)EXECUTABLE_MEM_ADDR;
	int exeSize              = 0;
	u32 exeEntryPointAddress = 0;
	entrypoint exeEntryPoint;
	
	/* int videomod */
	InitVideo();
	/* get imagedata */
	u8 * imgdata = GetImageData();
	/* fadein of image */
	for(int i = 0; i < 255; i = i+10)
	{
		if(i>255) i = 255;
		Background_Show(0, 0, 0, imgdata, 0, 1, 1, i);
		Menu_Render();
	}
	
	/* check devices */
	SDCard_Init();
	USBDevice_Init();
	
	char cfgpath[256];
	
	/* Open dol File and check exist */
	sprintf(cfgpath, PATH1);
	exeFile = fopen (cfgpath ,"rb");
	if (exeFile==NULL)
	{
		sprintf(cfgpath, PATH2);
		exeFile = fopen (cfgpath ,"rb");
	}
	if (exeFile==NULL)
	{
		sprintf(cfgpath, PATH3);
		exeFile = fopen (cfgpath ,"rb");
	}
	if (exeFile==NULL)
	{
		sprintf(cfgpath, PATH4);
		exeFile = fopen (cfgpath ,"rb");
	}
	
	if (PACK2)
	{
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH5);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH6);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH7);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH8);
			exeFile = fopen (cfgpath ,"rb");
		}
	}
	
	if (PACK3)
	{
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH9);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH10);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH11);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH12);
			exeFile = fopen (cfgpath ,"rb");
		}
	}
	
	if (PACK4)
	{
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH13);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH14);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH15);
			exeFile = fopen (cfgpath ,"rb");
		}
		if (exeFile==NULL)
		{
			sprintf(cfgpath, PATH16);
			exeFile = fopen (cfgpath ,"rb");
		}
	}
	
	// if nothing found exiting
	if (exeFile==NULL) {
           printf("\n\n\t\tCan't find DOL File...\n");
           Menu_Render();
           sleep(3);
           fclose (exeFile);
           SDCard_deInit();
           USBDevice_deInit();
           StopGX();
		SYS_ResetSystem(SYS_RETURNTOMENU, 0, 0);
	}

	fseek (exeFile, 0, SEEK_END);
	exeSize = ftell(exeFile);
	fseek (exeFile, 0, SEEK_SET);

	if(fread (exeBuffer, 1, exeSize, exeFile) != (unsigned int) exeSize)
	{
		printf("\n\n\t\tCan't open DOL File...\n");
		Menu_Render();
        fclose (exeFile);
		sleep(3);
        SDCard_deInit();
        USBDevice_deInit();
        StopGX();
		SYS_ResetSystem(SYS_RETURNTOMENU, 0, 0);
	}
	fclose (exeFile);

	/* load entry point */
	struct __argv args;
	bzero(&args, sizeof(args));
	args.argvMagic = ARGV_MAGIC;
	args.length = strlen(cfgpath) + 2;
	args.commandLine = (char*)malloc(args.length);
	if (!args.commandLine) SYS_ResetSystem(SYS_RETURNTOMENU, 0, 0);
	strcpy(args.commandLine, cfgpath);
	args.commandLine[args.length - 1] = '\0';
	args.argc = 1;
	args.argv = &args.commandLine;
	args.endARGV = args.argv + 1;

	int ret = valid_elf_image(exeBuffer);
	if (ret == 1)
		exeEntryPointAddress = load_elf_image(exeBuffer);
	else
		exeEntryPointAddress = load_dol_image(exeBuffer, &args);

	/* fadeout of image */
	for(int i = 255; i > 1; i = i-7)
	{
		if(i < 0) i = 0;
		Background_Show(0, 0, 0, imgdata, 0, 1, 1, i);
		Menu_Render();
	}
	SDCard_deInit();
	USBDevice_deInit();
	StopGX();
	if (exeEntryPointAddress == 0) {
		printf("EntryPointAddress  failed...\n");
        Menu_Render();
        sleep(3);
        fclose (exeFile);
        SDCard_deInit();
        USBDevice_deInit();
        StopGX();
		SYS_ResetSystem(SYS_RETURNTOMENU, 0, 0);;
	}
	exeEntryPoint = (entrypoint) exeEntryPointAddress;
	/* cleaning up and load dol */
	SYS_ResetSystem(SYS_SHUTDOWN, 0, 0);
	_CPU_ISR_Disable (cookie);
	__exception_closeall ();
	exeEntryPoint ();
	_CPU_ISR_Restore (cookie);
	return 0;
}
