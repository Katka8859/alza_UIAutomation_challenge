using UI.Template.Components.Containers;
using UI.Template.Pages;

namespace UI.Template.Tests;

[TestFixture]
public class AdminProductTest : BaseTest
{
    [Test]
    public void CreateProductInAdmin()
    {
        //** STEP 1: Open Admin section from home page***//
        HomePage homePage = new HomePage();
        homePage.Open();
        AdminPage adminPage = homePage.Header.OpenAdminPage();

        //** STEP 2: Check if the product already exists and reset app if necessary **//
        if (adminPage.ProductExists("Camera M25"))
        {
            Logger.LogInformation("Product 'Camera M25' already exists, resetting application");
            adminPage.ResetApplication();
        }

        //** STEP 3: Open Add new product modal ***//
        EditProductContainer editProduct = adminPage.OpenAddProductForm();

        //** STEP 4: Set product informations and click save ***//
        editProduct.FillAndSaveProduct("Camera M25", "Cameras", 50, 5, "Camera 2", "High quality RGB gaming camera with backlighting");


        //** STEP 5: Go back to user section - home page ***//
        homePage = new HomePage();
        homePage.Open();

        //** STEP 6: In user section go to category Caneras and open product detail ***//
        ProductDetailPage productDetail = homePage.OpenProductByNameFromCategory("Cameras", "Camera M25");

        //** STEP 7: Assert of values **//
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