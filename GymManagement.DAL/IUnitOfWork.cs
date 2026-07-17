using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL
{
    public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository { get; }

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
        Task<int> SaveChangesAsync();


    }
}
