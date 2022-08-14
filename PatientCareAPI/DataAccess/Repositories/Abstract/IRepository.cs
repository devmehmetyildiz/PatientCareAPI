using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IRepository<Tentity> where Tentity : class
    {
        Tentity GetbyName(string Name);
        Tentity GetByGuid(string guid);
        Tentity Getbyid(int id);

        List<Tentity> GetAll();

        List<Tentity> GetTop(int count);

        void update(Tentity olditem, Tentity newitem);

        void Add(Tentity entity);

        void AddRange(List<Tentity> entities);

        void Remove(int id);
        void RemoveByGuid(string Guid);

        void RemoveRange(List<Tentity> entities);
    }
}
