# DLMKNotificationCenter
Unity3D Notification Center

## How To

1. Import UnityPackage.
2. Drag "NotificationCenterServer" to your first Scene.
3. Use it.

*NotificationCenterServer*: Is a persistent gameobject which mission is to flush all the pending notifications in every Update Call

## How it works

*NotificationCenter.DefaultCenter* is a SINGLETON which has: a pending queue of notifications and a list of observers.

So, the steps are:
1. Add an Observer for a specific Notification (Notification Queue).
2. Post the proper Notification, and the Observer will invoke the Observer.


## Observers

All the observers have to match the interface: *INotificable*, and then subscribe to a specific Notification Queue.

## Notifications

*The name of the notification:*
public string Name { get; private set; }

*The invoker. By default is NULL:*
public object Sender { get; private set; }

*The data. By default is NULL:*
public Dictionary<string, object> UserInfo { get; private set; }

*The notification Queue ID:*
public string QueueId { get; private set; }

## NotificationCenter public inteface

*Occurs before a notification is posted:*
public event System.Action<Notification> OnBeforeNotificationPosted;

*Occurs after any notification is posted:*
public event System.Action<Notification> OnAfterNotificationPosted;

*Post a Notification:*
public void PostNotification(Notification notification);

*Post a Notification:*
public void PostNotification(string name , object sender = null, Dictionary<string,object> userInfo = null, string queueId = null);

*Add an observer for a specific queue:*
public void AddObserver(INotificable obj, string queueId = null);

*Removes an observer from a specific queue:*
public void RemoveObserver(INotificable obj, string queueId = null);
 
 
 
 
 
