using Grand.Web.Framework;
using Grand.Web.Framework.Mvc;

namespace Grand.Plugin.Shipping.VoidShipping.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Shipping.ByVoid.Fields.Country")]
        public string Country { get; set; }
        public bool Country_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.ByVoid.Fields.State")]
        public string State { get; set; }
        public bool State_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.ByVoid.Fields.City")]
        public string City { get; set; }
        public bool City_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.ByVoid.Fields.Street")]
        public string Street { get; set; }
        public bool Street_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.ByVoid.Fields.ZipCode")]
        public string ZipCode { get; set; }
        public bool ZipCode_OverrideForStore { get; set; }


    }
}