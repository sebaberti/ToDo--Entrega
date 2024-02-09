using ToDoList_API.Data;
using ToDoList_API.Models;
using ToDoList_API.Repository.IRepository;

namespace ToDoList_API.Repository
{
    public class CategoriesRepositorio : Repositorio<Categories> , ICategoriesRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CategoriesRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Categories> Update(Categories entity)
        {
            
            _db.categories.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}
