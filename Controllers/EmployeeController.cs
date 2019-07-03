using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oauthExample.Utils;

namespace oauthExample.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IAzureTokenService _adminService;

        public EmployeeController(IAzureTokenService adminService)
        {
            _adminService = adminService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[]{"value1", "ValueTask"};
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var graphApi = "https://graph.microsoft.com";
            var currentUser = "/v1.0/me?$select=id,displayName,mail,officeLocation,userPrincipalName";

            var delegatedToken = await _adminService.GetDelegateAccessTokenForResource(graphApi);
            
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(graphApi)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", delegatedToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var userResponse = await httpClient.GetAsync(currentUser);
            var user = await userResponse.Content.ReadAsStringAsync();

            return Ok(user);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
