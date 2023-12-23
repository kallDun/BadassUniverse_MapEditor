using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapEditor.Repository;

public interface IRepository<T>
{
    public Task Add(T item);
    public Task Remove(T item);
    public Task Update(T item);
    public Task<T> Get(int id);
    public Task<IEnumerable<T>> GetAll();
}