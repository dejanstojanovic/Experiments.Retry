using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var attempt = Request.Headers["X-Retry"];

            if (int.TryParse(attempt, out int attemptNo))
            {
                _logger.LogWarning($"RETRY: {attemptNo}");
                if (attemptNo < 3)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new string(text.ToUpper().Reverse().ToArray()));
                }
            }
            return NotFound();
           
        }

 
    }
}
