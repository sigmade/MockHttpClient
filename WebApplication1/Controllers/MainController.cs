using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ArticleService _apiService;

        public MainController(ArticleService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet()]
        public async Task<ActionResult> Get()
        {

            //var result = await _apiService.GetPostsAsync();

            return Ok();
        }
    }
}
