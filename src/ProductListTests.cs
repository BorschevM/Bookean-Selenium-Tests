using System;
using BookeanTesting.Core.Facades;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookeanTesting
{
	/// <summary>
	/// Тесты проверяющие работу списка товаров (сортировку, паджинацию, навигацию и т.п. функциональность)
	/// </summary>
	[TestClass]
	public class ProductListTests : BaseTest
	{
		/// <summary>
		/// Проверяет работу переключения количества элементов на странице
		/// </summary>
		[TestMethod]
		public void SwitchItemsNumberTest()
		{
			// Список товаров Эксмо, там гарантировано больше 50 штук.
			Selenium.Open("company/5055/page/1/");

			foreach (var name in Enum.GetNames(typeof(ItemsPerPage)))
			{
				var itemsPerPage = (ItemsPerPage)Enum.Parse(typeof(ItemsPerPage), name);
				ProductListFacade.ChangeItemsPerPageNumber(itemsPerPage);
				Assert.IsTrue(ProductListFacade.GetProductCount() == (int)itemsPerPage);
			}
		}

		
	}
}