using System.Linq;

namespace NotificationsByAzureFunction
{
    public class NotificationDispatcherViewModel
    {
        public string NotifyBy { get; set; }

        public string[] Receivers { get; set; }

        public string Subject { get; set; }

        public bool IsValid
        {
            get
            {
                var isValid = !string.IsNullOrEmpty(NotifyBy) &&
                              !string.IsNullOrEmpty(Subject) &&
                              Receivers != null && Receivers.Any();

                return isValid;
            }
            set { }
        }
    }
}
