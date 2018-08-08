using System.Collections.Generic;
using CareHoursWebApp.Models;

namespace CareHoursWebApp.Services
{
    public interface IChildrenService
    {
        void Create(Child child);

        void Delete(Child child);

        Child Get(int childId);

        IEnumerable<Child> GetList();

        void Update(Child child);
    }
}