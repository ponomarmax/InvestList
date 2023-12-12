using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Interfaces;
using Common;

namespace WebApplication1.Filters
{
    public class EmailConfirmedAuthorizeFilter: IAuthorizationFilter
    {
        private readonly IUserRepository _repository;

        public EmailConfirmedAuthorizeFilter(IUserRepository repository)
        {
            _repository = repository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = Utils.GetUserId(context.HttpContext.User);
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Check if the email is confirmed (replace this with your actual email confirmation check)
            var emailConfirmed = _repository.IsEmailConfirmed(userId).GetAwaiter().GetResult();
            if (!emailConfirmed)
            {
                context.Result = new ForbidResult();
            }
        }
    }
    public class EmailConfirmedAuthorizeAttribute: TypeFilterAttribute
    {
        public EmailConfirmedAuthorizeAttribute() : base(typeof(EmailConfirmedAuthorizeFilter))
        {
        }
    }
    }
