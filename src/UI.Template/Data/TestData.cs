using UI.Template.Models;

namespace UI.Template.Data;

public static class TestData
{
    public static TestProduct ParametersTestProduct { get; } = new TestProduct
    {
        ProductCategory = "Accessories",
        ProductName = "Wireless Mouse",
        ProductUrl = "/product/2"
    };

    public static TestProduct CardTestProduct { get; } = new TestProduct
    {
        ProductCategory = "Accessories",
        ProductName = "Gaming Keyboard RGB"
    };

    public static TestProduct AdminTestProduct { get; } = new TestProduct
    {
        ProductName = "Camera M25",
        ProductCategory = "Cameras",
        ProductPrice = 50,
        ProductStock = 5,
        ProductImage = "Camera 2",
        ProductDescription = "High quality RGB gaming camera with backlighting"
    };

    public static class CheckoutTestData
    {
        public const string FirstName = "Test";
        public const string LastName = "User";
        public const string Address = "Test Street 123";
        public const string City = "Prague";
        public const string PostalCode = "11000";
        public const string Email = "test@example.com";
        public const string Phone = "123456789";
        public const string PaymentMethod = "PayPal";
    }
}
