
using System.Collections.Generic;


namespace DLMKNotificationCenter
{
    public class Notification
    {
        public string Name { get; private set; }
        public object Sender { get; private set; }
        public Dictionary<string, object> UserInfo { get; private set; }
        public string QueueId { get; private set; }
        public Notification(string name, object sender = null, Dictionary<string, object> userInfo = null, string queueId = null)
        {
            Name = name;
            Sender = sender;
            UserInfo = userInfo;
            QueueId = NotificationCenter.NORMALIZE(queueId);
        }
    }
}
