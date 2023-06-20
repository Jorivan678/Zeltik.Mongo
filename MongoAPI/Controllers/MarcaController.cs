using Microsoft.AspNetCore.Mvc;
using MongoAPI.Repositories;

namespace MongoAPI.Controllers
{
    /// <summary>
    /// Personal Testing purposes controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaRepository _repository;

        public MarcaController(IMarcaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var response = await _repository.GetAsync(id);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _repository.GetAsync();

            return Ok(response);
        }
    }
}
