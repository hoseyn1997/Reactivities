using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments.PrivateComments;

public class Create
{
    public class Command : IRequest<Result<CommentDto>>
    {
        public string Body { get; set; }
        public string TargetUsername { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDto>>
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

        public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var targetUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);
            if (targetUser == null) return null;

            var Authoruser = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            var privateComment = new PrivateComment
            {
                Author = Authoruser,
                Target = targetUser,
                Body = request.Body
            };

            targetUser.PrivateComments.Add(privateComment);

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Result<CommentDto>
                .Success(_mapper.Map<CommentDto>(privateComment));

            return Result<CommentDto>.Failure("Failed to add private comment");
        }
    }
}
