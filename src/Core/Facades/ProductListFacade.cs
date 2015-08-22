using System;
using System.Linq;

namespace BookeanTesting.Core.Facades
{
	/// <summary>
	/// Реализует фасад над селениумом для работы со списком товаров
	/// </summary>
	public class ProductListFacade
	{
		private readonly ISeleniumDecorator _selenium;

		public ProductListFacade(ISeleniumDecorator selenium)
		{
			_selenium = selenium;
		}

		public void ChangeItemsPerPageNumber(ItemsPerPage itemsPerPage)
		{
			const string location = "//div[@class='fright items-list-options']/select";
			var currentValue = _selenium.GetSelectedValue(location);
			if (Convert.ToInt32(currentValue) == (int) itemsPerPage)
				return;

			_selenium.SelectAndWait(location, "value=" + (int) itemsPerPage, WaitingType.PageLoad);
		}

		public bool ContainsProductWithKey(string nativeCode)
		{
			return _selenium.Products.Where(o => o.WareKey == nativeCode).Count() > 0;
		}

		public bool ContainsProductWithAuthor(string authorName)
		{
			return _selenium.Products.Where(o => o.Author == authorName).Count() > 0;
		}

		public bool ContainsProductWithYear(string year)
		{
			return _selenium.Products.Where(o => o.Year == year).Count() > 0;
		}

		public bool ContainsProductWithName(string name)
		{
			return _selenium.Products.Where(o => o.Name == name).Count() > 0;
		}

		public int GetProductCount()
		{
			return _selenium.Products.Count();
		}
	}

	public enum ItemsPerPage
	{
		Ten = 10,
		Twenty = 20,
		Thirty = 30,
		Forty = 40,
		Fifty = 50
	}
}