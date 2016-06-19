using System;
using System.Collections.Generic;
using System.Linq;
using Ayatta.Model;
using Ayatta.Domain;


namespace Ayatta.Web.Models
{
    public static class HomeModel
    {
        public class Index : ViewModel
        {
           public Prod.Item Item{get;set;}          
           
        }  
        
    }
}