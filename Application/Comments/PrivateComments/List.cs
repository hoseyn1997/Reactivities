using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments.PrivateComments;

public class List
{
    public class Query : IRequest<Result<List<CommentDto>>>
    {
        public string TargetUsername { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
        }
        public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());
            var targetUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);
            var privateComments = await _context.PrivateComments
                .Where(x =>
                (x.Author.Id == currentUser.Id && x.Target.Id == targetUser.Id)
                ||
                (x.Author.Id == targetUser.Id && x.Target.Id == currentUser.Id))
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

            return Result<List<CommentDto>>.Success(privateComments);
        }
    }
}
