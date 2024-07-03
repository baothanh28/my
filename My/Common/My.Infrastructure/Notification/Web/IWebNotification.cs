using System.Threading;
using System.Threading.Tasks;

namespace My.Infrastructure.Notification.Web;

public interface IWebNotification<T>
{
    Task SendAsync(T message, CancellationToken cancellationToken = default);
}
