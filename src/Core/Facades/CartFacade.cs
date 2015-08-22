namespace BookeanTesting.Core.Facades
{
	public sealed class CartFacade
	{
		private readonly ISeleniumDecorator _selenium;

		public CartFacade(ISeleniumDecorator selenium)
		{
			_selenium = selenium;
		}

		public void AddProduct(string wareKey)
		{
			_selenium.Open("/books/product/" + wareKey);
			_selenium.ClickAndWait("css=a.button", WaitingType.Ajax);
		}

		public void EnterCart()
		{
			_selenium.Open("/shoppingcart/");
		}
	}
}