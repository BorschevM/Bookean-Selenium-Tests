using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;
using Selenium;

namespace BookeanTesting.Core
{
	public sealed class SeleniumDecorator : DefaultSelenium, ISeleniumDecorator
	{
		private const string Timeout = "50000";
		private readonly List<Product> _products = new List<Product>();
		private HtmlDocument _document;
		private bool _isDocumentObsolete;

		public SeleniumDecorator(string serverHost, int serverPort, string browserString, string browserUrl)
			: base(serverHost, serverPort, browserString, browserUrl)
		{
		}

		#region Implementation of ISeleniumDecorator

		/// <summary>
		/// ������� html ��������
		/// </summary>
		private HtmlDocument Document
		{
			get
			{
				if (_document == null || IsDocumentObsolete)
				{
					_document = new HtmlDocument();
					_document.LoadHtml(GetHtmlSource());
					IsDocumentObsolete = false;
				}

				return _document;
			}
		}

		/// <summary>
		/// ��������� ������������� �������� � ������� html ��������� �� ���������� xpath ����
		/// </summary>
		/// <param name="xPath">XPath ���� �� �������� ��������</param>
		/// <returns>������, ���� ������� �� ���������� ���� ����������, ����� ����</returns>
		public new bool IsElementPresent(string xPath)
		{
			return Document.DocumentNode.SelectSingleNode(xPath) != null;
		}

		/// <summary>
		/// ��������� ������������� ���������� ������ � ������� html ��������� 
		/// </summary>
		/// <param name="text">������� �����</param>
		/// <returns>������, ���� ��������� ����� ����������, ����� ����</returns>
		public new bool IsTextPresent(string text)
		{
			return Document.DocumentNode.InnerHtml.Contains(text);
		}

		///// <summary>
		/////	���������� �������� �������� �� ���������� xpath ������
		///// </summary>
		///// <param name="xPath">XPath ���� �� �������� ��������</param>
		///// <returns>��������� �������� ��������</returns>
		//public new string GetValue(string xPath)
		//{
		//    var node = Document.DocumentNode.SelectSingleNode(xPath);
		//    return node.InnerText;
		//}

		/// <summary>
		/// �������� ��������� �������� � ��������� ������ ������ � ���������� �������� ��������
		/// </summary>
		/// <param name="selectLocator">XPath ���� �� ������ ������</param>
		/// <param name="optionLocator">XPath ���� �� ����������� ��������</param>
		/// <param name="waitingType">��� ��������</param>
		public void SelectAndWait(string selectLocator, string optionLocator, WaitingType waitingType)
		{
			Select(selectLocator, optionLocator);
			switch (waitingType)
			{
				case WaitingType.PageLoad:
					WaitForPageToLoad(Timeout);
					IsDocumentObsolete = true;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// ���������� ������� �� ���������� ������
		/// </summary>
		/// <param name="uri">�����</param>
		public new void Open(string uri)
		{
			base.Open(uri);
			SuppressAllAlerts();
			IsDocumentObsolete = true;
		}

		/// <summary>
		/// ���������� ������� ������ �� ��������� �������� � ���������� �������� ��������
		/// </summary>
		/// <param name="locator">XPath ���� �� ��������</param>
		/// <param name="keySequence">ASCII ��� ������</param>
		/// <param name="waitingType">��� ��������</param>
		public void KeyDownAndWait(string locator, string keySequence, WaitingType waitingType)
		{
			switch (waitingType)
			{
				case WaitingType.PageLoad:
					KeyDown(locator, keySequence);
					WaitForPageToLoad(Timeout);
					IsDocumentObsolete = true;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// ���������� ������� ������ �� ��������� �������� � ���������� �������� ��������
		/// </summary>
		/// <param name="locator">XPath ���� �� ��������</param>
		/// <param name="waitingType">��� ��������</param>
		public void ClickAndWait(string locator, WaitingType waitingType)
		{
			switch (waitingType)
			{
				case WaitingType.None:
					Click(locator);
					break;
				case WaitingType.Ajax:
					Click(locator);
					WaitForCondition(
						"!selenium.browserbot.getCurrentWindow().Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()",
						Timeout);
					IsDocumentObsolete = true;
					break;
				case WaitingType.PageLoad:
					Click(locator);
					WaitForPageToLoad(Timeout);
					SuppressAllAlerts();
					IsDocumentObsolete = true;
					break;
				case WaitingType.SmallTimeout:
					Click(locator);
					Thread.Sleep(500);
					break;
				default:
					throw new ArgumentOutOfRangeException("waitingType");
			}
		}

		public IEnumerable<Product> Products
		{
			get { return _products.AsEnumerable(); }
		}

		/// <summary>
		/// ��������� ��� �������� ������
		/// </summary>
		private void SuppressAllAlerts()
		{
			while (IsAlertPresent())
			{
				GetAlert();
			}

			//while (IsConfirmationPresent())
			//{
			//    ChooseOkOnNextConfirmation();
			//    GetConfirmation();
			//}

			//while (IsPromptPresent())
			//{
			//    AnswerOnNextPrompt("��");
			//    GetPrompt();
			//}
		}

		#endregion

		private bool IsDocumentObsolete
		{
			get { return _isDocumentObsolete; }
			set
			{
				_isDocumentObsolete = value;
				if (value)
				{
					PopulateProductList();
				}
			}
		}

		private void PopulateProductList()
		{
			lock (_products)
			{
				var regex1 = new Regex("/(.*)/", RegexOptions.IgnoreCase);
				var regex2 = new Regex("([0-9]{4})", RegexOptions.IgnoreCase);
				var i = -1;

				_products.Clear();

				do
				{
					var locator = "//div[@class='box box-empty']/table/tbody/tr[" + (i += 2) +
								  "]/td/table/tbody/tr/td[@class='description']";

					var node = Document.DocumentNode.SelectSingleNode(locator);
					if (node == null)
						break;

					var tmpProduct = new Product();
					var tagName = Document.DocumentNode.SelectSingleNode(locator + "/div[@class='details']/a");
					var tagYear = Document.DocumentNode.SelectSingleNode(locator + "/div[@class='mt5 short']");
					tmpProduct.WareKey = regex1.Replace(tagName.Attributes["href"].Value, "").Trim();
					tmpProduct.Name = tagName.InnerText;
					if (tagYear != null)
						tmpProduct.Year = regex2.Matches(tagYear.InnerText)[0].Value;

					var authorTag = Document.DocumentNode.SelectSingleNode(locator + "/div[@class='mb2']");
					if (authorTag != null)
						tmpProduct.Author = authorTag.InnerText;

					_products.Add(tmpProduct);
				} while (true);
			}
		}
	}
}