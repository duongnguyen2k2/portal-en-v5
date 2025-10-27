using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ManagerFile
{
    public class ManagerFile
    {
        public class TinymceFolder
        {
            public int Id { get; set; }
            public int Acl { get; set; } = 0;
            public int ParentId { get; set; } = 0;
            public int IdUser { get; set; } = 0;
            public Boolean HasChildren { get; set; } = false;
            public string? Name { get; set; }
            public string? Url { get; set; }
            public string? Path { get; set; }
            public string? PathParent { get; set; }
            public string? Img { get; set; }
            public string? Alias { get; set; }
            public string? Description { get; set; }
            public string? Icon { get; set; }
            public string? Ids { get; set; }
            public string? StrPermission { get; set; }
            public string? Link { get; set; }
            public string? Support { get; set; }
            public string? CreatedDateShow { get; set; }

            public List<TinymceFolder> Child { get; set; }
            public Boolean Selected { get; set; } = false;
            public int CreatedBy { get; set; } = 0;
            public int ModifiedBy { get; set; } = 0;
            public int IsActive { get; set; } = 0;
            public int Ordering { get; set; } = 0;
            public int FolderChild { get; set; } = 0;
            public int FolderChildAll { get; set; } = 0;
            public int FileChild { get; set; } = 0;
            public int FileChildAll { get; set; } = 0;
        }

        public class TinymceFile
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Alias { get; set; }
            public Double Size { get; set; }
            public string? StrSize { get; set; }
            public string? CreatedDateShow { get; set; }
            public string? Date { get; set; }
            public string? Path { get; set; }
            public string? Img { get; set; }
            public string? Extension { get; set; }
            public string? Description { get; set; }
            public Boolean IsImage { get; set; } = false;
            public string? Space { get; set; } = "Byte";
            public Boolean Selected { get; set; } = false;
            public string? Msg { get; set; }
            public string? Ids { get; set; }
            public string? PathParent { get; set; }
            public string? FolderTitle { get; set; }
            public int CreatedBy { get; set; } = 0;
            public int IdFolder { get; set; } = 0;
            public int ModifiedBy { get; set; } = 0;
            public int Ordering { get; set; } = 0;

        }

        public class TinymceGetFiles
        {
            public TinymceFolder CurrentFolder { get; set; } = new TinymceFolder() { };
            public List<TinymceFile> Files { get; set; } = new List<TinymceFile>() { };
        }


        public class SearchTinymceFolder
        {
            public string? Command { get; set; }
            public string? Lang { get; set; }
            public string? Type { get; set; }
            public string? CurrentFolder { get; set; }
            public string? Hash { get; set; }
            public int CurrentPage { get; set; }
            public int ItemsPerPage { get; set; }
            public int ParentId { get; set; }
            public int IdGroup { get; set; }
            public int TypeSearch { get; set; }
            public string? ParentIds { get; set; }
            public string? Keyword { get; set; }
        }

        public class SearchTinymce
        {
            public string? Path { get; set; }
            public string? Name { get; set; }
            public int flag { get; set; }
            public string? Extension { get; set; }
            public int CurrentPage { get; set; }
            public int ItemsPerPage { get; set; }
            public int IdFolder { get; set; }
            public int IdGroup { get; set; }
            public string? IdsFolder { get; set; }
            public string? Ordering { get; set; }
            public string? Keyword { get; set; }
        }

        public class TinymceError
        {
            public Boolean Success { get; set; }
            public string? Msg { get; set; }
            public dynamic Data { get; set; }
        }

        public class Breadcrumb
        {
            public string? Name { get; set; }
            public string? Path { get; set; }
            public Boolean IsActive { get; set; }
        }

        public class FolderGroup
        {
            public int IdFolder { get; set; }
            public int IdGroup { get; set; }
            public string? GroupTitle { get; set; }
            public string? FolderName { get; set; }
            public string? IdsFolder { get; set; }
            public string? IdsGroup { get; set; }
            public Boolean Selected { get; set; }
            public Boolean Status { get; set; }
        }

        public class InfoFile
        {
            public string? Name { get; set; }// Tên file
            public string? Path { get; set; }// File url gốc 
            public string? Folder { get; set; }
            public string? FilePath { get; set; } // Link File trên web mới //documents/imgurl.jpg
            public string? DirFilePath { get; set; } // Link File gốc
            public int TypeFile { get; set; } = 1;// 1=documents, 2=journal


        }

    }
}
