using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Reviews;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewsController : Controller
    {
        private string controllerName = "ReviewsController";
        private string controllerSecret;
        public ReviewsController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index([FromQuery] SearchReviews dto)
        {
            int TotalItems = 0;

            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            ReviewsModel data = new ReviewsModel() { SearchData = dto };
            data.ListItems = ReviewsService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }            
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListStar = ReviewsService.GetListStar();
            return View(data);
        }

        public IActionResult SaveItem(string Id = null, int CatId = 0, int IdCoQuan = 1)
        {
            ReviewsModel data = new ReviewsModel();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            data.SearchData = new SearchReviews() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "", IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) };
            
            if (IdDC == 0)
            {
                data.Item = new Reviews() { ReviewDate = DateTime.Now, ReviewDateShow = DateTime.Now.ToString("dd/MM/yyyy") };
            }            
            else
            {
                data.Item = ReviewsService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
            }
            data.ListStar = ReviewsService.GetListStar();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(Reviews model)
        {
            
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            ReviewsModel data = new ReviewsModel() { Item = model };

            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
                    ReviewsService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới thành công";
                    }
                    return RedirectToAction("Index", new {  });
                }
            }
            else
            {
                data.ListStar = ReviewsService.GetListStar();
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            Reviews model = new Reviews() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    ReviewsService.DeleteItem(model);
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
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Reviews item = new Reviews() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = ReviewsService.UpdateStatus(item);
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

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeatured([FromQuery] string Ids, Boolean Featured)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            Reviews item = new Reviews() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = ReviewsService.UpdateFeatured(item);
                    TempData["MessageSuccess"] = "Cập nhật Featured thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Featured Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Featured không thành công";
                return Json(new MsgError());
            }
        }

    }
}