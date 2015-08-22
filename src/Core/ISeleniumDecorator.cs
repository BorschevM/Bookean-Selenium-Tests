using System.Collections.Generic;
using Selenium;

namespace BookeanTesting.Core
{
	/// <summary>
	/// ��������� ������� ����� � ���������
	/// </summary>
	public interface ISeleniumDecorator : ISelenium
	{
		/// <summary>
		/// ������ ������� �� ������� ��������
		/// </summary>
		IEnumerable<Product> Products { get; }

		/// <summary>
		/// �������� ��������� �������� � ��������� ������ ������ � ���������� �������� ��������
		/// </summary>
		/// <param name="selectLocator">XPath ���� �� ������ ������</param>
		/// <param name="optionLocator">XPath ���� �� ����������� ��������</param>
		/// <param name="waitingType">��� ��������</param>
		void SelectAndWait(string selectLocator, string optionLocator, WaitingType waitingType);

		/// <summary>
		/// ���������� ������� ������ �� ��������� �������� � ���������� �������� ��������
		/// </summary>
		/// <param name="locator">XPath ���� �� ��������</param>
		/// <param name="keySequence">ASCII ��� ������</param>
		/// <param name="waitingType">��� ��������</param>
		void KeyDownAndWait(string locator, string keySequence, WaitingType waitingType);

		/// <summary>
		/// ���������� ������� ������ �� ��������� �������� � ���������� �������� ��������
		/// </summary>
		/// <param name="locator">XPath ���� �� ��������</param>
		/// <param name="waitingType">��� ��������</param>
		void ClickAndWait(string locator, WaitingType waitingType);
	}
}