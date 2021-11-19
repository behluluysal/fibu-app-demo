using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class Permission
    {
        public static class Dashboards
        {
            public const string View = "Dashboard.View";
            public static readonly List<string> _metrics =
                new List<string>(new[]
                {
                    View
                });
        }

        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class Tags
        {
            public const string View = "Tags.View";
            public const string Create = "Tags.Create";
            public const string Edit = "Tags.Edit";
            public const string Delete = "Tags.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class Roles
        {
            public const string View = "Roles.View";
            public const string Create = "Roles.Create";
            public const string Edit = "Roles.Edit";
            public const string Delete = "Roles.Delete";
            public const string AssignPermission = "Roles.AssignPermission";
            public const string WithdrawPermission = "Roles.WithdrawPermission";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete,AssignPermission,WithdrawPermission
               });
        }

        public static class Products
        {
            public const string View = "Products.View";
            public const string Create = "Products.Create";
            public const string Edit = "Products.Edit";
            public const string Delete = "Products.Delete";
            public const string EditTag = "Products.EditTag";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete,EditTag
               });
        }

        public static class SupplierCompanies
        {
            public const string View = "SupplierCompanies.View";
            public const string Create = "SupplierCompanies.Create";
            public const string Edit = "SupplierCompanies.Edit";
            public const string Delete = "SupplierCompanies.Delete";
            public const string EditTag = "SupplierCompanies.EditTag";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete,EditTag
               });
        }

        public static class SCResponsiblePeople
        {
            public const string View = "SCResponsiblePeople.View";
            public const string Create = "SCResponsiblePeople.Create";
            public const string Edit = "SCResponsiblePeople.Edit";
            public const string Delete = "SCResponsiblePeople.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class SCRPEmails
        {
            public const string View = "SCRPEmails.View";
            public const string Create = "SCRPEmails.Create";
            public const string Edit = "SCRPEmails.Edit";
            public const string Delete = "SCRPEmails.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class SCRPPhones
        {
            public const string View = "SCRPPhones.View";
            public const string Create = "SCRPPhones.Create";
            public const string Edit = "SCRPPhones.Edit";
            public const string Delete = "SCRPPhones.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class BusinessPartners
        {
            public const string View = "BusinessPartners.View";
            public const string Create = "BusinessPartners.Create";
            public const string Edit = "BusinessPartners.Edit";
            public const string Delete = "BusinessPartners.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class BPResponsiblePeople
        {
            public const string View = "BPResponsiblePeople.View";
            public const string Create = "BPResponsiblePeople.Create";
            public const string Edit = "BPResponsiblePeople.Edit";
            public const string Delete = "BPResponsiblePeople.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class BPRPEmails
        {
            public const string View = "BPRPEmails.View";
            public const string Create = "BPRPEmails.Create";
            public const string Edit = "BPRPEmails.Edit";
            public const string Delete = "BPRPEmails.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class BPRPPhones
        {
            public const string View = "BPRPPhones.View";
            public const string Create = "BPRPPhones.Create";
            public const string Edit = "BPRPPhones.Edit";
            public const string Delete = "BPRPPhones.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class RequestedProducts
        {
            public const string View = "RequestedProducts.View";
            public const string MyView = "RequestedProducts.MyView";
            public const string Create = "RequestedProducts.Create";
            public const string Edit = "RequestedProducts.Edit";
            public const string Delete = "RequestedProducts.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,MyView,Create,Edit,Delete
               });
        }

        public static class Offers
        {
            public const string View = "Offers.View";
            public const string Create = "Offers.Create";
            public const string Edit = "Offers.Edit";
            public const string Delete = "Offers.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }

        public static class Requests
        {
            public const string View = "Requests.View";
            public const string MyView = "Requests.MyView";
            public const string Create = "Requests.Create";
            public const string Edit = "Requests.Edit";
            public const string Delete = "Requests.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,MyView,Create,Edit,Delete
               });
        }

        public static class Chats
        {
            public const string View = "Chats.View";
            public const string Create = "Chats.Create";
            public const string Edit = "Chats.Edit";
            public const string Delete = "Chats.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }
    }
}
