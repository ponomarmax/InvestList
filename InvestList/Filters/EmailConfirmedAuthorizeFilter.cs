using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Interfaces;
using Common;
using Core.Interfaces;

namespace InvestList.Filters
{
    public class EmailConfirmedAuthorizeFilter: IAuthorizationFilter
    {
        private readonly IUserRepository _repository;
        private readonly IInvestAdRepository _inVepository;

        public EmailConfirmedAuthorizeFilter(IUserRepository repository, IInvestAdRepository inVepository)
        {
            _repository = repository;
            _inVepository = inVepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = Utils.GetUserId(context.HttpContext.User);
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", new {area="Identity"});
                return;
            }

            // Check if the email is confirmed (replace this with your actual email confirmation check)
            var emailConfirmed = _repository.IsEmailConfirmed(userId).GetAwaiter().GetResult();
            if (!emailConfirmed)
            {
                context.Result = new ForbidResult();
            }

            var postId = context.HttpContext.Request.RouteValues["id"] as string;
            if (!string.IsNullOrEmpty(postId))
            {
                if (!_inVepository.IsOwnerOfPost(userId, postId).GetAwaiter().GetResult())
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
