using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using ClosedXML.Excel;
using Newtonsoft.Json;
using API.Models;
using System.Data;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]    
    public class ImportExcelController : Controller
    {
        private IConfiguration Configuration;
        public ImportExcelController(IConfiguration config)
        {
            Configuration = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ImportDMCoQuan()
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            if (MyInfo.CQLevel > 1)
            {
                return Redirect("/Admin/Home/Index");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public IActionResult ImportListDMCoQuan([FromBody] List<DMCoQuan> dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            if (MyInfo.CQLevel > 1)
            {
                return Redirect("/Admin/Home/Index");
            }
            else
            {
                if (dto != null && dto.Count() > 0)
                {
                    for (int i = 0; i < dto.Count(); i++)
                    {

                        if (dto[i].Title != null && dto[i].Title.Trim() != "")
                        {
                            dto[i].Status = true;
                            dto[i].CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                            dto[i].ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                            //DMCoQuanService.SaveItem(dto[i]);
                        }

                    }
                    return Json(new API.Models.MsgSuccess() { Msg = "Thanh Cong", Data = dto });
                }
                else
                {
                    return Json(new API.Models.MsgError() { Msg = "Dữ liệu rỗng", Data = dto });
                }
            }



        }
        [HttpPost]
        public async Task<dynamic> UploadExcelDMCoQuan()
        {
            try
            {
                List<DMCoQuan> listItems = new List<DMCoQuan>();
                var file = Request.Form.Files[0];
                string filePath = Path.Combine("temp", "FileUpload");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);

                    }
                    XLWorkbook workBook = new XLWorkbook(filePath + "/" + file.FileName);
                    IXLWorksheet workSheet = workBook.Worksheet(1);
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        if (firstRow)
                        {
                            firstRow = false;
                        }
                        else
                        {
                            int i = -1;
                            DMCoQuan item = new DMCoQuan() { Id = 0, Status = true };
                            foreach (IXLCell cell in row.Cells())
                            {
                                i++;
                                if (i == 0) { item.Code = cell.Value.ToString(); }
                                if (i == 1) { item.Title = cell.Value.ToString(); }
                                if (i == 2) { item.ParentId = Int32.Parse(cell.Value.ToString()); }                                
                                if (i == 3) { item.FolderUpload = cell.Value.ToString(); }
                                if (i == 4) { item.TemplateName = cell.Value.ToString(); }
                                if (i == 5) { item.CompanyName = cell.Value.ToString(); }
                                if (i == 6) { item.Slogan = cell.Value.ToString(); }
                                if (i == 7) { item.Description = cell.Value.ToString(); }
                                if (i == 8) { item.CategoryId = Int32.Parse(cell.Value.ToString()); }
                                if (i == 9) { item.Address = cell.Value.ToString(); }
                                if (i == 10) { item.Telephone = cell.Value.ToString(); }
                                if (i == 11) { item.Fax = cell.Value.ToString(); }
                                if (i == 12) { item.Email = cell.Value.ToString(); }
                                if (i == 13) { item.CssName = cell.Value.ToString(); }
                                if (i == 14) {
                                    item.MetadataCV = new Metadata() { 
                                        MST = cell.Value.ToString()
                                    };
                                }
                            }
                            if (item.Title != null && item.Title.Trim() != "")
                            {
                                listItems.Add(item);
                            }

                        }
                    }
                }

                return new API.Models.MsgSuccess() { Msg = "Thanh Cong", Data = listItems };
            }
            catch (Exception e)
            {
                return new API.Models.MsgError() { Msg = e.Message };
            }
        }


        [HttpPost]
        [RequestSizeLimit(40000000)]
        public async Task<dynamic> UploadExcelUSUsers()
        {
            int n = 0;
            int Col = 0;
            string Username = "";
            IXLRow BK_Row = null;
            try
            {
                List<USUsers> listItems = new List<USUsers>();

                string filePath = Path.Combine("temp", "FileUpload");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (Request.Form.Files[0].Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(filePath, Request.Form.Files[0].FileName), FileMode.Create))
                    {
                        await Request.Form.Files[0].CopyToAsync(fileStream);

                    }
                    XLWorkbook workBook = new XLWorkbook(filePath + "/" + Request.Form.Files[0].FileName);
                    IXLWorksheet workSheet = workBook.Worksheet(1);
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        n++;
                        if (n == 21)
                        {
                            BK_Row = row;
                        }

                        if (firstRow)
                        {
                            firstRow = false;
                        }
                        else
                        {
                            int i = -1;
                            USUsers item = new USUsers() { Id = 0 };
                            foreach (IXLCell cell in row.Cells())
                            {
                                i++;
                                Col = i;
                                if (i == 0)
                                {
                                    item.UserName = cell.Value.ToString().Trim();
                                    Username = item.UserName;
                                }
                                else if (i == 1) { item.FullName = cell.Value.ToString().Trim(); }
                                else if (i == 2) { item.IdCoQuan = Int32.Parse(cell.Value.ToString().Trim()); }
                                else if (i == 3)
                                {
                                    try
                                    {
                                        item.Gender = Byte.Parse(cell.Value.ToString().Trim());
                                    }
                                    catch
                                    {
                                        item.Gender = 2;
                                    }
                                }
                                else if (i == 4)
                                {
                                    if (cell.Value.IsBlank || cell.Value.ToString().Trim() == "")
                                    {
                                        item.BirthdayShow = DateTime.Now.ToString("yyyy");
                                    }
                                    else
                                    {
                                        item.BirthdayShow = cell.Value.ToString().Trim();
                                    }

                                }
                                else if (i == 5)
                                {
                                    item.NamSinh = Byte.Parse(cell.Value.ToString().Trim());
                                    if (item.NamSinh == 1)
                                    {
                                        //item.Birthday = int.Parse(item.BirthdayShow);
                                        string str_bd = item.BirthdayShow + "0101";
                                        item.Birthday = Int32.Parse(str_bd);
                                    }
                                    else
                                    {
                                        item.Birthday = Int32.Parse(item.BirthdayShow);
                                    }

                                    if (item.Birthday > Int32.Parse(DateTime.Now.ToString("yyyyMMdd")) || item.Birthday < 19000101)
                                    {
                                        item.Flag = false;
                                        item.Msg = item.Msg + " Ngày Sinh không hợp lệ";
                                    }
                                }
                                else if (i == 6) { item.Address = cell.Value.ToString().Trim(); }
                                else if (i == 7) { item.IdDanToc = Int32.Parse(cell.Value.ToString().Trim()); }
                                else if (i == 8) { item.IdTonGiao = Int32.Parse(cell.Value.ToString().Trim()); }
                                else if (i == 9) { item.DangVien = Int32.Parse(cell.Value.ToString().Trim()); }
                                else if (i == 10) { item.IdTrinhDo = Int32.Parse(cell.Value.ToString().Trim()); }
                                else if (i == 11) { item.Specialize = cell.Value.ToString().Trim(); }                               
                            }
                            if (item.FullName != null && item.FullName.Trim() != "")
                            {
                                listItems.Add(item);
                            }

                        }
                    }
                }
                return new API.Models.MsgSuccess() { Msg = "Thanh Cong", Data = listItems };
            }
            catch (Exception e)
            {
                string Err = "; Cột=" + Col.ToString();
                if (Col == 12)
                {
                    Err = Err + " ;Ngày vào hội không được để trống";
                }

                return new API.Models.MsgError() { Msg = e.Message + "- Dòng= " + n.ToString() + Err };
            }
        }
        /*
        [HttpPost]
        public IActionResult ImportListUSUsers([FromBody] List<USUsers> dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            string Password = BCrypt.Net.BCrypt.HashPassword("Abc@123" + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);//USUsersService.GetMD5("Abc@123");
            if (MyInfo.CQLevel > 1)
            {
                return Redirect("/Admin/Home/Index");
            }
            else
            {
                if (dto != null && dto.Count() > 0)
                {
                    for (int i = 0; i < dto.Count(); i++)
                    {
                        if (dto[i].FullName != null && dto[i].FullName.Trim() != "")
                        {
                            dto[i].IdGroup = 2;
                            dto[i].Password = Password;
                            dto[i].Status = 1;
                            dto[i].CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                            dto[i].ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                        }

                        //USUsersService.SaveItem(dto[i]);
                    }
                    return Json(new API.Models.MsgSuccess() { Msg = "Thanh Cong", Data = dto });
                }
                else
                {
                    return Json(new API.Models.MsgError() { Msg = "Dữ liệu rỗng", Data = dto });
                }

            }
                

        }
        */

        
        [HttpPost]
        public ActionResult ImportListUSUsersAll([FromBody] List<USUsersExcel> ListItems)
        {

            DataTable tbItem = new DataTable();
            tbItem.Columns.Add("Id", typeof(int));
            tbItem.Columns.Add("Email", typeof(string));
            tbItem.Columns.Add("EmailConfirmed", typeof(string));
            tbItem.Columns.Add("Password", typeof(string));
            tbItem.Columns.Add("Telephone", typeof(string));
            tbItem.Columns.Add("UserName", typeof(string));
            tbItem.Columns.Add("UserCode", typeof(string));
            tbItem.Columns.Add("FullName", typeof(string));
            tbItem.Columns.Add("Birthday", typeof(int));
            tbItem.Columns.Add("Gender", typeof(byte));
            tbItem.Columns.Add("Avatar", typeof(string));
            tbItem.Columns.Add("Fax", typeof(string));
            tbItem.Columns.Add("IdGroup", typeof(int));
            tbItem.Columns.Add("CreatedBy", typeof(int));
            tbItem.Columns.Add("ModifiedBy", typeof(int));
            tbItem.Columns.Add("IdCoQuan", typeof(int));
            tbItem.Columns.Add("Specialize", typeof(string));
            tbItem.Columns.Add("IdRegency", typeof(int));
            tbItem.Columns.Add("Address", typeof(string));
            tbItem.Columns.Add("IdDanToc", typeof(int));
            tbItem.Columns.Add("IdTonGiao", typeof(int));
            tbItem.Columns.Add("IdTrinhDo", typeof(int));
            tbItem.Columns.Add("DangVien", typeof(int));                                    
            tbItem.Columns.Add("NamSinh", typeof(byte));
            tbItem.Columns.Add("Ordering", typeof(int));


            
            string Password = BCrypt.Net.BCrypt.HashPassword("Abc@123" + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);//USUsersService.GetMD5("Abc@123");
            if (ListItems != null && ListItems.Count() > 0)
            {
                for (int i = 0; i < ListItems.Count(); i++)
                {

                    var row = tbItem.NewRow();
                    row["Id"] = 0;
                    row["UserName"] = ListItems[i].UserName.ToString();
                    row["Avatar"] = null;
                    row["IdCoQuan"] = ListItems[i].IdCoQuan;
                    row["Password"] = Password;
                    row["UserCode"] = ListItems[i].IdCoQuan.ToString();
                    row["FullName"] = ListItems[i].FullName;
                    row["Telephone"] = null;
                    row["Email"] = null;
                    row["EmailConfirmed"] = null;
                    row["Birthday"] = ListItems[i].Birthday;
                    row["Fax"] = null;
                    row["IdGroup"] = 2;
                    row["CreatedBy"] = int.Parse(HttpContext.Request.Headers["Id"]);
                    row["ModifiedBy"] = int.Parse(HttpContext.Request.Headers["Id"]);
                    row["Gender"] = ListItems[i].Gender;
                    row["Specialize"] = ListItems[i].Specialize;
                    row["IdRegency"] = 0;
                    row["IdDanToc"] = ListItems[i].IdDanToc;
                    row["IdTonGiao"] = ListItems[i].IdTonGiao;
                    row["IdTrinhDo"] = ListItems[i].IdTrinhDo;
                    row["DangVien"] = ListItems[i].DangVien;
                    row["NamSinh"] = ListItems[i].NamSinh;
                    row["Address"] = ListItems[i].Address;                    
                    row["Ordering"] = ListItems[i].Ordering;                    
                    tbItem.Rows.Add(row);
                }
                try
                {
                    dynamic DataSave = null;
                    //dynamic DataSave = USUsersService.SaveImportExcelAll(tbItem);
                    TempData["MessageSuccess"] = "Đỗ dữ liệu <strong>(" + ListItems.Count.ToString() + ")</strong> Hội viên  thành công";
                    return Json(new MsgSuccess() { Msg = "Đỗ dữ liệu <strong>(" + ListItems.Count.ToString() + ")</strong> Hội viên thành công", Data = DataSave });
                }
                catch (Exception e)
                {
                    TempData["MessageError"] = "Đỗ dữ liệu Hội viên Sinh hoạt Không thành công";
                    return Json(new MsgError() { Msg = "Đỗ dữ liệu Hội viên Sinh hoạt Không thành công", Data = e.Message });
                }
            }
            TempData["MessageError"] = "Đỗ dữ liệu Hội viên Sinh hoạt Không thành công";
            return Json(new MsgError() { Msg = "Đỗ dữ liệu Hội viên Sinh hoạt Không thành công" });


        }


        public IActionResult ImportUsers()
        {
            return View();
        }


    }
}