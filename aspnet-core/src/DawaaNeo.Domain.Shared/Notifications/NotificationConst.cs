using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Notifications
{
    public static class NotificationConst
    {
        private const string DefaultSorting = "{0} CreationTime asc";
        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Notification." : string.Empty);
        }

    }
}
