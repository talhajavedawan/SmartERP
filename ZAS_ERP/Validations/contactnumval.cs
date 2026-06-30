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
    class contactnumval :ValidationRule
    {
        public bool required { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            

            Regex regex = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$");
            Match match = regex.Match(value.ToString());
            string charString = value as string;
            if (required == false && charString.Length == 0)
            {
                return ValidationResult.ValidResult;
            }
            else if (match == null || match == Match.Empty)
            {
                return new ValidationResult(false, "Please enter valid Number");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
