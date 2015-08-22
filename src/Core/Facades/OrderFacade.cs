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
			_selenium.ClickAndWait("//input[@alt='�������� �����'][1]", WaitingType.PageLoad);
			try
			{
				_selenium.Type("//input[@class='text'][@type='text']", index);
			}
			catch(SeleniumException)
			{
				//	NOTE: ��� ������� ��� workaround ���� � �������������� ������ ���� ������ �������� �����
				_selenium.ClickAndWait("//input[@alt='�������� �����'][1]", WaitingType.PageLoad);
			}
			_selenium.ClickAndWait("link=���������� ���������� ������", WaitingType.Ajax);
		}

		public bool IsWrongIndex()
		{
			return _selenium.IsElementPresent("//span/img");
		}

		public bool IsNonExistentIndex()
		{
			var result = _selenium.IsTextPresent("�� �� ����� ��������� ����� �� ����� ������");
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
			_selenium.ClickAndWait("link=���������� ���������� ������", WaitingType.Ajax); //Next
		}

		public void ProceedStep3(string customerName)
		{
			_selenium.Type("//input[@class='text'][@type='text']", customerName); //���� ����� ��� ����������
			_selenium.ClickAndWait("link=���������� ���������� ������", WaitingType.Ajax); //Next
		}


		public void ProceedStep4()
		{
			_selenium.ClickAndWait("link=����������� �����", WaitingType.Ajax); //Next
		}

		public void ProceedQuickOrder(string customerName)
		{
			_selenium.Open("/shoppingcart/");
			_selenium.ClickAndWait("//input[@alt='�������� �����'][2]", WaitingType.PageLoad); //������ �������� �� 1 ���
			_selenium.Type("//input[@class='text w90'][@type='text']", customerName); //���� ����� ��� ����������
			_selenium.ClickAndWait("link=����������� �����", WaitingType.Ajax); //Next
		}

		public bool IsGoodOrder()
		{
			return _selenium.IsTextPresent("���������� ��� �� ���������� ������!");
		}

		public void BackToStep3()
		{
			_selenium.ClickAndWait("link=�� ���������� ���", WaitingType.Ajax); //Back
		}

		public void BackToStep2()
		{
			_selenium.ClickAndWait("link=�� ���������� ���", WaitingType.Ajax); //Back
		}

		public void BackToStep1()
		{
			_selenium.ClickAndWait("link=�� ���������� ���", WaitingType.Ajax); //Back
		}
	}
}