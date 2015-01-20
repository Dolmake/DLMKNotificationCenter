
using System.Collections.Generic;

namespace DLMKNotificationCenter
{
    public class NotificationCenter
    {
        public static string DEFAULT_QUEUE = "DEFAULT_QUEUE";
        public static string NORMALIZE(string queueId)
        {
            return string.IsNullOrEmpty(queueId) ? DEFAULT_QUEUE : queueId;
        }

      
       

        Queue<Notification> _notificationsBuffer = new Queue<Notification>();
        Queue<Notification> _secondaryBuffer = new Queue<Notification>();
        Dictionary<string, List<INotificable>> _observers = new Dictionary<string, List<INotificable>>();

        private static NotificationCenter _instance = null;

        /// <summary>
        /// SINGLETON Accessor
        /// </summary>
        public static NotificationCenter DefaultCenter
        {
            get
            {
                if (_instance == null)
                    _instance = new NotificationCenter();
                return _instance;
            }
        }


        /// <summary>
        /// Occurs before a notification is posted
        /// </summary>
        public event System.Action<Notification> OnBeforeNotificationPosted;

        /// <summary>
        /// Occurs after any notification is posted
        /// </summary>
        public event System.Action<Notification> OnAfterNotificationPosted;


        /// <summary>
        /// Private constructor
        /// </summary>
        private NotificationCenter() { }

      
        /// <summary>
        /// Post a Notification
        /// </summary>
        /// <param name="notification">Notification</param>
        public void PostNotification(Notification notification)
        {
            _notificationsBuffer.Enqueue(notification);
        }

        /// <summary>
        /// Post a Notification
        /// </summary>
        /// <param name="name">Notification name</param>
        /// <param name="sender">Sender object. Null by default</param>
        /// <param name="userInfo">Dictionary with data. Null by default</param>
        /// <param name="queueId">Notification queue</param>
        public void PostNotification(string name , object sender = null, Dictionary<string,object> userInfo = null, string queueId = null)
        {
            PostNotification(new Notification(name, sender, userInfo, queueId));
        }

        /// <summary>
        /// Add an observer for a specific queue.
        /// </summary>
        /// <param name="obj">Observer obj. Accomplish INotificable</param>
        /// <param name="queueId">Specific Queue. Null by default</param>
        public void AddObserver(INotificable obj, string queueId = null)
        {
            queueId = NORMALIZE(queueId);
            if (!_observers.ContainsKey(queueId))
                _observers.Add(queueId, new List<INotificable>(2));
            _observers[queueId].Add(obj);
        }

        /// <summary>
        /// Removes an observer from a specific queue.
        /// </summary>
        /// <param name="obj">Observer obj</param>
        /// <param name="queueId">Specific Queue. Null by default</param>
        public void RemoveObserver(INotificable obj, string queueId = null)
        {
            queueId = NORMALIZE(queueId);
            if (_observers.ContainsKey(queueId))
                _observers[queueId].Remove(obj);
        }


        internal void Flush()
        {
            //Copy notifications into a secondary buffer
            foreach (Notification n in _notificationsBuffer)
                _secondaryBuffer.Enqueue(n);

            //Clear notifications buffer
            _notificationsBuffer.Clear();

            //Post notifications
            while (_secondaryBuffer.Count != 0)                          
                _PostNotification(_secondaryBuffer.Dequeue());            

            //Clean the secondary buffer
            _secondaryBuffer.Clear();
        }

        List<INotificable> _toInvoke = new List<INotificable>();
        void _PostNotification(Notification notification)
        {
            if (_observers.ContainsKey(notification.QueueId))
            {
                List<INotificable> observers = _observers[notification.QueueId];
                if (observers.Count > 0)
                {
                    _toInvoke.AddRange(observers);

                    if (OnBeforeNotificationPosted != null)
                        OnBeforeNotificationPosted(notification);

                    foreach (INotificable notificable in _toInvoke)
                        notificable.RecieveNotification(notification);

                    if (OnAfterNotificationPosted != null)
                        OnAfterNotificationPosted(notification);

                    _toInvoke.Clear();
                }
            }
        }
    }
}
