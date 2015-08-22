using System;
using BookeanTesting.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookeanTesting
{
	[TestClass]
	public class CredentialsTest : BaseTest
	{
		[TestCleanup]
		public new void TestCleanup()
		{
			if (UserFacade.IsAuthentificated())
				UserFacade.LogOut();
			base.TestCleanup();
		}

		[TestInitialize]
		public new void TestInitialize()
		{
			base.TestInitialize();
			if (UserFacade.IsAuthentificated())
				UserFacade.LogOut();
		}


		[TestMethod]
		public void RegistrationTest()
		{
			UserFacade.RegisterUser("user_" + DateTime.Now.Millisecond + "@top-kniga.ru", "123456");
			Assert.IsTrue(UserFacade.IsAuthentificated());
		}


		[TestMethod]
		public void LoginWithButtonTest()
		{
			UserFacade.Login("borschev_m@top-kniga.ru", "knigomir", LoginMethod.ButtonClick);
			Assert.IsTrue(UserFacade.IsAuthentificated());
		}

		[TestMethod]
		public void LoginWithKeypressTest()
		{
			UserFacade.Login("borschev_m@top-kniga.ru", "knigomir", LoginMethod.Keypress);
			Assert.IsTrue(UserFacade.IsAuthentificated());
		}


		[TestMethod]
		public void LogoutTest()
		{
			UserFacade.LoginUser();
			Assert.IsTrue(UserFacade.IsAuthentificated());
			UserFacade.LogOut();
			Assert.IsFalse(UserFacade.IsAuthentificated());
		}
	}
}