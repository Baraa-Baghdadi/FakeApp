using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.ProviderAuth
{
    public class VerifyCodeDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
