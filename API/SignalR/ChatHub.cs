using System.Text.RegularExpressions;
using Application.Comments;
using Application.Comments.PrivateComments;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
        public ChatHub(IMediator mediator, IUserAccessor userAccessor, DataContext context)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task SendComment(Application.Comments.Create.Command command)
        {
            var comment = await _mediator.Send(command);
            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var result = await _mediator.Send(new Application.Comments
                .List.Query
            { ActivityId = Guid.Parse(activityId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}