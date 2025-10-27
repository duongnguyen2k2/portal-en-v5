using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin.Controllers
{
    public class SmartVoiceController : Controller
    {
        public IActionResult Index()
        {

            return Json(new API.Models.MsgSuccess() { Data = null});
        }
    }
}
