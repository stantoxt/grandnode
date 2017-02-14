using System;
using System.Web.Routing;
using Grand.Core;
using Grand.Core.Domain.Shipping;
using Grand.Core.Plugins;
using Grand.Services.Catalog;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Shipping;
using Grand.Services.Shipping.Tracking;
using System.Collections.Generic;
using System.Web.Mvc;
using Grand.Plugin.Shipping.VoidShipping.Controllers;

namespace Grand.Plugin.Shipping.VoidShipping
{
    public class VoidShippingComputationMethod : BasePlugin, IShippingRateComputationMethod
    {
        #region Fields

        private readonly IShippingService _shippingService;
        private readonly IStoreContext _storeContext;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor
        public VoidShippingComputationMethod(IShippingService shippingService,
            IStoreContext storeContext,
            IPriceCalculationService priceCalculationService,
            ISettingService settingService)
        {
            this._shippingService = shippingService;
            this._storeContext = storeContext;
            this._priceCalculationService = priceCalculationService;
            this._settingService = settingService;
        }
        #endregion

        #region Utilities
        #endregion

        #region Methods

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="getShippingOptionRequest">A request for getting shipping options</param>
        /// <returns>Represents a response of getting shipping rate options</returns>
        public GetShippingOptionResponse GetShippingOptions(GetShippingOptionRequest getShippingOptionRequest)
        {


            //DEFINE: how many Shipping Options should be available for this plugin (it is defined programatically)

            if (getShippingOptionRequest == null)
                throw new ArgumentNullException("getShippingOptionRequest");

            var response = new GetShippingOptionResponse();



            //you can add any amount of ShippingOptions here
            //with any business logic


            var shippingMethods = new List<ShippingOption>();
            shippingMethods.Add(new ShippingOption()
            {
                Name = "By Aero",
                Description = "shipping via air",
                Rate = 10,
                //RestrictedMinimumOrderTotal = 0,
                ShippingRateComputationMethodSystemName = "Shipping.VoidShipping"
            });

            

            foreach (var shippingOption in shippingMethods)
            {
                response.ShippingOptions.Add(shippingOption);
            }

            

            return response;
        }

        /// <summary>
        /// Gets fixed shipping rate (if shipping rate computation method allows it and the rate can be calculated before checkout).
        /// </summary>
        /// <param name="getShippingOptionRequest">A request for getting shipping options</param>
        /// <returns>Fixed shipping rate; or null in case there's no fixed shipping rate</returns>
        public decimal? GetFixedRate(GetShippingOptionRequest getShippingOptionRequest)
        {
            return null;
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "VoidShipping";
            routeValues = new RouteValueDictionary { { "Namespaces", "Grand.Plugin.Shipping.VoidShipping.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            //var settings = new ShippingByWeightSettings
            //{
            //    LimitMethodsToCreated = false,
            //};
            //_settingService.SaveSetting(settings);


            //database objects
            //_objectContext.Install();

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.Country", "Country Void");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.State", "State Void");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.City", "City Void");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.Street", "Street Void");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.ZipCode", "ZipCode Void");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.ZipCode", "ZipCode Void");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.Fields.ZipCode", "ZipCode Void");

            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.ByAero.Fields.Address", "Your Home Address");
            this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.Void.ByAero.Fields.SomeNumber", "Any Random Number");






            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            //_settingService.DeleteSetting<ShippingByWeightSettings>();

            //database objects
            //_objectContext.Uninstall();

            //locales
            this.DeletePluginLocaleResource("Plugins.Shipping.ByVoid.Fields.Country");
            this.DeletePluginLocaleResource("Plugins.Shipping.ByVoid.Fields.State");
            this.DeletePluginLocaleResource("Plugins.Shipping.ByVoid.Fields.City");
            this.DeletePluginLocaleResource("Plugins.Shipping.ByVoid.Fields.Street");
            this.DeletePluginLocaleResource("Plugins.Shipping.ByVoid.Fields.ZipCode");

            this.DeletePluginLocaleResource("Plugins.Shipping.Void.ByAero.Fields.Address");
            this.DeletePluginLocaleResource("Plugins.Shipping.Void.ByAero.Fields.SomeNumber");


            

            base.Uninstall();
        }

        public Type GetControllerType()
        {
            return typeof(VoidShippingController);
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        public ShippingRateComputationMethodType ShippingRateComputationMethodType
        {
            get
            {
                return ShippingRateComputationMethodType.Offline;
            }
        }


        /// <summary>
        /// Gets a shipment tracker
        /// </summary>
        public IShipmentTracker ShipmentTracker
        {
            get
            {
                //uncomment a line below to return a general shipment tracker (finds an appropriate tracker by tracking number)
                //return new GeneralShipmentTracker(EngineContext.Current.Resolve<ITypeFinder>());
                return null;
            }
        }

        #endregion
    }
}
