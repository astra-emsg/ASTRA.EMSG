using System;
using System.Collections.Generic;
using System.Threading;
using ASTRA.EMSG.Business.Entities.Common;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public class AuditEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        public static void Register(Configuration configuration)
        {
            var insertEventListeners =
                new List<IPreInsertEventListener>(configuration.EventListeners.PreInsertEventListeners);
            insertEventListeners.Add(new AuditEventListener());
            configuration.EventListeners.PreInsertEventListeners = insertEventListeners.ToArray();

            var updateEventListeners =
                new List<IPreUpdateEventListener>(configuration.EventListeners.PreUpdateEventListeners);
            updateEventListeners.Add(new AuditEventListener());
            configuration.EventListeners.PreUpdateEventListeners = updateEventListeners.ToArray();

        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var audit = @event.Entity as IAuditableEntity;
            if (audit != null)
            {
                try
                {
                    var time = DateTime.Now;
                    var name = Thread.CurrentPrincipal.Identity.Name;

                    Set(@event.Persister, @event.State, "CreatedAt", time);
                    Set(@event.Persister, @event.State, "CreatedBy", name);
                    Set(@event.Persister, @event.State, "UpdatedAt", time);
                    Set(@event.Persister, @event.State, "UpdatedBy", name);

                    audit.CreatedAt = time;
                    audit.CreatedBy = name;
                    audit.UpdatedAt = time;
                    audit.UpdatedBy = name;
                }
                catch (Exception)
                {
                    //NOP
                }
            }
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var audit = @event.Entity as IAuditableEntity;
            if (audit != null)
            {
                try
                {
                    var time = DateTime.Now;
                    var name = Thread.CurrentPrincipal.Identity.Name;

                    Set(@event.Persister, @event.State, "UpdatedAt", time);
                    Set(@event.Persister, @event.State, "UpdatedBy", name);

                    audit.UpdatedAt = time;
                    audit.UpdatedBy = name;

                }
                catch (Exception)
                {
                    //NOP
                }
            }
            return false;
        }

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }
    }
}