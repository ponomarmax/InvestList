using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Common;
using Core.Interfaces;

namespace InvestList.Filters
{
    public class EmailConfirmedAuthorizeFilter(IUserRepository repository, IPostRepository inVepository)
        : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = Utils.GetUserId(context.HttpContext.User);
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", new {area="Identity"});
                return;
            }

            // Check if the email is confirmed (replace this with your actual email confirmation check)
            var emailConfirmed = repository.IsEmailConfirmed(userId).GetAwaiter().GetResult();
            if (!emailConfirmed)
            {
                context.Result = new ForbidResult();
            }

            var postId = context.HttpContext.Request.RouteValues["id"] as string;
            if (!string.IsNullOrEmpty(postId))
            {
                if (!inVepository.IsOwnerOfPost(userId, postId).GetAwaiter().GetResult())
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
