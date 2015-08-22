using System;
using System.Diagnostics;
using System.Text;
using BookeanTesting.Core;
using BookeanTesting.Core.Facades;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookeanTesting
{
	[TestClass]
	public abstract class BaseTest
	{
		private StringBuilder _verificationErrors;

		protected ISeleniumDecorator Selenium { get; private set; }

		protected ProductListFacade ProductListFacade { get; private set; }

		protected UserFacade UserFacade { get; private set; }

		protected CartFacade CartFacade { get; private set; }

		protected OrderFacade OrderFacade { get; private set; }

		protected SearchFacade SearchFacade { get; private set; }


		// Use TestInitialize to run code before running each test 
		[TestInitialize]
		public void TestInitialize()
		{
			//*firefoxchrome 
			//*iexplore
			//*opera
			//*safari
			//*googlechrome
			Selenium = new SeleniumDecorator("localhost", 4444, @"*iexplore", "http://bookean-dev/");
			//_selenium = new DefaultSelenium("localhost", 4444, @"*firefoxJ:\Program Files\Mozilla Firefox\firefox.exe", "http://bookean-dev/");
			Selenium.Start();
			Selenium.SetTimeout("50000");
			ProductListFacade = new ProductListFacade(Selenium);
			UserFacade = new UserFacade(Selenium);
			CartFacade = new CartFacade(Selenium);
			OrderFacade = new OrderFacade(Selenium);
			SearchFacade = new SearchFacade(Selenium);
			_verificationErrors = new StringBuilder();
		}

		//
		// Use TestCleanup to run code after each test has run
		[TestCleanup]
		public void TestCleanup()
		{
			try
			{
				Selenium.Stop();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			Assert.AreEqual("", _verificationErrors.ToString());
		}

		
	}
}