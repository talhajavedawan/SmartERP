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
    class zipvalidation : ValidationRule
    {
        public bool required { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex(@"^\d{5}(\-\d{4})?$");
            Match match = regex.Match(value.ToString());
            string charString = value as string;
            if (required == false && charString.Length == 0)
            {
                return ValidationResult.ValidResult;
            }
            else if (match == null || match == Match.Empty)
            {
                return new ValidationResult(false, "Invalid ZIP Code");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

    }
}
