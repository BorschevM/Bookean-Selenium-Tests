using BookeanTesting.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookeanTesting
{
	/// <summary>
	/// Тесты поиска
	/// </summary>
	[TestClass]
	public class SearchTests : BaseTest
	{
		/// <summary>
		/// Человек должен попасть на детальную страницу, если есть только 1 результат
		/// </summary>
		[TestMethod]
		public void SearchExactNameTest()
		{
			SearchFacade.SimpleSearch("deadline", WaitingType.PageLoad);
			Assert.IsTrue(Selenium.IsTextPresent("Deadline."));
			Assert.IsTrue(Selenium.GetLocation().Contains("/books/product/25000038009"));
		}

		/// <summary>
		/// Проверка точного поиска по ISBN
		/// </summary>
		[TestMethod]
		public void SearchExactIsbnTest()
		{
			SearchFacade.SimpleSearch("5-9626-0132-7", WaitingType.PageLoad);

			Assert.IsTrue(Selenium.IsTextPresent("Deadline."));
			Assert.IsTrue(Selenium.GetLocation().Contains("/books/product/25000038009"));
		}

		/// <summary>
		/// Проверка точного поиска по ISBN без черточек
		/// </summary>
		[TestMethod]
		public void SearchExactIsbnNodashTest()
		{
			SearchFacade.SimpleSearch("5962601327", WaitingType.PageLoad);

			Assert.IsTrue(Selenium.IsTextPresent("Deadline."));
			Assert.IsTrue(Selenium.GetLocation().Contains("/books/product/25000038009"));
		}

		/// <summary>
		/// Книга с точным совпаданием наименования должна иметь максимальную релевантность
		/// </summary>
		[TestMethod]
		public void SearchRelevantForFullMatchTest()
		{
			SearchFacade.SimpleSearch("Моя первая книга", WaitingType.PageLoad);

			Assert.IsTrue(ProductListFacade.ContainsProductWithKey("25000410034")); //	Моя первая книга
			Assert.IsTrue(ProductListFacade.ContainsProductWithKey("33000001800")); //	Моя первая книга о России
		}

		/// <summary>
		/// Пустой поиск - диалог предупреждения о некорректных условиях запроса
		/// </summary>
		[TestMethod]
		public void SearchEmptyTest()
		{
			SearchFacade.SimpleSearch("", WaitingType.SmallTimeout);
			Assert.IsTrue(SearchFacade.IsWrongSearchCriteria());
		}

		/// <summary>
		/// Поиск заведомо несуществующего товара - ничего не найдено
		/// </summary>
		[TestMethod]
		public void SearchEmptyResultTest()
		{
			SearchFacade.SimpleSearch("123456", WaitingType.PageLoad);
			Assert.IsTrue(SearchFacade.IsEmptyResult());
		}

		/// <summary>
		/// Пустой поиск - диалог предупреждения о некорректных условиях запроса
		/// </summary>
		[TestMethod]
		public void AdvSearchEmptyTest()
		{
			SearchFacade.AdvSearch("", "", "", "", "", WaitingType.SmallTimeout);
			Assert.IsTrue(SearchFacade.IsWrongSearchCriteria());
		}

		/// <summary>
		/// Начальный год больше конечного года - пустой результат
		/// </summary>
		[TestMethod]
		public void AdvSearchLastYearGreaterFirstYearTest()
		{
			SearchFacade.AdvSearch("Хессайон", "", "", "2005", "2004", WaitingType.PageLoad);
			Assert.IsTrue(SearchFacade.IsEmptyResult());
		}

		/// <summary>
		/// Начальный и конечный годы равны, автор задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchYearsEqualTest()
		{
			SearchFacade.AdvSearch("", "Хессайон", "", "2006", "2006", WaitingType.PageLoad);
			Assert.IsTrue(ProductListFacade.ContainsProductWithAuthor("Хессайон Д. Г.")); // На странице есть товар этого автора
			Assert.IsTrue(ProductListFacade.ContainsProductWithYear("2006")); // только издания 2005 года
			Assert.IsFalse(ProductListFacade.ContainsProductWithYear("2005"));
			Assert.IsFalse(ProductListFacade.ContainsProductWithYear("2007"));
		}

		/// <summary>
		/// Начальный год, автор задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchStartYearSetTest()
		{
			SearchFacade.AdvSearch("", "Хессайон", "", "2005", "", WaitingType.PageLoad);
			Assert.IsTrue(ProductListFacade.ContainsProductWithAuthor("Хессайон Д. Г.")); // На странице есть товар этого автора
			foreach (var product in Selenium.Products)
			{
				int year;
				if (int.TryParse(product.Year, out year))
				{
					Assert.IsTrue(year >= 2005);
				}
			}
		}

		/// <summary>
		/// Конечный год, автор задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchEndYearSetTest()
		{
			SearchFacade.AdvSearch("", "Хессайон", "", "", "2005", WaitingType.PageLoad);
			Assert.IsTrue(ProductListFacade.ContainsProductWithAuthor("Хессайон Д. Г.")); // На странице есть товар этого автора
			Assert.IsTrue(ProductListFacade.ContainsProductWithYear("2005")); // только издания 2005 года и старее
			Assert.IsTrue(ProductListFacade.ContainsProductWithYear("2003"));
			Assert.IsFalse(ProductListFacade.ContainsProductWithYear("2007"));
		}

		/// <summary>
		/// Наименование задано - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchNameTest()
		{
			SearchFacade.AdvSearch("Все о розах", "", "", "", "", WaitingType.PageLoad);

			Assert.IsTrue(ProductListFacade.ContainsProductWithName("Все о розах: Исчерпывающее руководство по выращиванию и уходу за розами"));
			Assert.IsTrue(ProductListFacade.ContainsProductWithName("Все о розах"));
			Assert.IsTrue(ProductListFacade.ContainsProductWithAuthor("Сладкова О. В."));
			Assert.IsTrue(ProductListFacade.ContainsProductWithAuthor("Хессайон Д. Г."));
		}

		/// <summary>
		/// Наименование и автор заданы - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchNameAuthorTest()
		{
			SearchFacade.AdvSearch("Все о розах", "Хессайон", "", "", "", WaitingType.PageLoad);
			Assert.IsTrue(ProductListFacade.ContainsProductWithName("Все о розах: Исчерпывающее руководство по выращиванию и уходу за розами"));
			Assert.IsFalse(ProductListFacade.ContainsProductWithAuthor("Сладкова О. В."));
			Assert.IsTrue(ProductListFacade.ContainsProductWithAuthor("Хессайон Д. Г."));
		}

		/// <summary>
		/// Издатель задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchPublisherTest()
		{
			SearchFacade.AdvSearch("Неприхотливый сад", "", "Кладезь-Букс", "", "", WaitingType.PageLoad);
			Assert.IsTrue(Selenium.IsTextPresent("Неприхотливый сад"));
			Assert.IsTrue(Selenium.IsTextPresent("Кладезь-Букс"));
		}

		/// <summary>
		/// Издатель и Автор задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchPublisherAuthorTest()
		{
			SearchFacade.AdvSearch("", "Хессайон", "Кладезь-Букс", "", "", WaitingType.PageLoad);
			Assert.IsTrue(ProductListFacade.ContainsProductWithKey("54000028527"));
			Assert.IsFalse(ProductListFacade.ContainsProductWithKey("911007682"));
		}

		/// <summary>
		/// Название, Издатель и Автор задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchPublisherAuthorNameTest()
		{
			SearchFacade.AdvSearch("Все о розах", "Хессайон", "Кладезь-Букс", "", "", WaitingType.PageLoad);
			Assert.IsTrue(ProductListFacade.ContainsProductWithName("Все о розах: Исчерпывающее руководство по выращиванию и уходу за розами"));
			Assert.IsTrue(ProductListFacade.ContainsProductWithName("Все о розах: Исчерпывающее руководство по выращиванию и уходу за розами (пер. с англ. Романовой О.И.)"));
			Assert.IsFalse(ProductListFacade.ContainsProductWithName("Все об орхидеях"));
		}

		/// <summary>
		/// Название, Издатель задан - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchPublisherNameTest()
		{
			SearchFacade.AdvSearch("Все о розах", "", "Олма-Пресс", "", "", WaitingType.PageLoad);
			Assert.IsTrue(Selenium.GetLocation().Contains("/books/product/930527209"));
		}

		/// <summary>
		/// Издательство с символом в наименовании - ненулевой результат, правильная фильтрация
		/// </summary>
		[TestMethod]
		public void AdvSearchFitonPlusTest()
		{
			SearchFacade.AdvSearch("Удивительный мир жуков", "", "Фитон+", "", "", WaitingType.PageLoad);
			Assert.IsTrue(Selenium.IsTextPresent("Удивительный мир жуков"));
			Assert.IsTrue(Selenium.GetLocation().Contains("/books/product/930921014"));
		}
	}
}