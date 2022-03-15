using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface INotificationService
    {
        Task<bool> SendEmail(Notification email);
    }
}
