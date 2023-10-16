namespace My_Schedule.Shared.Helpers
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// Merge 2 dictionaries together.
        /// </summary>
        /// <typeparam name="T"> type a </typeparam>
        /// <typeparam name="TA"> type b </typeparam>
        /// <param name="dict1"> dictionary 1 </param>
        /// <param name="dict2"> dictionary 2 </param>
        /// <returns> the combined ditionaries </returns>
        public static Dictionary<T, TA> MergeDictionaries<T, TA>(Dictionary<T, TA> dict1, Dictionary<T, TA> dict2)
        {
            // Creating a new dictionary so the old ones don't change.
            var dict = new Dictionary<T, TA>(dict1);
            foreach (var item in dict2)
            {
                if (!dict.ContainsKey(item.Key))
                {
                    dict.Add(item.Key, item.Value);
                }
            }
            return dict;
        }
    }
}