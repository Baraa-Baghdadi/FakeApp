using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Orders
{
    public static class OrderConst
    {
        private const string DefaultSorting = "{0} CreationTime asc";
        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Order." : string.Empty);
        }

        public const string OrderCreationTitle = "OrderCreationTitle";
        public const string OrderCreationContent = "OrderCreationContent";
    }
}
