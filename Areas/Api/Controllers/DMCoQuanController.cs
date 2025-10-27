using API.Areas.Admin.Models.DMCoQuan;
using API.Models;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class DMCoQuanController : Controller
	{
		public IActionResult GetListByParent([FromQuery] int Id)
		{
			var ListItems =  DMCoQuanService.GetListChild(Id);
			return Json(new MsgSuccess() { Data = ListItems});
		}

		public IActionResult Detail([FromQuery] int Id)
		{
			var Items = DMCoQuanService.GetItem(Id);
			return Json(new MsgSuccess() { Data = Items });
		}
	}
}
