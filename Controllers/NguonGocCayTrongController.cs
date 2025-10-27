using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.NguonGocCayTrong;
using API.Areas.Admin.Models.NhatKyNongHo;
using API.Areas.Admin.Models.CoSoSXKD;
using API.Areas.Admin.Models.CoSoSXKDNhatKy;
using API.Models;

namespace API.Controllers
{
    public class NguonGocCayTrongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetListJson([FromBody] SearchNguonGocCayTrong dto)
        {
            
            List<NguonGocCayTrong> ListItems = NguonGocCayTrongService.GetListPaginationFE(dto,"dsd");
            return Json(new MsgSuccess() { Data = ListItems });
            
        }
        
        public IActionResult Detail(int id)
        {
            NguonGocCayTrongModel data = new NguonGocCayTrongModel();
            data.Item = NguonGocCayTrongService.GetItem(id);
            return View(data);
        }
    }
}
