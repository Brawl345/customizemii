**These are some basic instructions for CustomizeMii.**<br>
<br>
<b><i>At the very beginning, let me say this again: Don't install any WADs without a proper brick protection!</i></b><br>
<br>
Ok, so you want to create your own custom channels?<br>
First it is important to understand how the creation of a custom channel with CustomizeMii works.<br>
Skip this if you already know.<br>
<br>
Basically, you can't create a channel from scratch. Well, you could, but not with this application.<br>
You need a base WAD which you can modify. You can either download one within this application or use<br>
any WAD (must be a channel of course) from anywhere.<br>
You can then edit the WAD file.<br>
The editing of animations is for advanced users, so if you're not familiar with brlyt's and brlan's<br>
(I guess you're not unless you created your own animations), get a static base WAD or one with the<br>
animation you want.<br>
<br>
So, let's start to create a channel. Download a base WAD through the application or load one from your HDD.<br>
You can preview the downloadable WADs through the preview button.<br>
All options below are optional!<br>
<br>
<br>
<br>
<b>MIXING BANNER, ICON AND SOUND</b><br>
<br>
The options below the downloadable base WADs are the "replace" options.<br>
Let's say you have loaded WAD A as a base, but you want the icon of WAD B and you have a 00000000.app that<br>
contains a sound that you want to use.<br>
No problem! Just use the dropdownbox which yet says "Banner" and choose "Icon". Click on the browse button<br>
next to it and choose WAD B. The icon of WAD B will replace the icon of WAD A.<br>
After that, choose "Sound" from the dropdownbox and browse for the 00000000.app, the sound.bin will be<br>
extracted and used instead of the one within WAD A.<br>
<br>
Note: These features are non-destructive, i.e. you can always use the clear button to get back to the<br>
banner/icon/sound of WAD A!<br>
<br>
<br>
<br>
<b>CHANNEL INFORMATION</b><br>
<br>
You may want to change the channel information, i.e. the title and ID.<br>
Let's change the channel title first. It's the text that will be displayed when you hold your cursor<br>
over the channel. Goto the "Title" tab and enter a title for all languages. If you want to change the<br>
title for a specific language, just use the language's textbox. If you want a different title for every<br>
language, you don't need to fill in a title for all languages.<br>
You may also use the translate "Channel" button. Enter the english name in the all languages textbox<br>
(e.g. "MPlayer Channel"). Click translate and it will be automatically translated to each language.<br>
Now, the title ID. Open the "Options" tab and you'll see a textbox for the ID. The ID is 4 characters long<br>
and only contains letters and numerics. Lower case letters will automatically converted to upper case.<br>
Change it to a unique(!) ID, because channels will overwrite existing channles with the same ID!<br>
<br>
I recommend not to use any title ID beginning with the following characters, because official channels<br>
use these and thus your channel may overwrite them:<br>
C, E, F, H, J, L, M, N, P, Q, W<br>
<br>
<br>
<br>
<b>INSERTING A NEW DOL</b><br>
<br>
Let's bring some life into the channel. In the "Options" tab, use the browse button for a new DOL.<br>
Either load a forwarder or the DOL of any application. Note that some channles require more than just a DOL<br>
and thus may not work in a channel (e.g. MPlayer CE).<br>
Choose a NAND loader or just stick with the one selected (both will do fine).<br>
<br>
You can also use the built-in forwarder creation by using the forwarder button right below the browse button for DOLs.<br>
You need the ForwardMii.dll in order to use the forwarder creation.<br>
<br>
<br>
<br>
<b>INSERTING A NEW SOUND</b><br>
<br>
For the sound, you can either use a wave file or the sound replace function to use the sound of an<br>
existing sound.bin/00000000.app/WAD.<br>
If you want looped sound, open your wave file with wavosaur and add loop points before.<br>
<br>
To save space, you can convert your wav or mp3 files to BNS. You can take the loop from a prelooped wave file<br>
or enter the loop start point manually. Wave files must be 16bit PCM.<br>
It is possible to directly convert stereo Wave files to mono BNS files, note that only the left channel of<br>
the Wave will be taken.<br>
<br>
<br>
<br>
<b>EDITING THE BANNER/ICON (If you're an advanced user and want to edit the brlyt/brlan, do that first!)</b><br>
<br>
So, let's begin with the real customization. I will only talk about the banner here, the instructions<br>
are the same for the icon.<br>
Goto the "Banner" tab. You shouldn't touch the add and remove buttons, they're for advanced users that<br>
change the animation. (However, they can't really harm your channel, as CustomizeMii will check for missing<br>
and unneeded TPLs while creating a WAD).<br>
You will see a list with all TPLs inside the banner.bin. When you select a TPL,<br>
it's current format will be shown in the "Format" dropdownbox. Note that CustomizeMii can read 8 different<br>
TPL formats, but only write 3 (RGBA8, RGB565 and RGB5A3), that should be enough for your needs.<br>
You can use the preview button to preview a TPL (obvious, right?), but you get one more important info,<br>
the image size. It will be shown in the title of the preview window. If your images aren't the same size,<br>
they will be resized! So be sure to have at least the correct aspect ratio, so your images wont be<br>
squeezed or whatever.<br>
Before replacing the image, choose a format from the dropdownbox.<br>
(RGBA8 = High Quality, Big Size --- RGB565 = Moderate Quality, Small Size --- RGB5A3 = Bad Quality, Small Size)<br>
Now you can use the replace button to insert your image. Preview the TPL after replacing to check the<br>
image. Maybe you want to use another format though? No problem, just replace the TPL again.<br>
<br>
Note: You can use the "Make Transparent" checkbox to make a TPL transparent, e.g. if you don't like<br>
one piece of an animation (It's non-destructive, i.e. you can always uncheck the box).<br>
<br>
<br>
<br>
<b>EDITING THE ANIMATION (Advanced users only!)</b><br>
<br>
Skip this part, if you don't really know what brlyt and brlan files are and how they're structured.<br>
Goto the "brlyt" tab. You will see the banner.brlyt and icon.brlyt there.<br>
Above the buttons is a text that will indicate whether you're doing actions on the "Banner" or<br>
the "Icon" (When you select the banner.brlyt, you're editing the "Banner" and vice versa).<br>
Now, just replace the banner.brlyt and icon.brlyt files as you want. You can use the list TPLs button<br>
afterwards to see all TPLs that are required by the banner.brlyt/icon.brlyt (Don't worry, CustomizeMii<br>
won't let you create a WAD, if you forgot a required TPL).<br>
Now, goto the "brlan" tab. It's similar to the "brlyt" tab. You shouldn't touch the add or delete button<br>
unless your base WAD only has a banner.brlan and you want to use a banner_Start.brlan and banner_Loop.brlan.<br>
In this case, first add the two files and then delete the old banner.brlan.<br>
Else just replace the files with yours. Be absolutely sure the your brlan files only refer to panes that are<br>
indicated in your brlyt files!<br>
That's it, here's nothing left to do.<br>
<br>
<br>
<br>
<b>CREATING THE WAD</b><br>
<br>
Well, your channel should be ready to be created.<br>
Just click on the create WAD button.<br>
CustomizeMii will do some failure checks and if all went fine, a save dialog will pop up.<br>
If you get an error or warning, read the message carefully. It should give you enough information to fix<br>
the problem yourself.<br>
You can also send the Channel directly to the Wii. For it to work, the Channel must be less than 4 MB of space.<br>
Make sure the Homebrew Channel is running and connected. Click the send WAD button. Choose a protocol<br>
and enter your Wii's IP and the IOS to use for installation. Click on transmit to start the creation and<br>
transmission process. After the channel was sent, you will be asked if you want to save the Channel.<br>
<br>
So, if you got down to here, you're done by now. Please, if you find any bugs or have suggestions, take some<br>
seconds to report them at the issue tracker: <a href='http://code.google.com/p/customizemii/issues/list'>http://code.google.com/p/customizemii/issues/list</a>