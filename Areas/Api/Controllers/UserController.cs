using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using API.Controllers;
using API.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private IConfiguration Configuration;
        public UserController(IConfiguration config)
        {
            Configuration = config;
        }

        public IActionResult GetListByGroupPB([FromQuery] int IdCoQuan=0, int IdPhongBan=0)
        {
            string Msg = "Lấy thông tin thành công";
            var ListItems = USUsersService.GetListByGroupPB(7, IdCoQuan,IdPhongBan);
            return Ok(new MsgSuccess() { Data = ListItems, Msg = Msg });
        }

        [HttpPost]
        [CustomAuthorizeAttribute]
        public IActionResult Info()
        {
            string Msg = "Lấy thông tin tài khoản thành công";            
            int Id = int.Parse(HttpContext.Request.Headers["Id"]);
            USUsers Item = USUsersService.GetItem(Id);

            UserToken data = USUsersService.BuildUserToken(Item);
            return Ok(new MsgSuccess() { Data = data, Msg  = Msg });
        }
    }
}
