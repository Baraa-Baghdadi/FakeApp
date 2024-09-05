using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Patients
{
    public static class PatientProviderConst
    {
        private const string DefaultSorting = "{0} AddingDate desc";
        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PatientProvider." : string.Empty);
        }

    }
}
