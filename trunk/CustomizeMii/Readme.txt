CustomizeMii is a custom channel creator for the Wii.
The .NET Framework 2.0 is required to run this application!

For any further information, see: http://customizemii.googlecode.com
Please use the Issue Tracker there to report any occuring bugs.

Thanks to icefire / Xuzz for the basic idea of this Application!

-----------------------------------------------------------------------------------------
Changelog:

Version 2.1
	- Replaced the TPL preview window with the one from ShowMiiWads for easier handling
	- Added loop prelistening to the BNS conversion window (only for wave files)
	- Added drag & drop ability cause the file dialogs kept bothering me
	- Improvement in startup speed (thanks shadow1643)
	- Added Unix timestamp as footer (interesting to know when channels were created, huh?)
	- Little improvements
	
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
Xuzz, SquidMan, megazig, Matt_P, Omega and The Lemon Man for Wii.py
SquidMan for Zetsubou
Andre Perrot for gbalzss
comex and Waninkoko for both their NAND Loader
djdynamite123 for the forwarder base files (ForwardMii-Plugin)
The USB Loader GX Team for their forwarder source (ForwardMii-Plugin)
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