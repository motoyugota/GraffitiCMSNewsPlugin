using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graffiti.Core;
using System.Collections.Specialized;

namespace Graffiti.Plugins.News
{

	public class NewsPlugin : GraffitiEvent
	{
		private readonly string startDateFieldName = "Start Date";
		private readonly string endDateFieldName = "End Date";
		private readonly string priorityFieldName = "Priority";
		private readonly string newsCategoryName = "News";

		public bool EnableNews { get; set; }

		public override bool IsEditable
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "News Plugin"; }
		}

		public override string Description
		{
			get { return "Allow Scheduling News Items.<br />Note that disabling this plugin does not turn off the functionality, because all this plugin does is add a category and custom fields to the site.<br />Likewise, if you save changes on the \"Edit\" screen, the plugin will make changes, even if it is disabled here."; }
		}

		public override void Init(GraffitiApplication ga)
		{
		}

		protected override FormElementCollection AddFormElements()
		{
			FormElementCollection fec = new FormElementCollection();

			fec.Add(new CheckFormElement("enableNews", "Enable News", "Allows you to schedule news items, which can be displayed during a range of dates", false));

			return fec;
		}

		protected override System.Collections.Specialized.NameValueCollection DataAsNameValueCollection()
		{
			NameValueCollection nvc = new NameValueCollection();
			nvc["enableNews"] = this.EnableNews.ToString();
			return nvc;
		}

		public override StatusType SetValues(System.Web.HttpContext context, NameValueCollection nvc)
		{
			this.EnableNews = CommonFunctions.ConvertStringToBool(nvc["enableNews"]);

			if (this.EnableNews)
			{
				SetUpNews();
			}

			return StatusType.Success;
		}

		private void SetUpNews()
		{
			Category newsCategory = AddNewsCategory();
			AddNewsFields(newsCategory);
		}

		private void AddNewsFields(Category newsCategory)
		{
			bool startDateFieldExists = false;
			bool endDateFieldExists = false;
			bool priorityFieldExists = false;

			CustomFormSettings cfs = CustomFormSettings.Get(newsCategory, false);
			if (cfs.Fields != null && cfs.Fields.Count > 0)
			{
				foreach (CustomField cf in cfs.Fields)
				{
					if (!startDateFieldExists && Util.AreEqualIgnoreCase(startDateFieldName, cf.Name))
					{
						startDateFieldExists = true;
					}
					if (!endDateFieldExists && Util.AreEqualIgnoreCase(endDateFieldName, cf.Name))
					{
						endDateFieldExists = true;
					}
					if (!priorityFieldExists && Util.AreEqualIgnoreCase(priorityFieldName, cf.Name))
					{
						priorityFieldExists = true;
					}

					if (endDateFieldExists && startDateFieldExists && priorityFieldExists)
					{
						break;
					}
				}
			}

			if (!startDateFieldExists)
			{
				CustomField startDateField = new CustomField();
				startDateField.Name = startDateFieldName;
				startDateField.Description = "The first date to show the news item";
				startDateField.Enabled = true;
				startDateField.Id = Guid.NewGuid();
				startDateField.FieldType = FieldType.Date;

				cfs.Name = newsCategory.Id.ToString();
				cfs.Add(startDateField);
				cfs.Save();
			}

			if (!endDateFieldExists)
			{
				CustomField endDateField = new CustomField();
				endDateField.Name = endDateFieldName;
				endDateField.Description = "The last date to show the news item";
				endDateField.Enabled = true;
				endDateField.Id = Guid.NewGuid();
				endDateField.FieldType = FieldType.Date;

				cfs.Name = newsCategory.Id.ToString();
				cfs.Add(endDateField);
				cfs.Save();
			}

			if (!priorityFieldExists)
			{
				CustomField priorityField = new CustomField();
				priorityField.Name = priorityFieldName;
				priorityField.Description = "Priority of the news item. Higher priority displays first";
				priorityField.Enabled = true;
				priorityField.Id = Guid.NewGuid();
				priorityField.FieldType = FieldType.List;
				priorityField.ListOptions = new List<ListItemFormElement>();
				priorityField.ListOptions.Add(new ListItemFormElement("Low", "1"));
				priorityField.ListOptions.Add(new ListItemFormElement("Normal", "2", true));
				priorityField.ListOptions.Add(new ListItemFormElement("High", "3"));

				cfs.Name = newsCategory.Id.ToString();
				cfs.Add(priorityField);
				cfs.Save();
			}
		}

		private Category AddNewsCategory()
		{
			CategoryController cc = new CategoryController();
			Category newsCategory = cc.GetCachedCategory(this.newsCategoryName, true);
			if (newsCategory == null)
			{
				newsCategory = new Category();
				newsCategory.Name = this.newsCategoryName;
				newsCategory.ParentId = -1;
				newsCategory.Save();
			}

			return newsCategory;
		}
	}
}
