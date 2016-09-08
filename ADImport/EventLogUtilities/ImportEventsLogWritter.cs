using System;
using System.Collections.Generic;
using System.Linq;

using CMS.EventLog;

namespace ADImport
{
    /// <summary>
    /// Writes <see cref="WellKnownEventLogEventsEnum"/> events to the CMS Event Log.
    /// </summary>
    internal static class ImportEventsLogWritter
    {
        /// <summary>
        /// Stores  a <see cref="WellKnownEventLogEventsEnum"/>'s event properties (i.e. event code, source, description).
        /// </summary>
        private class WellKnowEventProperties
        {
            /// <summary>
            /// Event code of well-known event (<seealso cref="EventLogInfo"/>)
            /// </summary>
            public string EventCode
            {
                get;
                set;
            }


            /// <summary>
            /// Source of well-known event (<seealso cref="EventLogInfo"/>)
            /// </summary>
            public string Source
            {
                get;
                set;
            }


            /// <summary>
            /// Description of well-known event (<seealso cref="EventLogInfo"/>) that might contain <see cref="String"/> format items
            /// </summary>
            public string Description
            {
                get;
                set;
            }


            /// <summary>
            /// Returns true if <see cref="Description"/> contains <see cref="String"/> format items.
            /// </summary>
            public bool DescriptionContainsFormattingItems
            {
                get;
                set;
            }
        }


        /// <summary>
        /// Dictionary providing <see cref="WellKnowEventProperties"/> for given <see cref="WellKnownEventLogEventsEnum"/>.
        /// </summary>
        private static readonly IDictionary<WellKnownEventLogEventsEnum, WellKnowEventProperties> WELL_KNOWN_EVENT_PROPERTIES = new Dictionary<WellKnownEventLogEventsEnum, WellKnowEventProperties>()
        {
            {
                WellKnownEventLogEventsEnum.UserAddedToRoles,
                new WellKnowEventProperties()
                {
                    EventCode = "USERTOROLE",
                    Source = "Add user to roles",
                    Description = "User \"{0}\" has been added to following roles:",
                    DescriptionContainsFormattingItems = true,
                }
            },
            {
                WellKnownEventLogEventsEnum.UserRemovedFromRoles,
                new WellKnowEventProperties()
                {
                    EventCode = "USERFROMROLE",
                    Source = "Remove user from roles",
                    Description = "User \"{0}\" has been removed from following roles:",
                    DescriptionContainsFormattingItems = true,
                }
            },
            {
                WellKnownEventLogEventsEnum.UsersCreated,
                new WellKnowEventProperties()
                {
                    EventCode = "CREATEUSER",
                    Source = "Users created",
                    Description = "Following users have been created during AD import:"
                }
            },
            {
                WellKnownEventLogEventsEnum.UsersUpdated,
                new WellKnowEventProperties()
                {
                    EventCode = "UPDATEUSER",
                    Source = "Users updated",
                    Description = "Following users have been updated during AD import:"
                }
            },
            {
                WellKnownEventLogEventsEnum.UsersDeleted,
                new WellKnowEventProperties()
                {
                    EventCode = "REMOVEUSER",
                    Source = "Users removed",
                    Description = "Following users have been created during AD import:"
                }
            },
            {
                WellKnownEventLogEventsEnum.RolesCreated,
                new WellKnowEventProperties()
                {
                    EventCode = "CREATEROLE",
                    Source = "Roles created",
                    Description = "Following roles have been created during AD import:"
                }
            },
            {
                WellKnownEventLogEventsEnum.RolesUpdated,
                new WellKnowEventProperties()
                {
                    EventCode = "UPDATEROLE",
                    Source = "Roles updated",
                    Description = "Following roles have been updated during AD import:"
                }
            },
            {
                WellKnownEventLogEventsEnum.RolesDeleted,
                new WellKnowEventProperties()
                {
                    EventCode = "REMOVEROLE",
                    Source = "Roles removed",
                    Description = "Following roles have been removed during AD import:"
                }
            },
        };


        /// <summary>
        /// Logs a <see cref="WellKnownEventLogEventsEnum"/> provided it is a cumulated event (e.g. all roles added or roles removed) to CMS event log.
        /// </summary>
        /// <remarks>
        /// Provided <paramref name="event"/> determines all the description and source and event code of the event log entry.
        /// </remarks>
        /// <param name="names">Names of items (i.e. roles or users) that were added or removed or updated.</param>
        /// <param name="event">Type of event to log</param>
        /// <param name="descriptionArguments">Arguments required for correct event description composition (e.g user name for certain <paramref name="event"/> types – i.e. <see cref="WellKnownEventLogEventsEnum.UserAddedToRoles"/> and <see cref="WellKnownEventLogEventsEnum.UserRemovedFromRoles"/>)</param>
        internal static void LogCumulativeWellKnownEvent(this ICollection<string> names, WellKnownEventLogEventsEnum @event, params object[] descriptionArguments)
        {
            // No names, no event in the log
            if (!names.Any())
            {
                return;
            }            

            // Compose event message based on eventCode
            var eventProperties = WELL_KNOWN_EVENT_PROPERTIES[@event];
            var namesFormatted = String.Join("," + Environment.NewLine, names.Where(name => name != null));
            var eventDescription = (eventProperties.DescriptionContainsFormattingItems
                    ? String.Format(eventProperties.Description, descriptionArguments)
                    : eventProperties.Description)
                + Environment.NewLine
                + Environment.NewLine
                + namesFormatted;

            // Write down the log
            EventLogProvider.LogInformation(
                eventProperties.Source,
                eventProperties.EventCode,
                eventDescription);
        }
    }
}
