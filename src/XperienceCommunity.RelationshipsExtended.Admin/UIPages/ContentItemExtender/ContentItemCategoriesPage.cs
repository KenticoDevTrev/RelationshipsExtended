using CMS.ContentEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using RelationshipsExtended;
using System.Threading.Tasks;
using XperienceCommunity.RelationshipsExtended.UIPages.ContentItemExtender;
/*
[assembly: UIPage(
    parentType: typeof(ContentItemEditSection),
    slug: "content-item-categories",
    uiPageType: typeof(ContentItemCategoriesPage),
    name: "Categories",
    templateName: TemplateNames.BINDING,
    order: UIPageOrder.NoOrder)]
*/
namespace XperienceCommunity.RelationshipsExtended.UIPages.ContentItemExtender
{

    public class ContentItemCategoriesPage : InfoBindingPage<ContentItemCategoryInfo, TagInfo>
    {
        [PageParameter(typeof(IntPageModelBinder))]
        public override int EditedObjectId { get; set; }

        protected override string SourceBindingColumn => nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID);

        protected override string TargetBindingColumn => nameof(ContentItemCategoryInfo.ContentItemCategoryTagID);

        // UI page configuration
        public override Task ConfigurePage()
        {
            PageConfiguration.ExistingBindingsListing.ColumnConfigurations
                .AddColumn(nameof(TagInfo.TagName), "Category", searchable: true, defaultSortDirection: SortTypeEnum.Asc);
            
            // Sets the caption for the 'Add items' button
            PageConfiguration.AddBindingButtonText = "Add Categories";

            // Sets the heading of the page
            PageConfiguration.ExistingBindingsListing.Caption = "Add Categories to the Content Item.  These are Language-Agnostic and not Workflow-compatible, all changes are instant.";

            return Task.CompletedTask;
        }
    }
}
