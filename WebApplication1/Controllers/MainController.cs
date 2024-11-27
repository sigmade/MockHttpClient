using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ApiService _apiService;

        public MainController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet()]
        public async Task<ActionResult> Get()
        {

            var result = await _apiService.GetPostsAsync();

            return Ok(result);
        }
    }
}
