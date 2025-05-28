using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
   public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, string title, string body, int? caseRequestId , int? caseConsultantId);

    }
}
