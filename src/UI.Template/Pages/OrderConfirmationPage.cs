using OpenQA.Selenium;
using UI.Template.Components.Basic;

namespace UI.Template.Pages;

public class OrderConfirmationPage() : BasePage("/confirmation")
{
    private readonly Simple _totalPrice = new(By.XPath("//span[@ko-id='order-total-value']"));
    private readonly Simple _paymentMethod = new(By.XPath("//p[@ko-id='order-paymentMethod']"));

    /// <summary>
    /// Returns the total price from the confirmation page.
    /// </summary>
    /// <returns>Total price as string</returns>
    public string GetTotalPrice()
    {
        return _totalPrice.GetText();
    }

    /// <summary>
    /// Returns the payment method from the confirmation page.
    /// </summary>
    /// <returns>Payment method as string</returns>
    public string GetPaymentMethod()
    {
        return _paymentMethod.GetText();
    }
}