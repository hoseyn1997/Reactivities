using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Text.RegularExpressions;
using Application.Comments;
using Application.Comments.PrivateComments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;
public class PaymentController : BaseController
{
    private readonly IUserAccessor _userAccessor;
    private readonly DataContext _context;
    public PaymentController(DataContext context, IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    [HttpPost("{Amount}")]
    public async Task<ActionResult<string>> Pay(int amount)
    {
        var username = _userAccessor.GetUserName();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

        var payment = new ZarinpalSandbox.Payment(amount);
        var res = payment.PaymentRequest("شارژ حساب", "http://localhost:3000/PaymentResult/" + username);

        if (res.Result.Status == 100)
        {
            var redirectionURL = "https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority;
            return redirectionURL;
        }

        return "Failed to connect to the bank";
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<string>> PaymentResult([FromQuery] string Authority,
        [FromQuery] string status, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (Authority != ""
            && status.ToString().ToLower() == "ok"
            && username == _userAccessor.GetUserName())
        {
            var payment = new ZarinpalSandbox.Payment(150000);
            var res = payment.Verification(Authority).Result;
            if (res.Status == 100)
            {
                return res.RefId.ToString();
            }
            else
            {
                return "تراکنش ناموفق";
            }
        }
        return null;
    }
}