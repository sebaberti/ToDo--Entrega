using ToDoList_API.Models;

namespace ToDoList_API.Repository.IRepository
{
    public interface ITaskRepositorio : IRepositorio<Tasks>
    {
        Task GetById(int id);
        Task<Tasks> Update(Tasks entity);

    }
}
