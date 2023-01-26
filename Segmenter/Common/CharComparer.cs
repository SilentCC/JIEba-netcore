using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segmenter.Common
{
    public class CharComparer : IEqualityComparer<char>
    {
        public static CharComparer IgnoreCase { get; } = new CharComparer();

        public bool Equals(char x, char y)
        {
            return char.IsLetter(x) && char.IsLetter(y) ?
                char.ToLowerInvariant(x) == char.ToLowerInvariant(y)
                : x.Equals(y);
        }

        public int GetHashCode([DisallowNull] char obj)
        {
            return char.IsLetter(obj) ?
                char.ToLowerInvariant(obj).GetHashCode() :
                obj.GetHashCode();
        }
    }
}
