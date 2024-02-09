using ToDoList_API.Models;

namespace ToDoList_API.Repository.IRepository
{
    public interface ICatTaskRepositorio : IRepositorio<CategoryTask>
    {
        Task<CategoryTask> Update(CategoryTask entity);

    }
}
