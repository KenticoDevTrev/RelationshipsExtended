using CMS.Core;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelationshipsExtended;
using Kentico.Content.Web.Mvc.Routing;
using CMS.DocumentEngine.Types.Test;
using BlankSite;
using BlankSite.ViewModels;
using CMS.Taxonomy;
using CMS.DataEngine;
using CMS;
using System.Globalization;
using Demo;
using RelationshipsExtended.Enums;
using CMS.DocumentEngine;

[assembly: RegisterPageRoute(Testing.CLASS_NAME, typeof(TestController))]
[assembly: RegisterPageRoute(Testing.CLASS_NAME, typeof(TestController), ActionName ="IndexSimplified", Path ="/Test-Page-2")]
[assembly: RegisterPageRoute(Testing.CLASS_NAME, typeof(TestController), ActionName ="IndexReversed", Path ="/Test-Page-3")]
namespace BlankSite
{
    public class TestController : Controller
    {
        public TestController(IPageRetriever pageRetriever, 
            IPageDataContextRetriever dataRetriever,
            ICategoryInfoProvider categoryInfoProvider,
            IFooInfoProvider fooInfoProvider,
            IBazInfoProvider bazInfoProvider,
            IBarInfoProvider barInfoProvider
            )
        {
            PageRetriever = pageRetriever;
            DataRetriever = dataRetriever;
            CategoryInfoProvider = categoryInfoProvider;
            FooInfoProvider = fooInfoProvider;
            BazInfoProvider = bazInfoProvider;
            BarInfoProvider = barInfoProvider;
        }

        public IPageRetriever PageRetriever { get; }
        public IPageDataContextRetriever DataRetriever { get; }
        public ICategoryInfoProvider CategoryInfoProvider { get; }
        public IFooInfoProvider FooInfoProvider { get; }
        public IBazInfoProvider BazInfoProvider { get; }
        public IBarInfoProvider BarInfoProvider { get; }

        public ActionResult Index()
        {
            // Get current page
            var CurrentTestPage = DataRetriever.Retrieve<Testing>().Page;

            TestViewModel model = new TestViewModel();

            model.Banners = PageRetriever.Retrieve<Banner>(query =>
                query.InRelationWithOrder(CurrentTestPage.NodeID, "Banners")
            ).ToList();

            //PageRetriever.Retrieve<Banner>(query => query.InCustomRelationshipWithOrder(new BarNodeInfo()));

            model.Categories = CategoryInfoProvider.Get()
                .BindingCondition(TreeCategoryInfo.OBJECT_TYPE, "cms.node", nameof(CategoryInfo.CategoryID), nameof(TreeCategoryInfo.CategoryID), nameof(TreeCategoryInfo.NodeID), new string[] { CurrentTestPage.NodeID.ToString() })
                .ToList();

            model.Regions = CategoryInfoProvider.Get()
                .InCustomRelationshipWithOrder(NodeRegionInfo.OBJECT_TYPE, "cms.node", CurrentTestPage.NodeID, nameof(CategoryInfo.CategoryID), nameof(NodeRegionInfo.NodeRegionNodeID), nameof(NodeRegionInfo.NodeRegionCategoryID))
                .ToList();

            model.Foos = FooInfoProvider.Get()
                .InCustomRelationshipWithOrder(NodeFooInfo.OBJECT_TYPE, "cms.node", CurrentTestPage.NodeID, nameof(FooInfo.FooID), nameof(NodeFooInfo.NodeFooNodeID), nameof(NodeFooInfo.NodeFooFooID), nameof(NodeFooInfo.NodeFooOrder))
                .ToList();

            model.Bazs = BazInfoProvider.Get()
                .InCustomRelationshipWithOrder(NodeBazInfo.OBJECT_TYPE, "cms.node", CurrentTestPage.NodeID, nameof(BazInfo.BazID), nameof(NodeBazInfo.NodeBazNodeID), nameof(NodeBazInfo.NodeBazBazID))
                .ToList();

            // Get Foo Bars
            foreach (var Foo in model.Foos)
            {
                model.FooBars.Add(Foo.FooID,
                BarInfoProvider.Get()
                    .InCustomRelationshipWithOrder(FooBarInfo.OBJECT_TYPE, BarInfo.OBJECT_TYPE, Foo.FooID, nameof(BarInfo.BarID), nameof(FooBarInfo.FooBarFooID), nameof(FooBarInfo.FooBarBarID), nameof(FooBarInfo.FooBarOrder))
                    .ToList()
                );
            }

            // Get Foo Baz
            foreach (var Foo in model.Foos)
            {
                model.FooBazs.Add(Foo.FooID,
                BazInfoProvider.Get()
                    .InCustomRelationshipWithOrder(FooBazInfo.OBJECT_TYPE, BazInfo.OBJECT_TYPE, Foo.FooID, nameof(BazInfo.BazID), nameof(FooBazInfo.FooBazFooID), nameof(FooBazInfo.FooBazBazID))
                    .ToList()
                );
            }

            // Get Foo Categories
            foreach (var Foo in model.Foos)
            {
                model.FooCategories.Add(Foo.FooID,
                CategoryInfoProvider.Get()
                    .InCustomRelationshipWithOrder(FooCategoryInfo.OBJECT_TYPE, CategoryInfo.OBJECT_TYPE, Foo.FooID, nameof(CategoryInfo.CategoryID), nameof(FooCategoryInfo.FooCategoryID), nameof(FooCategoryInfo.FooCategoryCategoryID))
                    .ToList()
                );
            }

            return View(model);
        }

        public ActionResult IndexSimplified()
        {
            // Get current page
            var CurrentTestPage = DataRetriever.Retrieve<Testing>().Page;

            TestViewModel model = new TestViewModel();

            model.Banners = PageRetriever.Retrieve<Banner>(query =>
                query.InRelationWithOrder(CurrentTestPage.NodeID, "Banners")
            ).ToList();

            model.Categories = CategoryInfoProvider.Get()
                .BindingCondition<CategoryInfo, TreeCategoryInfo>(BindingConditionType.FilterChildrenByParents, new object[] { CurrentTestPage.NodeID })
                .ToList();

            model.Regions = CategoryInfoProvider.Get()
                .InCustomRelationshipWithOrder<CategoryInfo, NodeRegionInfo>(BindingQueryType.GetChildrenByParent, CurrentTestPage.NodeID)
                .ToList();

            model.Foos = FooInfoProvider.Get()
                .InCustomRelationshipWithOrder<FooInfo, NodeFooInfo>(BindingQueryType.GetChildrenByParentOrdered, CurrentTestPage.NodeID)
                .ToList();

            model.Bazs = BazInfoProvider.Get()
                .InCustomRelationshipWithOrder<BazInfo, NodeBazInfo>(BindingQueryType.GetChildrenByParent, CurrentTestPage.NodeID)
                .ToList();

            // Get Foo Bars
            foreach (var Foo in model.Foos)
            {
                model.FooBars.Add(Foo.FooID,
                BarInfoProvider.Get()
                    .InCustomRelationshipWithOrder<BarInfo, FooBarInfo>(BindingQueryType.GetChildrenByParentOrdered, Foo.FooID)
                    .ToList()
                );
            }

            // Get Foo Baz
            foreach (var Foo in model.Foos)
            {
                model.FooBazs.Add(Foo.FooID,
                BazInfoProvider.Get()
                    .InCustomRelationshipWithOrder<BazInfo, FooBazInfo>(BindingQueryType.GetChildrenByParent, Foo.FooID)
                    .ToList()
                );
            }

            // Get Foo Categories
            foreach (var Foo in model.Foos)
            {
                model.FooCategories.Add(Foo.FooID,
                CategoryInfoProvider.Get()
                    .InCustomRelationshipWithOrder<CategoryInfo, FooCategoryInfo>(BindingQueryType.GetChildrenByParentOrdered, Foo.FooID)
                    .ToList()
                );
            }

            return View("Index", model);
        }

        public ActionResult IndexReversed()
        {
            // Get "Banner1"
            var Banner1 = PageRetriever.Retrieve<Banner>(applyQueryParametersAction: query =>
                query.Path("/Banners/Banner-1")
            ).FirstOrDefault();

            // Get Items to look up on
            int MobileCategoryID = CategoryInfoProvider.Get("Mobile", 0).CategoryID;
            List<int> WesternUSOrEasternUSCategoryIDs = CategoryInfoProvider.Get()
                .WhereIn(nameof(CategoryInfo.CategoryName), new string[] { "RegionWestUSA", "RegionEasternUSA" })
                .Select(x => x.CategoryID).ToList();
            int FooAID = FooInfoProvider.Get("FooA").FooID;
            int BazAID = BazInfoProvider.Get("BazA").BazID;
            int BarAID = BarInfoProvider.Get("BarA").BarID;
            int BlahCategoryID = CategoryInfoProvider.Get("BlahCategory", 0).CategoryID;

            TestReverseViewModel model = new TestReverseViewModel();

            // Get Test pages related to banner 1
            model.Banner1Pages = PageRetriever.Retrieve<Testing>(query =>
                query.InRelationWithOrder(Banner1.NodeID, "Banners", ReverseRelationship: true)
            ).ToList();

            model.MobilePages = PageRetriever.Retrieve<Testing>(query => 
                query.TreeCategoryCondition(new object[] { MobileCategoryID })
                )
                .ToList();

            model.WesternUSOrEasternUSPages = PageRetriever.Retrieve<Testing>(query =>
                query.BindingCategoryCondition<Testing, NodeRegionInfo>(BindingConditionType.FilterParentsByChildren, WesternUSOrEasternUSCategoryIDs.Cast<object>())
                )
                .ToList();

            model.FooAPages = PageRetriever.Retrieve<Testing>(query =>
                query.BindingCondition<Testing, NodeFooInfo>(BindingConditionType.FilterParentsByChildren, new object[] { FooAID })
                )
                .ToList();

            model.BazAPages = PageRetriever.Retrieve<Testing>(query =>
                query.BindingCondition<Testing, NodeBazInfo>(BindingConditionType.FilterParentsByChildren, new object[] { BazAID })
                )
                .ToList();

            model.BarAFoos = FooInfoProvider.Get()
                .BindingCondition<FooInfo, FooBarInfo>(BindingConditionType.FilterParentsByChildren, new object[] { BarAID })
                .ToList();


            model.BazAFoos = FooInfoProvider.Get()
                .BindingCondition<FooInfo, FooBazInfo>(BindingConditionType.FilterParentsByChildren, new object[] { BarAID })
                .ToList();

            model.BlahCategoryFoos = FooInfoProvider.Get()
                .BindingCategoryCondition<FooInfo, FooCategoryInfo>(BindingConditionType.FilterParentsByChildren, new object[] { BlahCategoryID })
                .ToList();


            return View("IndexReversed", model);
        }
    }
}
