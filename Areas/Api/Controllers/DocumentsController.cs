
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.DocumentsCategories;
using API.Areas.Admin.Models.DocumentsType;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using API.Areas.Admin.Models.CategoriesArticles;

namespace API.Areas.Api.Controllers
{
    [Area("Api")]
    public class DocumentsController : Controller
    {
        public IActionResult GetList([FromQuery] SearchDocuments dto)
        {
            dto.ShowStartDate = "01/01/1990";
            dto.Status = 1;
            DocumentsModel data = new DocumentsModel() { SearchData = dto };
            var ListItem = DocumentsService.GetListPagination(data.SearchData,"");                        
            return Json(new MsgSuccess() { Data = ListItem });
        }

        public IActionResult Detail([FromQuery] int Id = 0, int IdCoQuan = 0)
        {           
            var Item = DocumentsService.GetItem(Id, "12", IdCoQuan);
            return Json(new MsgSuccess() { Data = Item });
        }
    }
}
