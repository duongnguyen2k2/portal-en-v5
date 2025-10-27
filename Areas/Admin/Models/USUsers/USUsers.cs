using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.USUsers
{
    public class USUsers
    {
        public int Id { get; set; }
        [Display(Name = "UserName")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài Tài khoản lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string? UserName { get; set; }
        public string? UserCode { get; set; }
        [Display(Name = "Email")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài Email lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordShow { get; set; }
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Description { get; set; }
        public int Birthday { get; set; }
        public string? BirthdayShow { get; set; }
        public byte Gender { get; set; } = 1;
        [Range(1, 999999999, ErrorMessage = "Quyền người dùng không được để trống")]
        public int IdGroup { get; set; }
        public string? GroupsTitle { get; set; }
        public int TotalRows { get; set; }
        public byte Status { get; set; } = 1;
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public int IdCoQuan { get; set; }
        public int IdHuyen { get; set; } = 0;
        public int IdTinhThanh { get; set; } = 0;
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public Boolean Deleted { get; set; } = false;
        public string? Ids { get; set; }
        public string? TenCoQuan { get; set; }
        public string? Specialize { get; set; }
        public string? TenChucVu { get; set; }
        public int IdRegency { get; set; }

        public int CQLevel { get; set; }
        public int Ordering { get; set; } = 100;

        public string? Address { get; set; } = "";
        public int IdDanToc { get; set; } = 1;
        public string? TenDanToc { get; set; } = "";
        public string? TenTonGiao { get; set; } = "";
        public string? TenTrinhDo { get; set; } = "";
        public int IdTonGiao { get; set; } = 0;
        public int IdTrinhDo { get; set; } = 0;
        public int DangVien { get; set; } = 0;
        public string? ShowDangVien { get; set; } = "";
        public string? GenderNam { get; set; } = "";
        public string? GenderNu { get; set; } = "";
        public byte NamSinh { get; set; } = 1;
        public int ShowDateUser { get; set; } = 0;
        public string? BirthdayNamSinhShow { get; set; }
        public Boolean Flag { get; set; } = true;
        public string? Msg { get; set; } = "";
        public int MasterId { get; set; } = 0;
        public int IdPhongBan { get; set; } = 0;
        public string? TitlePhongBan { get; set; } = "";
        public int ManufacturerId { get; set; }

        public string? ManufacturerTitle { get; set; }
        public string? ManufacturerAddress { get; set; }
        public string? ManufacturerTelephone { get; set; }
    }

    public class USUsersLogin
    {
        public int Id { get; set; }
        public int IdGroup { get; set; }
        public int CQLevel { get; set; }
        public int CategoryId { get; set; }
        public int MasterId { get; set; }
        public int IdCoQuan { get; set; }
        public int IdHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? UserCode { get; set; }
        public string? TenCoQuan { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? CompanyName { get; set; }
        public string? ListCatId { get; set; }
        public string? ListMenuId { get; set; }
        public byte Gender { get; set; } = 1;
    }

    public class USUsersExcel
    {
        public int IdCoQuan { get; set; }
        public int Birthday { get; set; }
        public int NamSinh { get; set; }
        public int IdDanToc { get; set; }
        public int DangVien { get; set; }
        public int IdTonGiao { get; set; }
        public int IdTrinhDo { get; set; }
        public int Ordering { get; set; }
        public byte Gender { get; set; }
        public string? Specialize { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? UserName { get; set; }
    }

    public class USInformation
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? UserCode { get; set; }
        public string? Avatar { get; set; }
        public string? Fax { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public int Birthday { get; set; }
        public byte Gender { get; set; } = 1;
        //        public MyEntity.MyDateTime BirthdayShow { get; set; }
        public string? Image { get; set; }
        public string? Token { get; set; }
        public int IdGroup { get; set; }
        public int IdCoQuan { get; set; }
        public XSRF XSRFInfo { get; set; }
        public UserToken UserToken { get; set; }
        public dynamic ListRoutes { get; set; }
        public dynamic MenuInfo { get; set; }
    }

    public class USUsersLog
    {
        public string? Browser { get; set; }
        public string? Platform { get; set; }
        public int IdUser { get; set; } = 0;
        public string? LoginInfo { get; set; }
        public string? Ip { get; set; }
        public string? Token { get; set; }
        public string? Description { get; set; }
        public int CreatedDate { get; set; }
    }
    public class AccountLogin
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? TokenRecaptcha { get; set; }
        public int IdTypeSocial { get; set; }

    }
    public class ChangePassword
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? RePassword { get; set; }
        public int Id { get; set; }
    }

    public class SearchUSUsers
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public int IdGroup { get; set; }
        public int IdLevel { get; set; }
        public string? Keyword { get; set; }
        public int Status { get; set; } = -1;
    }



    public class UserToken
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? UserCode { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
        public string? FullName { get; set; }
        public int Birthday { get; set; }
        public byte Gender { get; set; } = 1;
        public int IdGroup { get; set; }
        public int IdCoQuan { get; set; }
        public int IdHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public int CategoryId { get; set; }
        public string? GroupsName { get; set; }
        public string? TemplateName { get; set; }
        public string? FolderUpload { get; set; }
        public string? Avatar { get; set; }
        public string? TenChucVu { get; set; }
        public int IdRegency { get; set; }
        public int ManufacturerId { get; set; }

    }

    public class USUsersModel
    {
        public List<USUsers> ListItems { get; set; } = new List<USUsers>() { };
        public List<SelectListItem> ListItemsGroups { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsStatus { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListDMChucVu { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListDMDanToc { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListDMTonGiao { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListDMTrinhDo { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemDangVien { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemDanhMuc { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsManufacturer { get; set; } = new List<SelectListItem>() { };
        public SearchUSUsers SearchData { get; set; } = new SearchUSUsers() { };
        public USUsers Item { get; set; } = new USUsers() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class XSRF
    {
        public string? X_XSRF_TOKEN { get; set; }
        public string? DateTime { get; set; }
        public string? Token { get; set; }
    }
}
