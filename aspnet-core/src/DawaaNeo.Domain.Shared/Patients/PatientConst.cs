using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Patients
{
    public static class PatientConst
    {
        private const string DefaultSorting = "{0} MobileNumber asc";
        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Patient." : string.Empty);
        }

        public const int MobileNumberMinLength = 5;
        public const int MobileNumberMaxLength = 20;
        public const int CountryCodeMinLength = 1;
        public const int CountryCodeMaxLength = 6;
    }
}
