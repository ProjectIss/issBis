using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Bis.Custom
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int EId { get; set; }
        #endregion

        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            if (role.Contains(Role))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetRole()
        {
            return Role;
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}