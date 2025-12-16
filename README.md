
# RelationshipsExtended
Unlike Kentico Xperience 13 or lower versions, Xperience by Kentico handles relationships MUCH better, including tools to create many to one relationships between Content Items and the ability to make custom fields that can store relationships between Pages and Objects (in a Serialized Json Array).

This package then is much more limited in scope than the others in the past.  It contains a "Content Item Category [Tag]" table and interfaces to set this.  This allows linking of Tags on the Content Item (language agnostic) instead of in a language-specific field on the content item.  This does 2 things:

1. Ensures Taxonomy is language agnostic (so you don't end up with taxonomy differences across multiple languages)
2. Allows for faster filtering for large number of content items (The default Taxonomy fields on a Content Item store the taxonomy in a JSON array, and to filter it has to parse all the content item's Json arrays and do matches, which is slower than a Where In)

It also has some extension methods to the `ContentTypeQueryParameters` to leverage these language agnostic categories, as well as other Binding/Relational Binding Condition Generators in the `IRelationshipsExtendedHelper`. It is important to note if migrating from KX13 or prior, that due to the limitation of the Where Condition on the `ContentTypeQueryParameters` Only the `BindingTagsCondition` was implementable.

I have hopes that eventually this will also have logic to 'sync' taxonomy fields that are in the Content Item Fields if you want to keep using the built-in taxonomy field type.


# Installation
Install the `XperienceCommunity.RelationshipsExtended.Web.Admin` Package on your Project.  Optionally you can install the Admin only on the Admin project, and the `XperienceCommunity.RelationshipsExtended.Core` on the MVC or `Kentico.Xperience.WebApp` dependent project.


## Package Installation

Add the package to your application using the .NET CLI
```powershell
dotnet add package XperienceCommunity.RelationshipsExtended.Web.Admin
```

Additionally, you can elect to install only the required packages on specific projects if you have separation of concerns:

**XperienceCommunity.RelationshipsExtended.Core**: Kentico.Xperience.WebApp Dependent (No Admin)
**XperienceCommunity.RelationshipsExtended.Web.Admin** : Kentico.Xperience.Admin (Admin Items)

## Quick Start
In your startup, when you call the `.AddRelationshipsExtended(options => ...)` ...

This will hook up all the interfaces (including `IRelationshipsExtendedHelper`, and `IRelHelper`), as well as  and run the installation logic on application run (will set up the `RelationshipsExtended_ContentItemCategory` table and `ContentItemCategoryInfo` class).


## Library Version Matrix

This project is using [Xperience Version v31.0.0](https://docs.kentico.com/changelog).

| Xperience Version  | Library Version |
| ------------------ | --------------- |
| >= 31.0.*          | 1.1.0           |
|    30.0.0-30.12.3  | 1.0.1           |



# Documentation
Documentation is still TBD, i think the nav I created broke with some update so I need to revisit.  Overall though the `IRelationshipsExtendedHelper` allows you to leverage custom relationships/binding tables in lookups for objects, and there is one `BindingTagsCondition` for the `ContentTypeQueryParameters` that will filter the content items by the Tag IDs, Code Names, or Guids you pass.

# Contributions, bug fixes and License
Feel free to Fork and submit pull requests to contribute.

You can submit bugs through the issue list and i will get to them as soon as i can, unless you want to fix it yourself and submit a pull request!

This is free to use and modify!

# Compatability
This version is for Xperience by Kentico 31.0.0+, but older versions are available for Kentico 10.0.52, 11.0.48+, and Kentico 12 SP site (hotfix 29 or above), and Kentico 13.0
