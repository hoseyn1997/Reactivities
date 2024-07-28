
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Payment;

public class Result
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Username { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
        private readonly HttpContent _httpContent;
        public Handler(DataContext context, IUserAccessor userAccessor, HttpContent httpContent)
        {
            _httpContent = httpContent;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var username = _userAccessor.GetUserName();
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == username);
            // if(_httpContent.re)
            return null;

        }
    }
}
