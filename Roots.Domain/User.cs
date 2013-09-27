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
        public string UserName { get; set; }
        public Guid ConfirmationToken { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public ICollection<Login> Logins { get; set; }
    }
}
