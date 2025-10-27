using API.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.DMCoQuan;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.EMMA;

namespace API.Areas.Api.Controllers
{
    [Area("Api")]
    public class AccountController : Controller
    {
        private IConfiguration Configuration;
        public AccountController(IConfiguration config)
        {
            Configuration = config;
        }
        
        [HttpPost]
        public IActionResult Login([FromBody] AccountLogin model)
        {
            string Msg = "";
            model.UserName = model.UserName.Trim();
            

            if (model.UserName == null || model.Password == null || model.UserName.Trim() == "" || model.Password.Trim() == "")
            {

                Msg = "Thông tin đăng nhập không được để trống";
            }
            else
            {
                USUsersLogin Item = new USUsersLogin();
                try
                {
                    Item = USUsersService.CheckLogin(model.UserName);

                    if (Item == null)
                    {
                        Msg = "Tài khoản hoặc mật khẩu không chính xác";
                    }
                    else
                    {                        
                        bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password + Configuration["Security:SecretPassword"], Item.Password);
                        if (validPassword && Item.IdGroup==8)// Tài khoản người dân mới được login
                        {
                            DMCoQuan ItemCoQuan = DMCoQuanService.GetItemLevel(Item.IdCoQuan);
                            Item.CQLevel = ItemCoQuan.Level;
                            Item.CategoryId = ItemCoQuan.CategoryId;
                            UserToken UserToken = new UserToken()
                            {
                                Id = Item.Id,
                                Email = Item.Email,
                                UserName = Item.UserName,
                                IdCoQuan = Item.IdCoQuan,
                                IdHuyen = Item.IdHuyen,
                                IdTinhThanh = Item.IdTinhThanh,
                                IdGroup = Item.IdGroup
                            };
                            string Token = USUsersService.GenerateToken(UserToken,50);
                            return Json(new MsgSuccess() { Data = Token, Msg = Msg });
                        }
                        else
                        {
                            Msg = "Tài khoản hoặc mật khẩu không chính xác";
                        }

                    }
                }
                catch (Exception e)
                {
                    Msg = e.Message;
                }


            }

            return Json(new MsgError() { Data = model, Msg = Msg });
        }
    }
}
