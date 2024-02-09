using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using ToDoList_API.Models;
using ToDoList_API.Models.Dto;
using ToDoList_API.Repository.IRepository;

namespace ToDoList_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class CatTaskController : ControllerBase
    {
        private readonly ILogger<CatTaskController> _logger;
        private readonly ITaskRepositorio _taskRepo;
        private readonly ICategoriesRepositorio _categoriesRepo;
        private readonly ICatTaskRepositorio _catTaskRepositorio;
        private readonly IMapper _mapper;
        protected ApiResponse _response;


        public CatTaskController(ILogger<CatTaskController> logger, ITaskRepositorio taskrepo,ICategoriesRepositorio categoriesRepo, ICatTaskRepositorio cattaskrep, IMapper maper)
        {
            _logger = logger;
            _taskRepo = taskrepo;
            _mapper = maper;
            _response = new();
            _categoriesRepo = categoriesRepo;
            _catTaskRepositorio = cattaskrep;
        }




        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponse>> GetCatTask()
        {
            try
            {
                _logger.LogInformation("Obtener las Categorias de las tareas");

                IEnumerable<CategoryTask> CatTaskList = await _catTaskRepositorio.GetAll();

                _response.Result = _mapper.Map<IEnumerable<CatTaskDto>>(CatTaskList);
                _response.statusCode = HttpStatusCode.OK;


                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("id:int", Name = "GetCategoriesTask")]  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCatTask(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer la categoria de la tarea con Id " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }
                
                var CatTask = await _catTaskRepositorio.GetOne(v => v.TaskId == id);

                if (CatTask == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<TaskDto>(CatTask);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> CreateCatTask([FromBody] CatTaskCreateDto createDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _catTaskRepositorio.GetOne(v => v.TaskId == createDto.TaskId) != null)
                {
                    ModelState.AddModelError("ExistName", "La categoria de tarea con ese id ya existe ");
                    return BadRequest(ModelState);
                }
                if (await _taskRepo.GetOne(v => v.Id == createDto.TaskId) == null || 
                    await _taskRepo.GetOne(v=> v.Id == createDto.CategoryId)== null)
                {
                    ModelState.AddModelError("Clave Foranea", "El id no existe ");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }


                CategoryTask model = _mapper.Map<CategoryTask>(createDto);


               
                await _catTaskRepositorio.Create(model);
                _response.Result = model;
                _response.statusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetCatTask", new { id = model.TaskId }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteCatTask(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var  categoryTask = await _catTaskRepositorio.GetOne(v => v.TaskId == id);
                if (categoryTask == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _catTaskRepositorio.Remove(categoryTask);

                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateCatTask(int id, [FromBody] CatTaskUpdateDto updateDto)

        {
            if (updateDto == null || id != updateDto.TaskId || id!= updateDto.CategoryId)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            CategoryTask model = _mapper.Map<CategoryTask>(updateDto);


            
            await _catTaskRepositorio.Update(model);
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);

        }

    }
}
