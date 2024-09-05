using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Providers
{
    public class ProviderConst
    {
        private const string DefaultSorting = "{0}Email asc";
        public static string GetDeafultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Provider." : string.Empty);
        }
        public const int AddressMaxLength = 50;
        public const int AddressMinLength = 1;
    }
}
