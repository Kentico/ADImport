namespace ADImport
{
    /// <summary>
    /// Described well known event log events, events that can be written by <see cref="ImportEventsLogWritter"/>
    /// </summary>
    internal enum WellKnownEventLogEventsEnum
    {
        /// <summary>
        /// All roles a user was added into (<seealso cref="CumulatedRolesMembership"/>)
        /// </summary>
        UserAddedToRoles,


        /// <summary>
        /// All roles a user was removed from (<seealso cref="CumulatedRolesMembership"/>)
        /// </summary>
        UserRemovedFromRoles,


        /// <summary>
        /// All roles that were added during the AD import (<seealso cref="CumulatedChanges"/>)
        /// </summary>
        RolesCreated,


        /// <summary>
        /// All roles that were modified during the AD import (<seealso cref="CumulatedChanges"/>)
        /// </summary>
        RolesUpdated,


        /// <summary>
        /// All roles that were removed during the AD import (<seealso cref="CumulatedChanges"/>)
        /// </summary>
        RolesDeleted,


        /// <summary>
        /// All users that were added during the AD import (<seealso cref="CumulatedChanges"/>)
        /// </summary>
        UsersCreated,


        /// <summary>
        /// All users that were modified during the AD import (<seealso cref="CumulatedChanges"/>)
        /// </summary>
        UsersUpdated,


        /// <summary>
        /// All users that were removed during the AD import (<seealso cref="CumulatedChanges"/>)
        /// </summary>
        UsersDeleted,
    }
}
