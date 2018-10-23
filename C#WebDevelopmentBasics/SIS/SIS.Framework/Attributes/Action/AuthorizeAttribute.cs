namespace SIS.Framework.Attributes.Action
{
    using System;
    using System.Linq;
    using Security.Contracts;

    public class AuthorizeAttribute : Attribute
    {
        private readonly string role;

        public AuthorizeAttribute() { }

        public AuthorizeAttribute(string role)
        {
            this.role = role;
        }

        public bool IsAuthorized(IIdentity user)
        {
            if (this.role == null)
            {
                return IsIdentityPresent(user);
            }

            return this.IsIdentityInRole(user);
        }

        private bool IsIdentityPresent(IIdentity identity) => identity != null;

        private bool IsIdentityInRole(IIdentity identity)
        {
            if (this.IsIdentityPresent(identity))
            {
                return false;
            }

            return identity.Roles.Any(r => r == this.role);
        }
    }
}
