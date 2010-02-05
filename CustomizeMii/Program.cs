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
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace CustomizeMii
{
    static class Program
    {
        static Mutex mtx;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CleanupRemains();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CustomizeMii_Main());
        }

        static void CleanupRemains()
        {
            bool firstInstance = false;
            mtx = new System.Threading.Mutex(false, "CustomizeMii", out firstInstance);

            if (firstInstance)
            {
                if (Directory.Exists(Path.GetTempPath() + "CustomizeMii_Temp"))
                    Directory.Delete(Path.GetTempPath() + "CustomizeMii_Temp", true);
                if (Directory.Exists(Path.GetTempPath() + "ForwardMii_Temp"))
                    Directory.Delete(Path.GetTempPath() + "ForwardMii_Temp", true);
            }
        }
    }
}
