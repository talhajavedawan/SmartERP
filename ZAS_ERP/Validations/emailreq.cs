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
    class emailreq : ValidationRule
    {
        public bool required { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
                Match match = regex.Match(value.ToString());
                string charString = value as string;
                if (required == false&&charString.Length==0)
                {
                    return ValidationResult.ValidResult;
                }
                else if (match == null || match == Match.Empty)
                {
                    return new ValidationResult(false, "Invalid Email adress");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Please enter a valid Email.");
            }
            
        }
    }
}
