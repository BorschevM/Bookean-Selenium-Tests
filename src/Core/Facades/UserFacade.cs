using System;

namespace BookeanTesting.Core.Facades
{
	/// <summary>
	/// ��������� ����� ��� ���������� ��� �������� �������� ��������� � �������������
	/// </summary>
	public class UserFacade
	{
		private readonly ISeleniumDecorator _selenium;

		public UserFacade(ISeleniumDecorator selenium)
		{
			_selenium = selenium;
		}

		/// <summary>
		/// ������������ ���� �� ���� ��� ������� ������������
		/// </summary>
		public void LoginUser()
		{
			Login("bochkov_a@top-kniga.ru", "knigomir", LoginMethod.ButtonClick);
		}

		/// <summary>
		/// ������������ ���� �� ���� ��� ���������� ��������
		/// </summary>
		public void LoginOperator()
		{
			Login("knigomir@top-kniga.ru", "knigomir", LoginMethod.ButtonClick);
		}

		/// <summary>
		/// ������������ ����� � �����
		/// </summary>
		public void LogOut()
		{
			_selenium.ClickAndWait("link=�����", WaitingType.PageLoad);
		}

		/// <summary>
		/// ��������� ���������������� �� ������������ �� �����
		/// </summary>
		/// <returns>������, ���� ������������ ����������������, ����� ����</returns>
		public bool IsAuthentificated()
		{
			return _selenium.IsTextPresent("��� ������");
		}

		public void RegisterUser(string login, string password)
		{
			_selenium.Open("/registration/");
			_selenium.Type("//input[@class='text']", login);
			_selenium.Type("//input[@class='text'][@type='password'][1]", password);
			_selenium.Type("document.forms[0].elements[10]", password);
			_selenium.Click("id=iagree");
			_selenium.ClickAndWait("//input[@alt='������������������']", WaitingType.PageLoad);
		}

		public void Login(string login, string password, LoginMethod loginMethod)
		{
			_selenium.Open("/");
			_selenium.Click("link=����� � �������"); //Login link in header
			_selenium.Type("//div[@id='dlgContainer']/table/tbody/tr[2]/td[2]/input", login);
			_selenium.Type("//div[@id='dlgContainer']/table/tbody/tr[3]/td[2]/input", password);
			_selenium.Uncheck("//div[@id='dlgContainer']/table/tbody/tr[4]/td[2]/input"); //uncheck "remmember me check-box"

			switch (loginMethod)
			{
				case LoginMethod.ButtonClick:
					_selenium.ClickAndWait("//div[@id='dlgContainer']/table/tbody/tr[4]/td[3]/a", WaitingType.PageLoad);
					break;
				case LoginMethod.Keypress:
					_selenium.KeyDownAndWait("//div[@id='dlgContainer']/table/tbody/tr[2]/td[2]/input", "\\13", WaitingType.PageLoad);
					break;
				default:
					throw new NotImplementedException();
			}
		}
	}
}