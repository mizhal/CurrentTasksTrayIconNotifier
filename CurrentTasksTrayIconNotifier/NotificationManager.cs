using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace CurrentTasksTrayIconNotifier
{
    public class NotificationManager
    {
        public const string APP_ID = "Kata.Winforms.CurrentTasks";
        
        private static NotificationManager Singleton;

        private NotificationManager() {}

        public static NotificationManager GetInstance()
        {
            if (Singleton == null)
                Singleton = new NotificationManager();
            return Singleton;
        }

        public void SendNotification(string text)
        {
            XmlDocument ToastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);

            int cut1, cut2;
            cut1 = text.Length > 80 ? 80 : text.Length - 1;
            cut2 = text.Length > 160 ? 160 : text.Length - 1;

            string line1, line2;
            line1 = text.Substring(0, cut1);
            if (text.Length > 80)
                line2 = text.Substring(cut1 + 1, cut2 - (cut1 + 1));
            else
                line2 = "";

            XmlNodeList string_elements = ToastXml.GetElementsByTagName("text");
            string_elements[0].AppendChild(ToastXml.CreateTextNode("Current Tasks"));
            string_elements[1].AppendChild(ToastXml.CreateTextNode(line1));
            string_elements[2].AppendChild(ToastXml.CreateTextNode(line2));
            
            ToastNotification toast = new ToastNotification(ToastXml);
            //toast.Activated += ToastActivated;
            //toast.Dismissed += ToastDismissed;
            //toast.Failed += ToastFailed;

            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }
    }
}
