using System;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;


namespace Grand.Plugin.Shipping.VoidShipping.Domain
{
    public class ByAero
    {
        public string Address { get; set; }
        public int SomeNumber { get; set; }
    }
}