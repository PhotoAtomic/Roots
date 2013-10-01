using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Domain
{
    public class SecurityToken
    {
        public Guid Id { get; set; }

        public DateTime UtcExpireDate { get; set; }

        public string Value { get; set; }
    }
}
