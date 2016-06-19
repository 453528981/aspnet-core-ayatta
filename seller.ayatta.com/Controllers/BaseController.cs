using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Ayatta.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        
        protected DefaultStorage DefaultStorage { get; private set; }
        public BaseController(DefaultStorage defaultStorage)
        {
            DefaultStorage = defaultStorage;
        }       
       
    }
}