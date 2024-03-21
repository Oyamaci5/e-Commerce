using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ParanumusTask.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Hello, World!");
		}

	}
}
