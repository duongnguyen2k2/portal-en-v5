using API.Areas.Admin.Models.Reviews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            ReviewsModel data = new ReviewsModel() { Item = new Reviews(),SearchData = new SearchReviews()};
            
            data.ListStar = ReviewsService.GetListStar();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Reviews model)
        {
            model.Id = 0;
            model.CreatedBy = 0;
            model.ModifiedBy = 0;
            model.ReviewDateShow = DateTime.Now.ToString("dd/MM/yyyy");
            model.CreatedDate = DateTime.Now;
            ReviewsModel data = new ReviewsModel() { Item = model };

            if (ModelState.IsValid)
            {
                
                model.IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId"));
                try
                {
                    ReviewsService.SaveItem(model);
                    TempData["MessageSuccess"] = "Gửi đánh giá website thành công";
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    TempData["MessageError"] = "Gửi đánh giá website Thất bại. Xin vui lòng gửi lại(" + e.Message.ToString()+")";

                }                
            }
            data.ListStar = ReviewsService.GetListStar();
            return View(data);
        }
    }
}
