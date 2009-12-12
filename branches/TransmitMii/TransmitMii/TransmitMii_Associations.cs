/* This file is part of TransmitMii (Original code by dillonaumiller, thanks!)
 * Copyright (C) 2009 dillonaumiller, Leathl
 * 
 * TransmitMii is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * TransmitMii is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace TransmitMii
{
    public static class TransmitMii_Associations
    {
        public enum Extension
        {
            DOL = 0,
            ELF = 1,
            Both = 2,
            WAD = 3
        }

        public static string AssociationPath()
        {
            try { return Registry.GetValue("HKEY_CLASSES_ROOT\\WiiBin\\shell\\sendwii\\command", "", "").ToString().Replace("%1", "").Replace("\"", ""); }
            catch { return ""; }
            
        }

        public static bool CheckAssociation(Extension which)
        {
            try
            {
                if (which == Extension.Both)
                {
                    if (Registry.GetValue("HKEY_CLASSES_ROOT\\.dol", "", "").ToString().ToLower() == "wiibin" &&
                        Registry.GetValue("HKEY_CLASSES_ROOT\\.elf", "", "").ToString().ToLower() == "wiibin") return true;
                    else return false;
                }

                string regKey;
                if (which == Extension.DOL)
                    regKey = "HKEY_CLASSES_ROOT\\.dol";
                else if (which == Extension.ELF)
                    regKey = "HKEY_CLASSES_ROOT\\.elf";
                else
                    regKey = "HKEY_CLASSES_ROOT\\.wad";


                if (Registry.GetValue(regKey, "", "").ToString().ToLower() != "wiibin") return false;
                else return true;
            }
            catch { return false; }
        }

        public static bool DeleteAssociation(Extension which)
        {
            try
            {
                string regKey;
                if (which == Extension.DOL)
                    regKey = "HKEY_CLASSES_ROOT\\.dol";
                else if (which == Extension.ELF)
                    regKey = "HKEY_CLASSES_ROOT\\.elf";
                else
                    regKey = "HKEY_CLASSES_ROOT\\.wad";

                if (ValueExists(regKey, ""))
                    Registry.SetValue(regKey, "", "");

                return true;
            }
            catch { return false; }
        }

        public static bool AddAssociation(Extension which, bool makeDefault, string iconPath, bool updateIconCache)
        {
            //-------
            //OPTIONS
            //-------
            //if(makeDefault) then "Send to Wii" will be the default action for this file type
            //if((iconPath == null) || (iconPath == "")) then we won't create a "DefaultIcon" entry for this type
            //if(updateIconCache) then force explorer to update it's icon cache (without a logout/login cycle), NOTE: this has a side affect of possibly losing custom desktop icon positions

            if ((which == Extension.DOL) || (which == Extension.Both))
                if (AddExtension("dol", makeDefault, iconPath) == false)
                    return false;

            if ((which == Extension.ELF) || (which == Extension.Both))
                if (AddExtension("elf", makeDefault, iconPath) == false)
                    return false;

            if (which == Extension.WAD)
                if (AddExtension("wad", makeDefault, iconPath) == false)
                    return false;

            //get explorer to re-draw the new icon, if needed
            if(updateIconCache)
                    UpdateExplorerIconCache();

            return true;
        }

        //==========================================================================
        // private code
        //==========================================================================
        static bool AddExtension(string extension, bool makeDefault, string iconPath)
        {
            bool createClass = true;
            string rkEXT = "HKEY_CLASSES_ROOT\\." + extension;
            string rkCLS = "HKEY_CLASSES_ROOT\\WiiBin";

            //check if the file-type already has a class
            if (ValueExists(rkEXT, ""))
            {
                string currClass = (string)Registry.GetValue(rkEXT, "", "");
                if (currClass.Trim().Length > 0)
                {
                    rkCLS = "HKEY_CLASSES_ROOT\\" + currClass;
                    if (ValueExists(rkCLS, ""))  //exists, and is valid
                        createClass = false;
                }
                else //we need to create an entry for this filetype
                    if (KeyCreate(rkEXT, "WiiBin") == false)
                        return false;
            }
            else //we need to create an entry for this filetype
                if (KeyCreate(rkEXT, "WiiBin") == false)
                    return false;

            if(createClass) //then we'll be using our own "WiiBin" class
            {
                //create main key
                if (KeyCreate(rkCLS, "Wii Binary Image") == false)
                    return false;

                //create "DefaultIcon" subkey
                if (iconPath != null)
                    if (iconPath != "")
                        if (KeyCreate(rkCLS + "\\DefaultIcon", iconPath) == false)
                            return false;

                //create "Shell" subkey
                if (KeyCreate(rkCLS + "\\shell", "") == false)
                    return false;
            }

            //set default icon if desired
            if (iconPath != null)
                if (iconPath != "")
                    if (SetStringValue(rkCLS + "\\DefaultIcon", "", iconPath) == false)
                        return false;

            //set default action if desired
            if (makeDefault)
                if (SetStringValue(rkCLS + "\\shell", "", "sendwii") == false)
                    return false;

            //create "Shell\\sendwii" subkey
            if (KeyCreate(rkCLS + "\\shell\\sendwii", "Send to Wii") == false)
                return false;

            //create "Shell\\sendwii\\command
            if (KeyCreate(rkCLS + "\\shell\\sendwii\\command", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" \"%1\"") == false)
                return false;

            return true;
        }

        static bool ValueExists(string key, string value)
        {
            if (Registry.GetValue(key, value, null) == null)
                return false;
            return true;
        }

        static bool KeyCreate(string key, object defaultValue)
        {
            try
            {
                Registry.SetValue(key, "", defaultValue);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        static bool SetStringValue(string key, string value, string data)
        {
            try
            {
                Registry.SetValue(key, value, data);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        [DllImport("user32.dll")] static extern long SendMessageTimeout(int hWnd, int Msg, int wParam, string lParam, int fuFlags, int uTimeout, out int lpdwResult);

        static void UpdateExplorerIconCache()
        {
            string currMetricVal = (string)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "Shell Icon Size", null);
            if (currMetricVal == null)
                return; //how the hell did this happen?

            int iSize;
            try
            {
                iSize = int.Parse(currMetricVal) + 1;
            }
            catch (Exception)
            {
                iSize = 31;
            }

            RegistryKey siz;
            int res = 0;

            siz = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop").OpenSubKey("WindowMetrics", true);
            siz.SetValue("Shell Icon Size", iSize.ToString());
            siz.Flush();
            siz.Close();
            SendMessageTimeout(0xFFFF, 0x001A, 0, "", 0, 100, out res);
            siz = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop").OpenSubKey("WindowMetrics", true);
            siz.SetValue("Shell Icon Size", currMetricVal);
            siz.Flush();
            siz.Close();
            SendMessageTimeout(0xFFFF, 0x001A, 0, "", 0, 100, out res);
        }
    }
}
