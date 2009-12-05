/* This file is part of CustomizeMii
 * Copyright (C) 2009 Leathl
 * 
 * CustomizeMii is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * CustomizeMii is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace ForwardMii
{
    public class ForwardMii_Plugin
    {
        const string version = "1.0"; //Hint for myself: Never use a char in the Version (UpdateCheck)!

        public static string GetVersion()
        {
            return version;
        }

        public static bool CheckDevKit()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEVKITPRO")) ||
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEVKITPPC"))) return false;

            if (!System.IO.Directory.Exists(Environment.GetEnvironmentVariable("DEVKITPRO").Remove(0, 1).Insert(1, ":") + "/libogc")) return false;

            return true;
        }
    }
}
