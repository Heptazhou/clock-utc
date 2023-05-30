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
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using static Clock.Util;

namespace Clock
{
	/// <summary>
	/// Interaction logic for MainWindowMJD.xaml
	/// </summary>
	public partial class MainWindowMJD : Window
	{
		public MainWindowMJD()
		{
			InitializeComponent();

			var Clk = new DispatcherTimer();
			Clk.Interval = TimeSpan.FromMilliseconds(10);
			Clk.Tick += new EventHandler(Tick);
			Clk.Start();

			TextTime.Foreground = new SolidColorBrush(color("#66ccff"));
			TextDate.Foreground = new SolidColorBrush(color("#9999ff"));
			TextWeek.Foreground = new SolidColorBrush(color("#cccc00"));
			TextJmdt.Foreground = new SolidColorBrush(color("#599fff"));
			TextJvar.Foreground = new SolidColorBrush(color("#a43ee4"));
			TextJvar.Text = "MJD";
		}

		private void Tick(object? sender, EventArgs e)
		{
			var Time = DateTime.UtcNow;
			TextTime.Text = Time.ToString(timeformat("yyyy-MM-dd HH:mm:ss"));
			var Date = Time.Year * 1000 + Time.DayOfYear;
			TextDate.Text = Date.ToString(dateformat("0000-000"));
			var Week = week(Time) * 10 + week(Time.DayOfWeek);
			TextWeek.Text = Week.ToString(weekformat("W00-0"));
			var Jmdt = jmdt(Time);
			TextJmdt.Text = Jmdt.ToString(jmdtformat("00.00000"));
		}
	}
}
