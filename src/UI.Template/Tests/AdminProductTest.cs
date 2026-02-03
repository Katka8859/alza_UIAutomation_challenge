using UI.Template.Components.Containers;
using UI.Template.Data;
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
        if (adminPage.ProductExists(TestData.AdminTestProduct.ProductName))
        {
            Logger.LogInformation($"Product '{TestData.AdminTestProduct.ProductName}' already exists, resetting application");
            adminPage.ResetApplication();
        }

        //** STEP 3: Open Add new product modal ***//
        EditProductContainer editProduct = adminPage.OpenAddProductForm();

        //** STEP 4: Set product informations and click save ***//
        editProduct.FillAndSaveProduct(
            TestData.AdminTestProduct.ProductName,
            TestData.AdminTestProduct.ProductCategory,
            TestData.AdminTestProduct.ProductPrice,
            TestData.AdminTestProduct.ProductStock,
            TestData.AdminTestProduct.ProductImage,
            TestData.AdminTestProduct.ProductDescription);

        //** STEP 5: Go back to user section - home page ***//
        homePage = new HomePage();
        homePage.Open();

        //** STEP 6: In user section go to category Cameras and open product detail ***//
        ProductDetailPage productDetail = homePage.OpenProductByNameFromCategory(
            TestData.AdminTestProduct.ProductCategory,
            TestData.AdminTestProduct.ProductName);

        //** STEP 7: Assert of values **//
        Assert.Multiple(() =>
        {
            string actualName = productDetail.ProductInfoForm.GetName();
            Assert.That(actualName, Is.EqualTo(TestData.AdminTestProduct.ProductName), "Product name does not match");

            decimal actualPrice = productDetail.ProductInfoForm.GetPrice();
            Assert.That(actualPrice, Is.EqualTo((decimal)TestData.AdminTestProduct.ProductPrice), "Product price does not match");

            int actualStock = productDetail.ProductInfoForm.GetStockStatus();
            Assert.That(actualStock, Is.EqualTo(TestData.AdminTestProduct.ProductStock), "Product stock does not match");
        });

        Logger.LogInformation($"Product '{TestData.AdminTestProduct.ProductName}' was successfully added and verified");
    }
}