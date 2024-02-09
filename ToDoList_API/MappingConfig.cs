using AutoMapper;
using ToDoList_API.Models;
using ToDoList_API.Models.Dto;

namespace ToDoList_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            CreateMap<Tasks, TaskDto>().ReverseMap();

            CreateMap<Tasks, TaskCreateDto>().ReverseMap();

            CreateMap<Tasks, UpdateTaskDto>().ReverseMap();


            CreateMap<Categories, CategoriesDto>().ReverseMap();

            CreateMap<Categories, CategoriesCreateDto>().ReverseMap();

            CreateMap<Categories, CategoriesUpdateDto>().ReverseMap();


            CreateMap<CategoryTask, CatTaskDto>().ReverseMap();

            CreateMap<CategoryTask, CatTaskCreateDto>().ReverseMap();

            CreateMap<CategoryTask, CatTaskUpdateDto>().ReverseMap();



        }
    }
}
