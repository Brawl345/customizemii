CustomizeMii is a custom channel creator for the Wii.
The .NET Framework 2.0 is required to run this application!

For any further information, see: http://customizemii.googlecode.com
Please use the issue tracker there to report any occuring bugs.

Thanks to icefire / Xuzz for the basic idea of this application!

-----------------------------------------------------------------------------------------
Changelog:

Version 3.11
	- Updated libWiiSharp to 0.21
	=> - Fixed detection of loops in wave files

Version 3.1
	- Updated libWiiSharp to 0.2
	=> - Speed up in TPL conversion
	=> - Fixed IA8 TPL code (from/to)
	=> - Fixed CI14X2 TPL code (from)
	=> - Added conversion to CI4 / CI8
	=> - Added BNS to Wave conversion (Extract -> Sound -> As Audiofile)
	- Added option to make the sound silent

Version 3.0
	- Switched backend to libWiiSharp (http://libwiisharp.googlecode.com)
	- Speed improvements through using RAM instead of temp files on HDD
	- No separate Mono version anymore, the normal version works with Mono (even compiling forwarders!)
	- Fixed complex forwarder loading screen to be fullscreen (thanks wilsoff / tantric!)
	- Fixed bug where startup IOS was set to 0 when sending channel to Wii
	- Doesn't require common-key.bin anymore
	- Added checkerboard for transparent areas in images (preview window)
	- Added ability to change a TPLs format (preview window)
	- Added ability to rename and resize TPLs (right click on listbox)

Version 2.31
	- Fixed sending to Wii

Version 2.3
	- Fixed bug when extracting icon images
	- Added ability to change the startup IOS (IOS used to launch the title)
	- Added support for Korean channel title (reading and writing)
	- Added TPL width and height to Banner and Icon list
	- Added ability to replace multiple TPLs at once (images must have the same filename!)

Version 2.2
	- Fixed some bugs with the preview window
	- Fixed Complex Forwarder in combination with Waninkoko's NAND Loader
	- Fixed saving of base WADs
	- Fixed saving of WAD after sending it to the Wii
	- Fixed CMP TPL code (thanks pbsds)
	- Added conversion to IA8 (thanks pbsds), IA4, I8 and I4
	- Fixed conversion from IA8 and I4
	- Fixed some general TPL bugs (format detection, ...)
	- Added additional paths to the Complex Forwarder (max. 16)

Version 2.1
	- Added CustomizeMii Installer (by WiiCrazy / I.R.on)
	- Fixed rough edges (artifacts) on images (will be fixed automatically)
	- Replaced the TPL preview window with the one from ShowMiiWads for easier handling
	- Added loop prelistening to the BNS conversion window (only for wave files)
	- Added drag & drop ability cause the file dialogs kept bothering me
	- Improvement in startup speed (thanks shadow1643)
	- Added creation/last edited time (only for CustomizeMii 2.1+ channels)
	- Added a button to translate the word "Channel" to every language
	- Improved detection of required TPLs
	- Little improvements and fixes
	- Changed the complex forwarder to be more configurable (choose any path you want)
	- ForwardMii is now bundled with CustomizeMii
	
Version 2.01
	- Base WAD downloading works again

Version 2.0
	- Added BNS conversion (Mono and Stereo, with and without loop)
	- Fixed MP3 conversion (some files didn't convert)
	- Lz77 checkbox is now ticked by default
	- Removed Lz77 compression of sound.bin as most sounds will get bigger
	- Added ability to insert DOLs from any channel WAD
	- Added ability to re-add the interal DOL (To switch the NAND Loader)
	- Added ability to extract the contents, DOL, sound and all images
	- Added displaying of approx. blocks to the success-message
	- Deleted some functions of the complex forwarder as they weren't working properly
	- Some bugfixes and improvements

Version 1.2
	- Fixed writing/reading of channel titles, so japanese characters will work now
	- Added checkbox (Options tab) to turn security checks off
	- Added built-in forwarder creator (Needs the ForwardMii.dll which is separately avaiable)
	- You can choose MP3 files as channel sound (Needs lame.exe in application directory)
	- Bugfixes

Version 1.1
	- Note: License upgraded to GNU GPL v3!
	- Sound is working now
	- Added brlan and brlyt tabs (for advanced users)
	- Added displaying of image width and height in preview window
	- Added "Make Transparent" checkbox for TPLs
	- Fixed IA8 previewing / extracting
	- Improved bricksafety (hopefully!)
	- Added Tooltips
	- Added update check at startup
	- Wrote basic instructions (see Instructions.txt or help tab)

Version 1.0
	- Initial Release
-----------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------
Disclaimer:

Editing WAD files can result in a brick of your Wii.
Only use this application if you have a bricksafe Wii, meaning either Preloader or
BootMii/boot2 is installed, and if you know what you're doing.

This application comes without any express or implied warranty.
The author can't be held responsible for any damages arising from the use of it.
-----------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------
Thanks:

Xuzz for his idea and hard work
WiiCrazy / I.R.on for CustomizeMii Installer
Xuzz, SquidMan, megazig, Matt_P, Omega and The Lemon Man for Wii.py
megazig for his BNS conversion code
SquidMan for Zetsubou
Andre Perrot for gbalzss
comex and Waninkoko for both their NAND Loader
djdynamite123 for the forwarder base files
The USB Loader GX Team for their forwarder source
wilsoff for helping me debugging 3.0
-----------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------
License:

Copyright (C) 2009 Leathl

CustomizeMii is free software: you can redistribute it and/or
modify it under the terms of the GNU General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

CustomizeMii is distributed in the hope that it will be
useful, but WITHOUT ANY WARRANTY; without even the implied warranty
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
-----------------------------------------------------------------------------------------