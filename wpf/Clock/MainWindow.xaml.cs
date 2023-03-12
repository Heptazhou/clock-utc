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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Clock
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var Clk = new DispatcherTimer();
			Clk.Interval = TimeSpan.FromMilliseconds(10);
			Clk.Tick += new EventHandler(Tick);
			Clk.Start();

			TextTime.Foreground = new SolidColorBrush(color("#66ccff"));
			TextDate.Foreground = new SolidColorBrush(color("#9999ff"));
			TextWeek.Foreground = new SolidColorBrush(color("#cccc00"));
		}

		private void Tick(object? sender, EventArgs e)
		{
			var Time = DateTime.UtcNow;
			TextTime.Text = Time.ToString(timeformat("yyyy-MM-dd HH:mm:ss"));
			var Date = Time.Year * 1000 + Time.DayOfYear;
			TextDate.Text = Date.ToString(dateformat("0000-000"));
			var Week = week(Time) * 10 + week(Time.DayOfWeek);
			TextWeek.Text = Week.ToString(weekformat("W00-0"));
		}

		private static string timeformat(string fmt) { return fmt; }
		private static string dateformat(string fmt) { return fmt; }
		private static string weekformat(string fmt) { return fmt; }
		private static Color color(string s) { return (Color)ColorConverter.ConvertFromString(s); }
		private static int week(DayOfWeek d) { return d <= 0 ? 7 : (int)d; }
		private static int week(DateTime dt)
		{
			var wk = ISOWeek.GetWeekOfYear(dt);
			var ny = ISOWeek.GetWeekOfYear(new DateTime(dt.Year, 1, 1));
			if (ny > 1)
				if (wk > 10 && dt.Month == 1)
					wk = 01;
				else
					wk += 1;
			if (dt.Month == 12 && dt.Day > 25 && dt.Year < 9999)
			{
				var no = week(new DateTime(dt.Year + 1, 1, 1).DayOfWeek) - 1;
				if (no + dt.Day > 31)
					wk = 01;
			}
			else if (dt.Year == 9999)
				if (dt.Month == 12 && dt.Day > 26)
					wk = 01;
			return (wk);
		}
	}
}
