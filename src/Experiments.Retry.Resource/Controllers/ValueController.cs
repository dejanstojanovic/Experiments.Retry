using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Experiments.Retry.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
       readonly ILogger<ValueController> _logger;

        public ValueController(ILogger<ValueController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<String> Get(String text)
        {
            //Try to get the retry header value
            var attempt = Request.Headers["X-Retry"];

            if (int.TryParse(attempt, out int attemptNo))
            {
                _logger.LogWarning($"RETRY: {attemptNo}");
                if (attemptNo < 3)
                {
                    //First or second retry
                    return NotFound();
                }
                else
                {
                    //Third retry
                    return Ok(new string(text.ToUpper().Reverse().ToArray()));
                }
            }

            //Not a retry - initial call
            return StatusCode(500);
        }
    }
}
