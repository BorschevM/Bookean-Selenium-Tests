using BookeanTesting.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookeanTesting
{
	/// <summary>
	/// Тесты "оформления заказа"
	/// </summary>
	[TestClass]
	public class OrderTest : BaseTest
	{
		[TestCleanup]
		public new void TestCleanup()
		{
			if (UserFacade.IsAuthentificated())
				UserFacade.LogOut();

			base.TestCleanup();
		}

		private void SimpleOrder(bool isOperator)
		{
			if (isOperator)
				UserFacade.LoginOperator();
			else
				UserFacade.LoginUser();

			CartFacade.AddProduct("900219191");
			CartFacade.EnterCart();

			Assert.IsTrue(Selenium.IsElementPresent("//input[@alt='Оформить заказ'][2]") == isOperator);
			//Видимость кнопки "Заказ в 1 шаг"

			OrderFacade.ProceedStep1("630091");
			OrderFacade.ProceedStep2(DevliveryType.Shop);
			OrderFacade.ProceedStep3("Joe Doe");

			Assert.IsFalse(Selenium.IsElementPresent("//input[@type='checkbox'][1]") == isOperator);
			//Видимость радио-кнопки "Сохранить параметры заказа"

			OrderFacade.ProceedStep4();

			Assert.IsTrue(OrderFacade.IsGoodOrder());
		}

		[TestMethod]
		public void UserSimpleOrderTest()
		{
			/*
			 * Для обычного пользователя оформление заказа за много шагов работает -- в конце поздравляют с успешным заказом.
			 * Недоступен заказ в 1 шаг, зато есть флажок для сохранения настроек.
			 */
			SimpleOrder(false);
		}

		[TestMethod]
		public void OperatorSimpleOrderTest()
		{
			/* 
			 * Для оператора оформление заказа  за много шагов работает -- в конце поздравляют с успешным заказом. 
			 * Доступен заказ в 1 шаг, зато нет флажка для сохранения настроек. 
			 */
			SimpleOrder(true);
		}

		[TestMethod]
		public void OperatorQuickOrderTest()
		{
			/* 
			 * Оформление заказа в 1 шаг работает -- в конце поздравляют с успешным заказом.
			 */
			UserFacade.LoginOperator();
			CartFacade.AddProduct("900219191");
			OrderFacade.ProceedQuickOrder("Joe Doe");

			Assert.IsTrue(OrderFacade.IsGoodOrder());
		}

		[TestMethod]
		public void BadIndexTest()
		{
			/* 
			 * При вводе плохого индекса на 1 шаге -- предупреждение: знак "!".
			 */
			UserFacade.LoginUser();
			CartFacade.AddProduct("900219191");

			OrderFacade.ProceedStep1("");
			Assert.IsTrue(OrderFacade.IsWrongIndex()); // Знак "!"

			OrderFacade.ProceedStep1("xxx");
			Assert.IsTrue(OrderFacade.IsWrongIndex()); // Знак "!"

			OrderFacade.ProceedStep1("000");
			Assert.IsTrue(OrderFacade.IsWrongIndex()); // Знак "!"
		}

		[TestMethod]
		public void NonExistentIndexTest()
		{
			/* 
			 * При вводе неизвестного индекса на 1 шаге -- текстовое предупреждение в диалоге.
			 */
			UserFacade.LoginOperator();
			CartFacade.AddProduct("900219191");
			OrderFacade.ProceedStep1("030091");

			Assert.IsTrue(OrderFacade.IsNonExistentIndex());
		}

		[TestMethod]
		public void OrderBackBackTest()
		{
			/* 
			 * Работает переход в обратную сторону по всем шагам визарда
			 */
			UserFacade.LoginUser();
			CartFacade.AddProduct("900219191");

			OrderFacade.ProceedStep1("630091");
			OrderFacade.ProceedStep2(DevliveryType.Shop);
			OrderFacade.ProceedStep3("John Doe");

			OrderFacade.BackToStep3();
			OrderFacade.BackToStep2();
			OrderFacade.BackToStep1();

			Assert.IsTrue(Selenium.GetValue("//input[@class='text'][@type='text']") == "630091");
		}
	}
}