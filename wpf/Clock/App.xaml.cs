/*
 * Copyright (C) 2023 Heptazhou <zhou@0h7z.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Clock
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Main(object sender, StartupEventArgs e)
		{
			var MW = new MainWindow();
			SetAppPreferDarkMode();
			SetImmersiveDarkMode(new WindowInteropHelper(MW).EnsureHandle());
			MW.Show();
		}

		[DllImport("dwmapi.dll")]
		private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int size);

		[DllImport("uxtheme.dll", EntryPoint = "#135")]
		private static extern int AllowDarkModeForApp(int allow = 1);

		[DllImport("uxtheme.dll", EntryPoint = "#135")]
		private static extern int SetPreferredAppMode(int mode = (int)AppMode.ForceDark);

		private static void SetAppPreferDarkMode()
		{
			if (Environment.OSVersion.Version.Major < 10) return;
			if (Environment.OSVersion.Version.Build < 18362) // 1903
				AllowDarkModeForApp();
			else
				SetPreferredAppMode();
		}

		private static void SetImmersiveDarkMode(IntPtr hWnd, int value = 1)
		{
			if (Environment.OSVersion.Version.Major < 10) return;
			if (Environment.OSVersion.Version.Build < 17763) return; // 1809
			if (DwmSetWindowAttribute(hWnd, 20, ref value, sizeof(int)) != 0)
				DwmSetWindowAttribute(hWnd, 19, ref value, sizeof(int));
		}

		private enum AppMode { Default, AllowDark, ForceDark, ForceLight, Max }
	}
}
