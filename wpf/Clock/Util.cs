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
using System.Globalization;
using System.Windows.Media;

namespace Clock
{
	public static class Util
	{
		public static double mjd(DateTime dt) { return 15018 + dt.ToOADate(); }
		public static Color color(string s) { return (Color)ColorConverter.ConvertFromString(s); }
		public static int week(DayOfWeek d) { return d <= 0 ? 7 : (int)d; }
		public static int week(DateTime dt)
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
		public static int year(DateTime dt)
		{
			var carry = dt.Month == 12 && dt.Day > 25 && week(dt) < 10;
			return dt.Year + (carry ? 1 : 0);
		}
	}
}
