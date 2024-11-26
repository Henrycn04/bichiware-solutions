using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace UnitTesting
{
    public class ClientDashboardUITest
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            this.driver = new FirefoxDriver();
        }

        /*
         * Suppose the user has a created and active account in the platform and has it's shopping cart empty.
         */
        [Test]
        public void AddToCartFromDashboardTest()
        {
            // Arrange
            string urlLogin = "http://localhost:8080/login";
            string urlCart = "http://localhost:8080/shoppingCart";
            string username = "dmorarod16@gmail.com";
            string password = "12345678";
            int amountToBuy = 1;
            string product = "";
            // Act
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlLogin);

            // Step 1: Login
            driver.FindElement(By.Id("email")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            IWebElement loginButton = driver.FindElement(By.TagName("button"));
            if (loginButton.Text == "Iniciar Sesión")
            {
                loginButton.Click();
            }

            // Step 2: Add a product to cart
            IWebElement productos = driver.FindElement(By.Id("dashboardProducts"));
            IWebElement cardProduct = productos.FindElement(By.ClassName("card"));
            product = cardProduct.FindElement(By.ClassName("card-title")).Text;
            cardProduct.FindElement(By.TagName("button")).Click();

            // Step 2.5: Specify amount to add.
            IWebElement modalBody = driver.FindElement(By.ClassName("modal-content"));
            modalBody.FindElement(By.Name("AmountProduct")).SendKeys(Keys.Backspace + Convert.ToString(amountToBuy));
            modalBody.FindElement(By.Id("modalBuyButton")).Click();

            // Step 3: Visit Cart and verify
            driver.Navigate().GoToUrl(urlCart);
            IWebElement cardItemCart = null;
            do
            {
                IReadOnlyCollection<IWebElement> cartitems = driver.FindElements(By.ClassName("card-title"));
                foreach (IWebElement item in cartitems)
                {
                    if (item.Text == product)
                    {
                        cardItemCart = item;
                    }
                }
            } while (cardItemCart == null);

            // Assert
            Assert.That(product, Is.EqualTo(cardItemCart.Text));
        }
    }
}
