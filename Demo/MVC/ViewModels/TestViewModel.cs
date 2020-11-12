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
    public class TestViewModel
    {
        public TestViewModel()
        {

        }
        public List<Banner> Banners { get; set; }
        public List<CategoryInfo> Categories { get; set; }
        public List<CategoryInfo> Regions { get; set; }
        public List<FooInfo> Foos { get; set; }
        public List<BazInfo> Bazs { get; set; }

        public Dictionary<int, List<BarInfo>> FooBars { get; set; } = new Dictionary<int, List<BarInfo>>();
        public Dictionary<int, List<BazInfo>> FooBazs { get; set; } = new Dictionary<int, List<BazInfo>>();
        public Dictionary<int, List<CategoryInfo>> FooCategories { get; set; } = new Dictionary<int, List<CategoryInfo>>();
        

    }
}
