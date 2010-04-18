TransmitMii is a little tool to transmit your DOL, ELF or WAD files to the Wii (WAD files
only with USB Loader GX).
Once you typed in your IP and protocol, you can simply drag a file onto the EXE to
transmit it.

The .NET Framework 2.0 is required to run this application!
TransmitMii is hosted on the CustomizeMii project page: http://customizemii.googlecode.com

-----------------------------------------------------------------------------------------
Changelog:

Version 1.3
	- Fixed sending zip files

Version 1.2
	- Improvement in startup speed (thanks shadow1643)
	- Added ability to send ZIPs (to HBC)
	  ZIP files must conatin the *appfolder* containing
		boot.dol
		meta.xml (optional)
		icon.png (optional)
	  and will be extracted to SD:/apps/*appfolder*

Version 1.15
	- Fixed transmitting to HBC -1.0.4 (HAXX)
	- Fixed transmitting to USB Loader GX
	- Bugfix

Version 1.1
	- Added ability to link and unlink extensions with TransmitMii
	- Added compression for HBC 1.0.5+ (similar to wiiload)

Version 1.0
	- Initial Release
-----------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------
Disclaimer:

This application comes without any express or implied warranty.
The author can't be held responsible for any damages arising from the use of it.
-----------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------
Thanks:

dhewg for wiiload
WiiCrazy / I.R.on for Crap
Jean-loup Gailly and Mark Adler for zlib1.dll (zlib)
dillonaumiller for Associaton.cs
-----------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------
License:

Copyright (C) 2009 Leathl

TransmitMii is free software: you can redistribute it and/or
modify it under the terms of the GNU General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TransmitMii is distributed in the hope that it will be
useful, but WITHOUT ANY WARRANTY; without even the implied warranty
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
-----------------------------------------------------------------------------------------