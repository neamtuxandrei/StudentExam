using ExamSupportToolAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.Abstractions
{
    public interface IBaseRepository<T> where T : User
    {
        void Add(T toAdd);

        void AddRange(ICollection<T> toAdd);
        void Remove(T entity);
        void Update(T entity);
        T GetById(Guid id);
        T? GetByIdFirstOrDefault(Guid id);
        Task<T> GetByEmail(string email);
        Task<T?> GetByEmailFirstOrDefault(string email);
        Task<bool> SaveChangesAsync();
    }
}
