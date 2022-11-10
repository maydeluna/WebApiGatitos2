using System.ComponentModel.DataAnnotations;

namespace WebApiGatos.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primerLetra = value.ToString()[0].ToString();

            if(primerLetra != primerLetra.ToUpper())
            {
                return new ValidationResult("Ingrese primer letra en mayuscula");
            }

            return ValidationResult.Success;
        }
    }
}
