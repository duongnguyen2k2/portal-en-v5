using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.WorkSchedules;
using API.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkSchedulesController : Controller
    {
        private string controllerName = "WorkSchedulesController";
        private string controllerSecret;
        public WorkSchedulesController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetListJson([FromBody] SearchWorkSchedules dto)
        {
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

            List<WorkSchedules> ListItems = WorkSchedulesService.GetListPagination(dto, this.controllerSecret);
            return Json(new MsgSuccess() { Data = ListItems });
        }

        [HttpPost]
        public IActionResult GetItem([FromQuery] string Id = null)
        {
            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());
            WorkSchedules Item = new WorkSchedules();
            if (IdDC > 0)
            {
                Item = WorkSchedulesService.GetItem(IdDC, this.controllerSecret);
            }
            return Json(new MsgSuccess() { Data = Item, Msg = "Lấy sản phẩm thành công" });
        }
        public IActionResult SaveItem(string Id = null)
        {
            WorkSchedulesModel data = new WorkSchedulesModel();

            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());

            data.SearchData = new SearchWorkSchedules() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

            if (IdDC == 0)
            {
                data.Item = new WorkSchedules();
            }
            else
            {
                data.Item = WorkSchedulesService.GetItem(IdDC, controllerSecret);
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem([FromBody] WorkSchedules dto)
        {
            if (ModelState.IsValid)
            {
                int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, this.controllerSecret).ToString());

                if (dto.Id == IdDC)
                {
                    dto.CreatedBy = dto.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
                    dto.Id = IdDC;
                    dynamic DataSave = WorkSchedulesService.SaveItem(dto);
                    string Msg = "Thêm mới Lịch công tác thành công";
                    if (dto.Id > 0)
                    {
                        Msg = "Cập nhật Lịch công tác thành công";
                    }
                    return Json(new MsgSuccess() { Data = dto, Msg = Msg });
                }
                return Json(new MsgError() { Msg = "Thông tin không hợp lệ" });
            }
            else
            {
                var DataError = ModelState.Values.SelectMany(v => v.Errors)
                               .Select(v => v.ErrorMessage + " " + v.Exception).ToList();

                return Json(new MsgError() { Data = DataError, Msg = "Dữ liệu không hợp lệ" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem([FromBody] WorkSchedules dto)
        {
            int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, this.controllerSecret).ToString());
            try
            {
                if (IdDC > 0 && dto.Id == IdDC)
                {
                    dto.CreatedBy = dto.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic DataDelete = WorkSchedulesService.DeleteItem(dto);

                    return Json(new MsgSuccess());
                }
                else
                {
                    return Json(new MsgError() { Msg = "Xóa Không thành công" });
                }

            }
            catch
            {
                return Json(new MsgError() { Msg = "Xóa Không thành công" });
            }
        }


        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {

            WorkSchedules item = new WorkSchedules() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = WorkSchedulesService.UpdateStatus(item);

                    return Json(new MsgSuccess() { Msg = "Cập nhật Trạng Thái thành công" });
                }
                else
                {

                    return Json(new MsgError() { Msg = "Cập nhật Trạng Thái Không thành công" });
                }
            }
            catch
            {

                return Json(new MsgError() { Msg = "Cập nhật Trạng Thái không thành công.." });
            }
        }
    }
}
