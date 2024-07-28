using Application.Comments.PrivateComments;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.SignalR;

public class PrivateChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IUserAccessor _userAccessor;
    private readonly DataContext _context;
    public PrivateChatHub(IMediator mediator, IUserAccessor userAccessor, DataContext context)
    {
        _context = context;
        _userAccessor = userAccessor;
        _mediator = mediator;
    }

    public async Task SendPrivateComment(Create.Command command)
    {



        var authorUser = await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());
        var targetUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == command.TargetUsername);
        var comment = await _mediator.Send(command);

        // var clients = Clients.Users(targetUser.Id, authorUser.Id);
        // await Clients.Users(targetUser.Id, authorUser.Id)
        // .SendAsync("ReceivePrivateComment", comment.Value);

        var groupName = String.Compare(authorUser.UserName, targetUser.UserName) > 0
            ? authorUser.UserName + "+" + targetUser.UserName
            : targetUser.UserName + "+" + authorUser.UserName;

        await Clients.Group(groupName)
            .SendAsync("ReceivePrivateComment", comment.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var targetUsername = httpContext.Request.Query["targetUsername"];
        var authorUser = await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

        var groupName = String.Compare(authorUser.UserName, targetUsername) > 0
            ? authorUser.UserName + "+" + targetUsername
            : targetUsername + "+" + authorUser.UserName;

        var authorUsername = authorUser.UserName;
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var result = await _mediator.Send(new List.Query { TargetUsername = targetUsername });
        await Clients.Caller.SendAsync("LoadPrivateComments", result.Value);
    }
}