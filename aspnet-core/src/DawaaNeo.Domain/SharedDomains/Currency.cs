using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace DawaaNeo.SharedDomains
{
    public class Currency : Entity<Guid>
    {
        [Required]
        public string? Code { get; private set; }

        [NotNull]
        public int? Decimals { get; private set; }

        [NotNull]
        public string? Name { get; private set; }

        public Currency()
        {
            
        }

        private Currency(string code, int decimals, string name)
        {
            Code = code;
            Decimals = decimals;
            Name = name;
        }

        public static Currency Create(string code, int decimals, string name)
        {
            return new Currency(code, decimals, name);
        }

    }
}
