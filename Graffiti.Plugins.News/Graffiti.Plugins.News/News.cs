using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graffiti.Core;
using DataBuddy;
using System.Data;

namespace Graffiti.Plugins.News
{
	[Chalk("news")]
	[Serializable()]
	public class News
	{
		public PostCollection GetNewsItems(int count)
		{
			CategoryController cc = new CategoryController();
			// TODO: Need to get category name from plugin
			Category newsCategory = cc.GetCachedCategory("News", true);

			DataBuddy.Table table = new DataBuddy.Table("graffiti_posts", "PostCollection");
			Query query = new Query(table);
			query.Top = "100 PERCENT *";

			Column categoryColumn = new Column("CategoryId", DbType.Int32, typeof(Int32), "CategoryId", false, false);
			query.AndWhere(categoryColumn, newsCategory.Id, Comparison.Equals);

			PostCollection posts = PostCollection.FetchByQuery(query);

			List<Post> newsPosts = posts.Where(p => p.GetStartDate() <= DateTime.Today)
				.Where(p => p.GetEndDate() >= DateTime.Today)
				.Where(p => !p.IsDeleted)
				.OrderByDescending(p => p.GetPriority())
				.ThenByDescending(p => p.Published).ToList();

			count = count > newsPosts.Count ? newsPosts.Count : count;
			newsPosts = newsPosts.GetRange(0, count);
			PostCollection ret = new PostCollection();
			ret.AddRange(newsPosts);
			return ret;
		}
	}
}
