using System.Collections.Generic;
using System.Web.Mvc;

namespace Grand.Web.Framework.Controllers
{
    /// <summary>
    /// Base controller for shipping plugins
    /// </summary>
    public abstract class BaseShippingController : BasePluginController
    {
        public abstract IList<string> ValidateShippingForm(FormCollection form);
        public abstract string GetFormPartialView(string shippingOption);
    }
}