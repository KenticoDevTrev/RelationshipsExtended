using CMS.ContentEngine;
using CMS.ContentEngine.Internal;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.FormEngine;
using CMS.Modules;
using RelationshipsExtended;

namespace XperienceCommunity.RelationshipsExtended
{
    public class RelationshipsExtendedModuleInstaller(RelationshipsExtendedOptions options, IInfoProvider<ResourceInfo> resourceInfoProvider)
    {

        public RelationshipsExtendedOptions Options { get; } = options;
        public IInfoProvider<ResourceInfo> ResourceInfoProvider { get; } = resourceInfoProvider;

        public void Install()
        {
            var resource = ResourceInfoProvider.Get("RelationshipsExtended") ?? new ResourceInfo();
            InitializeRelationshipsExtendedResource(resource);

            if (Options.AllowContentItemCategories) {
                // create binding table
                InitializeContentItemCategory(resource);
            }

        }

        private void InitializeRelationshipsExtendedResource(ResourceInfo resource)
        {

            resource.ResourceDisplayName = "Relationships Extended";
            resource.ResourceName = "RelationshipsExtended";
            resource.ResourceDescription = "Extensions and systems aimed to help extend Xperience by Kentico to handle broader relationships between objects. v1.0.0";
            resource.ResourceIsInDevelopment = false;

            if(resource.HasChanged) {
                ResourceInfoProvider.Set(resource);
            }
        }

        private static void InitializeContentItemCategory(ResourceInfo resource)
        {
            var info = DataClassInfoProvider.GetDataClassInfo(ContentItemCategoryInfo.OBJECT_TYPE) ?? DataClassInfo.New(ContentItemCategoryInfo.OBJECT_TYPE);

            info.ClassName = "RelationshipsExtended.ContentItemCategory";
            info.ClassTableName = "RelationshipsExtended_ContentItemCategory";
            info.ClassDisplayName = "Content Item Category";
            info.ClassType = ClassType.OTHER;
            info.ClassResourceID = resource.ResourceID;

            var formInfo = FormHelper.GetBasicFormDefinition("ContentItemCategoryID");

            var formItem = new FormFieldInfo {
                Name = nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                DataType = "integer",
                ReferenceType = ObjectDependencyEnum.Binding,
                ReferenceToObjectType = ContentItemInfo.OBJECT_TYPE,
                Enabled = true
            };
            formInfo.AddFormItem(formItem);
            formItem = new FormFieldInfo {
                Name = nameof(ContentItemCategoryInfo.ContentItemCategoryTagID),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                DataType = "integer",
                ReferenceType = ObjectDependencyEnum.Binding,
                ReferenceToObjectType = TagInfo.OBJECT_TYPE,
                Enabled = true
            };
            formInfo.AddFormItem(formItem);

            SetFormDefinition(info, formInfo);

            if (info.HasChanged) {
                DataClassInfoProvider.SetDataClassInfo(info);
                try {
                    // run SQL to set foreign keys
                    var foreignKeySql =
    @"
IF(OBJECT_ID('FK_RelationshipsExtended_ContentItemCategory_ContentItem', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[RelationshipsExtended_ContentItemCategory]  WITH CHECK ADD  CONSTRAINT [FK_RelationshipsExtended_ContentItemCategory_ContentItem] FOREIGN KEY([ContentItemCategoryContentItemID])
	REFERENCES [dbo].[CMS_ContentItem] ([ContentItemID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[RelationshipsExtended_ContentItemCategory] CHECK CONSTRAINT [FK_RelationshipsExtended_ContentItemCategory_ContentItem]
END


IF(OBJECT_ID('FK_RelationshipsExtended_ContentItemCategory_Tag', 'F') IS NULL)
BEGIN
	ALTER TABLE [dbo].[RelationshipsExtended_ContentItemCategory]  WITH CHECK ADD  CONSTRAINT [FK_RelationshipsExtended_ContentItemCategory_Tag] FOREIGN KEY([ContentItemCategoryTagID])
	REFERENCES [dbo].[CMS_Tag] ([TagID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[RelationshipsExtended_ContentItemCategory] CHECK CONSTRAINT [FK_RelationshipsExtended_ContentItemCategory_Tag]
END
";
                    ConnectionHelper.ExecuteNonQuery(foreignKeySql, [], QueryTypeEnum.SQLQuery);
                } catch (Exception ex) {
                    EventLogProvider.LogEvent(new EventLogInfo("E", "RelationshipsExtended", "InitializeContentItemCategory Error") {
                        Exception = ex
                    });
                }
            }
        }

        private static void SetFormDefinition(DataClassInfo info, FormInfo form)
        {
            if (info.ClassID > 0) {
                var existingForm = new FormInfo(info.ClassFormDefinition);
                existingForm.CombineWithForm(form, new());
                info.ClassFormDefinition = existingForm.GetXmlDefinition();
            } else {
                info.ClassFormDefinition = form.GetXmlDefinition();
            }
        }
    }
}
