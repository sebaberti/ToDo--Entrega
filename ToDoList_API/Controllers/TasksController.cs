using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Globalization;
using System.Net;
using ToDoList_API.Models;
using ToDoList_API.Models.Dto;
using ToDoList_API.Repository.IRepository;

namespace ToDoList_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskRepositorio _taskRepo;
       private readonly ICategoriesRepositorio _categoriesRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;


        public TasksController(ILogger<TasksController> logger,ICategoriesRepositorio categoriesRepo, ITaskRepositorio taskrepo, IMapper maper)
        {
            _logger = logger;
            _taskRepo = taskrepo;
            _mapper = maper;
            _categoriesRepo = categoriesRepo;
            _response = new();
        }




        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponse>> GetTasks()
        {
            try
            {
                _logger.LogInformation("Obtener las tareas");

                IEnumerable<Tasks> taskList = await _taskRepo.GetAll();

                

                _response.Result = _mapper.Map<IEnumerable<TaskDto>>(taskList);
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


        [HttpGet("id:int", Name = "GetTask")]  //  le asigno un nombre al endpoint 
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTask(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer la Tarea con Id " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }
                
                var task = await _taskRepo.GetOne(v => v.Id == id);

                if (task == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<TaskDto>(task);
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

        public async Task<ActionResult<ApiResponse>> CreateTask([FromBody] TaskCreateDto createDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _taskRepo.GetOne(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ExistName", "La tarea con ese nombre ya existe ");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                string formattedDeadline = createDto.DeadLine.ToString("dd/MM/yyyy");
                //string formattedCreatedline = createDto.CreatedAt.ToString("dd/MM/yyyy");




                Tasks model = _mapper.Map<Tasks>(createDto);


                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = DateTime.Now;
                model.DeadLine = DateTime.ParseExact(createDto.DeadLine.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                await _taskRepo.Create(model);
                _response.Result = model;
                _response.statusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetTask", new { id = model.Id }, _response);
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

        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var task = await _taskRepo.GetOne(v => v.Id == id);
                if (task == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _taskRepo.Remove(task);

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

        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateDto)

        {
            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Tasks model = _mapper.Map<Tasks>(updateDto);


            model.UpdatedAt = DateTime.Now;
            model.DeadLine = updateDto.DeadLine;
            await _taskRepo.Update(model);
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);

        }

    }
}
