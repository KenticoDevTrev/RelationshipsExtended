using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Test;
using CMS.Taxonomy;
using Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlankSite.ViewModels
{
    public class TestReverseViewModel
    {
        public TestReverseViewModel()
        {

        }
        public List<Testing> Banner1Pages { get; set; }
        public List<Testing> MobilePages { get; set; }
        public List<Testing> WesternUSOrEasternUSPages { get; set; }
        public List<Testing> FooAPages { get; set; }
        public List<Testing> BazAPages { get; set; }

        public List<FooInfo> BarAFoos { get; set; } = new List<FooInfo>();
        public List<FooInfo> BazAFoos { get; set; } = new List<FooInfo>();
        public List<FooInfo> BlahCategoryFoos { get; set; } = new List<FooInfo>();


    }
}
