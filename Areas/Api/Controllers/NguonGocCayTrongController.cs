using API.Areas.Admin.Models.NguonGocCayTrong;
using API.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Areas.Api.Controllers
{
    [Area("Api")]
    public class NguonGocCayTrongController : Controller
    {
        private string KeyItem = "FENguonGocCayTrong";
        
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult GetListJson([FromQuery] SearchNguonGocCayTrong dto)
        {
            List<NguonGocCayTrong> ListItems = NguonGocCayTrongService.GetListPaginationFE(dto, KeyItem);
            return Json(new MsgSuccess() { Data = ListItems });

        }

        public IActionResult Detail(string id)
        {
            int IdDC = Int32.Parse(MyModels.Decode(id, KeyItem).ToString());

            NguonGocCayTrongModel data = new NguonGocCayTrongModel();
            data.Item = NguonGocCayTrongService.GetItemFE(IdDC);
            return Json(new MsgSuccess() { Data = data.Item });
        }
    }
}
