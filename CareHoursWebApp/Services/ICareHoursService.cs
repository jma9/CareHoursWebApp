using System.Collections.Generic;
using CareHoursWebApp.Models;

namespace CareHoursWebApp.Services
{
    public interface ICareHoursService
    {
        IEnumerable<CareHours> GetCareHoursForChild(int childId);

        void Create(CareHours careHours);
    }
}