using Grand.Web.Framework;
using Grand.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

namespace Grand.Plugin.Shipping.VoidShipping.Models
{
    public class ByAeroModel : BaseNopModel
    {       
        [NopResourceDisplayName("Plugins.Shipping.Void.ByAero.Fields.Address")]
        [AllowHtml]
        public string Address { get; set; }


        [NopResourceDisplayName("Plugins.Shipping.Void.ByAero.Fields.SomeNumber")]
        [AllowHtml]
        public int SomeNumber { get; set; }

    }
}