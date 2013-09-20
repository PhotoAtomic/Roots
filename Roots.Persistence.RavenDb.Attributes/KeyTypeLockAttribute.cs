using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb.Attributes
{
    /// <summary>
    /// in case of a hierarcky of classes in the model, together withthe rule SetupKeyTypeLockRule in Roots.Persistence.RavenDb.RavenDbUnitOfWorkFactory 
    /// makes the derived types to generate keys with the name of the first base class that has this attribute on
    /// for example class Base with [KeyTypeLock] on  and class Derived : Base, makes Derived Keys have key prefix Base\
    /// </summary>
    [AttributeUsage (AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class KeyTypeLockAttribute : Attribute
    {
    }
}
