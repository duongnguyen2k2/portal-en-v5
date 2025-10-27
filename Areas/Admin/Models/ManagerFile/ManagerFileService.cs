using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ManagerFile
{
    public class ManagerFileService
    {
        public static List<ManagerFile.TinymceFolder> GetListPagination(ManagerFile.SearchTinymceFolder dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" , "@ParentId" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.ParentId });
            if (tabl == null)
            {
                return new List<ManagerFile.TinymceFolder>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ManagerFile.TinymceFolder
                        {
                            Id = (int)r["Id"],
                            ParentId = (int)r["ParentId"],
                            Name = (string)r["Title"],
                            Path = (string)r["Path"],
                            PathParent = (string)r["PathParent"],
                            Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                            Support = (string)((r["Support"] == System.DBNull.Value) ? "" : r["Support"]),                                                        
                            Img = (string)((r["Img"] == System.DBNull.Value) ? "/images/tinymce/folder.png" : r["Img"]),                                                        
                            Alias = (string)r["Alias"],
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            StrPermission = (string)((r["StrPermission"] == System.DBNull.Value) ? "" : r["StrPermission"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            FolderChild = (int)((r["FolderChild"] == System.DBNull.Value) ? 0 : r["FolderChild"]),
                            FolderChildAll = (int)((r["FolderChildAll"] == System.DBNull.Value) ? 0 : r["FolderChildAll"]),
                            FileChild = (int)((r["FileChild"] == System.DBNull.Value) ? 0 : r["FileChild"]),
                            FileChildAll = (int)((r["FileChildAll"] == System.DBNull.Value) ? 0 : r["FileChildAll"]),
                            CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy")),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),                            
                        }).ToList();
            }


        }

        public static List<ManagerFile.TinymceFolder> GetListPaginationFE(ManagerFile.SearchTinymceFolder dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@ParentId", "@IdGroup", "@TypeSearch" },
                new object[] { "GetListPaginationFE", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.ParentId,dto.IdGroup,dto.TypeSearch });
            if (tabl == null)
            {
                return new List<ManagerFile.TinymceFolder>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ManagerFile.TinymceFolder
                        {
                            Id = (int)r["Id"],
                            ParentId = (int)r["ParentId"],
                            Name = (string)r["Title"],
                            Path = (string)r["Path"],
                            Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                            Support = (string)((r["Support"] == System.DBNull.Value) ? "" : r["Support"]),
                            PathParent = (string)r["PathParent"],
                            Img = (string)((r["Img"] == System.DBNull.Value) ? "/images/tinymce/folder.png" : r["Img"]),
                            Alias = (string)r["Alias"],
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),                            
                            CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy")),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            FolderChild = (int)((r["FolderChild"] == System.DBNull.Value) ? 0 : r["FolderChild"]),
                            FolderChildAll = (int)((r["FolderChildAll"] == System.DBNull.Value) ? 0 : r["FolderChildAll"]),
                            FileChild = (int)((r["FileChild"] == System.DBNull.Value) ? 0 : r["FileChild"]),
                            FileChildAll = (int)((r["FileChildAll"] == System.DBNull.Value) ? 0 : r["FileChildAll"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }


        }

        public static List<ManagerFile.TinymceFile> GetListPaginationFiles(ManagerFile.SearchTinymce dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdFolder" },
                new object[] { "GetListPaginationFiles", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdFolder });
            if (tabl == null)
            {
                return new List<ManagerFile.TinymceFile>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ManagerFile.TinymceFile
                        {
                            Id = (int)r["Id"],
                            IdFolder = (int)r["IdFolder"],
                            Name = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                            Path = (string)((r["Path"] == System.DBNull.Value) ? "" : r["Path"]),
                            PathParent = (string)((r["PathParent"] == System.DBNull.Value) ? "" : r["PathParent"]),
                            FolderTitle = (string)((r["FolderTitle"] == System.DBNull.Value) ? "" : r["FolderTitle"]),
                            Img = "/images/tinymce/image.png",
                            Extension = (string)((r["Extension"] == System.DBNull.Value) ? "" : r["Extension"]),
                            Size = (Double)((r["Size"] == System.DBNull.Value) ? Double.Parse("0"): r["Size"]),
                            IsImage = (Boolean)((r["IsImage"] == System.DBNull.Value) ? false: r["IsImage"]),
                            StrSize = (string)((r["StrSize"] == System.DBNull.Value) ? "" : r["StrSize"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy")),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }


        }

        public static List<ManagerFile.TinymceFile> GetListPaginationFilesFE(ManagerFile.SearchTinymce dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdFolder", "@IdGroup" },
                new object[] { "GetListPaginationFilesFE", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdFolder,dto.IdGroup });
            if (tabl == null)
            {
                return new List<ManagerFile.TinymceFile>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ManagerFile.TinymceFile
                        {
                            Id = (int)r["Id"],
                            IdFolder = (int)r["IdFolder"],
                            Name = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                            Path = (string)((r["Path"] == System.DBNull.Value) ? "" : r["Path"]),
                            PathParent = (string)((r["PathParent"] == System.DBNull.Value) ? "" : r["PathParent"]),
                            FolderTitle = (string)((r["FolderTitle"] == System.DBNull.Value) ? "" : r["FolderTitle"]),
                            Img = "/images/tinymce/image.png",
                            Extension = (string)((r["Extension"] == System.DBNull.Value) ? "" : r["Extension"]),
                            Size = (Double)((r["Size"] == System.DBNull.Value) ? Double.Parse("0") : r["Size"]),
                            IsImage = (Boolean)((r["IsImage"] == System.DBNull.Value) ? false : r["IsImage"]),
                            StrSize = (string)((r["StrSize"] == System.DBNull.Value) ? "" : r["StrSize"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy")),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }


        }

        public static List<ManagerFile.FolderGroup> GetListGroupsByFolder(int IdFolder, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@IdFolder" }, new object[] { "GetListGroupsByFolder", IdFolder });
            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.FolderGroup
                    {                       
                        IdFolder = (int)r["IdFolder"],                        
                        IdGroup = (int)r["IdGroup"],
                        FolderName = (string)((r["FolderName"] == System.DBNull.Value) ? "" : r["FolderName"]) ,
                        GroupTitle = (string)((r["GroupTitle"] == System.DBNull.Value) ? "" : r["GroupTitle"]) ,
                        IdsFolder = MyModels.Encode((int)r["IdFolder"], SecretId),
                        IdsGroup = MyModels.Encode((int)r["IdGroup"], SecretId),
                        Selected = (Boolean)((r["Selected"] == System.DBNull.Value) ? false : r["Selected"]),                        
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),                        
                    }).ToList();
        }

        public static ManagerFile.TinymceFile GetItemFile(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id" }, new object[] { "GetItemFile", Id });
            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFile
                    {
                        Id = (int)r["Id"],
                        IdFolder = (int)r["IdFolder"],
                        Name = (string)r["Title"],
                        Path = (string)r["Path"],                        
                        Img = "/images/tinymce/folder.png",
                        Extension = (string)r["Extension"],
                        Size = (Double)((r["Size"] == System.DBNull.Value) ? Double.Parse("0") : r["Size"]),
                        StrSize = (string)((r["StrSize"] == System.DBNull.Value) ? "" : r["StrSize"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy")),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }



        public static List<ManagerFile.TinymceFolder> GetListBreadcrumb(int ParentId,string SecretId)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
                new string[] { "@flag", "@ParentId" }, new object[] { "GetListBreadcrumb", ParentId });

            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFolder
                    {
                        Id = (int)r["Id"],
                        ParentId = (int)r["ParentId"],
                        IsActive = (int)r["IsActive"],
                        Name = (string)r["Title"],
                        Path = (string)r["Path"],
                        PathParent = (string)r["PathParent"],
                        Alias = (string)r["Alias"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Support = (string)((r["Support"] == System.DBNull.Value) ? "" : r["Support"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).ToList();

        }

        public static List<ManagerFile.TinymceFolder> GetListSelectBox(int Id)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id" }, new object[] { "GetListSelectBox", Id });
            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFolder
                    {
                        Id = (int)r["Id"],
                        ParentId = (int)r["ParentId"],
                        Name = (string)r["Title"],
                        Path = (string)r["Path"],
                        PathParent = (string)r["PathParent"],
                        Alias = (string)r["Alias"]                        
                    }).ToList();
        }

        public static ManagerFile.TinymceFolder GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFolder
                    {
                        Id = (int)r["Id"],
                        ParentId = (int)r["ParentId"],                        
                        Name = (string)r["Title"],
                        Path = (string)r["Path"],
                        PathParent = (string)r["PathParent"],
                        Alias = (string)r["Alias"],
                        Img = (string)((r["Img"] == System.DBNull.Value) ? "/images/tinymce/folder.png" : r["Img"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Support = (string)((r["Support"] == System.DBNull.Value) ? "" : r["Support"]),
                        StrPermission = (string)((r["StrPermission"] == System.DBNull.Value) ? "" : r["StrPermission"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static ManagerFile.TinymceFolder GetItemByAlias(string Alias, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFolder
                    {
                        Id = (int)r["Id"],
                        ParentId = (int)r["ParentId"],
                        Name = (string)r["Title"],
                        Path = (string)r["Path"],
                        PathParent = (string)r["PathParent"],
                        Alias = (string)r["Alias"],
                        Img = (string)((r["Img"] == System.DBNull.Value) ? "/images/tinymce/folder.png" : r["Img"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Support = (string)((r["Support"] == System.DBNull.Value) ? "" : r["Support"]),
                        StrPermission = (string)((r["StrPermission"] == System.DBNull.Value) ? "" : r["StrPermission"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveListFile(List<ManagerFile.TinymceFile> ListFiles, int IdFolder)
        {
            DataTable tbItem = new DataTable();
            tbItem.Columns.Add("IdFolder", typeof(int));            
            tbItem.Columns.Add("Title", typeof(string));
            tbItem.Columns.Add("Alias", typeof(string));
            tbItem.Columns.Add("Description", typeof(string));
            tbItem.Columns.Add("Size", typeof(float));
            tbItem.Columns.Add("CreatedBy", typeof(float));                        
            tbItem.Columns.Add("StrSize", typeof(string));
            tbItem.Columns.Add("Path", typeof(string));
            tbItem.Columns.Add("Extension", typeof(string));
            tbItem.Columns.Add("IsImage", typeof(Boolean));


            for (int k = 0; k < ListFiles.Count(); k++)
            {
                if (ListFiles[k].Selected)
                {
                    var row = tbItem.NewRow();
                    row["IdFolder"] = IdFolder;
                    row["Title"] = ListFiles[k].Name;
                    row["Alias"] = API.Models.MyHelper.StringHelper.UrlFriendly(ListFiles[k].Name);
                    row["Description"] = "";
                    row["Size"] = ListFiles[k].Size;
                    row["CreatedBy"] = ListFiles[k].CreatedBy;
                    row["StrSize"] = ListFiles[k].StrSize;
                    row["Path"] = ListFiles[k].Path;
                    row["Extension"] = ListFiles[k].Extension.Replace(".", "").Trim();
                    row["IsImage"] = ListFiles[k].IsImage;
                    tbItem.Rows.Add(row);
                }
                
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@TBL_ListFiles" },
            new object[] { "SaveListFiles",tbItem });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveManagerFolderGroup(List<ManagerFile.FolderGroup> ListFiles, int IdFolder)
        {
            List<string> ListTitle = new List<string>();
            DataTable tbItem = new DataTable();
            tbItem.Columns.Add("IdFolder", typeof(int));
            tbItem.Columns.Add("IdGroup", typeof(int));
            
            for (int k = 0; k < ListFiles.Count(); k++)
            {
                if (ListFiles[k].Selected)
                {
                    var row = tbItem.NewRow();
                    row["IdFolder"] = IdFolder;
                    row["IdGroup"] = ListFiles[k].IdGroup;                
                    tbItem.Rows.Add(row);
                    ListTitle.Add(ListFiles[k].GroupTitle);
                }

            }
            string StrPermission = string.Join(",", ListTitle);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@TBL_ManagerFolderGroup", "@IdFolder", "@StrPermission" },
            new object[] { "SaveManagerFolderGroup", tbItem , IdFolder, StrPermission });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveItem(ManagerFile.TinymceFolder dto)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@ParentId", "@Path", "@PathParent", "@Title", "@Alias", "@Description",  "@CreatedBy", "@ModifiedBy", "@StrPermission" , "@Link", "@Support", "@Img" },
            new object[] { "SaveItem", dto.Id,dto.ParentId,dto.Path,dto.PathParent, dto.Name,dto.Alias,  dto.Description,  dto.CreatedBy, dto.ModifiedBy,dto.StrPermission ,dto.Link,dto.Support,dto.Img});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(ManagerFile.TinymceFolder dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id",  "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic MoveFolder(ManagerFile.TinymceFolder dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@ModifiedBy", "@ParentId", "@PathParent" },
            new object[] { "MoveFolder", dto.Id, dto.ModifiedBy,dto.ParentId,dto.PathParent });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static dynamic DeleteFile(ManagerFile.TinymceFile dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteFile", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static dynamic UpdateFileInfo(ManagerFile.TinymceFile dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@ModifiedBy","@Title" },
            new object[] { "UpdateFileInfo", dto.Id, dto.ModifiedBy,dto.Name });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static dynamic DeleteItem(ManagerFile.TinymceFolder dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static string GetSizeInMemory(Double bytesize)
        {


            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Convert.ToDouble(bytesize);
            int order = 0;
            while (len >= 1024D && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", len, sizes[order]);
        }

        public static List<ManagerFile.TinymceFolder> GetListAllFolder()
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag"},
            new object[] { "GetListAllFolder" });

            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFolder
                    {
                        Id = (int)r["Id"],
                        ParentId = (int)r["ParentId"],
                        Name = (string)r["Title"],
                        Path = (string)r["Path"],
                        PathParent = (string)r["PathParent"],
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Support = (string)((r["Support"] == System.DBNull.Value) ? "" : r["Support"]),
                        Img = (string)((r["Img"] == System.DBNull.Value) ? "/images/tinymce/folder.png" : r["Img"]),
                        Alias = (string)r["Alias"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        StrPermission = (string)((r["StrPermission"] == System.DBNull.Value) ? "" : r["StrPermission"])                        
                    }).ToList();

        }

        public static List<ManagerFile.TinymceFile> GetListAllFiles()
        {
           
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
                new string[] { "@flag"},
                new object[] { "GetListAllFiles" });
            return (from r in tabl.AsEnumerable()
                    select new ManagerFile.TinymceFile
                    {
                        Id = (int)r["Id"],
                        IdFolder = (int)r["IdFolder"],
                        Name = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Path = (string)((r["Path"] == System.DBNull.Value) ? "" : r["Path"]),
                        PathParent = (string)((r["PathParent"] == System.DBNull.Value) ? "" : r["PathParent"]),
                        FolderTitle = (string)((r["FolderTitle"] == System.DBNull.Value) ? "" : r["FolderTitle"]),
                        Img = "/images/tinymce/image.png",
                        Extension = (string)((r["Extension"] == System.DBNull.Value) ? "" : r["Extension"]),
                        Size = (Double)((r["Size"] == System.DBNull.Value) ? Double.Parse("0") : r["Size"]),
                        IsImage = (Boolean)((r["IsImage"] == System.DBNull.Value) ? false : r["IsImage"]),
                        StrSize = (string)((r["StrSize"] == System.DBNull.Value) ? "" : r["StrSize"]),
                        CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy"))                        
                    }).ToList();


        }

        public static dynamic UpdateAliasFile(int Id,string Alias)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@Alias"},
            new object[] { "UpdateAliasFile", Id, Alias });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static dynamic UpdateAliasFolder(int Id,string Alias)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ManagerFolder",
            new string[] { "@flag", "@Id", "@Alias" },
            new object[] { "UpdateAliasFolder", Id, Alias });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

    }
}
