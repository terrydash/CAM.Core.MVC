
namespace CAM.Core.MVC
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Newtonsoft.Json;
    using CAM.Core.Model.Filter;

    public partial class _Controller_ForFilter
    {
        /// <summary>
        /// 将标准Filter序列化为JSON字符串
        /// </summary>
        /// <typeparam name="TFilterType"></typeparam>
        /// <returns></returns>
        protected string serializeFilter<TFilterType>() where TFilterType : class, new()
        {
            TFilterType filterObject = new TFilterType();
            string json = JsonConvert.SerializeObject(filterObject);
            return json;
        }

        protected TFilter deserializeFilter<TFilter>(string filterString = "")
            where TFilter : BaseFilter, new()
        {
            filterString = string.IsNullOrEmpty(filterString) ? "" : filterString;
            TFilter filterObject;
            if (string.IsNullOrWhiteSpace(filterString))
            {
                filterObject = new TFilter();
            }
            else
            {
                filterObject = JsonConvert.DeserializeObject<TFilter>(filterString);
            }
            return filterObject;
        }
    }
}
