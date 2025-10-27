using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.AnToanThongTin;
using API.Areas.Admin.Models.DMCoQuan;
using API.Models.MyHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace API.Controllers
{

	public class AnToanThongTinController : Controller
	{
		private string controllerName = "AnToanThongTinController";
		private string controllerSecret;
		private IConfiguration Configuration;
		private readonly IGoogleService Google;
        public AnToanThongTinController(IConfiguration config, IGoogleService google)
		{
			Google = google;
            Configuration = config;
			controllerSecret = config["Security:SecretId"] + controllerName + "client";
		}
		public IActionResult Index()
		{
			AnToanThongTinModel data = new AnToanThongTinModel()
			{
				SearchData = new SearchAnToanThongTin() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" },
				Item = new AnToanThongTin()
			};
			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(AnToanThongTin dto)
		{
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            dto.Title = rRemScript.Replace(dto.Title, "").Trim();

            if (dto.Description != null && dto.Description != "")
            {
                dto.Description = rRemScript.Replace(dto.Description, "").Trim();
            }
            if (dto.Introtext != null && dto.Introtext != "")
            {
                dto.Introtext = rRemScript.Replace(dto.Introtext, "").Trim();
            }
            if (dto.Fullname != null && dto.Fullname != "")
            {
                dto.Fullname = rRemScript.Replace(dto.Fullname, "").Trim();
            }
            if (dto.Phone != null && dto.Phone != "")
            {
                dto.Phone = rRemScript.Replace(dto.Phone, "").Trim();
            }
            if (dto.Email != null && dto.Email != "")
            {
                dto.Email = rRemScript.Replace(dto.Email, "").Trim();
            }
            if (dto.Address != null && dto.Address != "")
            {
                dto.Address = rRemScript.Replace(dto.Address, "").Trim();
            }

            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
			AnToanThongTinModel data = new AnToanThongTinModel() { Item = dto };

			
			Boolean CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], dto.Token, int.Parse(Configuration["flagDevCoQuan"].ToString()));
			if (!CheckGoogle)
			{
				TempData["MessageError"] = "Trình duyệt máy bạn Gửi yêu cầu thất bại. Xin vui lòng gửi lại";
			}
			else
			{
				if (ModelState.IsValid && dto.Title!="" && dto.Fullname!="")
				{
                    dto.Id = 0;
                    dto.CreatedBy = 0;
                    dto.ModifiedBy = 0;
					try
					{
						AnToanThongTinService.SaveItem(dto);

						/*  ---------------- Gửi thông báo Tele -----------------------  */
						Telegram Tele = new Telegram();
						Telegram.Info TeleInfo = new Telegram.Info() { Msg = ItemCoQuan.Code + "-AnToanThongTin" };
						dynamic Data = Tele.SendNotification(TeleInfo);
						/*  ---------------- End Gửi thông báo Tele -----------------------  */

						TempData["MessageSuccess"] = "Gửi Ứng cứu sự cố an toàn thông tin mạng thành công";
					}
					catch
					{
						TempData["MessageError"] = "Gửi Ứng cứu sự cố an toàn thông tin mạng Thất bại. Xin vui lòng gửi lại";

					}
					return RedirectToAction("Index");

				}
			}

			return View(data);
		}

	}
}