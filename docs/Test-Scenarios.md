# Test Scenarios

### Content:
- [Introduction](Introduction.md)
- [Prerequisites](Prerequisites.md)
- [Guidelines](Guidelines.md)
- [Running Tests](Running-Tests.md)
- Test Scenarios
---

This document describes the newly added test scenarios and changes made to the existing test suite.

---

## New Tests

#### TC3 - Admin Product Creation
**Class:** `AdminProductTest` | **Method:** `CreateProductInAdmin`

Verifies the full flow of creating a new product through the admin panel and confirming it is visible in the e-shop with correct values.

**Steps:**
1. Open the admin panel via the home page header (clicking the shop title)
2. Check if the test product "Camera M25" already exists; if so, reset the application to ensure a clean state
3. Open the Add Product form
4. Fill in all product fields (name, category, price, stock, image, description) and save
5. Navigate back to the home page
6. Open the product detail page from the Cameras category
7. Assert that the product name, price, and stock match the expected values

**Test Data:**

| Field       | Value                                            |
|-------------|--------------------------------------------------|
| Name        | Camera M25                                       |
| Category    | Cameras                                          |
| Price       | 50                                               |
| Stock       | 5                                                |
| Image       | Camera 2                                         |
| Description | High quality RGB gaming camera with backlighting |

---

#### TC4 - Checkout Flow
**Class:** `CheckoutFlowTest` | **Method:** `CheckoutAndPurchaseTest`

Verifies the complete checkout flow from adding a product to the cart through to order confirmation. This test is designed to be fully independent — it checks whether the required product exists and creates it if necessary, so it can run standalone without relying on TC3.

**Steps:**
1. Open the admin panel and check if "Camera M25" exists; create it if it does not
2. Navigate to the home page and open the product detail page from the Cameras category
3. Add the product to the cart
4. Open the basket and verify that the product name and price are correct
5. Proceed to the checkout page
6. Fill in the checkout form with valid customer and delivery data
7. Select PayPal as the payment method
8. Click the Pay button to complete the purchase
9. Verify the order confirmation page displays the correct total price and payment method

**Test Data:**

| Field       | Value            |
|-------------|------------------|
| First Name  | Test             |
| Last Name   | User             |
| Address     | Test Street 123  |
| City        | Prague           |
| Postal Code | 11000            |
| Email       | test@example.com |
| Phone       | 123456789        |
| Payment     | PayPal           |

---

## Changes Made

#### New Page Objects
- **`CheckoutPage.cs`** — Page object for the checkout page. Provides methods for filling the checkout form (`FillCheckoutForm`), selecting a payment method (`SelectPayment`), and completing the purchase (`CompletePurchase`).
- **`OrderConfirmationPage.cs`** — Page object for the order confirmation page. Provides methods to retrieve the total price (`GetTotalPrice`) and payment method (`GetPaymentMethod`).

#### New Methods in Existing Classes
- **`EditProductContainer.FillAndSaveProduct()`** — Fills all product form fields and saves the product in a single method call, replacing multiple individual setter calls.
- **`AdminPage.ProductExists()`** — Checks whether a product with the given name exists in the admin product grid.
- **`AdminPage.ResetApplication()`** — Resets the application state by clicking the Reset Application button.
- **`AdminPage.OpenAddProductForm()`** — Opens the Add Product modal and waits for it to be displayed.
- **`HeaderContainer.ProceedToCheckout()`** — Opens the basket if not already open and navigates to the checkout page.

#### BaseTest Changes
- **Authentication** — Added HTTP Basic Auth handling in `ExecuteBeforeTestStart()`. Credentials are injected into the URL before each test starts, ensuring reliable authentication across all test classes.
- **Driver initialization order** — `InitializeDriver()` is now called before `ExecuteBeforeTestStart()` in `BaseSetUp()` to ensure the WebDriver session exists before authentication runs.
- **Driver lifecycle** — `CloseDriver()` was moved from `[TearDown]` to `[OneTimeTearDown]` so that the browser session persists across all tests within the same test class.

#### WebConfiguration Changes
- **Environment variables support** — `UserName` and `UserPassword` are now loaded from environment variables (`ESHOP_USERNAME`, `ESHOP_PASSWORD`) first, falling back to `appsettings.json` values. This enables credential management in CI/CD pipelines via GitHub Actions secrets.

#### Test Data 
- All test data for TC3 and TC4 is in `TestData.cs`. Product data is stored in `TestData.AdminTestProduct`, checkout form data in `TestData.CheckoutTestData`.
- `TestProduct.cs` model was extended with new properties: `ProductPrice`, `ProductStock`, `ProductImage`, `ProductDescription`.

---

## Known Issues

#### TC4 - Application Bug: "The following products cannot be bought"
TC4 (`CheckoutAndPurchaseTest`) currently fails at the payment step with the following alert from the application:

> The following products cannot be bought: Camera M25

The product is successfully created in the admin panel and added to the cart, but the application rejects the purchase at checkout. This appears to be a bug on the application side, as all product data is correctly filled in and the product is available in the e-shop. The issue is being investigated.


> **Note:** The pre-existing tests (`CartTest`, `ProductParametersTest`) are currently failing when run on the remote server via GitHub Actions, but pass when run locally. Fixing these tests to work in the remote environment is a TODO and is not in scope of this challenge.

