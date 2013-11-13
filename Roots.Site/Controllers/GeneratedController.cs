using Roots.SupportedFileTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Roots.Site.Controllers
{
    public class GeneratedController : Controller
    {        
        public ActionResult Index()
        {
            return PartialView(
                MimeTypes
                    .GetAllProperties()
                    .Select(x=>
                        new KeyValuePair<string,string>
                        (
                            x.Name, 
                            ((FileType)x.GetValue(null)).MimeType
                        )));
        }  
    }
}
