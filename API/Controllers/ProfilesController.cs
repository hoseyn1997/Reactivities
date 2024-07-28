using Application.Activities;
using Application.Comments.PrivateComments.ChatProfiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ProfilesController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleReslut(await Mediator.Send(new Application.Profiles.Details.Query { Username = username }));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string Predicate)
        {
            return HandleReslut(await Mediator
                .Send(new ListActivities.Query { Username = username, Predicate = Predicate }));
        }
        
        [HttpGet("headers")]
        public async Task<IActionResult> GetPrivateHeaders()
        {
            return HandleReslut(await Mediator.Send(new
                Application.Comments.PrivateComments.ChatProfiles.List.Query()));
        }
    }
}