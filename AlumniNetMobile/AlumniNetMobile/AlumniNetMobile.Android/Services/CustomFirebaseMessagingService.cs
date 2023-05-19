using AlumniNetMobile.Common;
using AlumniNetMobile.Helpers;
using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;

namespace TSEMobileApp.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class CustomFirebaseMessagingService : FirebaseMessagingService
    {
        public readonly ILocalNotificationService LocalNotificationsService;

        public CustomFirebaseMessagingService()
        {
            LocalNotificationsService = new LocalNotificationsService();
        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            Log.Debug("FMC_SERVICE", token);
            FCMTokenHelper.Token = token;
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            var notification = message.GetNotification();
            LocalNotificationsService.ShowNotification(notification.Title, notification.Body, message.Data);
        }

    }
}
