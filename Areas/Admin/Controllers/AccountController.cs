using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.USGroups;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using API.Areas.Admin.Models.DMChucVu;
using API.Areas.Admin.Models.DMCoQuan;
using API.Models;
using Microsoft.Extensions.Configuration;
using API.Models.MyHelper;
using DocumentFormat.OpenXml.EMMA;
using GoogleService = API.Models.MyHelper.GoogleService;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private IConfiguration Configuration;
        private readonly IGoogleService Google;  // Inject Google qua DI
        public AccountController(IConfiguration config, IGoogleService google)
        {
            Configuration = config;
            Google = google;  // Nhận từ DI container
        }
        public IActionResult Index()
        {
            return RedirectToAction("Info");
            
        }
        public IActionResult ChangePassword()
        {
            ChangePassword Model = new ChangePassword() {
                Id = int.Parse(HttpContext.Request.Headers["Id"])
            };
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePassword Model)
        {
            int IDUSER = int.Parse(HttpContext.Request.Headers["Id"]);
            try
            {

                
                Model.NewPassword = Model.NewPassword.Trim();
                Model.RePassword = Model.RePassword.Trim();
                Boolean StrongPass = USUsersService.ValidateStrongPassword(Model.NewPassword);
                if (Model.Id == IDUSER)
                {
                    if (StrongPass)
                    {
                        if (Model.NewPassword == Model.RePassword)
                        {
                            string UserName = HttpContext.Request.Headers["UserName"];
                            USUsersLogin Item = USUsersService.CheckLogin(UserName);
                            bool validPassword = BCrypt.Net.BCrypt.Verify(Model.OldPassword + Configuration["Security:SecretPassword"], Item.Password);
                            if (validPassword)
                            {
                                string new_pass = BCrypt.Net.BCrypt.HashPassword(Model.NewPassword + Configuration["Security:SecretPassword"], BCrypt.Net.SaltRevision.Revision2A);
                                var result = USUsersService.ChangePassword(IDUSER, new_pass);
                                TempData["MessageSuccess"] = "Thay đổi Mật khẩu thành công";                                
                            }
                            else
                            {
                                TempData["MessageError"] = "Mật khẩu cũ không chính xác";                                
                            }
                        }
                        else
                        {
                            TempData["MessageError"] = "Mật khẩu mới và mật khẩu Nhập lại không giống nhau";                            
                        }
                    }
                    else
                    {
                        TempData["MessageError"] = "Mật khẩu quá đơn giản. Độ dài Mật khẩu phải lới hơn 7, có các ký tự đặc biệt";                        
                    }
                }
                else
                {
                    TempData["MessageError"] = "Thay đổi Mật khẩu Không thành công";
                }
                             
            }
            catch (Exception e)
            {
                TempData["MessageError"] = e.Message;                
            }
            return View(new ChangePassword());
        }

        public IActionResult Info()
        {

            USUsersModel data = new USUsersModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = int.Parse(HttpContext.Request.Headers["Id"]);
            data.SearchData = new SearchUSUsers() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsGroups = USGroupsService.GetListSelectItems();
            data.ListItemsStatus = USUsersService.GetStatusSelectItems();
            data.ListDMChucVu = DMChucVuService.GetListSelectItems();
            if (IdDC == 0)
            {
                data.Item = new USUsers();
            }
            else
            {
                data.Item = USUsersService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Info(USUsers model)
        {

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = int.Parse(HttpContext.Request.Headers["Id"]);
            USUsersModel data = new USUsersModel() { Item = model };
            data.ListItemsGroups = USGroupsService.GetListSelectItems();
            data.ListItemsStatus = USUsersService.GetStatusSelectItems();
            data.ListDMChucVu = DMChucVuService.GetListSelectItems();
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    if (model.Id > 0)
                    {
                        dynamic DataSave = USUsersService.SaveAccountInfo(model);                        
                        TempData["MessageSuccess"] = "Cập nhật thành công";                        
                    }
                    
                }
            }
            else
            {
                TempData["MessageError"] = "Cập nhật Không thành công";
            }
            return View(data);
        }

        

        public IActionResult UpdateDMCoQuan()
        {
            DMCoQuanModel data = new DMCoQuanModel();
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.Item = DMCoQuanService.GetItem(MyInfo.IdCoQuan, API.Models.Settings.SecretId + ControllerName.ToString());
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDMCoQuan(DMCoQuan model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            DMCoQuanModel data = new DMCoQuanModel();
            data.Item = model;
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Obj = DMCoQuanService.SaveItemInfo(model);

                    string cacheCode = HttpContext.Request.Host.Value.ToLower() + DateTime.Now.ToString("yyyyMMddhh");
                    HttpContext.Session.Remove(cacheCode);
                }
            }
            else { 
            
            }

            return View(data);
        }

        public IActionResult Login()
        {
            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("ThongTinCoQuan", ThongTinCoQuan);
            AccountLogin model = new AccountLogin();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountLogin model)
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));

            model.UserName = model.UserName.Trim();
            if (model.TokenRecaptcha != null)
            {
                model.TokenRecaptcha = model.TokenRecaptcha.Trim();
            }

            // Add by TayD - Start

            Boolean CheckGoogle = false;
            string flagtele = Configuration["flagTelegram"];
            var validIdsSection = Configuration.GetSection("RecaptchaSettings:ValidIdCoQuans");
            var validIdCoQuans = validIdsSection.Get<int[]>() ?? new int[] { 0 };

            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = HttpContext;            

            if (validIdCoQuans.Contains(ItemCoQuan.Id))

            {

                CheckGoogle = Google.CheckGoogleV2(Configuration["RecaptchaSettings:SecretKey"], model.TokenRecaptcha, ItemCoQuan.Id, 1);
                flagtele = "1";

            }
            else
            {

                CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], model.TokenRecaptcha, int.Parse(Configuration["flagCaptCha"].ToString()));

            }

            // Add by TayD - End;

            string smsTele = ItemCoQuan.Code + " - <b>" + HttpContext.Connection.RemoteIpAddress + "</b> - <b>" + model.UserName + "</b> - Login - Captcha:"+ CheckGoogle.ToString();
            USUsersLog userLog = new USUsersLog()
            {
                Browser = ItemCoQuan.Code+"-Admin",
                Platform = "Login",
                IdUser = 0,
                LoginInfo = CheckGoogle.ToString(),
                Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                Token = model.TokenRecaptcha,
                Description = smsTele,
            };
           
            USUsersService.SaveUserLog(userLog);

			/*  ---------------- Gửi thông báo Tele -----------------------  */
			Telegram Tele = new Telegram();
            Telegram.Info TeleInfo = new Telegram.Info() { Msg = smsTele };
            // Add by TayD 
            dynamic Data = Tele.SendNotification(TeleInfo, flagtele);
            // Add by TayD - End
            /*  ---------------- End Gửi thông báo Tele -----------------------  */

            if (!CheckGoogle)
            {
                TempData["MessageError"] = "Trình duyệt máy bạn Gửi yêu cầu thất bại. Xin vui lòng gửi lại";
            }
            else
            {             
                if (model.UserName == null || model.Password == null || model.UserName.Trim() == "" || model.Password.Trim() == "")
                {

                    TempData["MessageError"] = "Thông tin đăng nhập không được để trống";
                }
                else
                {
                    USUsersLogin Item = new USUsersLogin();
                    try
                    {
                        Item = USUsersService.CheckLogin(model.UserName);

                        if (Item == null)
                        {
                            TempData["MessageError"] = "Tài khoản hoặc mật khẩu không chính xác";

                            USUsersLog userLogError = new USUsersLog()
                            {
                                Browser = ItemCoQuan.Code + " - Admin",
                                Platform = "Login",
                                IdUser = 0,
                                LoginInfo = CheckGoogle.ToString(),
                                Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                                Token = model.TokenRecaptcha,
                                Description = smsTele + " - Pass:"+ model.Password,
                            };

                            USUsersService.SaveUserLog(userLogError);
                        }
                        else
                        {
                            if (Item.Id == 1)
                            {
                                Item.MasterId = Item.Id;
                            }

                            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password + Configuration["Security:SecretPassword"], Item.Password);
                            if (validPassword)
                            {
                                ItemCoQuan = DMCoQuanService.GetItemLevel(Item.IdCoQuan);
                                Item.CQLevel = ItemCoQuan.Level;
                                Item.CategoryId = ItemCoQuan.CategoryId;
                                HttpContext.Session.SetString("Login", JsonConvert.SerializeObject(Item));
                                return Redirect("/Admin/Articles/Index");
                            }
                            else
                            {
                                TempData["MessageError"] = "Tài khoản hoặc mật khẩu không chính xác";
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        HttpContext.Session.SetString("LoginError", HttpContext.Session.GetString("LoginError") + e.Message);
                    }


                }
            }
            return View(model);
        }

        public IActionResult LoginMaster([FromQuery] string Username = "")
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            if (MyInfo.MasterId == 1 && Username!="")
            {
                if (Username == MyInfo.UserName)
                {
                    USUsers MasterUser = USUsersService.GetItem(1);
                    Username = MasterUser.UserName;
                }
                USUsersLogin Item = new USUsersLogin();
                Item = USUsersService.CheckLogin(Username);

                if (Item == null)
                {
                    return Json("Tài khoản hoặc mật khẩu không chính xác");
                }
                else
                {
                    Item.MasterId = MyInfo.MasterId;
                    DMCoQuan ItemCoQuan = DMCoQuanService.GetItemLevel(Item.IdCoQuan);
                    Item.CQLevel = ItemCoQuan.Level;
                    HttpContext.Session.SetString("Login", JsonConvert.SerializeObject(Item));

                    return Redirect("/Admin/Articles/Index");
                }
            }
            else
            {
                return Json("dsds");
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
        public ActionResult NoAuthorize()
        {
            return View();
        }
    }
}