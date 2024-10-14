using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class NotificationController : MonoBehaviour
{
    [SerializeField] AndroidNotifications androidNotifications;
    
    // Start is called before the first frame update
    private void Start()
    {
        androidNotifications.RequestAuthorization();
        androidNotifications.RegisterNotificationsChannel();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            androidNotifications.SendNotification("Great job!", "Your social media usage is down 50% today, and your pet is feeling healthier!", 5);
        }
    }

}
