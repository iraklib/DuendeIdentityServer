using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;


namespace AuthServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        [HttpPost(Name = "Login")]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            // Validate the login input model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Perform the login logic using Duende IdentityServer
            // ...

            // Redirect to the original URL after successful login
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (context != null)
            {
                return Redirect(model.ReturnUrl);
            }

            // Redirect to a default URL if no return URL is provided
            return Redirect("~/");
        }
    }



}
