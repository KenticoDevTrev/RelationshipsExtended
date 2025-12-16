
# RelationshipsExtended
Unlike Kentico Xperience 13 or lower versions, Xperience by Kentico handles relationships MUCH better, including tools to create many to one relationships between Content Items and the ability to make custom fields that can store relationships between Pages and Objects (in a Serialized Json Array).

This package then is much more limited in scope than the others in the past.  It contains a "Content Item Category [Tag]" table and interfaces to set this.  This allows linking of Tags on the Content Item (language agnostic) instead of in a language-specific field on the content item.  This does 2 things:

1. Ensures Taxonomy is language agnostic (so you don't end up with taxonomy differences across multiple languages)
2. Allows for faster filtering for large number of content items (The default Taxonomy fields on a Content Item store the taxonomy in a JSON array, and to filter it has to parse all the content item's Json arrays and do matches, which is slower than a Where In)

It also has some extension methods to the `ContentTypeQueryParameters` to leverage these language agnostic categories, as well as other Binding/Relational Binding Condition Generators in the `IRelationshipsExtendedHelper`. It is important to note if migrating from KX13 or prior, that due to the limitation of the Where Condition on the `ContentTypeQueryParameters` Only the `BindingTagsCondition` was implementable.

I have hopes that eventually this will also have logic to 'sync' taxonomy fields that are in the Content Item Fields if you want to keep using the built-in taxonomy field type.


# Installation
TBD


Lastly hook up RelationshipsExtendedHelper as the implementation for IRelationshipsExtendedHelper.

For MVC.Net Core, add to the Startup.cs's ConfigureServices
`services.AddSingleton(typeof(IRelationshipExtendedHelper), typeof(RelationshipsExtendedHelper));`

For MVC.Net Framework, you will have to use your own IoC, such as AutoFac
```csharp
// builder is of type ContainerBuilder
builder.RegisterType(typeof(RelationshipsExtendedHelper)).As(typeof(IRelationshipExtendedHelper));
```

This will provide you with TreeCategory, DocumentQuery/ObjectQuery extensions, and AdHoc relationship support and event hooks that the Admin (Mother) also contain, so any adjustments in code will also work properly with staging and such.


# Documentation
If you are new to the tool, you have two options for learning how to use this.

1. Check out the [Demo section](https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo), which contains an example project with each scenario and it's configuration.  You can include the Demo project on your Admin, and go to Site -> Import site or objects on the `RelationshipsExtendedDemoModule.13.0.0.zip`  file to install the Demo module and it's UI elements.
2. Check out the [Wiki page](https://github.com/KenticoDevTrev/RelationshipsExtended/wiki/Relationships-Extended-Overview) on this GitHub to get a general overview.

## Batch Modification
It is possible that during batch adjustments across multiple objects, that transactions can get locked, causing errors.  It is recommended in this case to not log the synchronization task during the batch operations, and then manually trigger an update a staging event after if something was changed. Under normaly operations on single items

```csharp

using(CMSActionContext context = new CMSActionContect() {
   LogSynchronization = false
   }) {
    // Batch operation where multiple related objects are done
   }
   
   if(UpdateWasMade) {
        // Tree node update
        DocumentSynchronizationHelper.LogDocumentChange(new LogMultipleDocumentChangeSettings()
        {
            NodeAliasPath = AssetParent.NodeAliasPath,
            CultureCode = AssetParent.DocumentCulture,
            TaskType = TaskTypeEnum.UpdateDocument,
            Tree = AssetParent.TreeProvider,
            SiteName = AssetParent.NodeSiteName,
            RunAsynchronously = false,
            User = MembershipContext.AuthenticatedUser
        });
        // Object update
        ParentObjectInfoProvider.Set(TheParentObject);
   }

    // In a global event hook
    private void ParentCategories_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
    {
        if (CMSActionContext.CurrentLogSynchronization)
        {
            RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((ParentCategoryInfo.TypesInfo)e.Object).refNodeID, ParentCategoryInfo.TypesInfo.OBJECT_TYPE);
        }
    }


```

## Query Extensions
The following Extension methods have been added to all ObjectQuery and DocumentQuery, see the project's readme for more info on usage.  Except for InRelationWithOrder which is available in all versions, these are only in 13+

* BindingCategoryCondition: Filter items based on a Binding table that leverages CMS_Categories
* DocumentCategoryCondition: Filter items based on Document Categories
* TreeCategoryCondition: Filter items based on Tree Categories
* BindingCondition: Filter items based on a Binding table
* InCustomRelationshipWithOrder: Show objects related through a custom binding table with ordering support
* InRelationWithOrder: Show related Pages with order support (Available in Kentico 10-13)

You can see some samples [check this MVC Controller](https://github.com/KenticoDevTrev/RelationshipsExtended/blob/master/Demo/MVC/Controller/TestController.cs)

# Contributions, bug fixes and License
Feel free to Fork and submit pull requests to contribute.

You can submit bugs through the issue list and i will get to them as soon as i can, unless you want to fix it yourself and submit a pull request!

This is free to use and modify!

# Compatability
Can be used on any Kentico 10.0.52, 11.0.48+, and Kentico 12 SP site (hotfix 29 or above), and Kentico 13.0.0
