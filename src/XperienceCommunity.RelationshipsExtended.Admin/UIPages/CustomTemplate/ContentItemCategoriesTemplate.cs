using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Common;
using CMS.ContentEngine;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using RelationshipsExtended;
using XperienceCommunity.RelationshipsExtended.Models;
using XperienceCommunity.RelationshipsExtended.Services;
using XperienceCommunity.RelationshipsExtended.Web.Admin;

[assembly: UIPage(
    parentType: typeof(ContentItemEditSection),
    slug: "content-item-categories",
    uiPageType: typeof(ContentItemCategoriesTemplate),
    name: "Categories",
    templateName: "@xperiencecommunity.relationshipsextended/web-admin/ContentItemCategories",
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.RelationshipsExtended.Web.Admin
{
    internal class ContentItemCategoriesTemplate(
        RelationshipsExtendedOptions options,
        IInfoProvider<ContentItemCategoryInfo> contentItemCategoryInfoProvider,
        IInfoProvider<TagInfo> tagInfoProvider,
        IContentItemCategoryUIService contentItemCategoryUIService) : Page<ContentItemCategoriesProperties>
    {

        [PageParameter(typeof(IntPageModelBinder))]
        public int ContentItemID { get; set; }

        public RelationshipsExtendedOptions Options { get; } = options;
        public IInfoProvider<ContentItemCategoryInfo> ContentItemCategoryInfoProvider { get; } = contentItemCategoryInfoProvider;
        public IInfoProvider<TagInfo> TagInfoProvider { get; } = tagInfoProvider;
        public IContentItemCategoryUIService ContentItemCategoryUIService { get; } = contentItemCategoryUIService;

        public override async Task<ContentItemCategoriesProperties> ConfigureTemplateProperties(ContentItemCategoriesProperties properties)
        {
            var configuration = await ContentItemCategoryUIService.GetCategoryUIOptions(ContentItemID);
            properties.Enabled = configuration.Enabled && Options.AllowContentItemCategories;
            
            properties.AvailableCategories = [.. (await GetCategoriesToSelectFrom(configuration.TaxonomyNames)).Values];

            var currentCategories = await TagInfoProvider.Get()
                .Source(x => x.InnerJoin<ContentItemCategoryInfo>(nameof(TagInfo.TagID), nameof(ContentItemCategoryInfo.ContentItemCategoryTagID)))
                .WhereEquals(nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID), ContentItemID)
                .Columns(nameof(TagInfo.TagName))
                .GetEnumerableTypedResultAsync();

            properties.SelectedCategories = currentCategories.Select(x => x.TagName).ToList();

            return properties;
        }

        private Task<Dictionary<string, TaxonomyItem>> GetCategoriesToSelectFrom(string[] taxonomyNames)
        {
            // get all categories
            var categoriesQuery = TagInfoProvider.Get()
                .Source(x => x.InnerJoin<TaxonomyInfo>(nameof(TagInfo.TagTaxonomyID), nameof(TaxonomyInfo.TaxonomyID)))
                .Columns(nameof(TagInfo.TagName), nameof(TagInfo.TagTitle), nameof(TaxonomyInfo.TaxonomyName), nameof(TaxonomyInfo.TaxonomyTitle))
                .OrderBy(nameof(TagInfo.TagOrder));

            if (taxonomyNames.Length != 0) {
                categoriesQuery.WhereIn(nameof(TaxonomyInfo.TaxonomyName), taxonomyNames);
            }

            // TODO: get this working again in async
            var allCategoryDrs = categoriesQuery.Result.Tables[0].Rows.Cast<DataRow>();
            var categoriesByTaxonomyName = new Dictionary<string, TaxonomyItem>();
            foreach (var categoryDr in allCategoryDrs) {
                var taxonomy = new TaxonomyInfo(categoryDr);
                var category = new TagInfo(categoryDr);
                var taxonomyName = taxonomy.TaxonomyName.ToLowerInvariant();
                if (!categoriesByTaxonomyName.TryGetValue(taxonomyName, out TaxonomyItem value)) {
                    value = new TaxonomyItem(
                        TaxonomyName: taxonomy.TaxonomyName,
                        TaxonomyDisplayName: taxonomy.TaxonomyTitle,
                        Categories: []);
                    categoriesByTaxonomyName.Add(taxonomyName, value);
                }

                value.Categories.Add(new TaxonomyCategoryItem(category.TagName, category.TagTitle));
            }
            return Task.FromResult(categoriesByTaxonomyName);
        }

        [PageCommand]
        public async Task<ICommandResponse<SetCategoriesResult>> SetCategories(SetCategoriesArguments data)
        {
            var configuration = await ContentItemCategoryUIService.GetCategoryUIOptions(ContentItemID);

            var currentCategories = (await TagInfoProvider.Get()
                .Source(x => x.InnerJoin<ContentItemCategoryInfo>(nameof(TagInfo.TagID), nameof(ContentItemCategoryInfo.ContentItemCategoryTagID)))
                .WhereEquals(nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID), ContentItemID)
                .Columns(nameof(TagInfo.TagName))
                .GetEnumerableTypedResultAsync())
                .Select(x => x.TagName);
            var availableCategories = await GetCategoriesToSelectFrom(configuration.TaxonomyNames);

            int added = 0;
            int removed = 0;
            int fromTaxonomy = 0;

            // need to perform bulk operations on these
            var categoryNamesToRemove = new List<string>();
            var categoryNamesToAdd = new List<string>();

            // Check for add/remove based on the taxonomies that were presented
            foreach(var taxonomy in availableCategories.Values) {
                var allTaxonomyCategories = taxonomy.Categories.Select(x => x.CategoryName.ToLowerInvariant());
                
                var currentCategoriesForTaxonomy = currentCategories.Where(x => allTaxonomyCategories.Contains(x, StringComparer.OrdinalIgnoreCase));
                var selectedCategoriesForTaxonomy = data.SelectedCategories.Where(x => allTaxonomyCategories.Contains(x, StringComparer.OrdinalIgnoreCase));

                var removeCategories = currentCategoriesForTaxonomy.Except(selectedCategoriesForTaxonomy, StringComparer.OrdinalIgnoreCase);
                var addCategories = selectedCategoriesForTaxonomy.Except(currentCategoriesForTaxonomy, StringComparer.OrdinalIgnoreCase);

                if (removeCategories.Any()) {
                    removed += removeCategories.Count();
                    categoryNamesToRemove.AddRange(removeCategories);
                }
                if (addCategories.Any()) {
                    added += addCategories.Count();
                    categoryNamesToAdd.AddRange(addCategories);
                }
                if (removeCategories.Any() || addCategories.Any()) {
                    fromTaxonomy++;
                }
            }

            // Perform operations
            if(categoryNamesToRemove.Any()) { 
                var removeTagIds = (await TagInfoProvider.Get()
                    .WhereIn(nameof(TagInfo.TagName), categoryNamesToRemove)
                    .Columns(nameof(TagInfo.TagID))
                    .GetEnumerableTypedResultAsync())
                    .Select(x => x.TagID);
                var removeWhereCondition = new WhereCondition()
                    .WhereEquals(nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID), ContentItemID)
                    .WhereIn(nameof(ContentItemCategoryInfo.ContentItemCategoryTagID), removeTagIds);
                ContentItemCategoryInfoProvider.BulkDelete(removeWhereCondition);
            }

            if(categoryNamesToAdd.Any()) {
                var addTagIds = (await TagInfoProvider.Get()
                .WhereIn(nameof(TagInfo.TagName), categoryNamesToAdd)
                .Columns(nameof(TagInfo.TagID))
                .GetEnumerableTypedResultAsync())
                .Select(x => x.TagID);

                var contentItemsToAdd = addTagIds.Select(x => new ContentItemCategoryInfo() {
                    ContentItemCategoryContentItemID = ContentItemID,
                    ContentItemCategoryTagID = x
                });
                ContentItemCategoryInfoProvider.BulkInsert(contentItemsToAdd);
            }
            return ResponseFrom(new SetCategoriesResult(SelectedCategories: data.SelectedCategories))
                .AddSuccessMessage($"{added} Added, {removed} Removed from {fromTaxonomy} taxonomy groups");
        }
    }


    // Data object encapsulating page command response
    internal readonly record struct SetCategoriesResult(List<string> SelectedCategories);


    // Defines the properties of the client template (CustomLayoutTemplate.tsx)
    class ContentItemCategoriesProperties : TemplateClientProperties
    {
        public bool Enabled { get; set; } = false;
        public List<string> SelectedCategories { get; set; } = [];
        public List<TaxonomyItem> AvailableCategories { get; set; } = [];
    }

    class SetCategoriesArguments
    {
        public List<string> SelectedCategories { get; set; }
    }
}
