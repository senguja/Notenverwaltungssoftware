﻿using Data.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Notenverwaltung.Core.Services
{
    /// <summary>
    /// LdapService.
    /// </summary>
    /// <seealso cref="Notenverwaltung.Core.Services.ILdapService" />
    public class LdapService : ILdapService
    {
        private readonly List<Principal> _domainGroups = new List<Principal>();
        private readonly List<UserPrincipal> _domainUsers = new List<UserPrincipal>();
        private readonly List<Principal> _userGroups = new List<Principal>();
        private readonly List<RoleType> _userRoles = new List<RoleType>();
        private string _password;
        private string _userName;

        #region InterfaceMethods

        public List<Principal> GetDomainGroups()
        {
            if (_domainGroups.Count == 0)
            {
                ReadDomainGroups();
            }

            return _domainGroups;
        }

        public List<UserPrincipal> GetDomainUsers()
        {
            if (_domainUsers.Count == 0)
            {
                ReadDomainUsers();
            }

            return _domainUsers;
        }

        public List<Principal> GetUserDomainGroups()
        {
            if (_userGroups.Count == 0)
            {
                ReadUserGroup();
            }

            return _userGroups;
        }

        public List<RoleType> GetUserRoles()
        {
            if (_userRoles.Count == 0)
            {
                ReadUserRoles();
            }

            return _userRoles;
        }

        public void LoginUser(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        #endregion InterfaceMethods

        private bool IsUserMemberOfGroup(string groupname)
        {
            GroupPrincipal groupPrincipal = (GroupPrincipal)_userGroups.Where(g => g.SamAccountName == groupname).FirstOrDefault();

            return (groupPrincipal != null);
        }

        private void ReadDomainGroups()
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
            {
                context.ValidateCredentials(_userName, _password);

                using (GroupPrincipal qbeGroup = new GroupPrincipal(context))
                {
                    using (PrincipalSearcher searcher = new PrincipalSearcher(qbeGroup))
                    {
                        _domainGroups.AddRange(searcher.FindAll().ToList());
                    }
                }
            }
        }

        private void ReadDomainUsers()
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
            {
                context.ValidateCredentials(_userName, _password);

                using (GroupPrincipal qbeGroup = new GroupPrincipal(context))
                {
                    using (PrincipalSearcher searcher = new PrincipalSearcher(qbeGroup))
                    {
                        _domainUsers.AddRange(searcher.FindAll().Select(u => (UserPrincipal)u).ToList());
                    }
                }
            }
        }

        private void ReadUserGroup()
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    context.ValidateCredentials(_userName, _password);

                    using (var userPrincipal = UserPrincipal.FindByIdentity(context, _userName))
                    {
                        _userGroups.AddRange(userPrincipal.GetGroups().ToList());
                    }
                }
            }
            catch
            {
            }
        }

        private void ReadUserRoles()
        {
            try
            {
                var roleTypeList = Enum.GetValues(typeof(RoleType)).Cast<RoleType>().ToList();
                foreach (var role in roleTypeList)
                {
                    if (IsUserMemberOfGroup(nameof(role)))
                    {
                        _userRoles.Add(role);
                    }
                }
            }
            catch
            {
            }

            // TODO: only for testing!
            if (_userRoles.Count == 0)
            {
                _userRoles.Add(RoleType.Admin);
            }
        }
    }
}