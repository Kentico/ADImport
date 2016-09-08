using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Base;
using CMS.Membership;

namespace ADImport
{
    /// <summary>
    /// Cumulates membership changes by remembering roles of a user before and after membership synchronization.
    /// </summary>
    internal class CumulatedRolesMembership
    {
        private IDictionary<Guid, string> mRolesBefore;
        private IDictionary<Guid, string> mRolesAfter;


        /// <summary>
        /// Subtracts roles before the synchronization from roles after the synchronization (roles user is still in without roles user was in before) and return their names.
        /// </summary>
        private ICollection<string> GetRoleNamesUserWasAddedTo()
        {
            return mRolesAfter.Except(mRolesBefore).Select(role => role.Value).ToArray();
        }


        /// <summary>
        /// Subtracts roles after the synchronization from roles before the synchronization (roles user was in before without roles user is still in) and return their names.
        /// </summary>
        /// <returns></returns>
        private ICollection<string> GetRoleNamesUserWasRemovedFrom()
        {
            return mRolesBefore.Except(mRolesAfter).Select(role => role.Value).ToArray();
        }


        /// <summary>
        /// Stores roles user was in before the synchronization.
        /// </summary>
        /// <remarks>Call before the synchronization</remarks>
        /// <param name="roles">Set of CMS roles the user in question is in – infos are supposed to contain RoleGUID and DisplayName.</param>
        public void SetRolesBefore(IEnumerable<RoleInfo> roles)
        {
            mRolesBefore = roles.ToDictionary(role => role.RoleGUID, role => role.RoleDisplayName);
        }


        /// <summary>
        /// Stores roles user is still in after the synchronization.
        /// </summary>
        /// <remarks>Call after the synchronization</remarks>
        /// <param name="roles">Set of CMS roles the user in question is in – infos are supposed to contain RoleGUID and DisplayName.</param>
        public void SetRolesAfter(IEnumerable<RoleInfo> roles)
        {
            mRolesAfter = roles.ToDictionary(role => role.RoleGUID, role => role.RoleDisplayName);
        }


        /// <summary>
        /// Writes down to the CMS event log both the event containing all roles user was added into and all roles user was removed from.
        /// </summary>
        /// <param name="userName">User name (login) of a user in question</param>
        public void WriteEventsToEventLog(string userName)
        {
            if (mRolesBefore == null)
            {
                throw new NullReferenceException("SetRolesBefore method was not called!");
            }
            if (mRolesAfter == null)
            {
                throw new NullReferenceException("SetRolesAfter method was not called!");
            }

            // Log created and removed memberships to EventLog
            using (new CMSActionContext { LogEvents = true })
            {
                GetRoleNamesUserWasAddedTo()
                    .LogCumulativeWellKnownEvent(WellKnownEventLogEventsEnum.UserAddedToRoles, userName);
                
                GetRoleNamesUserWasRemovedFrom()
                    .LogCumulativeWellKnownEvent(WellKnownEventLogEventsEnum.UserRemovedFromRoles, userName);
            }
        }
    }
}
