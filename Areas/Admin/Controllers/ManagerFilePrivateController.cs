using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.ManagerFile;
using System.IO;
using API.Areas.Admin.Models.ManagerFiles;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerFilePrivateController : Controller
    {
        private string UrlRoot = "/TaiLieuHop/";

        public ManagerFilePrivateController() :base()
        {
            //this.UrlRoot = "/TaiLieuHop/" + HttpContext.Session.GetString("DomainFolderUpload") + "/";
        }

        [HttpPost]

        public IActionResult Rename([FromBody] ManagerFile.SearchTinymce dto)
        {
            this.UrlRoot = "/TaiLieuHop/" + HttpContext.Session.GetString("DomainFolderUpload") + "/";

            List<string> ListPathArr = dto.Path.Split(new[] { '/' }).ToList<string>();
            string fileRename = "";
            for (int i = 0; i < ListPathArr.Count; i++) {
                if (i == ListPathArr.Count() - 1)
                {
                    fileRename = fileRename + dto.Name;
                }
                else {
                    fileRename = fileRename + ListPathArr[i] + "/";
                }
            }
            string filePath = ValidatePath(dto.Path);
            string filePathRename = ValidatePath(fileRename);
            
            Boolean flagSave = false;
            if (dto.Path.ToUpper() == fileRename.ToUpper()) {
                return Json(new API.Models.MsgError() { Msg = "<strong>" + dto.Name + "</strong> đã tồn tại. Xin vui lòng chọn tên khác " });
            }
            
            try
            {                
                if (dto.flag == 1) {
                    if (Directory.Exists(filePath))
                    {
                        if (Directory.Exists(filePathRename))
                        {
                            return Json(new API.Models.MsgError() { Msg = "Folder <strong>" + dto.Name + "</strong> đã tồn tại. Xin vui lòng chọn tên khác " });
                        }
                        else {
                            try
                            {
                                FileSystem.Rename(filePath, filePathRename);
                                return Json(new API.Models.MsgSuccess() { Msg = "Đổi tên folder <strong>" + dto.Name + "</strong> Thành công" });
                            }
                            catch (System.IO.IOException)
                            {

                                return Json(new API.Models.MsgError() { Msg = "Bạn không có quyền Đổi tên Folder <strong>" + dto.Name + "</strong> này" });
                            }
                        }
                        
                    }
                    else {
                        return Json(new API.Models.MsgError() { Msg = "Folder không tồn tại" });
                    }
                    
                }
                else {
                    if (!System.IO.File.Exists(filePath))
                    {                        
                        return Json(new API.Models.MsgError() { Msg = "File không tồn tại" });
                    }
                    else if (System.IO.File.Exists(filePathRename))
                    {
                        return Json(new API.Models.MsgError() { Msg = "Tên file <strong>" + dto.Name + "</strong> đã tồn tại. Xin vui lòng đặt tên khác" });                        
                    }
                    else {
                        string Extension = Path.GetExtension(filePathRename);
                        List<String> listMIMETypes = MIMETypesService.GetListTinymce();
                        for (int j = 0; j < listMIMETypes.Count(); j++)
                        {
                            if (Extension.ToLower() == listMIMETypes[j].ToLower())
                            {
                                flagSave = true;
                            }
                        }

                        try
                        {
                            if (flagSave)
                            {
                                FileSystem.Rename(filePath, filePathRename);
                                return Json(new API.Models.MsgSuccess() { Msg = "Đổi tên file <strong>" + dto.Name + "</strong> thành công" });
                            }
                            else {
                                return Json(new API.Models.MsgError() { Msg = "Định Dạng file <strong>" + dto.Name + "</strong> Không hợp lệ" });
                            }                          
                        }
                        catch (System.IO.IOException)
                        {
                            return Json(new API.Models.MsgError() { Msg = "Bạn không có quyền Đổi tên file <strong>" + dto.Name + "</strong> này" });
                        }
                    }

                   
                    
                }
                
            }
            catch
            {
                return Json(new API.Models.MsgError() { Msg = "Xóa file <strong>" + dto.Name + "</strong> Không thành công" });
            }
        }

        [HttpPost]
        
        public IActionResult DeleteFile([FromBody] ManagerFile.SearchTinymce dto)
        {
            this.UrlRoot = "/TaiLieuHop/" + HttpContext.Session.GetString("DomainFolderUpload") + "/";
            try
            {               
                string filePath = ValidatePath(dto.Path); 
                if (System.IO.File.Exists(filePath)) {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (System.IO.IOException)
                    {

                        return Json(new API.Models.MsgError() { Msg = "Bạn không có quyền xóa file <strong>" + dto.Name + "</strong> này" });
                    }
                }
                return Json(new API.Models.MsgSuccess() { Msg = "Xóa file <strong>" + dto.Name + "</strong> thành công" });
            }
            catch {
                return Json(new API.Models.MsgError() { Msg = "Xóa file <strong>" + dto.Name + "</strong> Không thành công" });
            }
        }

        [HttpPost]
        public IActionResult DeleteFolder([FromBody] ManagerFile.SearchTinymce dto)
        {
            this.UrlRoot = "/TaiLieuHop/" + HttpContext.Session.GetString("DomainFolderUpload") + "/";
            string dirPath = ValidatePath(dto.Path);
            try
            {
                if (Directory.Exists(dirPath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);//"C:/Windows/System32/drivers"                    
                    DirectoryInfo[] childDirs = dirInfo.GetDirectories();
                    FileInfo[] childFiles = dirInfo.GetFiles();
                    if (childDirs.Length == 0 && childFiles.Length == 0)
                    {
                        Directory.Delete(dirPath, true);
                        return Json(new API.Models.MsgError() { Success = true, Msg = "Xóa Folder <strong>" + dto.Name + "</strong> Thành công" });
                    }
                    else
                    {
                        return Json(new API.Models.MsgError() { Success = false, Msg = "Folder <strong>" + dto.Name + "</strong> phải rỗng mới xóa được" });
                    }

                }
                else {
                    return Json(new API.Models.MsgError() { Success = false, Msg = "Folder <strong>" + dto.Name + "</strong> không tồn tại" });
                }

            }
            catch {
                return Json(new API.Models.MsgError() { Msg = "Xóa Folder <strong>" + dto.Name + "</strong> Không thành công" });
            }
        }

        [HttpPost]
        public IActionResult CreateFolder([FromBody] ManagerFile.SearchTinymce dto) {
            this.UrlRoot = "/TaiLieuHop/" + HttpContext.Session.GetString("DomainFolderUpload") + "/";
            string dirPath = ValidatePath(dto.Path+ dto.Name);

            try
            {
                if (!Directory.Exists(dirPath)) {
                    Directory.CreateDirectory(dirPath);
                    return Json(new API.Models.MsgSuccess() { Success = true, Msg = "Tạo folder <strong>" + dto.Name + "</strong> thành công" });
                }
                else
                {
                    return Json(new API.Models.MsgError() { Msg = "Tên folder  <strong>"+ dto.Name + "</strong> đã tồn tại" });
                }
            }
            catch {
                return Json(new API.Models.MsgError() { Msg = "Tạo Folder <strong>" + dto.Name + "</strong> Không thành công" });
            }
        }

        [HttpPost]
        public IActionResult GetListItems([FromBody] ManagerFile.SearchTinymceFolder dto)
        {
            
            this.UrlRoot = HttpContext.Session.GetString("DomainFolderUpload") + "/";
                        
            string dirPathRootFolder = ValidatePath(this.UrlRoot);

            try
            {
                if (!Directory.Exists(dirPathRootFolder))
                {
                    Directory.CreateDirectory(dirPathRootFolder);                   
                }
               
            }
            catch
            {
                
            }


            Boolean isRoot = true;            
            List<ManagerFile.TinymceFile> ListFiles = new List<ManagerFile.TinymceFile>();
            ManagerFile.TinymceFolder TinyFolder = new ManagerFile.TinymceFolder()
            {
                PathParent = "",
                Path = this.UrlRoot,
                Img = "/images/tinymce/folder.png",
                HasChildren = true
            };

            

            ManagerFile.TinymceGetFiles TinyFiles = new ManagerFile.TinymceGetFiles()
            {
                CurrentFolder = TinyFolder
            };

            List<ManagerFile.TinymceFolder> ListFolders = new List<ManagerFile.TinymceFolder>();

            if (dto.CurrentFolder == "" || dto.CurrentFolder == "/" || dto.CurrentFolder.ToUpper() == this.UrlRoot.ToUpper())
            {
                dto.CurrentFolder = this.UrlRoot;
            }
            else {
                isRoot = false;
                dto.CurrentFolder = this.UrlRoot + dto.CurrentFolder.Substring(this.UrlRoot.Length, dto.CurrentFolder.Length - this.UrlRoot.Length);

                ManagerFile.TinymceFolder FolderBack = new ManagerFile.TinymceFolder() 
                {
                    Name = "Quay Lại",Url="",Path="",Img= "/images/tinymce/folder_back.png",
                    PathParent="",HasChildren = false
                };
                ListFolders.Add(FolderBack);
            }
            
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop/")+  dto.CurrentFolder;
            
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

            if (Directory.Exists(dirPath))
            {
                if (!isRoot) {
                    ListFolders[0].Path =  dto.CurrentFolder.Substring(0, dto.CurrentFolder.Length - (dirInfo.Name+"/").Length);
                }
                //DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                DirectoryInfo[] childDirs = dirInfo.GetDirectories();
                FileInfo[] childFiles = dirInfo.GetFiles();

                List<string> listExtImage = new List<string>();
                //listExtImage.Add("")
                string str = "jpg,JPG,png,PNG,jpeg,JPEG,webp,WEBP,gif,GIF,bmp,BMP,jfif,JFIF";
                listExtImage = str.Split(new[] { ',' }).ToList<string>();

                foreach (DirectoryInfo childDir in childDirs)
                {
                    ManagerFile.TinymceFolder folder = new ManagerFile.TinymceFolder() { };
                    folder.Name = childDir.Name;
                    folder.Url = childDir.FullName;
                    folder.Path = dto.CurrentFolder + childDir.Name + "/";
                    folder.PathParent = dto.CurrentFolder;
                    folder.HasChildren = false;
                    folder.Img = TinyFolder.Img;
                    ListFolders.Add(folder);
                }

                foreach (FileInfo childFile in childFiles)
                {
                    ManagerFile.TinymceFile file = new ManagerFile.TinymceFile() { };
                    file.Name = childFile.Name;
                    file.Path = dto.CurrentFolder + childFile.Name;
                    file.Extension = childFile.Extension.Replace(".", "").Trim();
                    file.PathParent = dto.CurrentFolder;
                    if (childFile.Length > 1024)
                    {
                        file.Size = childFile.Length;
                    }

                    for (int i = 0; i < listExtImage.Count(); i++)
                    {
                        if (listExtImage[i] == file.Extension)
                        {
                            file.IsImage = true;
                            file.Img = "/images/tinymce/image-icon.png?v=" + DateTime.Now.ToString("yyyyMMddhhss");
                        }

                    }
                    if (!file.IsImage) {
                        file.Img = "/images/tinymce/" + file.Extension+".png?v="+DateTime.Now.ToString("yyyyMMdd");
                    }
                    ListFiles.Add(file);
                }
                TinyFiles.Files = ListFiles;

                List<ManagerFile.Breadcrumb> ListBreadcrumb = new List<ManagerFile.Breadcrumb>();
                
                List<string> tmpListBreadcrumb = dto.CurrentFolder.Split(new char[] { '/' }).ToList();
                if (tmpListBreadcrumb != null && tmpListBreadcrumb.Count() > 2) {
                    string PathBreadcrumb = this.UrlRoot;
                    for (int j = 3; j < tmpListBreadcrumb.Count(); j++) {
                        PathBreadcrumb = PathBreadcrumb + tmpListBreadcrumb[j] + "/";
                        if (j == (tmpListBreadcrumb.Count()-1))
                        {
                            ListBreadcrumb.Add(new ManagerFile.Breadcrumb() { Name = "("+ ListFiles .Count()+ " Files - "+ childDirs.Count() + " Folders)", Path = "",IsActive=false });
                        }
                        else {
                            if (j == (tmpListBreadcrumb.Count() - 2))
                            {
                                ListBreadcrumb.Add(new ManagerFile.Breadcrumb() { Name = tmpListBreadcrumb[j], Path = PathBreadcrumb, IsActive = true }) ;
                            }
                            else {
                                ListBreadcrumb.Add(new ManagerFile.Breadcrumb() { Name = tmpListBreadcrumb[j], Path = PathBreadcrumb });
                            }
                            
                        }
                    }
                }

                return Json(new API.Models.MsgSuccess() { Data = new { ListFolders = ListFolders , ListFiles= ListFiles, ListBreadcrumb= ListBreadcrumb, TinyCurrentFolder=dto.CurrentFolder } });
            }
            else {
                return Json(new API.Models.MsgError() { Msg = "Thư mục không tồn tại" });
            }
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFiles()
        {
            this.UrlRoot = "/TaiLieuHop/" + HttpContext.Session.GetString("DomainFolderUpload") + "/";
            string CurrentFolder = Request.Form["CurrentFolder"];
            string dirPath = this.ValidatePath(CurrentFolder);
            string FilePathUpload = "";
            string Extension = "";
            string Size = "";
            Boolean flagSave = false;

            List<String> listMIMETypes = MIMETypesService.GetListTinymce();
            List<ManagerFile.TinymceFile> ListFiles = new List<ManagerFile.TinymceFile>();
            
            try
            {
                if (!Directory.Exists(dirPath))
                {                    
                    return Json(new API.Models.MsgError() { Success = false, Msg = "Folder Không tồn tại" });
                }
                else
                {
                    if (Request.Form.Files.Count > 0) {
                        for (int i = 0; i < Request.Form.Files.Count(); i++) {
                            if (Request.Form.Files[i].Length>0) {
                                FilePathUpload = dirPath + Request.Form.Files[i].FileName;
                                Extension = Path.GetExtension(FilePathUpload);
                                Size = Request.Form.Files[i].Length.ToString();
                                for (int j = 0; j < listMIMETypes.Count(); j++)
                                {
                                    if (Extension.ToLower() == listMIMETypes[j].ToLower())
                                    {
                                        flagSave = true;
                                    }
                                }
                                if (flagSave) {// Được quyền upload
                                    if (System.IO.File.Exists(FilePathUpload))
                                    {
                                        ListFiles.Add(new ManagerFile.TinymceFile() { Name = Request.Form.Files[i].FileName, StrSize = Size, Selected = false, Msg = "File đã tồn tại" });
                                    }
                                    else
                                    {
                                        using (var fileStream = new FileStream(FilePathUpload, FileMode.Create))
                                        {
                                            try
                                            {
                                                await Request.Form.Files[i].CopyToAsync(fileStream);
                                                ListFiles.Add(new ManagerFile.TinymceFile() { Name = Request.Form.Files[i].FileName, StrSize = Size, Selected = true });
                                            }
                                            catch
                                            {
                                                ListFiles.Add(new ManagerFile.TinymceFile() { Name = Request.Form.Files[i].FileName, StrSize = Size, Selected = false, Msg = "Bạn không có quyền" });
                                            }
                                        }
                                        
                                    }
                                    
                                }
                                else
                                {
                                    ListFiles.Add(new ManagerFile.TinymceFile() { Name = Request.Form.Files[i].FileName, StrSize = Size, Selected = false, Msg = "File không hợp lệ" });
                                }
                                
                                flagSave = false;
                            }
                        }
                    }
                    return Json(new API.Models.MsgSuccess() { Msg = "i" ,Data = ListFiles });
                }
            }
            catch
            {
                return Json(new API.Models.MsgError() { Msg = "" });
            }
        }


        public string ValidatePath(string CurrentFolder) {
            this.UrlRoot = HttpContext.Session.GetString("DomainFolderUpload") + "/";

            if (CurrentFolder == "" || CurrentFolder == "/" || CurrentFolder.ToUpper() == this.UrlRoot.ToUpper())
            {
                CurrentFolder = this.UrlRoot;
            }
            else
            {
                CurrentFolder = this.UrlRoot + CurrentFolder.Substring(this.UrlRoot.Length, CurrentFolder.Length - this.UrlRoot.Length);
            }
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop/") + CurrentFolder;
            
            return filePath;
        }

        [HttpGet]
        public async Task<dynamic> DownloadFile([FromQuery] string url, string folder = "temp")
        {
            try
            {
                if (folder == "" || folder == null) { folder = "temp"; }
                string TenFile = ManagerFilesService.ValidateFileName(url);
                folder = ManagerFilesService.ValidateFileName(folder);
                if (url == null) { return Content("filename not present"); };
                var path = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop", folder + "/" + url);
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var contentType = "application/octet-stream";
                return File(memory, contentType, Path.GetFileName(path));
            }
            catch (Exception e)
            {
                return new API.Models.MsgError() { Msg = e.Message };
            }
        }

        [HttpGet]
        public async Task<dynamic> DownloadFileURL([FromQuery] string url)
        {
            try
            {
                var path = url;
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var contentType = "application/octet-stream";
                return File(memory, contentType, Path.GetFileName(path));
            }
            catch (Exception e)
            {
                return new API.Models.MsgError() { Msg = e.Message };
            }
        }




    }
}