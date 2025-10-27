using API.Areas.Admin.Models.DMPhongBan;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class DMPhongBanController : Controller
	{
		// Lấy danh sách DMPhongBan theo Loại Phòng Ban
		public IActionResult GetListByType([FromQuery] SearchDMPhongBan dto)
		{
			var ListItems = DMPhongBanService.GetListAPI(true, dto.IdType, dto.IdCoQuan);
			return Json(new MsgSuccess() { Data = ListItems });
		}

		// Lấy chi tiết Phòng Ban
		public IActionResult Detail([FromQuery] int Id)
		{
			DMPhongBan Item = DMPhongBanService.GetItem(Id);
			return Json(new MsgSuccess() { Data = Item });
		}
	}
}
