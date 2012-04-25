using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graffiti.Core;

namespace Graffiti.Plugins.News
{
	public static class NewsExtensions
	{
		public static int GetPriority(this Post post)
		{
			return CommonFunctions.TryIntParse(post.Custom("Priority"), 1);
		}

		public static DateTime GetStartDate(this Post post)
		{
			return CommonFunctions.TryDateTimeParse(post.Custom("Start Date"));
		}
		public static DateTime GetEndDate(this Post post)
		{
			return CommonFunctions.TryDateTimeParse(post.Custom("End Date"), DateTime.MaxValue);
		}
	}
}
