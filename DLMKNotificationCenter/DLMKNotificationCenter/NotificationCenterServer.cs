using System;
using System.Collections.Generic;
using UnityEngine;


namespace DLMKNotificationCenter
{
    public class NotificationCenterServer : MonoBehaviour
    {
        void Awake()
        {
            //To make it persistent
            DontDestroyOnLoad(this);
        }

        void Update()
        {
            NotificationCenter.DefaultCenter.Flush();
        }
    }
}
