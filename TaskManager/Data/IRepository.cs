using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.Data
{
    /// <summary>
    /// The repository pattern will be used.
    /// the entity repositories must implement this interface
    /// </summary>
    /// <typeparam name="T">Type of entity for the repository</typeparam>
    public interface IRepository<T> where T : Entity
    {
        T Get(long id);
        IList<T> GetAll();
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}