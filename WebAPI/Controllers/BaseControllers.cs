using Base.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class PortalBaseController: ControllerBase
    {
        public Customer? Customer { get; set; }
    }

    public class APIBaseController : ControllerBase
    {
        
    }
}
