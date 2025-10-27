using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.HopTrucTuyen;
using API.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using API.Areas.Admin.Models.DMCoQuan;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HopTrucTuyenController : Controller
    {
        private string controllerName = "HopTrucTuyenController";
        private string controllerSecret;
        public HopTrucTuyenController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index([FromQuery] SearchHopTrucTuyen dto)
        {
            int TotalItems = 0;
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            HopTrucTuyenModel data = new HopTrucTuyenModel() { SearchData = dto };

            data.ListItems = HopTrucTuyenService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
            data.ListDMCoQuan = DMCoQuanService.GetListByParent(0,int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()),false);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            HopTrucTuyenModel data = new HopTrucTuyenModel();

            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());

            data.SearchData = new SearchHopTrucTuyen() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

            if (IdDC <=0)
            {
                data.Item = new HopTrucTuyen() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") , PublishDown = DateTime.Now, PublishDownShow = DateTime.Now.ToString("dd/MM/yyyy") };
            }
            else
            {
                data.Item = HopTrucTuyenService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
            }
            data.ListCat = HopTrucTuyenService.GetListCat();
            data.ListDMCoQuan = DMCoQuanService.GetListByParent(0, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()),false);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(HopTrucTuyen model)
        {

            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            HopTrucTuyenModel data = new HopTrucTuyenModel() { Item = model };
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

                    string DomainFolderUpload = HttpContext.Session.GetString("DomainFolderUpload").ToString();
                    string DomainName = HttpContext.Session.GetString("DomainName").ToString();
                    string CategoryId = HttpContext.Session.GetString("CategoryId").ToString();

                    if (CategoryId == "2")
                    {
                        if (DomainName.Contains("daklak.gov.vn"))
                        {
                            if (!HttpContext.Request.IsHttps)
                            {
                                DomainName = "https://" + DomainName;
                            }
                            else
                            {
                                DomainName = "http://" + DomainName;
                            }

                        }
                        else
                        {
                            DomainName = "http://" + DomainName;
                        }
                    }
                    else
                    {
                        DomainName = "http://" + DomainName;
                    }

                    dynamic DataSave = HopTrucTuyenService.SaveItem(model, DomainFolderUpload, DomainName);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới thành công";
                    }
                    return RedirectToAction("Index");
                }
            }
            data.ListCat = HopTrucTuyenService.GetListCat();
            data.ListDMCoQuan = DMCoQuanService.GetListByParent(0, int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString()),false);
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {

            HopTrucTuyen item = new HopTrucTuyen() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

                    dynamic DataDelete = HopTrucTuyenService.DeleteItem(item);
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
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {

            HopTrucTuyen item = new HopTrucTuyen() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = HopTrucTuyenService.UpdateStatus(item);
                    TempData["MessageSuccess"] = "Cập nhật Trạng Thái thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Trạng Thái Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Trạng Thái không thành công";
                return Json(new MsgError());
            }
        }
    }
}