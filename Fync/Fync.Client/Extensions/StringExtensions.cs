using System.IO;

namespace Fync.Client.Extensions
{
    internal static class StringExtensions
    {
        public static string Slash(this string s, params string[] strings)
        {
            return Path.Combine(s, Path.Combine(strings));
        }
    }
}
