using System.Globalization;

namespace BmsKhameleon.UI.Utilities
{
    public static class DecimalExtensions
    {
        public static string ToCurrencyString(this decimal amount)
        {
            return amount.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}
