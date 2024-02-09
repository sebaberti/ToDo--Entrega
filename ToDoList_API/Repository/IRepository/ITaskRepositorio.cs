using ToDoList_API.Models;

namespace ToDoList_API.Repository.IRepository
{
    public interface ITaskRepositorio : IRepositorio<Tasks>
    {
        Task<Tasks> Update(Tasks entity);

    }
}
