using System.Collections.Generic;
using System.Threading.Tasks;
using CareHoursWebApp.Models;

namespace CareHoursWebApp.Services
{
    public interface ICareHoursService
    {
        Task<IEnumerable<CareHours>> GetCareHoursForChildAsync(int childId);

        Task<CareHours> CreateAsync(CareHours careHours);

        Task<CareHours> GetAsync(int eventId);

        Task DeleteAsync(CareHours careHours);
    }
}