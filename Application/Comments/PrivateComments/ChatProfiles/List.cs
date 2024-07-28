using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments.PrivateComments.ChatProfiles;

public class List
{
    public class Query : IRequest<Result<List<string>>>
    {
    }
    public class Handler : IRequestHandler<Query, Result<List<string>>>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            var privateComments1 = await _context.PrivateComments
                .Where(x => x.Target.Id == user.Id)
                .Select(x => x.Author.UserName).ToListAsync();
            var privateComments2 = await _context.PrivateComments
                .Where(x => x.Author.Id == user.Id)
                .Select(x => x.Target.UserName).ToListAsync();

            var finalPrivateComments = privateComments1.Union(privateComments2).ToList();

            return Result<List<string>>.Success(finalPrivateComments);


        }
    }
}
