using ToDoList_API.Data;
using ToDoList_API.Models;
using ToDoList_API.Repository.IRepository;

namespace ToDoList_API.Repository
{
    public class TaskRepositorio : Repositorio<Tasks> , ITaskRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TaskRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Tasks> Update(Tasks entity)
        {
            entity.CreatedAt = DateTime.Now;
            _db.tasks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}
