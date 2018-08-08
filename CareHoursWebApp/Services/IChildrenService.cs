using System.Collections.Generic;
using System.Threading.Tasks;
using CareHoursWebApp.Models;

namespace CareHoursWebApp.Services
{
    public interface IChildrenService
    {
        Task<Child> CreateAsync(Child child);

        Task DeleteAsync(Child child);

        Task<Child> GetAsync(int childId);

        Task<IEnumerable<Child>> GetListAsync();

        Task<Child> UpdateAsync(Child child);
    }
}