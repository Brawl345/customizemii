**CustomizeMii** is a custom channel creator for the Wii.<br>
The .NET Framework 2.0 is required to run this application!<br>
<br>
For any further information, see: <a href='http://customizemii.googlecode.com'>http://customizemii.googlecode.com</a><br>
Please use the Issue Tracker there to report any occuring bugs.<br>
<br>
Thanks to icefire / Xuzz for the basic idea of this Application!<br>
<br>
<hr />
<h2>Changelog:</h2>

<h3>Version 3.11</h3>
<ul><li>Updated libWiiSharp to 0.21<br>
</li><li>=> Fixed detection of loops in wave files</li></ul>

<h3>Version 3.1</h3>
<ul><li>Updated libWiiSharp to 0.2<br>
</li><li>=> Speed up in TPL conversion<br>
</li><li>=> Fixed IA8 TPL code (from/to)<br>
</li><li>=> Fixed CI14X2 TPL code (from)<br>
</li><li>=> Added conversion to CI4 / CI8<br>
</li><li>=> Added BNS to Wave conversion (Extract -> Sound -> As Audiofile)<br>
</li><li>Added option to make the sound silent</li></ul>

<h3>Version 3.0</h3>
<ul><li>Switched backend to libWiiSharp (<a href='http://libwiisharp.googlecode.com'>http://libwiisharp.googlecode.com</a>)<br>
</li><li>Speed improvements through using RAM instead of temp files on HDD<br>
</li><li>No separate Mono version anymore, the normal version works with Mono (even compiling forwarders!)<br>
</li><li>Fixed complex forwarder loading screen to be fullscreen (thanks wilsoff / tantric!)<br>
</li><li>Fixed bug where startup IOS was set to 0 when sending channel to Wii<br>
</li><li>Doesn't require common-key.bin anymore<br>
</li><li>Added checkerboard for transparent areas in images (preview window)<br>
</li><li>Added ability to change a TPLs format (preview window)<br>
</li><li>Added ability to rename and resize TPLs (right click on listbox)</li></ul>

<h3>Version 2.31</h3>
<ul><li>Fixed sending to Wii</li></ul>

<h3>Version 2.3</h3>
<ul><li>Fixed bug when extracting icon images<br>
</li><li>Added ability to change the startup IOS (IOS used to launch the title)<br>
</li><li>Added support for Korean channel title (reading and writing)<br>
</li><li>Added TPL width and height to Banner and Icon list<br>
</li><li>Added ability to replace multiple TPLs at once (images must have the same filename!)</li></ul>

<h3>Version 2.2</h3>
<ul><li>Fixed some bugs with the preview window<br>
</li><li>Fixed Complex Forwarder in combination with Waninkoko's NAND Loader<br>
</li><li>Fixed saving of base WADs<br>
</li><li>Fixed saving of WAD after sending it to the Wii<br>
</li><li>Fixed CMP TPL code (thanks pbsds)<br>
</li><li>Added conversion to IA8 (thanks pbsds), IA4, I8 and I4<br>
</li><li>Fixed conversion from IA8 and I4<br>
</li><li>Fixed some general TPL bugs (format detection, ...)<br>
</li><li>Added additional paths to the Complex Forwarder (max. 16)</li></ul>

<h3>Version 2.1</h3>
<ul><li>Added CustomizeMii Installer (by WiiCrazy / I.R.on)<br>
</li><li>Fixed rough edges (artifacts) on images (will be fixed automatically)<br>
</li><li>Replaced the TPL preview window with the one from ShowMiiWads for easier handling<br>
</li><li>Added loop prelistening to the BNS conversion window (only for wave files)<br>
</li><li>Added drag & drop ability cause the file dialogs kept bothering me<br>
</li><li>Improvement in startup speed (thanks shadow1643)<br>
</li><li>Added creation/last edited time (only for CustomizeMii 2.1+ channels)<br>
</li><li>Added a button to translate the word "Channel" to every language<br>
</li><li>Improved detection of required TPLs<br>
</li><li>Little improvements and fixes<br>
</li><li>Changed the complex forwarder to be more configurable (choose any path you want)<br>
</li><li>ForwardMii is now bundled with CustomizeMii</li></ul>

<h3>Version 2.01</h3>
<ul><li>Base WAD downloading works again</li></ul>

<h3>Version 2.0</h3>
<ul><li>Added BNS conversion (Mono and Stereo, with and without loop)<br>
</li><li>Fixed MP3 conversion (some files didn't convert)<br>
</li><li>Lz77 checkbox is now ticked by default<br>
</li><li>Removed Lz77 compression of sound.bin as most sounds will get bigger<br>
</li><li>Added ability to insert DOLs from any channel WAD<br>
</li><li>Added ability to re-add the interal DOL (To switch the NAND Loader)<br>
</li><li>Added ability to extract the contents, DOL, sound and all images<br>
</li><li>Added displaying of approx. blocks to the success-message<br>
</li><li>Deleted some functions of the complex forwarder as they weren't working properly<br>
</li><li>Some bugfixes and improvements</li></ul>

<h3>Version 1.2</h3>
<ul><li>Fixed writing/reading of channel titles, so japanese characters will work now<br>
</li><li>Added checkbox (Options tab) to turn security checks off<br>
</li><li>Added built-in forwarder creator (Needs the ForwardMii.dll which is separately avaiable)<br>
</li><li>You can choose MP3 files as channel sound (Needs lame.exe in application directory)<br>
</li><li>Bugfixes</li></ul>

<h3>Version 1.1</h3>
<ul><li>Note: License upgraded to GNU GPL v3!<br>
</li><li>Sound is working now<br>
</li><li>Added brlan and brlyt tabs (for advanced users)<br>
</li><li>Added displaying of image width and height in preview window<br>
</li><li>Added "Make Transparent" checkbox for TPLs<br>
</li><li>Fixed IA8 previewing / extracting<br>
</li><li>Improved bricksafety (hopefully!)<br>
</li><li>Added Tooltips<br>
</li><li>Added update check at startup<br>
</li><li>Wrote basic instructions (see Instructions.txt or help tab)</li></ul>

<h3>Version 1.0</h3>
<ul><li>Initial Release<br>
<hr />
<br>
<hr />
<h2>Disclaimer:</h2></li></ul>

Editing WAD files can result in a brick of your Wii.<br>
Only use this application if you have a bricksafe Wii, meaning either Preloader or<br>
BootMii/boot2 is installed, and if you know what you're doing.<br>
<br>
This application comes without any express or implied warranty.<br>
The author can't be held responsible for any damages arising from the use of it.<br>
<hr />
<br>
<hr />
<h2>Thanks:</h2>

Xuzz for his idea and hard work<br>
WiiCrazy / I.R.on for CustomizeMii Installer<br>
Xuzz, SquidMan, megazig, Matt_P, Omega and The Lemon Man for Wii.py<br>
SquidMan for Zetsubou<br>
Andre Perrot for gbalzss<br>
comex and Waninkoko for both their NAND Loader<br>
djdynamite123 for the forwarder base files<br>
The USB Loader GX Team for their forwarder source<br>
<hr />
<br>
<hr />
<h2>License:</h2>

Copyright (C) 2009 Leathl<br>
<br>
CustomizeMii is free software: you can redistribute it and/or<br>
modify it under the terms of the GNU General Public License as published<br>
by the Free Software Foundation, either version 3 of the License, or<br>
(at your option) any later version.<br>
<br>
CustomizeMii is distributed in the hope that it will be<br>
useful, but WITHOUT ANY WARRANTY; without even the implied warranty<br>
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the<br>
GNU General Public License for more details.<br>
<br>
You should have received a copy of the GNU General Public License<br>
along with this program.  If not, see <<a href='http://www.gnu.org/licenses/>'>http://www.gnu.org/licenses/&gt;</a>.<br>
<hr />