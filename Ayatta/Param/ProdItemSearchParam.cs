using System;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Ayatta.Param
{
    public class SearchParam
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;        
        public IDictionary<string, bool> OrderBy{get;set;}
    }
    public class ProdItemSearchParam : SearchParam
    {

        public int? UserId { get; set; }
        public int Id { get; set; } = 0;
        public int CRId { get; set; } = 0;
        public string Code { get; set; }
        public string Name { get; set; }

        public decimal PriceFrom { get; set; }
        public decimal PriceEnd { get; set; }

        public int SaleFrom { get; set; }
        public int SaleEnd { get; set; }
    }

}