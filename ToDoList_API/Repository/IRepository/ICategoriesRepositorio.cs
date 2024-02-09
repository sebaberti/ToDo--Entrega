using ToDoList_API.Models;

namespace ToDoList_API.Repository.IRepository
{
    public interface ICategoriesRepositorio : IRepositorio<Categories>
    {
        Task<Categories> Update(Categories entity);

    }
}
