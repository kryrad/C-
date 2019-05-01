using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab01
{
    public class NumberValidation : ValidationRule
    {
        public NumberValidation()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number=0;
            try
            {
                if (((string)value).Length > 0)
                    number = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "To nie jest cyfra");
            }
            if (number < 0)
                return new ValidationResult(false, $"Tylko liczby dodatnie");
            return new ValidationResult(true, null);
        }
    }
}