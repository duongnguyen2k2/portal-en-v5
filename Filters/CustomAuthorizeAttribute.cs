using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using API.Areas.Admin.Models.Dashboard;
using System.Linq;
using API.Areas.Admin.Models.USUsers;

internal class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Xử lý ở đây
        if(context.HttpContext.Request.Headers["Authorization"].Any())
        {
            UserToken UserToken;
            context.HttpContext.Request.Headers.Add("Id", "");
            context.HttpContext.Request.Headers.Add("IdGroup", "0");
            context.HttpContext.Request.Headers.Add("UserName", "");
            context.HttpContext.Request.Headers.Add("Email", "");
            context.HttpContext.Request.Headers.Add("IdCoQuan", "");
            context.HttpContext.Request.Headers.Add("IdHuyen", "");
            context.HttpContext.Request.Headers.Add("IdTinhThanh", "");
            
            string Token = context.HttpContext.Request.Headers["Authorization"][0];
            if (!string.IsNullOrEmpty(Token))
            {
                //BearerToken a = 
                Token = Token.Replace("Bearer ","").Trim();
                Boolean isAuth =  USUsersService.ValidateToken(Token , out UserToken);
                if (isAuth)
                {
                    context.HttpContext.Request.Headers["Id"] = UserToken.Id.ToString();
                    context.HttpContext.Request.Headers["IdGroup"] = UserToken.IdGroup.ToString();
                    context.HttpContext.Request.Headers["UserName"] = UserToken.UserName;
                    context.HttpContext.Request.Headers["Email"] = UserToken.Email;
                    context.HttpContext.Request.Headers["IdCoQuan"] = UserToken.IdCoQuan.ToString();
                    context.HttpContext.Request.Headers["IdHuyen"] = UserToken.IdHuyen.ToString();
                    context.HttpContext.Request.Headers["IdTinhThanh"] = UserToken.IdTinhThanh.ToString();
                }
                else
                {
                    context.Result = new UnauthorizedObjectResult(string.Empty);
                }
            }
            
            
        }
        else
        {
            context.Result = new UnauthorizedObjectResult(string.Empty);
        }
        
        

    }
}