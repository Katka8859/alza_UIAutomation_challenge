using OpenQA.Selenium;
using UI.Template.Components.Basic;

namespace UI.Template.Pages;

public class CheckoutPage() : BasePage("/checkout")
{
    private readonly TextInput _firstNameInput = new(By.XPath("//input[@id='firstName']"));
    private readonly TextInput _lastNameInput = new(By.XPath("//input[@id='lastName']"));
    private readonly TextInput _addressInput = new(By.XPath("//input[@id='street']"));
    private readonly TextInput _cityInput = new(By.XPath("//input[@id='city']"));
    private readonly TextInput _zipCodeInput = new(By.XPath("//input[@id='zipCode']"));
    private readonly TextInput _emailInput = new(By.XPath("//input[@id='email']"));
    private readonly TextInput _phoneNumberInput = new(By.XPath("//input[@id='phoneNumber']"));
    private readonly DropDownList _paymentMethod = new(By.XPath("//select[@id='paymentMethod']"));
    private readonly Button _payButton = new(By.XPath("//button[@ko-id='pay-button']"));


/// <summary>
/// Fills all checkout form fields with provided data.
/// </summary>
/// <param name="firstName">First name</param>
/// <param name="lastName">Last name</param>
/// <param name="address">Street address</param>
/// <param name="city">City</param>
/// <param name="postalCode">Postal/ZIP code</param>
/// <param name="email">Email address</param>
/// <param name="phone">Phone number</param>
public void FillCheckoutForm(string firstName, string lastName, string address, string city, string postalCode, string email, string phone)
{
    _firstNameInput.SendKeys(firstName);
    _lastNameInput.SendKeys(lastName);
    _addressInput.SendKeys(address);
    _cityInput.SendKeys(city);
    _zipCodeInput.SendKeys(postalCode);
    _emailInput.SendKeys(email);
    _phoneNumberInput.SendKeys(phone);
}

/// <summary>
/// Selects payment method from dropdown.
/// </summary>
/// <param name="paymentMethod">Payment method to select (e.g., "PayPal", "Cash", "Credit Card")</param>
public void SelectPayment(string paymentMethod)
{
    _paymentMethod.SelectByText(paymentMethod);
}
    /// <summary>
    /// Completes the purchase by clicking the Pay button.
    /// </summary>
    /// <returns>OrderConfirmationPage</returns>
    public OrderConfirmationPage CompletePurchase()
    {
        _payButton.Click();
        return new OrderConfirmationPage();
    }
}