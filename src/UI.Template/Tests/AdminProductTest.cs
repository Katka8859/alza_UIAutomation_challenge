using UI.Template.Components.Containers;
using UI.Template.Pages;

namespace UI.Template.Tests;

[TestFixture]
public class AdminProductTest : BaseTest
{
    [Test]
    [Order(1)]
    public void AddAndVerifyNewProductTest()
    {
        //** STEP 1: Open Admin section from home page***//
        HomePage homePage = new HomePage();
        homePage.Open();
        AdminPage adminPage = homePage.Header.OpenAdminPage();

        //** STEP 2: Open Add new product modal ***//
        EditProductContainer editProduct = adminPage.OpenAddProductForm();

        //** STEP 3: Set product informations and click save ***//
        editProduct.SetName("Camera M25");
        editProduct.SelectCategory("Cameras");
        editProduct.SetPrice(50);
        editProduct.SetStock(5);
        editProduct.SelectImage("Camera 2");
        editProduct.SetDescription("High quality RGB gaming camera with backlighting");
        editProduct.SaveChanges();

        //** STEP 4: Go back to user section - home page ***//
        homePage = new HomePage();
        homePage.Open();

        //** STEP 5: In user section go to category Caneras and open product detail ***//
        ProductDetailPage productDetail = homePage.OpenProductByNameFromCategory("Cameras", "Camera M25");

        //** STEP 6: Assert of values **//
        Assert.Multiple(() =>
        {
            string actualName = productDetail.ProductInfoForm.GetName();
            Assert.That(actualName, Is.EqualTo("Camera M25"), "Product name does not match");

            decimal actualPrice = productDetail.ProductInfoForm.GetPrice();
            Assert.That(actualPrice, Is.EqualTo(50m), "Product price does not match");

            int actualStock = productDetail.ProductInfoForm.GetStockStatus();
            Assert.That(actualStock, Is.EqualTo(5), "Product stock does not match");
        });

        Logger.LogInformation("Product 'Camera M25' was successfully added and verified");
    }
}