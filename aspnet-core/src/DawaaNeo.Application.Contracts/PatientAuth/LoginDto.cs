using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.PatientAuth
{
    public class LoginDto
    {
        public string? MobileNumber { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
