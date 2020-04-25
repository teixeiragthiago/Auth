using System.Collections.Generic;
using System.Linq;

namespace Auth.SharedKernel.Domain.Notification
{
    public class Notification : INotification
    {
        private List<DomainNotification> _notifications;

        public Notification()
        {
            _notifications = new List<DomainNotification>();
        }

        public virtual void Add(string message) => _notifications.Add(new DomainNotification(message));

        public virtual string GetErrors() => _notifications.FirstOrDefault()?.Value;

        public virtual T AddWithReturn<T>(string message)
        {
            _notifications.Add(new DomainNotification(message));

            return default(T);
        }

        public bool Any() => _notifications.Any();
    }

    public class DomainNotification
    {
        public string Value { get; }

        #region Description in the type value
        // This Type for message in notification ex: 
        // warning: that's not an error message, but is a notification on about a due action
        // error: this an error message
        // info: this an addcional message 
        #endregion

        public DomainNotification(string value)
        {
            Value = value;
        }
    }
}