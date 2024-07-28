using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Payment;

public class Create
{
    public class Command : IRequest<Result<string>>
    {
        public int Amount { get; set; }
    }
    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var username = _userAccessor.GetUserName();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var payment = new ZarinpalSandbox.Payment(request.Amount);
            var res = payment.PaymentRequest("شارژ حساب", "http://localhost:3000/PaymentResult/" + username);
            if (res.Result.Status == 100)
            {
                var redirectionURL = "https://www.sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                return Result<string>.Success(redirectionURL);
            }
            return Result<string>.Failure("Failed to connect to the bank");
        }
    }
}
