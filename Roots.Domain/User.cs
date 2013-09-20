using Roots.Persistence.RavenDb.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Domain
{
    [KeyTypeLock]
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ConfirmationToken { get; set; }

        public object[] LastName { get; set; }

        public string FirstName { get; set; }
    }
}
