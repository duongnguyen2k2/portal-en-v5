using API.Areas.Admin.Models.Contacts;
using API.Models.MyHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using API.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using API.Areas.Admin.Models.DMCoQuan;

namespace API.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private IConfiguration Configuration;
        public ContactsController(IConfiguration config)
        {
            Configuration = config;
        }

        [HttpPost]        
        public IActionResult SendContact(Contacts model)
        {
            string Msg = "Dữ liệu không hợp lệ";
            model.Id = 0;
            model.CreatedBy = 0;
            model.ModifiedBy = 0;
            model.TypeId = 2; // Góp ý dứng dụng
                              //model.IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId"));
            DMCoQuan ItemCoQuan = null;//DMCoQuanService.GetItem(model.IdCoQuan);
            try
            {
                ContactsService.SaveItem(model);
                /*  ---------------- Gửi thông báo Tele -----------------------  */
                Telegram Tele = new Telegram();
                string smsTele = ItemCoQuan.Code + " - <b>" + HttpContext.Connection.RemoteIpAddress + "</b> - <b>Contacts</b> - Liên Hệ";
                Telegram.Info TeleInfo = new Telegram.Info() { Msg = smsTele };
                dynamic Data = Tele.SendNotification(TeleInfo, Configuration["flagTelegram"]);
                /*  ---------------- End Gửi thông báo Tele -----------------------  */

                Msg = "Góp ý về ứng dụng của bạn đã gửi thành công, đang chuyển bộ phận xử lý và trả lời trong thời gian sớm nhất!";

                if (Configuration["flagSendEmail"] == "1") // Gửi Email
                {
                    MailSettings settings = new MailSettings()
                    {
                        Host = Configuration["MailSettings:Host"],
                        Port = Int32.Parse(Configuration["MailSettings:Port"]),
                        Password = Configuration["MailSettings:Password"],
                        DisplayName = Configuration["MailSettings:DisplayName"],
                        Mail = Configuration["MailSettings:Mail"],
                    };
                    string linkEmail = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportTemplate/email_contact.html");
                    string textBody = System.IO.File.ReadAllText(linkEmail);

                    textBody = ContactsService.ConvertBodyEmail(model, textBody);


                    string EmailHuyen = "phucbv.vnpt@gmail.com";
                    MailRequest request = new MailRequest()
                    {
                        ToEmail = EmailHuyen,
                        Subject = model.Title,
                        Body = textBody,
                    };

                }
                return Ok(new MsgSuccess() { Data = model, Msg = Msg });

            }
            catch
            {
                Msg = "Gửi câu hỏi Thất bại. Xin vui lòng gửi lại";
                return Ok(new MsgError() { Data = model, Msg = Msg });
            }
            
        }
    }
}
