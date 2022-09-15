using System;
using Generic.Abp.Domain.Entities;
using JetBrains.Annotations;

namespace Generic.Abp.Identity.Roles
{
    [Serializable]
    public class RoleTranslation: Translation
    {
        public virtual string Name { get; protected set; }

        public RoleTranslation([NotNull] string language, string name) : base(language)
        {
            Name = name;
        }
    }
}
