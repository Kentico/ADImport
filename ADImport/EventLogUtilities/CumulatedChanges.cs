using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Base;

namespace ADImport
{
    /// <summary>
    /// Cumulates changes by the action type (<seealso cref="ChangeActionEnum"/>) – items in question are both users and roles (yet single instance is meant to cumulate change of but one of them).
    /// </summary>
    internal class CumulatedChanges
    {
        /// <summary>
        /// Separately stores created and updated and deleted items (i.e. users or groups).
        /// For each item, there is its <see cref="Guid"/> (key of equivalence between CMS and AD) and its display name (<see cref="string"/>).
        /// </summary>
        private readonly IDictionary<ChangeActionEnum, Dictionary<Guid, string>> mSets = new Dictionary<ChangeActionEnum, Dictionary<Guid, string>>()
        {
            {ChangeActionEnum.Created, new Dictionary<Guid, string>()},
            {ChangeActionEnum.Updated, new Dictionary<Guid, string>()},
            {ChangeActionEnum.Deleted, new Dictionary<Guid, string>()},
        };


        private readonly WellKnownEventLogEventsEnum mCreated;
        private readonly WellKnownEventLogEventsEnum mUpdated;
        private readonly WellKnownEventLogEventsEnum mDeleted;


        /// <summary>
        /// Creates new instance of <see cref="CumulatedChanges"/>
        /// </summary>
        /// <param name="created"><see cref="CMS.EventLog.EventLogInfo"/> event type to be logged for created items (i.e. <see cref="WellKnownEventLogEventsEnum.UsersCreated"/> or <see cref="WellKnownEventLogEventsEnum.RolesCreated"/>)</param>
        /// <param name="updated"><see cref="CMS.EventLog.EventLogInfo"/> event type to be logged for updated items (i.e. <see cref="WellKnownEventLogEventsEnum.UsersUpdated"/> or <see cref="WellKnownEventLogEventsEnum.RolesUpdated"/>)</param>
        /// <param name="deleted"><see cref="CMS.EventLog.EventLogInfo"/> event type to be logged for deleted items (i.e. <see cref="WellKnownEventLogEventsEnum.UsersDeleted"/> or <see cref="WellKnownEventLogEventsEnum.RolesDeleted"/>)</param>
        public CumulatedChanges(WellKnownEventLogEventsEnum created, WellKnownEventLogEventsEnum updated, WellKnownEventLogEventsEnum deleted)
        {
            mCreated = created;
            mUpdated = updated;
            mDeleted = deleted;
        }


        /// <summary>
        /// Gets stored item display names for given <see cref="ChangeActionEnum"/> type.
        /// </summary>
        /// <param name="changeType">Type of change to get display names for</param>
        private ICollection<string> GetDisplayNames(ChangeActionEnum changeType)
        {
            return mSets[changeType].Select(x => x.Value).ToArray();
        }


        /// <summary>
        /// Adds new item (i.e. role or user) into the cumulated collections.
        /// </summary>
        /// <param name="guid">ID that is common for both CMS and AD</param>
        /// <param name="displayName">Text that should be displayed in CMS event log (i.e. user name or role display name)</param>
        /// <param name="action">Action that was performed over the item.</param>
        public void Add(Guid guid, string displayName, ChangeActionEnum action)
        {
            mSets[action][guid] = displayName;
        }


        /// <summary>
        /// Writes down to the CMS event log all three events containing all added or updated or removed items (i.e. roles or users).
        /// </summary>
        public void WriteEventsToEventLog()
        {
            using (new CMSActionContext { LogEvents = true })
            {
                GetDisplayNames(ChangeActionEnum.Created)
                    .LogCumulativeWellKnownEvent(mCreated);
                
                GetDisplayNames(ChangeActionEnum.Updated)
                    .LogCumulativeWellKnownEvent(mUpdated);
                
                GetDisplayNames(ChangeActionEnum.Deleted)
                    .LogCumulativeWellKnownEvent(mDeleted);
            }
        }
    }
}
