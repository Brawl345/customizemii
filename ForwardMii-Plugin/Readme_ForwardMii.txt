This is a plugin for CustomizeMii to create forwarders. Put it in CustomizeMii's directory.

It can create simple SD / SDHC forwarders that will point to one ELF or DOL file.
It's also able to create complex forwarders based on the official USB Loader GX
forwarder (Needs devkitPPC + libOGC installed). It will look for the DOL or ELF
in the specified folder on USB and SD, you can change the order it searches.
It can also display an image while loading.
If it fails to compile, you may have the wrong libOGC, try the one downloadable on
the CustomizeMii project page.

-----------------------------------------------------------------------------------------
Changelog:

Version 1.01
	- Fixed bug that caused an object error while creating forwarders

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
djdynamite123 for the forwarder base files
The USB Loader GX Team for their forwarder source
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