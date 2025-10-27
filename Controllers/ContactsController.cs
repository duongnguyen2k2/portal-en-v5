using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Contacts;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using API.Models.MyHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace API.Controllers
{

    public class ContactsController : Controller
    {
        private string controllerName = "ContactsController";
        private string controllerSecret;
        private IConfiguration Configuration;
        private readonly IGoogleService Google;  // Inject Google qua DI
        public ContactsController(IConfiguration config, IGoogleService google)
        {
            Google = google;
            Configuration = config;
            controllerSecret = config["Security:SecretId"] + controllerName+"client";
        }
        public IActionResult Index()
        {
            // Hỏi Đáp
            ContactsModel data = new ContactsModel() {
                SearchData = new SearchContacts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" },
                Item = new Contacts()
            };                        
            return View(data);            
        }

        public async Task<IActionResult> TestSendEmail([FromQuery] string Email="phucbv.vnpt@gmail.com")
        {
            Contacts model = new Contacts();
            MailSettings settings = new MailSettings()
            {
                Host = Configuration["MailSettings:Host"],
                Port = Int32.Parse(Configuration["MailSettings:Port"]),
                Password = Configuration["MailSettings:Password"],
                DisplayName = Configuration["MailSettings:DisplayName"],
                Mail = Configuration["MailSettings:Mail"],
            };
            string linkEmail = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportTemplate/email_contact.html"); // Đọc mẫu template
            string textBody = System.IO.File.ReadAllText(linkEmail);//Lấy nội dung của mẫu

            textBody = ContactsService.ConvertBodyEmail(model, textBody); // Đưa các biến vào 

            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));

            MailRequest request = new MailRequest()
            {
                ToEmail = Email,
                Subject = ItemCoQuan.CompanyName +" - " + "Tieu De",
                Body = textBody,
            };
            await MyEmail.SendEmailAsync(settings, request); // Gửi Mail Tất cả order cho Đơn vị chủ quản trang web
            return Json(new API.Models.MsgSuccess() { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Contacts model)
        {
            ContactsModel data = new ContactsModel() { Item = model };
            
            Boolean CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], model.Token, int.Parse(Configuration["flagCaptCha"].ToString()));

            USUsersLog userLog = new USUsersLog()
            {
                Browser = "Contacts",
                Platform = "Index",
                IdUser = 0,
                LoginInfo = CheckGoogle.ToString(),
                Ip = "",
                Token = model.Token,
                Description = "",
            };

            USUsersService.SaveUserLog(userLog);

            if (!CheckGoogle)
            {
                TempData["MessageError"] = "Trình duyệt máy bạn Gửi yêu cầu thất bại. Xin vui lòng gửi lại";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
                    model.Id = 0;
                    model.CreatedBy = 0;
                    model.ModifiedBy = 0;
                    model.TypeId = 0;
                    model.IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId"));
                    try
                    {
                        ContactsService.SaveItem(model);

                        /*  ---------------- Gửi thông báo Tele -----------------------  */
                        Telegram Tele = new Telegram();
                        string smsTele = ItemCoQuan.Code + " - <b>" + HttpContext.Connection.RemoteIpAddress + "</b> - <b>Contacts</b> - Hỏi Đáp";
                        Telegram.Info TeleInfo = new Telegram.Info() { Msg = smsTele };
                        dynamic Data = Tele.SendNotification(TeleInfo, Configuration["flagTelegram"]);
                        /*  ---------------- End Gửi thông báo Tele -----------------------  */
                        TempData["MessageSuccess"] = "Câu hỏi của bạn đã gửi thành công, đang chuyển bộ phận xử lý và trả lời trong thời gian sớm nhất!";

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
                            string EmailHuyen = ItemCoQuan.Email;
                            MailRequest request = new MailRequest()
                            {
                                ToEmail = EmailHuyen,
                                Subject = model.Title,
                                Body = textBody,
                            };
                            await MyEmail.SendEmailAsync(settings, request); // Gửi Mail cho ban biên tập

                        }
                        

                    }
                    catch
                    {
                        TempData["MessageSuccess"] = "";
                        TempData["MessageError"] = "Gửi câu hỏi Thất bại. Xin vui lòng gửi lại";

                    }
                    return RedirectToAction("Index");

                }
            }

            return View(data);
        }

        public IActionResult Detail()
        {
            // Liên Hệ
            ContactsModel data = new ContactsModel()
            {
                SearchData = new SearchContacts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" },
                Item = new Contacts()
            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(Contacts model)
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            
            Boolean CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], model.Token, int.Parse(Configuration["flagCaptCha"].ToString()));
            ContactsModel data = new ContactsModel() { Item = model };
            USUsersLog userLog = new USUsersLog()
            {
                Browser = "Contacts",
                Platform = "Detail",
                IdUser = 0,
                LoginInfo = CheckGoogle.ToString(),
                Ip = "",
                Token = model.Token,
                Description = "",
            };

            USUsersService.SaveUserLog(userLog);

            if (!CheckGoogle)
            {
                TempData["MessageError"] = "Trình duyệt máy bạn Gửi yêu cầu thất bại. Xin vui lòng gửi lại";
            }
            else
            {

                
                if (ModelState.IsValid)
                {
                    model.Id = 0;
                    model.CreatedBy = 0;
                    model.ModifiedBy = 0;
                    model.TypeId = 1; // Liên Hệ
                    model.IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId"));
                    try
                    {
                        ContactsService.SaveItem(model);
                        /*  ---------------- Gửi thông báo Tele -----------------------  */
                        Telegram Tele = new Telegram();
                        string smsTele = ItemCoQuan.Code + " - <b>" + HttpContext.Connection.RemoteIpAddress + "</b> - <b>Contacts</b> - Liên Hệ";
                        Telegram.Info TeleInfo = new Telegram.Info() { Msg = smsTele };
                        dynamic Data = Tele.SendNotification(TeleInfo, Configuration["flagTelegram"]);
                        /*  ---------------- End Gửi thông báo Tele -----------------------  */

                        TempData["MessageSuccess"] = "câu hỏi của bạn đã gửi thành công, đang chuyển bộ phận xử lý và trả lời trong thời gian sớm nhất!";

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
                            await MyEmail.SendEmailAsync(settings, request); // Gửi Mail cho ban biên tập
                        }
                       

                    }
                    catch
                    {
                        TempData["MessageError"] = "Gửi câu hỏi Thất bại. Xin vui lòng gửi lại";

                    }
                    return RedirectToAction("Index");

                }
            }
            return View(data);
        }

        public IActionResult List([FromQuery] SearchContacts dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            ContactsModel data = new ContactsModel()
            {
                SearchData = new SearchContacts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "",IdCoQuan = IdCoQuan, Status=1 },
                Item = new Contacts()
            };

            data.ListItems = ContactsService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }



        public IActionResult ContactApp()
        {
            // Liên Hệ
            ContactsModel data = new ContactsModel()
            {
                SearchData = new SearchContacts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" },
                Item = new Contacts()
            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactApp(Contacts model)
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            
            Boolean CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], model.Token, int.Parse(Configuration["flagCaptCha"].ToString()));
            ContactsModel data = new ContactsModel() { Item = model };
            USUsersLog userLog = new USUsersLog()
            {
                Browser = "Contacts",
                Platform = "ContactApp",
                IdUser = 0,
                LoginInfo = CheckGoogle.ToString(),
                Ip = "",
                Token = model.Token,
                Description = "",
            };

            USUsersService.SaveUserLog(userLog);

            if (!CheckGoogle)
            {
                TempData["MessageError"] = "Trình duyệt máy bạn Gửi yêu cầu thất bại. Xin vui lòng gửi lại";
            }
            else
            {


                if (ModelState.IsValid)
                {
                    model.Id = 0;
                    model.CreatedBy = 0;
                    model.ModifiedBy = 0;
                    model.TypeId = 2; // Góp ý dứng dụng
                    model.IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId"));
                    try
                    {
                        ContactsService.SaveItem(model);
                        /*  ---------------- Gửi thông báo Tele -----------------------  */
                        Telegram Tele = new Telegram();
                        string smsTele = ItemCoQuan.Code + " - <b>" + HttpContext.Connection.RemoteIpAddress + "</b> - <b>Contacts</b> - Liên Hệ";
                        Telegram.Info TeleInfo = new Telegram.Info() { Msg = smsTele };
                        dynamic Data = Tele.SendNotification(TeleInfo, Configuration["flagTelegram"]);
                        /*  ---------------- End Gửi thông báo Tele -----------------------  */

                        TempData["MessageSuccess"] = "Góp ý ứng dụng của bạn đã gửi thành công, đang chuyển bộ phận xử lý và trả lời trong thời gian sớm nhất!";

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
                            await MyEmail.SendEmailAsync(settings, request); // Gửi Mail cho ban biên tập
                        }


                    }
                    catch
                    {
                        TempData["MessageError"] = "Gửi câu hỏi Thất bại. Xin vui lòng gửi lại";

                    }
                    return RedirectToAction("ContactApp");

                }
            }
            return View(data);
        }

    }
}