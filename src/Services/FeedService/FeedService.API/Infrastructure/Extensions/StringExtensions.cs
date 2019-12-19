using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedService.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string word)
        {
            var words = word.Split(' ');
            var newWords = words.Select(w => w.Substring(0, 1).ToUpper() + w.Substring(1).ToLower()).ToList();

            var sb = new StringBuilder();

            foreach (var nw in newWords)
            {
                sb.Append(nw);
                sb.Append(" ");
            }

            sb.Length--;

            return sb.ToString();
        }
    }
}