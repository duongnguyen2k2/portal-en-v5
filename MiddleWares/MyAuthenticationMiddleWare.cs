using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.USUsers;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using API.Areas.Admin.Models.USMenu;
//using AngleSharp.Io;
using API.Areas.Admin.Models.DMCoQuan;
using Microsoft.AspNetCore.Localization;

namespace API.MiddleWares
{
    public class MyAuthenticationMiddleWare
    {
                
        private readonly RequestDelegate _next;

        public MyAuthenticationMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Boolean flagDevCoQuan = false;
            Boolean flagDev = false;   
            
            Boolean flagSession = true;
            
            if (Startup._config["FlagCache"] != "1")
            {
                flagSession = false;
            }

            if (Startup._config["flagDevCoQuan"] != "1")
            {
                flagDevCoQuan = false;
            }
            else
            {
                flagDevCoQuan = true;
            }
            

            
            string link = context.Request.Path.Value;
            List<string> ListLink = link.Split("/").ToList();
            string TemplateName = "Default";
            string hostValue = context.Request.Host.Value.ToLower();

           

            /*
            if (!Areas.Admin.Models.DMCoQuan.DMCoQuanService.KiemTraBMT(hostValue))
            {
                if (link.ToLower() != "/home/LoiCauHinh".ToLower())
                {
                    context.Response.Redirect("/home/LoiCauHinh");
                }
            }*/

            var CssName = context.Session.GetString("CssName");
            //var CssName = "vnpt-style-nnmt.css?v=20250630";
            string cacheCode = hostValue + DateTime.Now.ToString("yyyyMMddhh");
            string CodeDMCoQuan = context.Session.GetString(cacheCode);

            API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan = new Areas.Admin.Models.DMCoQuan.DMCoQuan();
            List<string> ListCVisit = API.Models.MyModels.CountVisit();

            if (CodeDMCoQuan!=null && CodeDMCoQuan != "" && flagSession)
            {
                ItemCoQuan = JsonConvert.DeserializeObject<API.Areas.Admin.Models.DMCoQuan.DMCoQuan>(CodeDMCoQuan);
            }
            else
            {
                //ItemCoQuan = API.Areas.Admin.Models.DMCoQuan.DMCoQuanService.GetItemByCode(hostValue, flagDevCoQuan, CssName, CultureInfo.CurrentCulture.Name.ToLower());
                if (hostValue == "portal-bmt.vnptdaklak.vn")
                {
                    ItemCoQuan = API.Areas.Admin.Models.DMCoQuan.DMCoQuanService.GetItemByCode("buonmathuot.daklak.gov.vn", flagDevCoQuan, CssName);
                }
                else
                {
                    ItemCoQuan = API.Areas.Admin.Models.DMCoQuan.DMCoQuanService.GetItemByCode(hostValue, flagDevCoQuan, CssName, CultureInfo.CurrentCulture.Name.ToLower());
                    //ItemCoQuan = API.Areas.Admin.Models.DMCoQuan.DMCoQuanService.GetItemByCode(hostValue, flagDevCoQuan, CssName);
                }

                if (ItemCoQuan != null && ItemCoQuan.Id > 0 && flagSession)
                {
                    context.Session.SetString(cacheCode, JsonConvert.SerializeObject(ItemCoQuan));
                }
                else
                {
                    context.Session.Remove(cacheCode);
                }
                    
            }


            
            if (ItemCoQuan == null || ItemCoQuan.Id == 0)
            {
                if (link.ToLower() != "/home/LoiCauHinh".ToLower()) {
                    context.Response.Redirect("/home/LoiCauHinh");
                }               
            }
            else {
                /*
                string cacheCodeLang = "LangCache_" + ItemCoQuan.Id.ToString() + "-" + DateTime.Now.ToString("yyyyMMddhh");
                string CodeLang = context.Session.GetString(cacheCodeLang);
                if (CodeLang == null || CodeLang == "")
                {
                    context.Session.SetString(cacheCodeLang, "vi");

                    Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("vi")),
                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );
                }*/

                if (ItemCoQuan.CategoryId == 2)
                {
                    if (hostValue == "portal-bmt.vnptdaklak.vn")
                    {

                    }
                    else
                    {
                        if (ItemCoQuan.Code.Contains("daklak.gov.vn"))
                        {
                            if (!context.Request.IsHttps)
                            {
                                context.Response.Redirect("https://" + ItemCoQuan.Code);
                            }

                        }
                    }
                }

                TemplateName = ItemCoQuan.TemplateName;
                //TemplateName = "ThemeNNMTTwo";

                context.Session.SetString("TemplateName", TemplateName);

                if (CssName != null && CssName != "")
                {
                    context.Session.SetString("DomainCssName", CssName);
                }
                else
                {
                    context.Session.SetString("DomainCssName", ItemCoQuan.CssName);
                }
                //ItemCoQuan.CssName = "vnpt-style-nnmt.css?v=20250830";
                context.Session.SetString("DomainName", ItemCoQuan.Code);
                context.Session.SetString("DomainId", ItemCoQuan.Id.ToString());
                context.Session.SetString("IdCoQuan", ItemCoQuan.Id.ToString());
                context.Session.SetString("CategoryId", ItemCoQuan.CategoryId.ToString());
                context.Session.SetString("ParentId", ItemCoQuan.ParentId.ToString());
                context.Session.SetString("Slogan", ItemCoQuan.Slogan);
                context.Session.SetString("DomainFolderUpload", ItemCoQuan.FolderUpload);                
                context.Session.SetString("ThongTinCoQuan",JsonConvert.SerializeObject(ItemCoQuan));
            }

            if (flagDev)
            {
                UserToken UserToken = new UserToken()
                {
                    Id = 1,
                    IdGroup = 1,
                    IdCoQuan = 1,
                    UserName = "phucbv.dlc",
                    Email = "phucbv.dlc@vnpt.vn",
                    TemplateName = TemplateName,
                };
                context.Request.Headers.Add("Id", UserToken.Id.ToString());
                context.Request.Headers.Add("IdGroup", UserToken.IdGroup.ToString());
                context.Request.Headers.Add("IdCoQuan", UserToken.IdCoQuan.ToString());
                context.Request.Headers.Add("CategoryId", UserToken.CategoryId.ToString());
                context.Request.Headers.Add("UserName", UserToken.UserName);
                context.Request.Headers.Add("Email", UserToken.Email);

                context.Session.SetString("IdCoQuan", UserToken.IdCoQuan.ToString());
                context.Session.SetString("TenCoQuan", UserToken.Id.ToString());
                context.Session.SetString("IdUser", UserToken.Id.ToString());
                context.Session.SetString("IdGroup", UserToken.IdGroup.ToString());
                await _next(context);
            }
            else {
                
               
                if (ListLink[1].ToString().ToLower() == "admin")
                {
                    
                    var data = new Models.MsgError() { Success = false, Msg = "Bạn không có quyền truy cập vào Hệ Thống." };
                    try
                    {
                        
                        if (ListLink.Count() > 3 && (ListLink[2].ToString().ToLower() == "account" && ListLink[3].ToString().ToLower() == "login"))
                        {
                            
                            await _next(context);
                        }
                        else
                        {
                            
                            var Login = context.Session.GetString("Login");
                            if (Login != null && Login != "")
                            {
                                USUsersLogin MyInfo = JsonConvert.DeserializeObject<USUsersLogin>(Login);
                                List<USMenuPermission> ListPer = USMenuService.GetListPermission(MyInfo.ListMenuId, MyInfo.Id);
                                if (MyInfo.IdCoQuan != ItemCoQuan.Id)
                                {
                                    ItemCoQuan = API.Areas.Admin.Models.DMCoQuan.DMCoQuanService.GetItem(MyInfo.IdCoQuan);
                                }
                                
                                UserToken UserToken = new UserToken()
                                {
                                    Id = MyInfo.Id,
                                    IdGroup = MyInfo.IdGroup,
                                    IdCoQuan = MyInfo.IdCoQuan,
                                    UserName = MyInfo.UserName,
                                    Email = MyInfo.Email,
                                    TemplateName = TemplateName,
                                    FolderUpload = ItemCoQuan.FolderUpload
                                };
                                context.Session.SetString("IdCoQuan", MyInfo.IdCoQuan.ToString());
                                context.Session.SetString("CategoryId", MyInfo.CategoryId.ToString());
                                context.Session.SetString("TenCoQuan", MyInfo.TenCoQuan);
                                context.Session.SetString("IdUser", MyInfo.Id.ToString());
                                context.Session.SetString("IdGroup", MyInfo.IdGroup.ToString());
                                context.Session.SetString("FolderUpload", ItemCoQuan.FolderUpload);
                                context.Session.SetString("DomainFolderUpload", ItemCoQuan.FolderUpload);
                                context.Request.Headers.Add("Id", UserToken.Id.ToString());
                                context.Request.Headers.Add("IdGroup", UserToken.IdGroup.ToString());
                                context.Request.Headers.Add("IdCoQuan", UserToken.IdCoQuan.ToString());
                                context.Request.Headers.Add("CategoryId", UserToken.CategoryId.ToString());
                                context.Request.Headers.Add("UserName", UserToken.UserName);
                                context.Request.Headers.Add("Email", UserToken.Email);
                                context.Session.SetString("LoginError", context.Session.GetString("LoginError") + JsonConvert.SerializeObject(UserToken));

                                Boolean flagPermission = false;
                                if (ListPer != null && ListPer.Count > 0)
                                {
                                    foreach (var itemPer in ListPer)
                                    {
                                        if (itemPer.Controller != null && (itemPer.Controller.ToLower() == ListLink[2].ToString().ToLower()))
                                        {
                                            flagPermission = true;
                                        }
                                    }
                                }
                                if (flagPermission)
                                {
                                    await _next(context);

                                }
                                else
                                {
                                    context.Response.Redirect("/Admin/Home");
                                }
                            }
                            else
                            {
                                
                                context.Response.Redirect("/Admin/Account/Login");
                            }

                        }// if Login Action

                    }
                    catch (Exception e)
                    {
                        data.Data = e.Message;
                        await _next(context);                       
                    }
                }else if(ListLink[1].ToString().ToLower() == "trang-chu")
                {
                    context.Response.Redirect("/");
                }
                else
                {
                    if (ListLink.Count == 3 && ListLink[1].ToLower() == "web" && ListLink[2].ToLower().Substring(0, 5) == "guest")
                    {
                        context.Response.Redirect("/");
                    }
                    else
                    {
                        for (int vs = 0; vs < ListCVisit.Count(); vs++)
                        {
                            if (ListCVisit[vs].ToLower() == ListLink[1].ToString().ToLower() || ListLink[1].ToString() == "")
                            {
                                var OnlineUserCounter = context.Session.GetInt32("OnlineUserCounter");
                                if (OnlineUserCounter == null)
                                {
                                    API.Areas.Admin.Models.SiteVisit.SiteVisitService.SaveItem(DateTime.Now.ToString("yyyyMMdd"), ItemCoQuan.Id);
                                    context.Session.SetInt32("OnlineUserCounter", 1);
                                }
                                else
                                {
                                    context.Session.SetInt32("OnlineUserCounter", int.Parse(OnlineUserCounter.ToString()) + 1);
                                }
                            }
                        }
                        await _next(context);
                    }
                    
                    
                }
            }// If Dev
            
        }
       
    }

    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder UseMyAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyAuthenticationMiddleWare>();
        }
    }

}
