using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Utility
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public string Type { get; set; }

        public PermissionRequirement(string permission, string type)
        {
            Permission = permission;
            Type = type;
        }
    }
}