using System.Collections.Generic;
using Selenium;

namespace BookeanTesting.Core
{
	/// <summary>
	/// Описывает базовый фасад к селениуму
	/// </summary>
	public interface ISeleniumDecorator : ISelenium
	{
		/// <summary>
		/// Список товаров на текущей странице
		/// </summary>
		IEnumerable<Product> Products { get; }

		/// <summary>
		/// Выбирает указанное значение в указанном списке выбора и дожидается загрузки страницы
		/// </summary>
		/// <param name="selectLocator">XPath путь до списка выбора</param>
		/// <param name="optionLocator">XPath путь до выбираемого значения</param>
		/// <param name="waitingType">Тип ожидания</param>
		void SelectAndWait(string selectLocator, string optionLocator, WaitingType waitingType);

		/// <summary>
		/// Производит нажатие кнопки на указанном элементе и дожидается загрузки страницы
		/// </summary>
		/// <param name="locator">XPath путь до элемента</param>
		/// <param name="keySequence">ASCII код кнопки</param>
		/// <param name="waitingType">Тип ожидания</param>
		void KeyDownAndWait(string locator, string keySequence, WaitingType waitingType);

		/// <summary>
		/// Производит нажатие мышкой на указанном элементе и дожидается загрузки страницы
		/// </summary>
		/// <param name="locator">XPath путь до элемента</param>
		/// <param name="waitingType">Тип ожидания</param>
		void ClickAndWait(string locator, WaitingType waitingType);
	}
}