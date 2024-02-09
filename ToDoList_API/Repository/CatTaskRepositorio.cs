using ToDoList_API.Data;
using ToDoList_API.Models;
using ToDoList_API.Repository.IRepository;

namespace ToDoList_API.Repository
{
    public class CatTaskRepositorio : Repositorio<CategoryTask> , ICatTaskRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CatTaskRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CategoryTask> Update(CategoryTask entity)
        {
            
            _db.categoryTasks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}
