using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Grand.Core;
using Grand.Services.Localization;
using Grand.Web.Framework.Controllers;
using Grand.Services.Common;
using System.IO;
using System.Xml.Serialization;
using Grand.Plugin.Shipping.VoidShipping.Domain;
using Grand.Core.Domain.Customers;
using Grand.Plugin.Shipping.VoidShipping.Models;
using System.Xml;


namespace Grand.Plugin.Shipping.VoidShipping.Controllers
{
    [AdminAuthorize]
    public class VoidShippingController : BaseShippingController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        //private readonly IShippingService _shippingService;
        //private readonly IStoreService _storeService;
        private readonly ILocalizationService _localizationService;

        public VoidShippingController(
            IWorkContext workContext,
            IStoreContext storeContext,
            IGenericAttributeService genericAttributeService,
            //IShippingService shippingService,
            //IStoreService storeService,
            ILocalizationService localizationService
            )
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._genericAttributeService = genericAttributeService;
            //this._shippingService = shippingService;
            //this._storeService = storeService;
            this._localizationService = localizationService;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }

        [ChildActionOnly]
        public ActionResult Configure()
        {
            //DEFINE: a custom configuration logic

            //var model = new ShippingByWeightListModel();

            return View("~/Plugins/Shipping.VoidShipping/Views/VoidShipping/Configure.cshtml"/*, model*/);
        }

        public override string GetFormPartialView(string shippingOption)
        {
            //DEFINE: how many Views should be available and with which Models
            if (shippingOption == "By Aero")
            {
                //try to get values from Generic Attribute - if customer comes again here



                var serializedAttribute = _workContext.CurrentCustomer.GetAttribute<string>(
                    SystemCustomerAttributeNames.ShippingOptionAttributeXml,
                    _storeContext.CurrentStore.Id);


                ByAero model123 = new ByAero();

                if(!string.IsNullOrWhiteSpace(serializedAttribute))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ByAero));

                        XmlReaderSettings settings = new XmlReaderSettings();
                        // No settings need modifying here

                        using (StringReader textReader = new StringReader(serializedAttribute))
                        {
                            using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                            {
                                model123 = (ByAero)serializer.Deserialize(xmlReader);
                            }
                        }
                    }
                    catch { }
                }












                ////var stringBuilder = new StringBuilder();
                ////string deserializedAttribute;
                //ByAeroModel model123;
                //try
                //{
                //    using (var sr = new StringReader(serializedAttribute))
                //    {
                //        var xmlS = new XmlSerializer(typeof(ByAeroModel));
                //        //xmlS.Serialize(tw, domainModel);
                //        model123 = (ByAeroModel)xmlS.Deserialize(sr);
                //        //serializedAttribute = stringBuilder.ToString();

                //    }
                //}
                //catch { }


                //try
                //{
                //    using (var sr = new StreamReader(serializedAttribute))
                //    {
                //        var xmlS = new XmlSerializer(typeof(ByAeroModel));
                //        //xmlS.Serialize(tw, domainModel);
                //        model123 = (ByAeroModel)xmlS.Deserialize(sr);
                //        //serializedAttribute = stringBuilder.ToString();

                //    }
                //}
                //catch { }




                //ByAeroModel model1234;


                //try
                //{
                //    using (var sr = new StringReader(serializedAttribute))
                //    {
                //        var xmlS = new XmlSerializer(typeof(ByAero));
                //        //xmlS.Serialize(tw, domainModel);
                //        model1234 = (ByAeroModel)xmlS.Deserialize(sr);
                //        //serializedAttribute = stringBuilder.ToString();

                //    }
                //}
                //catch { }


                //try
                //{
                //    using (var sr = new StreamReader(serializedAttribute))
                //    {
                //        var xmlS = new XmlSerializer(typeof(ByAero));
                //        //xmlS.Serialize(tw, domainModel);
                //        model1234 = (ByAeroModel)xmlS.Deserialize(sr);
                //        //serializedAttribute = stringBuilder.ToString();

                //    }
                //}
                //catch { }




                var model = new ByAeroModel()// { Address = "38-340 Somewhere", SomeNumber = 9876 };
                {
                    Address = (model123.Address != null ? model123.Address : ""),
                    SomeNumber = model123.SomeNumber//(model123.SomeNumber != 0 ? model123.SomeNumber : 0)
                };

                return this.RenderPartialViewToString("~/Plugins/Shipping.VoidShipping/Views/VoidShipping/_ByAero.cshtml", model);
            }

            return "Given Shipping Option Cannnot Be Parsed";
        }


        public override IList<string> ValidateShippingForm(FormCollection form)
        {
            var warnings = new List<string>();

            //you can use 'shippingoption' to differentiate what ShippingOption Customer has checked
            var shippingOption = form.GetValue("shippingoption").AttemptedValue.Replace("___", "_").Split(new[] { '_' })[0];


            if (shippingOption == "By Aero")
            {
                //DEFINE: how values should be validated
                //validate By Aero by Validator
                var validator = new Validators.ByAeroValidator(_localizationService);
                var model = new Models.ByAeroModel()
                {
                    Address = form["Address"],
                    SomeNumber = int.Parse(form["SomeNumber"])
                };
                var validationResult = validator.Validate(model);
                if (!validationResult.IsValid)
                    foreach (var error in validationResult.Errors)
                        warnings.Add(error.ErrorMessage);

                if (warnings.Count == 0)
                {
                    //DEFINE: how values should be saved as GenericAttribute
                    //GenericAttribute: ShippingOptionAttributeDescription
                    var forCustomer = string.Format("Address: {0}<br>SomeNumber: {1}<br>", model.Address, model.SomeNumber);

                    _genericAttributeService.SaveAttribute(
                        _workContext.CurrentCustomer,
                        SystemCustomerAttributeNames.ShippingOptionAttributeDescription,
                        forCustomer,
                         _storeContext.CurrentStore.Id);

                    //GenericAttribute: ShippingOptionAttributeXml
                    var domainModel = new ByAero() { Address = model.Address, SomeNumber = model.SomeNumber };

                    var stringBuilder = new StringBuilder();
                    string serializedAttribute;
                    using (var tw = new StringWriter(stringBuilder))
                    {
                        var xmlS = new XmlSerializer(typeof(ByAero));
                        xmlS.Serialize(tw, domainModel);
                        serializedAttribute = stringBuilder.ToString();

                    }

                    _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                        SystemCustomerAttributeNames.ShippingOptionAttributeXml,
                         serializedAttribute,
                         _storeContext.CurrentStore.Id);                    
                }
            }

            return warnings;
        }
    }
}
