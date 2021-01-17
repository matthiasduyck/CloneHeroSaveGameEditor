using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CloneHeroSaveGameEditor
{
    public class DifficultyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string stringValue = value.ToString();
            if(stringValue.Equals("expert") || stringValue.Equals("hard") || stringValue.Equals("easy") || stringValue.Equals("medium"))
            {
                return new ValidationResult(true, null);
            }

            int intValue = -1;
            if (!Int32.TryParse(stringValue, out intValue))
                return new ValidationResult(false, "Value must be either 'easy', 'medium','hard' or 'expert', or an integer between 0 and 3");
            if (intValue < 0 || intValue>3)
                return new ValidationResult(false, "Value must be between 0 and 3 (inclusive)");
            return new ValidationResult(true, null);
        }
        //public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        //{
        //    string stringValue = value.ToString();
        //    int _int = -1;
        //    if (!Int32.TryParse(stringValue, out _int))
        //        return new ValidationResult(false, "Value must be an integer");
        //    if (_int < 0)
        //        return new ValidationResult(false, "Value must be positive");
        //    return new ValidationResult(true, null);
        //}
    }
}
