
# RelationshipsExtended
Relationships Extended Module for Kentico
This tool extends Kentico by allowing support and management tools for 6 Relationship scenarios:

* Related Pages (Both Orderable AdHoc Relationships and Unordered Relationships)
* Node Categories (using CMS.TreeNode)
* Node Categories (using a Custom Joining Table)
* Object to Object binding with Ordering
* Node to Object binding with Ordering
* Node to Object binding without Ordering

# Installation
* Open your Kentico Solution and right click on the CMS Project, and select "Manage NuGet Packages..."
* Search for RelationshipsExtended and select the major version that mathces your Kentico version (ex 10.0.0 = Kentico 10, 11.0.0 = Kentico 11, etc)
* After your NuGet package finishes installing, run your Keintico site.  Ignore the Event Log Error for the RelationshipsExtended ErrorSettingForeignKeys as that always happens on the first start up.
* Go to System -> Restart Application
* Also go to System -> Macros -> Signatures and resign your macros.

# MVC
If you are using Kentico 12 MVC or Kentico 13 MVC (.Net or .Net Core) you should also install the `RelationshipsExtendedMVCHelper` NuGet package with the matching version on your MVC site, this will provide you with TreeCategory and AdHoc relationship support and event hooks that the Admin (Mother) also contain, so any adjustments in code will also work properly with staging and such.  Separate installation instructions can be found on that nuget package.

# Documentation
If you are new to the tool, you have two options for learning how to use this.

1. Check out the [Demo section](https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo), which contains an example project with each scenario and it's configuration.  You can include the Demo project on your Admin, and go to Site -> Import site or objects on the `RelationshipsExtendedDemoModule.13.0.0.zip`  file to install the Demo module and it's UI elements.
2. Check out the [Wiki page](https://github.com/KenticoDevTrev/RelationshipsExtended/wiki/Relationships-Extended-Overview) on this GitHub to get a general overview.

## Query Extensions
The following Extension methods have been added to all ObjectQuery and DocumentQuery, see the project's readme for more info on usage.  Except for InRelationWithOrder which is available in all versions, these are only in 13+

* BindingCategoryCondition: Filter items based on a Binding table that leverages CMS_Categories
* DocumentCategoryCondition: Filter items based on Document Categories
* TreeCategoryCondition: Filter items based on Tree Categories
* BindingCondition: Filter items based on a Binding table
* InCustomRelationshipWithOrder: Show objects related through a custom binding table with ordering support
* InRelationWithOrder: Show related Pages with order support (Available in Kentico 10-13)

# Contributions, bug fixes and License
Feel free to Fork and submit pull requests to contribute.

You can submit bugs through the issue list and i will get to them as soon as i can, unless you want to fix it yourself and submit a pull request!

This is free to use and modify!

# Compatability
Can be used on any Kentico 10.0.52, 11.0.48+, and Kentico 12 SP site (hotfix 29 or above), and Kentico 13.0.0
