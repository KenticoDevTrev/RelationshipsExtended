# RelationshipsExtended
Relationships Extended Module for Kentico
This tool extends Kentico by allowing support and management tools for 6 Relationship scenarios:

* Related Pages (Both Orderable AdHoc Relationships and Unordered Relationships)
* Node Categories (using CMS.TreeNode)
* Node Categories (using a Custom Joining Table)
* Object to Object binding with Ordering
* Node to Object binding with Ordering
* Node to Object binding without Ordering.

# Installation
* Open your Kentico Solution and right click on the CMS Project, and select "Manage NuGet Packages..."
* Search for RelationshipsExtended and select the major version that mathces your Kentico version (ex 10.0.0 = Kentico 10, 11.0.0 = Kentico 11)
* After your NuGet package finishes installing, run your Keintico ste.  Ignore the Event Log Error for the RelationshipsExtended ErrorSettingForeignKeys as that always happens on the first start up.
* Go to System -> Restart Application
* If on Kentico 10, also go to System -> Macros -> Signatures and resign your macros.

# Documentation
If you are new to the tool, please check out the Wiki page on this GtHub to get a general overview, then check out the full Demo Guid that shows you step by step how to leverage this tool.

# Contributions, bug fixes and License
Feel free to Fork and submit pull requests to contribute.

You can submit bugs through the issue list and I will get to them as soon as I can, unless you want to fix it yourself and submit a pull request!

This is free to use and modify!

# Compatability
Can be used on any Kentico 10.0.52, 11.0.48+, and Kentico 12 SP site (hotfix 29 or above).
