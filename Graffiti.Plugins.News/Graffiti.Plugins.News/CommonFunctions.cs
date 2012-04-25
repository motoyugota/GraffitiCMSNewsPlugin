using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graffiti.Plugins.News
{
	internal static class CommonFunctions
	{
		public static bool ConvertStringToBool(string checkValue)
		{
			if (string.IsNullOrEmpty(checkValue))
				return false;
			else if (checkValue == "checked" || checkValue == "on")
				return true;
			else
				return bool.Parse(checkValue);
		}

		public static DateTime TryDateTimeParse(string dateTime)
		{
			return TryDateTimeParse(dateTime, DateTime.MinValue);
		}

		public static DateTime TryDateTimeParse(string dateTime, DateTime defaultValue)
		{
			try
			{
				return DateTime.Parse(dateTime);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static int TryIntParse(string value, int defaultValue)
		{
			try
			{
				return int.Parse(value);
			}
			catch
			{
				return defaultValue;
			}
		}
	}
}
