using System.Reflection;
using System.Text.RegularExpressions;

namespace My_Schedule.Shared.Helpers.Validators
{
    public static class InputValidator
    {
        public static bool IsValidInput(string input)
        {
            // Check for common SQL injection patterns
            if (Regex.IsMatch(input, @"\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE)?|INSERT( +INTO)?|SELECT|UNION( +ALL)?|UPDATE)\b", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // Check for HTML tags and JavaScript events
            if (Regex.IsMatch(input, @"<\s*(\w+)[^>]*?on\w+\s*=|<\s*(\w+)[^>]*?>.*?\b(on\w+)\b.*?", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // Check for file path traversal attempts
            if (Regex.IsMatch(input, @"(\.\.[\\\/])+"))
            {
                return false;
            }

            // Additional custom checks can be added here

            return true;
        }

        // totaly created by me
        public static bool IsValidObjectInput(object obj, List<string> excludedProperties = null)
        {
            if (excludedProperties == null)
            {
                excludedProperties = new List<string>();
            }

            if (obj == null)
            {
                return false;
            }

            Type objectType = obj.GetType();
            PropertyInfo[] properties = objectType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string propertyName = property.Name;

                    if (excludedProperties.Contains(propertyName))
                        continue;

                    string value = (string)property.GetValue(obj);

                    if (!IsValidInput(value))
                    {
                        Console.WriteLine($"Invalid input found in property '{propertyName}': {value}");
                        return false;
                    }
                }
                else if (property.PropertyType.IsClass)
                {
                    object nestedObj = property.GetValue(obj);

                    if (!IsValidObjectInput(nestedObj, excludedProperties))
                    {
                        Console.WriteLine($"Invalid input found in nested object property '{property.Name}'");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}