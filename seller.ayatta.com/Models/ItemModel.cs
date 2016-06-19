using System;
using System.Collections.Generic;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Model;
using Ayatta.Param;

namespace Ayatta.Web.Models
{
    public static class ItemModel
    {
        public class Index : ViewModel
        {
            public ProdItemSearchParam Param { get; set; }
            public IPagedList<Prod.Item> Items { get; set; }
        }

        public class Catg : ViewModel
        {
            public string Name { get; set; }
            //public IList<Data.Catg> Catgs { get; set; }

        }
        public class Item : ViewModel<Prod.Mini>
        {
            public int CatgId { get; set; }
            public Item(Prod.Mini data) : base(data)
            {
            }


        }

    }
}