using Microsoft.AspNetCore.Mvc;

namespace StudyFiles.Core.Controllers
{
    [ApiController]
    [Route("healthCheck")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public StatusCodeResult IsAlive()
        {
            return Ok();
        }

        //[HttpGet]
        //public IHttpActionResult IsDatabaseAlive()
        //{
        //    const string query = "Select Top (5) * from Auth";
        //    var command = new SqlCommand(query);

        //    var result = DBHelper.ExecuteScalar(command);

        //    if (result is null)
        //        return NotFound();

        //    return Ok();
        //}
    }
}
