using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Orders;
using API.Models;
using Newtonsoft.Json;
using System.Data;
//using DocumentFormat.OpenXml.Drawing.Charts;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {        
        public IActionResult Index([FromQuery] SearchOrders dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OrdersModel data = new OrdersModel() { SearchData = dto};
                        
            data.ListItems = OrdersService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem(string Id=null)
        {
            OrdersModel data = new OrdersModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            
            data.SearchData = new SearchOrders() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};
            
            if (IdDC == 0)
            {
                data.Item = new Orders();
            }
            else {
                data.Item = OrdersService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            
           
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(Orders model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            OrdersModel data = new OrdersModel() { Item = model};            
            if (ModelState.IsValid)
            {
                System.Data.DataTable tbItem = new System.Data.DataTable();
                tbItem.Columns.Add("OrderId", typeof(int));
                tbItem.Columns.Add("ProductId", typeof(int));
                tbItem.Columns.Add("Price", typeof(float));
                tbItem.Columns.Add("Quantity", typeof(int));
                tbItem.Columns.Add("Total", typeof(float));
                tbItem.Columns.Add("TotalDisplay", typeof(string));

                if (model.Id == IdDC)
                {                    
                    model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic DataSave = OrdersService.SaveItem(model, tbItem);
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
            return View(data);
        }
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Orders item = new Orders() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

                    dynamic DataDelete = OrdersService.DeleteItem(item);
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