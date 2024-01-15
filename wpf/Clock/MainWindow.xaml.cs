/*
 * Copyright (C) 2023-2024 Heptazhou <zhou@0h7z.com>
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var Clk = new DispatcherTimer();
			Clk.Interval = TimeSpan.FromSeconds(1);
			Clk.Tick += new EventHandler(Tick);
			Clk.Start(); Tick(null, null);

			TextTime.Foreground = new SolidColorBrush(color("#66ccff"));
			TextDayL.Foreground = new SolidColorBrush(color("#9999ff"));
			TextDayR.Foreground = new SolidColorBrush(color("#cccc00"));
#if MJD
			TextMjdL.Foreground = new SolidColorBrush(color("#a43ee4"));
			TextMjdR.Foreground = new SolidColorBrush(color("#599fff"));
#else
			StackPanel.Children.Remove(GridMjd);
#endif
		}

		private void Tick(object? _sender, EventArgs? _e)
		{
			var Time = DateTime.UtcNow;
			TextTime.Text = Time.ToString("yyyy-MM-dd HH:mm:ss");
			var Week = year(Time) * 1000 + week(Time) * 10 + week(Time.DayOfWeek);
			TextDayL.Text = Week.ToString("0000-W00-0");
			var Date = Time.DayOfYear;
			TextDayR.Text = Date.ToString("000");
#if MJD
			var MjdT = mjd((Time));
			TextMjdR.Text = MjdT.ToString("00.00000");
#endif
		}
	}
}
