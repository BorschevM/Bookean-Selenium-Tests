using System;
using Selenium;

namespace BookeanTesting.Core.Facades
{
	public sealed class OrderFacade
	{
		private readonly ISeleniumDecorator _selenium;

		public OrderFacade(ISeleniumDecorator selenium)
		{
			_selenium = selenium;
		}

		public void ProceedStep1(string index)
		{
			_selenium.Open("/shoppingcart/");
			_selenium.ClickAndWait("//input[@alt='Оформить заказ'][1]", WaitingType.PageLoad);
			try
			{
				_selenium.Type("//input[@class='text'][@type='text']", index);
			}
			catch(SeleniumException)
			{
				//	NOTE: Сие сделано для workaround бага с необходимостью дважды жать кнопку Оформить заказ
				_selenium.ClickAndWait("//input[@alt='Оформить заказ'][1]", WaitingType.PageLoad);
			}
			_selenium.ClickAndWait("link=Продолжить оформление заказа", WaitingType.Ajax);
		}

		public bool IsWrongIndex()
		{
			return _selenium.IsElementPresent("//span/img");
		}

		public bool IsNonExistentIndex()
		{
			var result = _selenium.IsTextPresent("Мы не можем доставить заказ по этому адресу");
			_selenium.Click("//button[@type='button']"); //click OK
			return result;
		}

		public void ProceedStep2(DevliveryType devliveryType)
		{
			switch (devliveryType)
			{
				case DevliveryType.Shop:
					break;
				case DevliveryType.Post:
					break;
				case DevliveryType.Courier:
					break;
				default:
					throw new ArgumentOutOfRangeException("devliveryType");
			}
			_selenium.ClickAndWait("link=Продолжить оформление заказа", WaitingType.Ajax); //Next
		}

		public void ProceedStep3(string customerName)
		{
			_selenium.Type("//input[@class='text'][@type='text']", customerName); //Поле ввода ФИО получателя
			_selenium.ClickAndWait("link=Продолжить оформление заказа", WaitingType.Ajax); //Next
		}


		public void ProceedStep4()
		{
			_selenium.ClickAndWait("link=Подтвердить заказ", WaitingType.Ajax); //Next
		}

		public void ProceedQuickOrder(string customerName)
		{
			_selenium.Open("/shoppingcart/");
			_selenium.ClickAndWait("//input[@alt='Оформить заказ'][2]", WaitingType.PageLoad); //Кнопка оформить за 1 шаг
			_selenium.Type("//input[@class='text w90'][@type='text']", customerName); //Поле ввода ФИО получателя
			_selenium.ClickAndWait("link=Подтвердить заказ", WaitingType.Ajax); //Next
		}

		public bool IsGoodOrder()
		{
			return _selenium.IsTextPresent("Благодарим Вас за оформление заказа!");
		}

		public void BackToStep3()
		{
			_selenium.ClickAndWait("link=На предыдущий шаг", WaitingType.Ajax); //Back
		}

		public void BackToStep2()
		{
			_selenium.ClickAndWait("link=На предыдущий шаг", WaitingType.Ajax); //Back
		}

		public void BackToStep1()
		{
			_selenium.ClickAndWait("link=На предыдущий шаг", WaitingType.Ajax); //Back
		}
	}
}