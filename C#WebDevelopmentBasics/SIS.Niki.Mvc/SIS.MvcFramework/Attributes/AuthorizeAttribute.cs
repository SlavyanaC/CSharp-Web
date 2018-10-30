namespace SIS.MvcFramework.Attributes
{
    using System;

    public class AuthorizeAttribute : Attribute
    {
        public string RoleName { get; set; }

        public AuthorizeAttribute(string roleName = null)
        {
            this.RoleName = roleName;
        }
    }
}
