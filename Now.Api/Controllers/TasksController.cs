using Microsoft.AspNetCore.Mvc;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Now.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly Application.Services.TasksService _tasksService;

        public TasksController(Application.Services.TasksService tasksService)
        {
            _tasksService = tasksService;
        }

        // GET: api/<TasksController>
        [HttpGet]
        public ActionResult<IEnumerable<Domain.Entities.Task.Dto>> Get()
        {
            try
            {
                return Ok(_tasksService.GetAllTasks());
            }
            catch(Exception)
            {
                return Problem();
            }
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public ActionResult<Domain.Entities.Task.Dto> Get(string id)
        {
            try
            {
                Domain.Entities.Task.Dto? dto = _tasksService.Find(id);

                if (dto == null)
                {
                    return NotFound();
                }

                return Ok(dto);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // POST api/<TasksController>
        [HttpPost]
        public ActionResult Post([FromBody] Application.Services.TasksService.CreateTaskModel model)
        {
            try
            {
                _tasksService.Create(model);
                return NoContent();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // POST api/<TasksController>
        [HttpPost("{id}/done")]
        public ActionResult Done(string id)
        {
            try
            {
                _tasksService.Done(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // POST api/<TasksController>
        [HttpPost("{id}/undone")]
        public ActionResult Undone(string id)
        {
            try
            {
                _tasksService.Undone(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Application.Services.TasksService.UpdateTaskModel model)
        {
            try
            {
                _tasksService.Update(id, model);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                _tasksService.Remove(id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
