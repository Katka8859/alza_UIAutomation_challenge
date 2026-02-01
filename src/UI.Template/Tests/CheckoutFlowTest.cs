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

        if (!adminPage.ProductExists("Camera M25"))
        {
            Logger.LogInformation("Product 'Camera M25' does not exist, creating it for checkout test");
            EditProductContainer editProduct = adminPage.OpenAddProductForm();
            editProduct.FillAndSaveProduct("Camera M25", "Cameras", 50, 5, "Camera 2", "High quality RGB gaming camera with backlighting");
        }

        //** STEP 2: Add created product from AdminProductTest to basket**//
        homePage = new HomePage();
        homePage.Open();

        ProductDetailPage productDetail = homePage.OpenProductByNameFromCategory("Cameras", "Camera M25");

        productDetail.ProductInfoForm.AddToCart();

        //** STEP 3: Assert basket **//
        productDetail.Header.OpenBasketContainer();
        
        Assert.Multiple(() =>
        {
            bool productFound = productDetail.Header.GetNthProduct(1, out string productName, out string productDetailText);
            Assert.That(productFound, Is.True, "Product not found in cart");
            Assert.That(productName, Is.EqualTo("Camera M25"), "Product name in cart does not match");
            Assert.That(productDetailText.Contains("50"), Is.True, "Product price in cart does not match");
        });

        //** STEP 4: Click to checkout**//
        CheckoutPage checkoutPage = productDetail.Header.ProceedToCheckout();
        
        //** STEP 5: Fill checkout form with valid data and select payment method **//
        checkoutPage.FillCheckoutForm("Test", "User", "Test Street 123", "Prague", "11000", "test@example.com", "123456789");
        checkoutPage.SelectPayment("PayPal");

        //** STEP 6: Click to Pay button **//
        OrderConfirmationPage confirmationPage = checkoutPage.CompletePurchase();

        //** STEP 7: Confirmation of order **//
        Assert.Multiple(() =>
        {
            string totalPrice = confirmationPage.GetTotalPrice();
            Assert.That(totalPrice.Contains("$ 50.00"), Is.True, "Total price does not match");
            
            string paymentMethod = confirmationPage.GetPaymentMethod();
            Assert.That(paymentMethod.Contains("Payment Method: PayPal"), Is.True, "Payment method does not match");
        });

        Logger.LogInformation("Order completed successfully with PayPal payment");
    }
}