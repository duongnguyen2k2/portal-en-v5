using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.USGroups;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DMChucVu;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using API.Areas.Admin.Models.DMDanToc;
using API.Areas.Admin.Models.DMTonGiao;
using API.Areas.Admin.Models.DMTrinhDo;
using System.Globalization;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class USUsersController : Controller
    {
        private IConfiguration Configuration;
        private string PasswordDefault = "@ekisW@!123DoiLai";

        private string controllerName = "USUsersController";
        private string controllerSecret;

        public USUsersController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
            Configuration = config;
        }
        public IActionResult Index([FromQuery] SearchUSUsers dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int TotalItems = 0;
            if (dto.IdCoQuan == 0) {
                dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString());
            }
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsersModel data = new USUsersModel() { SearchData = dto };
            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()));
            data.ListItems = USUsersService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            data.ListItemsGroups = USGroupsService.GetListSelectItems(0, MyInfo);

            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }            
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            HttpContext.Session.SetString("STR_Action_Link_" + ControllerName, Request.QueryString.ToString());
            return View(data);
        }

        public IActionResult SaveItem(string Id = null, int IdCoQuan = 0, string UserName = "", string TenCoQuan = "")
        {
            USUsersModel data = new USUsersModel();
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int IdCoQuanSave = int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString());

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchUSUsers() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsGroups = USGroupsService.GetListSelectItems(0, MyInfo);
            data.ListItemsStatus = USUsersService.GetStatusSelectItems();

            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()));
            data.ListDMChucVu = DMChucVuService.GetListSelectItems(true, IdCoQuanSave);
            data.ListDMDanToc = DMDanTocService.GetListSelectItems();
            data.ListDMTonGiao = DMTonGiaoService.GetListSelectItems();
            data.ListDMTrinhDo = DMTrinhDoService.GetListSelectItems();
            data.ListItemDangVien = USUsersService.GetDangVienSelectItems();

            if (IdDC == 0)
            {
                if (IdCoQuan > 0)
                {
                    data.Item = new USUsers()
                    {
                        IdCoQuan = IdCoQuan,
                        UserName = UserName,
                        UserCode = UserName,
                        Gender = 2,
                        FullName = "Quản trị " + TenCoQuan,
                        IdRegency = 3,
                        Email = UserName + "_admin" + "@gmail.com",
                        BirthdayNamSinhShow = "1990",
                        PasswordShow = API.Models.MyHelper.StringHelper.PasswordGenerate(15),
                        IdGroup = 2
                    };
                }
                else
                {
                    data.Item = new USUsers()
                    {
                        IdCoQuan = 0,
                        IdRegency = 3,
                        Gender = 2,
                        Email = "_admin" + "@gmail.com",
                        BirthdayNamSinhShow = "1990",
                        PasswordShow = API.Models.MyHelper.StringHelper.PasswordGenerate(15),
                        IdGroup = 2
                    };
                }

            }
            else
            {
                data.Item = USUsersService.GetItem(IdDC, controllerSecret);
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(USUsers model)
        {
            PasswordDefault = API.Models.MyHelper.StringHelper.PasswordGenerate(15);
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            int IdCoQuanSave = int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString());
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret).ToString());

            USUsersModel data = new USUsersModel() { Item = model };
            DateTime Birthday = DateTime.Now;
            string MsgSuccess = "";
            
            if (model.NamSinh == 1)
            {
                if (model.BirthdayNamSinhShow == null || model.BirthdayNamSinhShow == "")
                {
                    TempData["MessageError"] = "Năm sinh không được để trống";
                    return View(data);
                }
                else
                {
                    int YearTest = Int32.Parse(Birthday.ToString("yyyy"));
                    model.Birthday = Int32.Parse(model.BirthdayNamSinhShow);
                    if ((model.Birthday > (YearTest - 10)) || model.Birthday < 1900)
                    {
                        TempData["MessageError"] = "Năm sinh không hợp lệ";
                        return View(data);
                    }
                    else
                    {
                        model.Birthday = Int32.Parse(model.BirthdayNamSinhShow + "0101");
                    }
                }
                
                
            }
            else {
                if (model.BirthdayShow != null)
                {
                    Birthday = DateTime.ParseExact(model.BirthdayShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.Birthday = Int32.Parse(Birthday.ToString("yyyyMMdd"));
                }
            }

            if (model.Birthday > Int32.Parse(DateTime.Now.ToString("yyyyMMdd"))) {
                TempData["MessageError"] = "Ngày sinh không được lớn hơn ngày hiện tại";
                data.ListItemsGroups = USGroupsService.GetListSelectItems(0, MyInfo);
                data.ListItemsStatus = USUsersService.GetStatusSelectItems();
                data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()));
                data.ListDMChucVu = DMChucVuService.GetListSelectItems(true, IdCoQuanSave);
                data.ListDMDanToc = DMDanTocService.GetListSelectItems();
                data.ListDMTonGiao = DMTonGiaoService.GetListSelectItems();
                data.ListDMTrinhDo = DMTrinhDoService.GetListSelectItems();
                data.ListItemDangVien = USUsersService.GetDangVienSelectItems();
            }
            else if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    if (model.Id == 0)
                    {
                        if (model.PasswordShow == null || model.PasswordShow == "")
                        {
                            MsgSuccess = "Mật khẩu đã được đổi thành công. Mật khẩu đăng nhập là: <strong>" + PasswordDefault + "</strong>";
                            model.Password = BCrypt.Net.BCrypt.HashPassword(PasswordDefault + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);//USUsersService.GetMD5("Abc@123");
                        }
                        else
                        {
                            model.Password = BCrypt.Net.BCrypt.HashPassword(model.PasswordShow + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);//USUsersService.GetMD5("Abc@123");
                        }

                    }

                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic DataSave = USUsersService.SaveItem(model);

                    if (DataSave.N > 0)
                    {                        

                        string Str_Url = HttpContext.Session.GetString("STR_Action_Link_" + ControllerName);
                        if (Str_Url != null && Str_Url != "")
                        {
                            return Redirect("/Admin/USUsers/Index" + Str_Url);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        if (DataSave.N == -1)
                        {
                            TempData["MessageError"] = "Email đã tồn tại";
                        }
                        else if (DataSave.N == 0)
                        {
                            TempData["MessageError"] = "Username đã tồn tại";
                        }

                        data.ListItemsGroups = USGroupsService.GetListSelectItems();
                        data.ListItemsStatus = USUsersService.GetStatusSelectItems();
                        data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()));
                        data.ListDMChucVu = DMChucVuService.GetListSelectItems();
                        data.ListDMDanToc = DMDanTocService.GetListSelectItems();
                        data.ListDMTonGiao = DMTonGiaoService.GetListSelectItems();
                        data.ListDMTrinhDo = DMTrinhDoService.GetListSelectItems();
                        data.ListItemDangVien = USUsersService.GetDangVienSelectItems();
                        return View(data);
                    }

                }

            }
            else
            {
                data.ListItemsGroups = USGroupsService.GetListSelectItems();
                data.ListItemsStatus = USUsersService.GetStatusSelectItems();
                data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()));
                data.ListDMChucVu = DMChucVuService.GetListSelectItems(true, IdCoQuanSave);
                data.ListDMDanToc = DMDanTocService.GetListSelectItems();
                data.ListDMTonGiao = DMTonGiaoService.GetListSelectItems();
                data.ListDMTrinhDo = DMTrinhDoService.GetListSelectItems();
                data.ListItemDangVien = USUsersService.GetDangVienSelectItems();
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsers item = new USUsers() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic DataDelete = USUsersService.DeleteItem(item);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Xóa Không thành công";
                    return Json(new MsgError());
                }

            }
            catch
            {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }


        }

        [ValidateAntiForgeryToken]
        public ActionResult RessetPassword(string Id)
        {
			PasswordDefault = API.Models.MyHelper.StringHelper.PasswordGenerate(15);
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsers item = new USUsers() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.Password = BCrypt.Net.BCrypt.HashPassword(PasswordDefault + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);//USUsersService.GetMD5("Abc@123");
                    dynamic DataDelete = USUsersService.ChangePassword(item.Id, item.Password);
                    string Msg = "Cập nhật mật khẩu thành công. Mật khẩu mặc định là<br /><strong>" + PasswordDefault + "<strong>";
                    return Json(new MsgSuccess() { Msg = Msg });
                }
                else
                {
                    string Msg = "Cập nhật mật khẩu Không thành công. Xin vui lòng làm lại";
                    return Json(new MsgError() { Msg = Msg });
                }

            }
            catch
            {
                string Msg = "Cập nhật mật khẩu Không thành công. Xin vui lòng làm lại.";
                return Json(new MsgError() { Msg = Msg });
            }


        }


        public IActionResult GetListByCoQuan(int IdCoQuan)
        {            
            List<USUsers> ListItem = new List<USUsers>();           
            if (IdCoQuan > 0)
            {
                ListItem = USUsersService.GetListByCoQuan(IdCoQuan);
            }
            return Json(ListItem);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Byte Status, string flag = "Status")
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            USUsers item = new USUsers() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            string Msg = "Trạng Thái";
            if (flag == "DangVien")
            {
                Msg = "Đảng Viên";
            }

            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = USUsersService.UpdateStatus(item, flag);
                    TempData["MessageSuccess"] = "Cập nhật " + Msg + " thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật " + Msg + " Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật " + Msg + " không thành công";
                return Json(new MsgError());
            }
        }
    }
}