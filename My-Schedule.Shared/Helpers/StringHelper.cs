namespace My_Schedule.Shared.Helpers
{
    public static class StringHelper
    {
        public static string SanitizeFileName(this string filepath)
        {
            var invalids = Path.GetInvalidFileNameChars().ToList();
            invalids.Add(' ');
            invalids.Add(',');
            var newPath = string.Join("_", filepath.Split(invalids.ToArray(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            return newPath;
        }
    }
}