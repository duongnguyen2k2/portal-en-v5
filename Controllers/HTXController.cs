using API.Areas.Admin.Models.HTX;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class HTXController : Controller
    {
        public IActionResult Index()
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            SearchHTX dto = new SearchHTX();
            dto.IdCoQuan = IdCoQuan;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            HTXModel model = new HTXModel() { SearchData = dto};
            model.ListItems = HTXService.GetListPaginationClient(dto, API.Models.Settings.SecretId + ControllerName, CultureInfo.CurrentCulture.Name.ToLower());

            dto.IdCoQuan = IdCoQuan;
            if (model.ListItems != null && model.ListItems.Count() > 0)
            {
                TotalItems = model.ListItems[0].TotalRows;
            }
            model.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = model.SearchData.CurrentPage, ItemsPerPage = model.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(model);
        }
    }
}
