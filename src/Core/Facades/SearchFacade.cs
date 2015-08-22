namespace BookeanTesting.Core.Facades
{
	public class SearchFacade
	{
		private readonly ISeleniumDecorator _selenium;

		public SearchFacade(ISeleniumDecorator selenium)
		{
			_selenium = selenium;
		}

		private void AddSearchCondition(string control, string value, int controlType)
		{
			if (value != "")
			{
				if (controlType == 1)
					_selenium.Type(control, value);
				else
					_selenium.Select(control, "label=" + value);
			}
		}

		public void SimpleSearch(string queryString, WaitingType waitingType)
		{
			_selenium.Open("/books/");

			_selenium.Type("css=.search-input-html", queryString);
			_selenium.ClickAndWait("css=.search-button", waitingType);
		}

		public bool IsEmptyResult()
		{
			return _selenium.IsTextPresent("По Вашему запросу ничего не найдено. Уточните критерий поиска.");
		}

		public bool IsWrongSearchCriteria()
		{
			return _selenium.IsTextPresent("Задан слишком общий критерий для поиска");
		}

		public void AdvSearch(string name, string author, string publisher, string startYear, string endYear, WaitingType waitingType)
		{
			_selenium.Open("/search/advanced/");

			AddSearchCondition("ctl00_ctl00_ctl00_PageView_PageView_phDefault_SearchAdvancedControl_tbName", name, 1);
			AddSearchCondition("ctl00_ctl00_ctl00_PageView_PageView_phDefault_SearchAdvancedControl_tbAuthor", author, 1);
			AddSearchCondition("ctl00_ctl00_ctl00_PageView_PageView_phDefault_SearchAdvancedControl_tbPublisher", publisher, 1);
			AddSearchCondition("ctl00_ctl00_ctl00_PageView_PageView_phDefault_SearchAdvancedControl_ddlPublishedFrom", startYear, 2);
			AddSearchCondition("ctl00_ctl00_ctl00_PageView_PageView_phDefault_SearchAdvancedControl_ddlPublishedTo", endYear, 2);
			_selenium.ClickAndWait("ctl00_ctl00_ctl00_PageView_PageView_phDefault_SearchAdvancedControl_btnAdvancedSearch", waitingType);
		}
	}
}