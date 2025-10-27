using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Filters
{
    public class SampleActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            int a = 0;
            a = a + 1;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            int a = 0;
            a = a + 1;
            // do something after the action executes
        }
    }
}
