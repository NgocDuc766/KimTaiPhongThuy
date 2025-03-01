using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KimTaiPhongThuy.DataAccess.Atrributes
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userName = context.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                context.Result = new RedirectToActionResult("Account", "Authentication", null); // Chuyển hướng đến trang login
                context.HttpContext.Session.SetString("ErrorMessage", "Bạn cần phải đăng nhập trước.");
            }
            base.OnActionExecuting(context);
        }
    }
}
