using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Experiments.Retry.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {

        readonly IHttpClientFactory _httpClientFactory;

        public SampleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<String>> Get(String word)
        {
            if (!String.IsNullOrWhiteSpace(word))
            {
                var result = await _httpClientFactory
                    .CreateClient(ServiceClientNames.ResourceService)
                    .GetAsync($"/api/value?text={word}");

                return Ok(await result.Content.ReadAsStringAsync());
            }
            return NoContent();
        }

      
    }
}
