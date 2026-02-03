using UI.Template.Data;
using UI.Template.Pages;
using UI.Template.Components.Containers;

namespace UI.Template.Tests;

[TestFixture]
public class CheckoutFlowTest : BaseTest
{
    [Test]
    public void CheckoutAndPurchaseTest()
    {
        //** STEP 1: Ensure test product exists **//
        HomePage homePage = new HomePage();
        homePage.Open();
        AdminPage adminPage = homePage.Header.OpenAdminPage();

        if (!adminPage.ProductExists(TestData.AdminTestProduct.ProductName))
        {
            Logger.LogInformation($"Product '{TestData.AdminTestProduct.ProductName}' does not exist, creating it for checkout test");
            EditProductContainer editProduct = adminPage.OpenAddProductForm();
            editProduct.FillAndSaveProduct(
                TestData.AdminTestProduct.ProductName,
                TestData.AdminTestProduct.ProductCategory,
                TestData.AdminTestProduct.ProductPrice,
                TestData.AdminTestProduct.ProductStock,
                TestData.AdminTestProduct.ProductImage,
                TestData.AdminTestProduct.ProductDescription);
        }

        //** STEP 2: Add created product to basket**//
        homePage = new HomePage();
        homePage.Open();

        ProductDetailPage productDetail = homePage.OpenProductByNameFromCategory(
            TestData.AdminTestProduct.ProductCategory,
            TestData.AdminTestProduct.ProductName);

        productDetail.ProductInfoForm.AddToCart();

        //** STEP 3: Assert basket **//
        productDetail.Header.OpenBasketContainer();
        
        Assert.Multiple(() =>
        {
            bool productFound = productDetail.Header.GetNthProduct(1, out string productName, out string productDetailText);
            Assert.That(productFound, Is.True, "Product not found in cart");
            Assert.That(productName, Is.EqualTo(TestData.AdminTestProduct.ProductName), "Product name in cart does not match");
            Assert.That(productDetailText.Contains(TestData.AdminTestProduct.ProductPrice.ToString()), Is.True, "Product price in cart does not match");
        });

        //** STEP 4: Click to checkout**//
        CheckoutPage checkoutPage = productDetail.Header.ProceedToCheckout();
        
        //** STEP 5: Fill checkout form with valid data and select payment method **//
        checkoutPage.FillCheckoutForm(
            TestData.CheckoutTestData.FirstName,
            TestData.CheckoutTestData.LastName,
            TestData.CheckoutTestData.Address,
            TestData.CheckoutTestData.City,
            TestData.CheckoutTestData.PostalCode,
            TestData.CheckoutTestData.Email,
            TestData.CheckoutTestData.Phone);
        checkoutPage.SelectPayment(TestData.CheckoutTestData.PaymentMethod);

        //** STEP 6: Click to Pay button **//
        OrderConfirmationPage confirmationPage = checkoutPage.CompletePurchase();

        //** STEP 7: Confirmation of order **//
        Assert.Multiple(() =>
        {
            string totalPrice = confirmationPage.GetTotalPrice();
            Assert.That(totalPrice.Contains("$ 50.00"), Is.True, "Total price does not match");
            
            string paymentMethod = confirmationPage.GetPaymentMethod();
            Assert.That(paymentMethod.Contains($"Payment Method: {TestData.CheckoutTestData.PaymentMethod}"), Is.True, "Payment method does not match");
        });

        Logger.LogInformation($"Order completed successfully with {TestData.CheckoutTestData.PaymentMethod} payment");
    }
}