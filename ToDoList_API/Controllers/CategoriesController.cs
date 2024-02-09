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


    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ITaskRepositorio _taskRepo;
        private readonly ICategoriesRepositorio _categoriesRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;


        public CategoriesController(ILogger<CategoriesController> logger, ITaskRepositorio taskrepo,ICategoriesRepositorio categoriesRepo, IMapper maper)
        {
            _logger = logger;
            _taskRepo = taskrepo;
            _mapper = maper;
            _response = new();
            _categoriesRepo = categoriesRepo;
        }




        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Obtener las Categorias");

                IEnumerable<Categories> taskList = await _categoriesRepo.GetAll();

                _response.Result = _mapper.Map<IEnumerable<CategoriesDto>>(taskList);
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


        [HttpGet("id:int", Name = "GetCategories")]  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCategories(int id)
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
                
                var category = await _categoriesRepo.GetOne(v => v.Id == id);

                if (category == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<TaskDto>(category);
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

        public async Task<ActionResult<ApiResponse>> CreateCategories([FromBody] CategoriesCreateDto createDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _categoriesRepo.GetOne(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ExistName", "La categoria con ese nombre ya existe ");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }


                Categories model = _mapper.Map<Categories>(createDto);


               
                await _categoriesRepo.Create(model);
                _response.Result = model;
                _response.statusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetCategories", new { id = model.Id }, _response);
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

        public async Task<IActionResult> DeleteCategories(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var  category = await _categoriesRepo.GetOne(v => v.Id == id);
                if (category == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _categoriesRepo.Remove(category);

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

        public async Task<IActionResult> UpdateCategories(int id, [FromBody] CategoriesUpdateDto updateDto)

        {
            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Categories model = _mapper.Map<Categories>(updateDto);


            
            await _categoriesRepo.Update(model);
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);

        }

    }
}
