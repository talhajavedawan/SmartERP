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
    class RequiredMinMaxValidation : ValidationRule
    {
        public int MinimumCharacters { get; set; }
        public bool required { get; set; }

        public int maxCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            string charString = value as string;
            
            if (required == true && charString.Length == 0)
            {
                return new ValidationResult(false, $"Required");
            }
            else if (required == false && MinimumCharacters==0&& maxCharacters==0)
            {
                return ValidationResult.ValidResult;
            }
            else    if (required == false && charString.Length==0)
            {
                return ValidationResult.ValidResult;
            }
            


            else if (charString.Length < MinimumCharacters)
            {

                return new ValidationResult(false, $"Shall have atleast {MinimumCharacters} characters.");
        }

            else if (charString.Length>maxCharacters && maxCharacters!=0)
            {
                return new ValidationResult(false, $"Can have Atmost {maxCharacters} characters");
            }
            else if (required == false && MinimumCharacters > charString.Length && charString.Length < maxCharacters)
            {
                return ValidationResult.ValidResult;
            }
            

            else
            {
                return ValidationResult.ValidResult;
            }

        }

    }
}
