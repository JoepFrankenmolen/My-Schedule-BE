using System.Text.RegularExpressions;

namespace My_Schedule.Shared.Helpers
{
    public static class HandlebarHelper
    {
        // Probably also possible with an extension but this also works.
        // handlebarsdotnet package
        private static readonly List<char> DefaultForbiddenChars = new List<char> { '#', '?', '/' };

        public static List<string> ExtractHandlebars(string input)
        {
            string pattern = @"\{\{\{?([^}]+)\}?\}\}";
            MatchCollection matches = Regex.Matches(input, pattern);

            var handlebars = new List<string>();
            foreach (Match match in matches)
            {
                handlebars.Add(match.Value);
            }

            return handlebars;
        }

        public static List<string> FilterHandlebars(List<string> handlebars, List<char> forbiddenChars = null)
        {
            // can also be done using a hashmap to get rid of duplicates
            if (forbiddenChars == null)
            {
                forbiddenChars = DefaultForbiddenChars;
            }

            var filteredHandlebars = new List<string>();

            foreach (string handlebar in handlebars)
            {
                // Remove handlebars that already exist in the list.
                if (filteredHandlebars.Contains(handlebar))
                {
                    continue;
                }

                // Remove handlebars with any forbidden chars.
                if (handlebar.Any(c => forbiddenChars.Contains(c)))
                {
                    continue;
                }

                filteredHandlebars.Add(handlebar);
            }

            return filteredHandlebars;
        }

        public static List<string> CleanHandlebars(List<string> handlebars)
        {
            return handlebars.Select(item => item
                .Replace("{{{", string.Empty)
                .Replace("}}}", string.Empty)
                .Replace("{{", string.Empty)
                .Replace("}}", string.Empty))
                    .ToList();
        }
    }
}