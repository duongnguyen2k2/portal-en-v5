using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Answer;
using API.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnswerController : Controller
    {
        private string controllerName = "AnswerController";
        private string controllerSecret;
        public AnswerController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult Index([FromQuery] SearchAnswer dto)
        {
            int TotalItems = 0;
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            AnswerModel data = new AnswerModel() { SearchData = dto};
                        
            data.ListItems = AnswerService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }
        
        
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Answer item = new Answer() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };            
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

                    dynamic DataDelete = AnswerService.DeleteItem(item);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else {
                    TempData["MessageError"] = "Xóa Không thành công";
                    return Json(new MsgError());
                }
                
            }
            catch {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }            
        }

        
    }
}