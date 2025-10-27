using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DuThaoVanBan;
using API.Areas.Admin.Models.DuThaoVanBanGopY;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using API.Areas.Admin.Models.USUsers;
using API.Models.MyHelper;
using API.Models;
using GoogleService = API.Models.MyHelper.GoogleService;
namespace API.Controllers
{
    public class DuThaoVanBanController : Controller
    {
        private string controllerName = "DuThaoVanBanController";
        private string controllerSecret;
        private readonly GoogleService Google;
        private IConfiguration Configuration;       
        public DuThaoVanBanController(IConfiguration config, GoogleService google)
        {
            Configuration = config;
            controllerSecret = config["Security:SecretId"] + controllerName;
            Google = google;
        }
        public IActionResult Index([FromQuery] SearchDuThaoVanBan dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            dto.IdCoQuan = IdCoQuan;

            DuThaoVanBanModel data = new DuThaoVanBanModel() { SearchData = dto };
            
            data.ListItems = DuThaoVanBanService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();

            return View(data);
        }

        public IActionResult DuThaoVanBanGopY(int Id, string Ids)
        {
            DuThaoVanBanGopYModel data = new DuThaoVanBanGopYModel()
            {
                SearchData = new SearchDuThaoVanBanGopY() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "",IdDuThao= Id, IdsDuThao=Ids },
                Item = new DuThaoVanBanGopY() { IdDuThao = Id, IdsDuThao = Ids }
            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuThaoVanBanGopY(DuThaoVanBanGopY model)        
        {
                        
            DuThaoVanBanGopYModel data = new DuThaoVanBanGopYModel()
            {
                SearchData = new SearchDuThaoVanBanGopY() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" },
                Item = model
            };

            Boolean CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], model.Token, int.Parse(Configuration["flagDevCoQuan"].ToString()));
            
            USUsersLog userLog = new USUsersLog()
            {
                Browser = "DuThaoVanBan",
                Platform = "DuThaoVanBanGopY",
                IdUser = 0,
                LoginInfo = CheckGoogle.ToString(),
                Ip = "",
                Token = model.Token,
                Description = "",
            };

            USUsersService.SaveUserLog(userLog);

            if (!CheckGoogle)
            {
                TempData["MessageError"] = "Trình duyệt máy bạn Gửi yêu cầu thất bại. Xin vui lòng gửi lại";
            }
            else
            {

                if (ModelState.IsValid && CheckGoogle)
                {
                    model.Id = 0;
                    model.CreatedBy = 0;
                    model.ModifiedBy = 0;

                    int IdDC = Int32.Parse(MyModels.Decode(model.IdsDuThao, controllerSecret).ToString());
                    if (IdDC > 0)
                    {

                        try
                        {
                            DuThaoVanBanGopYService.SaveItem(model);
                            TempData["MessageSuccess"] = "Gửi dự thảo văn bản thành công";
                        }
                        catch
                        {
                            TempData["MessageError"] = "Gửi dự thảo văn bản Thất bại. Xin vui lòng gửi lại";
                        }
                    }
                    else
                    {
                        TempData["MessageError"] = "Gửi dự thảo văn bản Thất bại. Xin vui lòng gửi lại ...";
                    }

                    return RedirectToAction("Index");

                }
            }
            
            return View(data);
        }
    }
}
