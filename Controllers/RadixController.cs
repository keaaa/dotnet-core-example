using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using oauthExample.Utils;


namespace oauthExample.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles="Radix")]
    [ApiController]
    public class RadixController : ControllerBase
    {
        private readonly IAzureTokenService _azureTokenService;

        public RadixController(IAzureTokenService azureTokenService)
        {
            _azureTokenService = azureTokenService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "only", "radixers", "should","see", "this" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var secret = await GetSecretAsync("super-secret");

            return Ok(secret);
        }

        private async Task<string> GetSecretAsync(string secretName)
        {
            var kv = new KeyVaultClient(_azureTokenService.GetApplicationToken);
            var sec = await kv.GetSecretAsync("https://omnia-test.vault.azure.net/", secretName);

            return sec.Value;
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
