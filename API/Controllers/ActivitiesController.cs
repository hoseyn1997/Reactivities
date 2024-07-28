using Domain;
using Microsoft.AspNetCore.Mvc;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using Application.Core;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet("[action]")] // api/activities/getactivities
        public async Task<IActionResult> GetActivities([FromQuery] ActivityParams param)
        {
            return HandlePagedReslut(await Mediator.Send(new List.Query { Params = param }));
        }

        [HttpGet("[action]/{id}")] // api/activities/getactivity/id
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleReslut(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpPost("[action]")] //api/activities/CreateActivity/Activity
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleReslut(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("[action]/{id}")] //api/activities/DeleteActivity/id
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleReslut(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("[action]/{id}")] //api/activities/EditActivity/id
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleReslut(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleReslut(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
        }
    }
}