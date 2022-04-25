using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess.Repositories.Abstract;

namespace PatientCareAPI.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRolesRepository RolesRepository { get; }

        IUsersRepository UsersRepository { get; }

        IUsertoRoleRepository UsertoRoleRepository { get; }

        IUsertoSaltRepository UsertoSaltRepository { get; }

        int Complate();
    }
}
