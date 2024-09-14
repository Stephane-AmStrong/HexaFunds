using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Services.Utils;

public class CustomValidator
{
    public static Dictionary<string, string> ValidateModel(object model)
    {
        var validationResults = new Dictionary<string, string>();

        var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(model);

            // Obtient tous les attributs de validation appliqués à la propriété
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();

            foreach (var attribute in validationAttributes)
            {
                if (!attribute.IsValid(value))
                {
                    validationResults.Add(property.Name, attribute.FormatErrorMessage(property.Name));
                }
            }
        }

        return validationResults;
    }
}
