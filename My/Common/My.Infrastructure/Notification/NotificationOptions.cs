using My.Infrastructure.Notification.Email;
using My.Infrastructure.Notification.Sms;
using My.Infrastructure.Notification.Web;

namespace My.Infrastructure.Notification;

public class NotificationOptions
{
    public EmailOptions Email { get; set; }

    public SmsOptions Sms { get; set; }

    public WebOptions Web { get; set; }
}
