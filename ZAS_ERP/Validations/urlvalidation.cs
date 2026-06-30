using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace ZAS_ERP.Validations
{
    public class urlvalidation :ValidationRule
    {
        public bool required { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex(@"^(http(s)?://)?([\da-zA-Z\.-]+)\.([a-zA-Z0-9\.]{2,6})([\/\w \.-]*)*\/?$");
            Match match = regex.Match(value.ToString());
            string charString = value as string;
            if (required == false && charString.Length == 0)
            {
                return ValidationResult.ValidResult;
            }
            else if (match == null || match == Match.Empty)
            {
                return new ValidationResult(false, "Invalid URL");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

    }
}
