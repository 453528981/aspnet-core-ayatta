using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Html;
namespace Microsoft.AspNetCore.Mvc.Rendering
{
	public static class HtmlHelperExtensions
	{
		public static IHtmlContent Json(this IHtmlHelper htmlHelper,object value)
		{
			if (htmlHelper == null)
			{
				throw new ArgumentNullException("htmlHelper");
			}
            var json=JsonConvert.SerializeObject(value);
			return htmlHelper.Raw(json);
		}
		
	}
}
