using CrudAPI.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IConfiguration    _configuration;
        IStudentRepository _studentRepository=null;
        public StudentController(IConfiguration configuration, IStudentRepository studentRepository)
        {
            _configuration = configuration;
            _studentRepository = studentRepository;
        }
        [HttpGet]
        [Route("Gets1")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Gets1()
        {
            var list = await _studentRepository.Gets1();
            return Ok(list);

        }

        [HttpGet]
        [Route("Gets2")]

        public async Task<IActionResult> Gets2()
        {
            var list = await _studentRepository.Gets2();
            return Ok(list);

        }
    }
}
