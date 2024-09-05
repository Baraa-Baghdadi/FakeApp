using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Patients
{
    public class PatientAddressConsts
    {
        private const string DefaultSorting = "{0} CreationTime desc";
        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PatientAddress." : string.Empty);
        }
    }
}
