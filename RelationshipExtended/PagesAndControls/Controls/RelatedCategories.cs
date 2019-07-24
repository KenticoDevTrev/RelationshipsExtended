using System;
using System.Collections.Generic;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.Membership;
using System.Data;
using CMS.Taxonomy;
using CMS.MacroEngine;
using CMS.Synchronization;
using CMS.CustomTables;
using CMS.DocumentEngine;
using System.Linq;
using TreeNode = System.Web.UI.WebControls.TreeNode;
using System.Web.UI.WebControls;
using CMS.EventLog;
using CMS.LicenseProvider;
using CMS.Base;
using CMS.FormEngine.Web.UI;
using CMS.Base.Web.UI;
using RelatedCategories;
using CMS.PortalEngine.Web.UI;
using RelationshipsExtended;

public partial class Compiled_CMSModules_RelationshipsExtended_Controls_RelatedCategories : CMSAbstractWebPart
{
    public Compiled_CMSModules_RelationshipsExtended_Controls_RelatedCategories() { }

    #region "Variables"

    private bool mEnableSiteSelection;
    private DialogConfiguration mConfig;
    private DisplayType DisplayMode;
    private SaveType SaveModeVal;
    private FieldSaveType FieldSaveModeVal;
    private string AllowableCategoryIDWhere;
    private string DefaultSortOrder;
    private int ExpandCategoryLevel;
    private List<CategoryInfo> PossibleCategories;
    private List<int> PossibleCategoryIDs;
    private List<CategoryInfo> SelectedCategories;
    private List<int> SelectedCategoryIDs;
    private List<CategoryInfo> CurrentCategories;


    private CMS.DocumentEngine.TreeNode PageDocument
    {
        get
        {
            int NodeID = DocumentContext.CurrentDocument != null ? DocumentContext.CurrentDocument.NodeID : QueryHelper.GetInteger("NodeID", 0);
            string Culture = (DocumentContext.CurrentDocument != null ? DocumentContext.CurrentDocument.DocumentCulture : QueryHelper.GetString("culture", "en-US"));
            return CacheHelper.Cache<CMS.DocumentEngine.TreeNode>(cs =>
            {
                if (BindOnPrimaryNodeOnly)
                {
                    NodeID = RelHelper.GetPrimaryNodeID(NodeID);
                }
                var ProperTreeNode = new DocumentQuery().WhereEquals("NodeID", NodeID).Culture(Culture).CombineWithDefaultCulture(true).FirstOrDefault();
                if(cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { "nodeid|" + NodeID, "nodeid|" + ProperTreeNode.NodeID });
                }
                return ProperTreeNode;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), NodeID, Culture));
        }
    }

    #endregion

    #region "Private properties"

    private bool BindOnPrimaryNodeOnly
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("BindOnPrimaryNodeOnly"), true);
        }
        set
        {
            SetValue("BindOnPrimaryNodeOnly", value);
        }
    }

    public string RootCategory
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RootCategory"), "");
        }
        set
        {
            SetValue("RootCategory", value);
        }
    }

    public string CategoryDisplayMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CategoryDisplayMode"), "List");
        }
        set
        {
            SetValue("CategoryDisplayMode", value);
        }
    }

    public int ExpandToLevel
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ExpandToLevel"), 1);
        }
        set
        {
            SetValue("ExpandToLevel", value);
        }
    }

    public bool OnlyLeafSelectable
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("OnlyLeafSelectable"), true);
        }
        set
        {
            SetValue("OnlyLeafSelectable", value);
        }
    }

    public bool ParentSelectsChildren
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ParentSelectsChildren"), true);
        }
        set
        {
            SetValue("ParentSelectsChildren", value);
        }
    }

    public int MinimumCategories
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MinimumCategories"), -1);
        }
        set
        {
            SetValue("MinimumCategories", value);
        }

    }

    public int MaximumCategories
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaximumCategories"), -1);
        }
        set
        {
            SetValue("MaximumCategories", value);
        }

    }

    public string WhereFilter
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WhereFilter"), "");
        }
        set
        {
            SetValue("WhereFilter", value);
        }
    }

    public string SaveMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SaveMode"), "ToField");
        }
        set
        {
            SetValue("SaveMode", value);
        }
    }

    public string JoinTableForeignKeySource
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableForeignKeySource"), "Page");
        }
        set
        {
            SetValue("JoinTableForeignKeySource", value);
        }
    }

    public string JoinTableThisObjectForeignKey
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableThisObjectForeignKey"), "ItemGUID");
        }
        set
        {
            SetValue("JoinTableThisObjectForeignKey", value);
        }
    }

    public string JoinTableName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableName"), "");
        }
        set
        {
            SetValue("JoinTableName", value);
        }
    }

    public string JoinTableLeftFieldName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableLeftFieldName"), "");
        }
        set
        {
            SetValue("JoinTableLeftFieldName", value);
        }
    }

    public string JoinTableRightFieldName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableRightFieldName"), "");
        }
        set
        {
            SetValue("JoinTableRightFieldName", value);
        }
    }

    public string JoinTableGUIDFieldOverride
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableGUIDFieldOverride"), "");
        }
        set
        {
            SetValue("JoinTableGUIDFieldOverride", value);
        }
    }

    public string JoinTableLastModifiedFieldOverride
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableLastModifiedFieldOverride"), "");
        }
        set
        {
            SetValue("JoinTableLastModifiedFieldOverride", value);
        }
    }

    public string JoinTableCodeNameFieldOverride
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableCodeNameFieldOverride"), "");
        }
        set
        {
            SetValue("JoinTableCodeNameFieldOverride", value);
        }
    }

    public string JoinTableSiteIDFieldOverride
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableSiteIDFieldOverride"), "");
        }
        set
        {
            SetValue("JoinTableSiteIDFieldOverride", value);
        }
    }

    public string FieldSaveMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FieldSaveType"), "CategoryName");
        }
        set
        {
            SetValue("FieldSaveMode", value);
        }
    }

    public string OrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderBy"), "");
        }
        set
        {
            SetValue("OrderBy", value);
        }
    }




    #endregion


    #region "Page events"

    /// <summary>
    /// Sets various values, hooks, and jquery elements to run the tool.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        // Add resources dynamically so can resolve the url.
        ScriptHelper.RegisterScriptFile(this.Page, ResolveUrl("~/CMSModules/RelationshipsExtended/Controls/RelatedCategories_Files/jquery.js"));
        ScriptHelper.RegisterScriptFile(this.Page, ResolveUrl("~/CMSModules/RelationshipsExtended/Controls/RelatedCategories_Files/AdvancedCategorySelector.js"));

        PossibleCategories = new List<CategoryInfo>();
        tbxOnlyLeafSelectable.Text = OnlyLeafSelectable.ToString();
        tbxParentSelectsChildren.Text = (OnlyLeafSelectable ? ParentSelectsChildren.ToString() : "False");
        // Set Enum Values
        switch (SaveMode)
        {
            case "ToCategories":
                SaveModeVal = SaveType.ToCategory;
                break;
            case "ToNodeCategories":
                // Same as Join table but with presets
                SaveModeVal = SaveType.ToJoinTable;
                JoinTableThisObjectForeignKey = "NodeID";
                JoinTableName = "CMS.TreeCategory";
                JoinTableLeftFieldName = "NodeID";
                JoinTableRightFieldName = "CategoryID";
                JoinTableGUIDFieldOverride = "";
                JoinTableLastModifiedFieldOverride = "";
                JoinTableCodeNameFieldOverride = "";
                JoinTableSiteIDFieldOverride = "";
                FieldSaveMode = "ID";
                break;
            case "ToJoinTable":
                SaveModeVal = SaveType.ToJoinTable;
                break;
        }

        switch (FieldSaveMode)
        {
            case "ID":
                FieldSaveModeVal = FieldSaveType.ID;
                break;
            case "GUID":
                FieldSaveModeVal = FieldSaveType.GUID;
                break;
            case "CategoryName":
                FieldSaveModeVal = FieldSaveType.CategoryName;
                break;
        }

        // Set the mode based on the field.
        DisplayMode = (CategoryDisplayMode == "Tree" ? DisplayType.Tree : DisplayType.List);
        btnSearch.OnClientClick = "SearchCategories('.FormTool_" + this.ID + "');return false;";
        tvwCategoryTree.LineImagesFolder = "~/CMSModules/RelationshipsExtended/Controls/RelatedCategories_Files";
        tvwCategoryTree.NodeStyle.CssClass = "InputNode";

    }


    /// <summary>
    /// Sets the category tree before load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setCategoryTree();
        }
    }

    /// <summary>
    /// Takes the existing value and sets the Category Tree
    /// </summary>
    protected void setCategoryTree()
    {
        tvwCategoryTree.Nodes.Clear();

        // Grab Current Categories
        CurrentCategories = new List<CategoryInfo>();
        if (SaveModeVal == SaveType.ToJoinTable)
        {
            // Get initial categories from Join Table.
            if (!IsPostBack)
            {
                // Get the CategoryNames currently in the join table.
                int totalRecords = 0;
                var currentCategoriesDS = ConnectionHelper.ExecuteQuery(JoinTableName + ".selectall", null, string.Format("{0} = '{1}'", JoinTableLeftFieldName, CurrentItemIdentification), null, -1, null, -1, -1, ref totalRecords);
                string CategoryFieldName = "";
                switch (FieldSaveModeVal)
                {
                    case FieldSaveType.ID:
                        CategoryFieldName = "CategoryID";
                        break;
                    case FieldSaveType.GUID:
                        CategoryFieldName = "CategoryGUID";
                        break;
                    case FieldSaveType.CategoryName:
                        CategoryFieldName = "CategoryName";
                        break;
                }
                List<string> currentCategories = new List<string>();
                foreach (DataRow dr in currentCategoriesDS.Tables[0].Rows)
                {
                    currentCategories.Add(ValidationHelper.GetString(dr[JoinTableRightFieldName], ""));
                }

                var CurrentCategoryObjects = CategoryInfoProvider.GetCategories(null, null, -1, null, SiteContext.CurrentSiteID).WhereIn(CategoryFieldName, currentCategories).Select(x => CategoryInfo.New(x));
                if (CurrentCategoryObjects != null)
                {
                    CurrentCategories.AddRange(CurrentCategoryObjects);
                }
            }
        }
        else
        {
            // Get initial categories from Document listing.
            if (!IsPostBack)
            {
                // Will set the txtValue to the current proper categories after the first setup.
                var CurrentCategoriesOfDoc = DocumentCategoryInfoProvider.GetDocumentCategories(ValidationHelper.GetInteger(PageDocument.GetValue("DocumentID"), -1));
                if (CurrentCategoriesOfDoc != null)
                {
                    CurrentCategories.AddRange(CurrentCategoriesOfDoc);
                }
                /*
                 * // Handle default if new form and no categories
                if (Form.Mode != FormModeEnum.Update && CurrentCategories.Count == 0 && !string.IsNullOrWhiteSpace(txtValue.Text))
                {
                    // Don't know if default is CodeName, Guid, or ID, so cover all
                    string[] DelimitedValues = SplitAndSecure(txtValue.Text);
                    string DefaultValueWhereCondition = "";
                    int[] intArray = StringArrayToIntArray(DelimitedValues);
                    Guid[] guidArray = StringArrayToGuidArray(DelimitedValues);
                    DefaultValueWhereCondition = SqlHelper.AddWhereCondition(DefaultValueWhereCondition, string.Format("CategoryName in ('{0}')", string.Join("','", DelimitedValues)), "OR");
                    if (intArray.Length > 0)
                    {
                        DefaultValueWhereCondition = SqlHelper.AddWhereCondition(DefaultValueWhereCondition, string.Format("CategoryID in ('{0}')", string.Join("','", intArray)), "OR");
                    }
                    if (guidArray.Length > 0)
                    {
                        DefaultValueWhereCondition = SqlHelper.AddWhereCondition(DefaultValueWhereCondition, string.Format("CategoryGUID in ('{0}')", string.Join("','", guidArray)), "OR");
                    }
                    foreach (CategoryInfo catInfo in CategoryInfoProvider.GetCategories().Where(DefaultValueWhereCondition))
                    {
                        CurrentCategories.Add(catInfo);
                    }
                }*/
            }
            else
            {
                /*
                var CurrentCategoriesOfDoc = CategoryInfoProvider.GetCategories("CategoryID in ('" + string.Join("','", SplitAndSecure(initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID);
                if (CurrentCategoriesOfDoc != null)
                {
                    CurrentCategories.AddRange(CurrentCategoriesOfDoc);
                }
                */
            }
        }


        if (OnlyLeafSelectable)
        {
            if (ParentSelectsChildren)
            {
                tvwCategoryTree.ShowCheckBoxes = TreeNodeTypes.All;
            }
            else
            {
                tvwCategoryTree.ShowCheckBoxes = TreeNodeTypes.Leaf;
            }
        }
        else
        {
            tvwCategoryTree.ShowCheckBoxes = TreeNodeTypes.All;
        }

        // Root Category given, all items must be under that root
        // If Tree Structure, must go through all children of root and then compare to the "Where" List of items, if where is set
        // If List, display all valid categories under the root.

        int RootCategoryID = 0;
        var rootCategory = CategoryInfoProvider.GetCategoryInfo(RootCategory, SiteContext.CurrentSiteName);
        if (rootCategory == null && int.TryParse(RootCategory, out RootCategoryID))
        {
            rootCategory = CategoryInfoProvider.GetCategoryInfo(RootCategoryID);
        }


        // Grab allowable Categories if user sets a WHERE
        string TempWhere = "CategoryNamePath like " + (rootCategory == null ? "'/%'" : "'" + rootCategory.CategoryNamePath + "/%'") + (string.IsNullOrWhiteSpace(WhereFilter) ? "" : " and " + WhereFilter);
        DefaultSortOrder = (string.IsNullOrWhiteSpace(OrderBy) ? (DisplayMode == DisplayType.List ? "CategoryDisplayName" : "CategoryLevel, CategoryOrder") : OrderBy);
        var AllowableCategoryList = CategoryInfoProvider.GetCategories(TempWhere, DefaultSortOrder, -1, null, SiteContext.CurrentSiteID);
        if (AllowableCategoryList.Count > 0)
        {
            AllowableCategoryIDWhere = "CategoryID in (" + string.Join(",", AllowableCategoryList.Select(x => new CategoryInfo(x).CategoryID)) + ")";
        }
        else
        {
            AllowableCategoryIDWhere = "";
        }

        if (DisplayMode == DisplayType.Tree)
        {
            pnlSearchFilter.Visible = false;
            tvwCategoryTree.ShowLines = true;
            TreeNode rootNode = null;
            if (rootCategory == null)
            {
                rootNode = new TreeNode("Root", GetNodeValue(rootCategory));
                rootNode.ShowCheckBox = (OnlyLeafSelectable && ParentSelectsChildren);
                rootNode.SelectAction = TreeNodeSelectAction.None;
                // Expand to the nth level from the Category Level.
                ExpandCategoryLevel = 0 + ExpandToLevel;
                rootNode.Expanded = (0 < ExpandCategoryLevel);
                SetNodeChecked(rootNode, rootCategory);
            }
            else
            {
                rootNode = new TreeNode(GetInputDataPrepend(rootCategory), GetNodeValue(rootCategory));
                rootNode.SelectAction = TreeNodeSelectAction.None;
                // Expand to the nth level from the Category Level.
                ExpandCategoryLevel = rootCategory.CategoryLevel + ExpandToLevel;
                rootNode.Expanded = (rootCategory.CategoryLevel < ExpandCategoryLevel);
                SetNodeChecked(rootNode, rootCategory);

                // If either all items selectable, or if only leaf selectable and this is a leaf node, save to the possible categories list.
                if (rootCategory.Children.Count == 0 || !OnlyLeafSelectable)
                {
                    PossibleCategories.Add(rootCategory);
                }
            }
            CreateChildTreeNodes(rootCategory, ref rootNode, ref CurrentCategories);
            tvwCategoryTree.Nodes.Add(rootNode);
        }
        else if (DisplayMode == DisplayType.List)
        {
            pnlTreeButtons.Visible = false;
            // Just loop through allowable Categories and add to list.
            foreach (CategoryInfo CategoryItem in AllowableCategoryList)
            {
                TreeNode childNode = new TreeNode(GetInputDataPrepend(CategoryItem), GetNodeValue(CategoryItem));
                childNode.SelectAction = TreeNodeSelectAction.None;
                childNode.Expanded = (CategoryItem.CategoryLevel < ExpandCategoryLevel);
                SetNodeChecked(childNode, CategoryItem);

                PossibleCategories.Add(CategoryItem);
                tvwCategoryTree.Nodes.Add(childNode);
            }
        }
    }

    /// <summary>
    /// Given the category, gets the Node Value that will be used.  Special logic for a null (root category)
    /// </summary>
    /// <param name="theCategory">The Category</param>
    /// <returns></returns>
    private string GetNodeValue(CategoryInfo theCategory)
    {
        if (theCategory == null)
        {
            return "";
        }
        switch (SaveModeVal)
        {
            case SaveType.ToField:

                switch (FieldSaveModeVal)
                {
                    case FieldSaveType.ID:
                        return (theCategory == null ? "0" : theCategory.CategoryID.ToString());
                    case FieldSaveType.GUID:
                        return (theCategory == null ? new Guid().ToString() : theCategory.CategoryGUID.ToString());
                    case FieldSaveType.CategoryName:
                        return (theCategory == null ? "" : theCategory.CategoryName);
                }
                break;
            case SaveType.Both:
            case SaveType.ToCategory:
            default:
                return theCategory.CategoryID.ToString();
        }
        return theCategory.CategoryID.ToString();
    }

    /// <summary>
    /// Populates the List with the children nodes.
    /// </summary>
    /// <param name="ParentCategoryNode">The Parent Category, can be null if it's the root</param>
    /// <param name="ParentNode">The Parent Tree Node</param>
    /// <param name="SelectedCategories">The currently selected categories</param>
    private void CreateChildTreeNodes(CategoryInfo ParentCategoryNode, ref TreeNode ParentNode, ref List<CategoryInfo> SelectedCategories)
    {
        // Grab all valid child categories
        var ChildCategories = (ParentCategoryNode == null ? CategoryInfoProvider.GetCategories(AllowableCategoryIDWhere, DefaultSortOrder, -1, null, SiteContext.CurrentSiteID).WhereEquals("CategoryLevel", 0) : CategoryInfoProvider.GetChildCategories(ParentCategoryNode.CategoryID, AllowableCategoryIDWhere, DefaultSortOrder, -1, null, SiteContext.CurrentSiteID));
        foreach (CategoryInfo childCategory in ChildCategories)
        {
            TreeNode childTreeObj = new TreeNode(GetInputDataPrepend(childCategory), GetNodeValue(childCategory));
            childTreeObj.SelectAction = TreeNodeSelectAction.None;
            // Must save as ID
            childTreeObj.Expanded = (childCategory.CategoryLevel < ExpandCategoryLevel);
            SetNodeChecked(childTreeObj, childCategory);

            // If either all items selectable, or if only leaf selectable and this is a leaf node, save to the possible categories list.
            if (childCategory.Children.Count == 0 || !OnlyLeafSelectable)
            {
                PossibleCategories.Add(childCategory);
            }
            CreateChildTreeNodes(childCategory, ref childTreeObj, ref SelectedCategories);
            ParentNode.ChildNodes.Add(childTreeObj);
        }
    }

    /// <summary>
    /// Sets the Checked value of the given TreeNode and corresponding Category if should be checked.
    /// </summary>
    /// <param name="treeNode">The Tree Node (.net Tree not CMS TreeNode)</param>
    /// <param name="category">That Tree Node's category</param>
    private void SetNodeChecked(TreeNode treeNode, CategoryInfo category)
    {
        switch (FieldSaveModeVal)
        {
            case FieldSaveType.ID:
                treeNode.Checked = CurrentCategories.Exists(x => x.CategoryID == (category == null ? 0 : category.CategoryID));
                break;
            case FieldSaveType.GUID:
                treeNode.Checked = CurrentCategories.Exists(x => x.CategoryGUID == (category == null ? new Guid() : category.CategoryGUID));
                break;
            case FieldSaveType.CategoryName:
                treeNode.Checked = CurrentCategories.Exists(x => x.CategoryName == (category == null ? "" : category.CategoryName));
                break;
        }
    }

    /// <summary>
    /// Gets the string html content that will go before the Input, used mainly for the client side scripting.
    /// </summary>
    /// <param name="category">The category, handles Null for root.</param>
    /// <returns>The html to prepend.</returns>
    private string GetInputDataPrepend(CategoryInfo category)
    {
        string dataText = GetDataText(category);
        return string.Format(" <div class=\"InputDataPrepend\" data-value=\"{0}\" data-text=\"{2}\">{1}</div>",
            HTMLHelper.HTMLEncode((SaveModeVal == SaveType.ToField ? dataText : (category == null ? "0" : category.CategoryID.ToString()))).Replace("\"", "&quot;"),
            HTMLHelper.HTMLEncode(MacroContext.CurrentResolver.ResolveMacros((category == null ? "Root" : category.CategoryDisplayName))).Replace("\"", "&quot;"),
            HTMLHelper.HTMLEncode(dataText));
    }

    /// <summary>
    /// Gets the display name that shows in the Textbox
    /// </summary>
    /// <param name="category">The Category, will show "Root" if null.</param>
    /// <returns>The display text</returns>
    private string GetDataText(CategoryInfo category)
    {
        string dataText = (category == null ? "Root" : MacroContext.CurrentResolver.ResolveMacros(category.CategoryDisplayName));
        if (SaveModeVal != SaveType.ToCategory)
        {
            switch (FieldSaveModeVal)
            {
                case FieldSaveType.ID:
                    if (category != null)
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.ToJoinTable ? category.CategoryName + " (" + category.CategoryID.ToString() + ")" : category.CategoryID.ToString());
                    }
                    else
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.ToJoinTable ? "Root (0)" : "0");
                    }
                    break;
                case FieldSaveType.GUID:
                    if (category != null)
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.ToJoinTable ? category.CategoryName : category.CategoryGUID.ToString());
                    }
                    else
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.ToJoinTable ? "Root" : new Guid().ToString());
                    }
                    break;
                case FieldSaveType.CategoryName:
                    if (category != null)
                    {
                        dataText = category.CategoryName;
                    }
                    else
                    {
                        dataText = "";
                    }
                    break;
            }
        }
        return dataText;
    }

    /// <summary>
    /// Handles the DataBinding of the Tree
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void tvwCategoryTree_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        // Convert value into category and get the proper text.
        int categoryID = 0;
        if (SaveModeVal != SaveType.ToCategory && SaveModeVal != SaveType.ToJoinTable)
        {
            switch (FieldSaveModeVal)
            {
                case FieldSaveType.ID:
                    int.TryParse(e.Node.Value, out categoryID);
                    break;
                case FieldSaveType.GUID:
                    CategoryInfo temp = CategoryInfoProvider.GetCategories("CategoryGUID = '" + (!string.IsNullOrWhiteSpace(e.Node.Value) ? new Guid().ToString() : e.Node.Value) + "'", null, -1, null, SiteContext.CurrentSiteID).FirstOrDefault();
                    if (temp != null)
                    {
                        categoryID = temp.CategoryID;
                    }
                    break;
                case FieldSaveType.CategoryName:
                    CategoryInfo temp2 = CategoryInfoProvider.GetCategories("CategoryName = '" + e.Node.Value + "'", null, -1, null, SiteContext.CurrentSiteID).FirstOrDefault();
                    if (temp2 != null)
                    {
                        categoryID = temp2.CategoryID;
                    }
                    break;
            }
        }
        else
        {
            int.TryParse(e.Node.Value, out categoryID);
        }
        if (categoryID > 0)
        {
            e.Node.Text = GetInputDataPrepend(CategoryInfoProvider.GetCategoryInfo(categoryID));
        }
    }

    #endregion

    #region "Private methods"



    protected object CurrentItemIdentification
    {
        get
        {
            if (JoinTableForeignKeySource == "Page")
            {
                return PageDocument.GetValue(JoinTableThisObjectForeignKey);
            }
            else
            {
                return ((BaseInfo)UIContext.EditedObject).GetValue(JoinTableThisObjectForeignKey);
            }
        }
    }

    /// <summary>
    /// Figure out using the Join Table which categories need to be removed and which added
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveToJoinTable()
    {
        SetPossibleAndSelectedCategories();

        // Do check of min/max here
        if (MinimumCategories > 0 && SelectedCategories.Count() < MinimumCategories)
        {
            AddError("Must select at least " + MinimumCategories + " " + (MinimumCategories == 1 ? "category" : "categories"));
            return;
        }
        if (MaximumCategories > 0 && SelectedCategories.Count() > MaximumCategories)
        {
            AddError("Can select no more than " + MaximumCategories + " " + (MaximumCategories == 1 ? "category" : "categories"));
            return;
        }

        // filter with only the possible categories
        DataClassInfo JoinTableClassInfo = DataClassInfoProvider.GetDataClassInfo(JoinTableName);
        if (JoinTableClassInfo != null)
        {
            List<int> DocumentCategoryIds = new List<int>();

            // Get all the categories the current entity has
            int totalRecords = 0;
            var currentCategoriesDS = ConnectionHelper.ExecuteQuery(JoinTableName + ".selectall", null, string.Format("{0} = '{1}'", JoinTableLeftFieldName, CurrentItemIdentification), null, -1, null, -1, -1, ref totalRecords);
            CategoryInfo catObject = null;
            string FieldSaveColumnName = "";
            // Convert to CategoryID
            switch (FieldSaveModeVal)
            {
                case FieldSaveType.ID:
                    FieldSaveColumnName = "CategoryID";
                    break;
                case FieldSaveType.GUID:
                    FieldSaveColumnName = "CategoryGUID";
                    break;
                case FieldSaveType.CategoryName:
                    FieldSaveColumnName = "CategoryName";
                    break;
            }

            foreach (DataRow dr in currentCategoriesDS.Tables[0].Rows)
            {
                // Convert to CategoryID
                switch (FieldSaveModeVal)
                {
                    case FieldSaveType.ID:
                        catObject = CategoryInfoProvider.GetCategoryInfo(ValidationHelper.GetInteger(dr[JoinTableRightFieldName], 0));
                        if (catObject != null)
                        {
                            DocumentCategoryIds.Add(catObject.CategoryID);
                        }
                        break;
                    case FieldSaveType.GUID:
                        var ClassObject = CategoryInfoProvider.GetCategories().WhereEquals("CategoryGUID", ValidationHelper.GetGuid(dr[JoinTableRightFieldName], new Guid())).FirstOrDefault();
                        if (ClassObject != null)
                        {
                            DocumentCategoryIds.Add(ValidationHelper.GetInteger(ClassObject["CategoryID"], 0));
                        }
                        break;
                    case FieldSaveType.CategoryName:
                        catObject = CategoryInfoProvider.GetCategoryInfo(ValidationHelper.GetString(dr[JoinTableRightFieldName], ""), SiteContext.CurrentSiteName);
                        if (catObject != null)
                        {
                            DocumentCategoryIds.Add(catObject.CategoryID);
                        }
                        break;
                }
            }

            // Find IDs we need to add and remove.
            List<int> NotSelectedIds = PossibleCategoryIDs.Except(SelectedCategoryIDs).ToList();
            List<int> DeselectIds = DocumentCategoryIds.Intersect(NotSelectedIds).ToList();
            List<int> SelectIds = SelectedCategoryIDs.Except(DocumentCategoryIds).ToList();

            if (DeselectIds.Count > 0)
            {
                foreach (int DeselectId in DeselectIds)
                {
                    if (JoinTableClassInfo.ClassIsCustomTable)
                    {
                        CustomTableItemProvider.GetItems(JoinTableClassInfo.ClassName).WhereEquals(JoinTableLeftFieldName, CurrentItemIdentification)
                            .WhereEquals(JoinTableRightFieldName, CategoryInfoProvider.GetCategoryInfo(DeselectId).GetValue(FieldSaveColumnName))
                            .ToList().ForEach(x => ((CustomTableItem)x).Delete());
                    }
                    else
                    {
                        new ObjectQuery(JoinTableClassInfo.ClassName)
                            .WhereEquals(JoinTableLeftFieldName, CurrentItemIdentification)
                            .WhereEquals(JoinTableRightFieldName, CategoryInfoProvider.GetCategoryInfo(DeselectId).GetValue(FieldSaveColumnName))
                            .ToList().ForEach(x => x.Delete());
                    }

                }
            }
            if (SelectIds.Count > 0)
            {
                foreach (int SelectId in SelectIds)
                {
                    if (JoinTableClassInfo.ClassIsCustomTable)
                    {
                        CustomTableItem newCustomTableItem = CustomTableItem.New(JoinTableName);
                        SetBaseInfoItemValues(newCustomTableItem, CategoryInfoProvider.GetCategoryInfo(SelectId).GetValue(FieldSaveColumnName), JoinTableClassInfo.ClassName);
                        InsertObjectHandler(newCustomTableItem);
                    }
                    else
                    {
                        // Create a dynamic BaseInfo object of the right type.
                        var JoinTableClassFactory = new InfoObjectFactory(JoinTableClassInfo.ClassName);
                        if (JoinTableClassFactory.Singleton == null)
                        {
                            AddError("Class does not have TypeInfo and TypeInfoProvider generated.  Must generate " + JoinTableClassInfo.ClassName + " Code before can bind.");
                            return;
                        }
                        BaseInfo newJoinObj = ((BaseInfo)JoinTableClassFactory.Singleton);
                        SetBaseInfoItemValues(newJoinObj, CategoryInfoProvider.GetCategoryInfo(SelectId).GetValue(FieldSaveColumnName), JoinTableClassInfo.ClassName);
                        InsertObjectHandler(newJoinObj);
                    }
                }
            }

            AddConfirmation(string.Format("{0} Categories Added, {1} Categories Removed.", SelectIds.Count, DeselectIds.Count));
        }
    }

    private void InsertObjectHandler(BaseInfo newJoinObj)
    {
        try
        {
            newJoinObj.Insert();
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.IndexOf("Cannot insert the value NULL into column") > -1)
            {
                AddError("One or more required fields are not defined.  Please either set the Object's TypeInfo with the proper Guid, Timestamp, CodeName, and/or SiteID columns, or add the field names to these Override in the properties.<br/><br/>If the required column is not one of these system columns, then please implement a global event on the Insert before to set these values programatically.<br/><br/>" + ex.Message.Replace("\n", "<br/>"));
                return;
            }
        }
        catch (Exception ex)
        {
            AddError("An error occured.<br/><br/>" + ex.Message.Replace("\n", "<br/>"));
            return;
        }
    }

    /// <summary>
    /// Sets the values of the Custom Module Class's new item.
    /// </summary>
    /// <param name="newItem">The new Item (gotten from your Custom Module Class's InfoProvider class)</param>
    /// <param name="CategoryValue">The Category's Reference Value</param>
    /// <param name="ClassName">The ClassName, used for the CodeName field generation</param>
    private void SetBaseInfoItemValues(BaseInfo newItem, object CategoryValue, string ClassName)
    {
        newItem.SetValue(JoinTableLeftFieldName, CurrentItemIdentification);
        newItem.SetValue(JoinTableRightFieldName, CategoryValue);
        string TimeStampCol = (!string.IsNullOrWhiteSpace(JoinTableLastModifiedFieldOverride) ? JoinTableLastModifiedFieldOverride : DataHelper.GetNotEmpty(newItem.TypeInfo.TimeStampColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        string GUIDCol = (!string.IsNullOrWhiteSpace(JoinTableGUIDFieldOverride) ? JoinTableGUIDFieldOverride : DataHelper.GetNotEmpty(newItem.TypeInfo.GUIDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        string CodeNameCol = (!string.IsNullOrWhiteSpace(JoinTableCodeNameFieldOverride) ? JoinTableCodeNameFieldOverride : DataHelper.GetNotEmpty(newItem.TypeInfo.CodeNameColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        string SiteIDCol = (!string.IsNullOrWhiteSpace(JoinTableSiteIDFieldOverride) ? JoinTableSiteIDFieldOverride : DataHelper.GetNotEmpty(newItem.TypeInfo.SiteIDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        if (!string.IsNullOrWhiteSpace(TimeStampCol)) { newItem.SetValue(TimeStampCol, DateTime.Now); }
        if (!string.IsNullOrWhiteSpace(GUIDCol)) { newItem.SetValue(GUIDCol, Guid.NewGuid()); }
        if (!string.IsNullOrWhiteSpace(CodeNameCol)) { newItem.SetValue(CodeNameCol, string.Format("{0}_{1}_{2}", ClassName.Replace(".", "_"), CurrentItemIdentification, CategoryValue.ToString())); }
        if (!string.IsNullOrWhiteSpace(SiteIDCol)) { newItem.SetValue(SiteIDCol, SiteContext.CurrentSiteID); }
    }

    /// <summary>
    /// Handles the logic of assigning categories to the document.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveToDocumentCategories()
    {
        SetPossibleAndSelectedCategories();

        // Do check of min/max here
        // Do check of min/max here
        if (MinimumCategories > 0 && SelectedCategories.Count() < MinimumCategories)
        {
            AddError("Must select at least " + MinimumCategories + " " + (MinimumCategories == 1 ? "category" : "categories"));
            return;
        }
        if (MaximumCategories > 0 && SelectedCategories.Count() > MaximumCategories)
        {
            AddError("Can select no more than " + MaximumCategories + " " + (MaximumCategories == 1 ? "category" : "categories"));
            return;
        }

        int DocumentID = ValidationHelper.GetInteger(PageDocument.GetValue("DocumentID"), -1);

        // Can only do Document/Category if there is a DocumentID on the current object.
        if (DocumentID > 0)
        {
            // Get selected categories, except only those in the possible categories
            List<int> DocumentCategoryIds = new List<int>();
            foreach (var DocCategory in DocumentCategoryInfoProvider.GetDocumentCategories(DocumentID))
            {
                DocumentCategoryIds.Add(DocCategory.CategoryID);
            }

            // Find IDs we need to add and remove.
            List<int> NotSelectedIds = PossibleCategoryIDs.Except(SelectedCategoryIDs).ToList();
            List<int> DeselectIds = DocumentCategoryIds.Intersect(NotSelectedIds).ToList();
            List<int> SelectIds = SelectedCategoryIDs.Except(DocumentCategoryIds).ToList();

            bool CategoriesChanged = false;
            foreach (int DeselectId in DeselectIds)
            {
                DocumentCategoryInfoProvider.DeleteDocumentCategoryInfo(DocumentID, DeselectId);
                CategoriesChanged = true;
            }
            foreach (int SelectId in SelectIds)
            {
                DocumentCategoryInfoProvider.AddDocumentToCategory(DocumentID, SelectId);
                CategoriesChanged = true;
            }

            // Page changes require custom logic for staging
            if (CategoriesChanged && LicenseHelper.CheckFeature(Request.Url.AbsoluteUri, FeatureEnum.Staging))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

                List<ServerInfo> targetServers = ServerInfoProvider.GetServers().Where(x => x.ServerSiteID == SiteContext.CurrentSiteID && x.ServerEnabled).ToList();
                foreach (ServerInfo targetServer in targetServers)
                {
                    var docObj = DocumentHelper.GetDocument(DocumentID, tree);
                    var settings = new LogMultipleDocumentChangeSettings()
                    {
                        EnsurePublishTask = true,
                        NodeAliasPath = docObj.NodeAliasPath,
                        TaskType = TaskTypeEnum.UpdateDocument,
                        ServerID = targetServer.ServerID,
                        KeepTaskData = false,
                        RunAsynchronously = false,
                        SiteName = docObj.Site.SiteName
                    };
                    // Logs parent task, which will run through the task on insert event and do the same check.
                    var currentNodeUpdateTask = DocumentSynchronizationHelper.LogDocumentChange(settings);
                }
            }
            AddConfirmation(string.Format("{0} Categories Added, {1} Categories Removed.", SelectIds.Count, DeselectIds.Count));
        }
    }

    private void SetPossibleAndSelectedCategories()
    {
        PossibleCategories = new List<CategoryInfo>();
        PossibleCategoryIDs = new List<int>();
        SelectedCategories = new List<CategoryInfo>();
        SelectedCategoryIDs = new List<int>();
        // navigate through the tree structure
        foreach (TreeNode node in tvwCategoryTree.Nodes)
        {
            SetPossibleAndSelectedCategoriesTreeRecursive(node);
        }
        // Get allowed category IDs and those found
        PossibleCategories = CategoryInfoProvider.GetCategories().WhereIn("CategoryID", PossibleCategoryIDs).TypedResult.ToList();
        SelectedCategories = CategoryInfoProvider.GetCategories().WhereIn("CategoryID", SelectedCategoryIDs).TypedResult.ToList();
    }

    private void SetPossibleAndSelectedCategoriesTreeRecursive(TreeNode treeNode)
    {
        if (DisplayMode == DisplayType.List || treeNode.ChildNodes.Count == 0 || !OnlyLeafSelectable)
        {
            PossibleCategoryIDs.Add(ValidationHelper.GetInteger(treeNode.Value, -1));
            if (treeNode.Checked)
            {
                SelectedCategoryIDs.Add(ValidationHelper.GetInteger(treeNode.Value, -1));
            }
        }
        foreach (TreeNode childNode in treeNode.ChildNodes)
        {
            SetPossibleAndSelectedCategoriesTreeRecursive(childNode);
        }
    }


    #endregion

    #region "Other Hooks"



    /// <summary>
    /// The expand all checked will recursive go through the full tree and expand if a check is found below it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExpandChecked_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in tvwCategoryTree.Nodes)
        {
            bool expandMe = RecursiveSelectedExpand(node);
            if (expandMe)
            {
                node.Expand();
            }
        }
    }

    /// <summary>
    /// Collapse all button collapses entie tree.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCollapseAll_Click(object sender, EventArgs e)
    {
        tvwCategoryTree.CollapseAll();
    }

    /// <summary>
    /// Expand button will expand every tree node that has children.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExpandAll_Click(object sender, EventArgs e)
    {
        tvwCategoryTree.ExpandAll();
    }

    /// <summary>
    /// Recursively checks for any nodes with a check and expands to that node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool RecursiveSelectedExpand(TreeNode node)
    {
        if (node.ChildNodes.Count == 0 && node.Checked)
        {
            return true;
        }
        bool returnVal = false;
        foreach (TreeNode childNode in node.ChildNodes)
        {
            returnVal = (RecursiveSelectedExpand(childNode) || returnVal);
        }
        if (returnVal)
        {
            node.Expand();
        }
        else
        {
            node.Collapse();
        }
        return returnVal;
    }

    #endregion


    protected void btnAddCategories_Click(object sender, EventArgs e)
    {
        if (SaveModeVal == SaveType.ToCategory)
        {
            SaveToDocumentCategories();
        }

        if (SaveModeVal == SaveType.ToJoinTable)
        {
            SaveToJoinTable();
        }
    }
}

namespace RelatedCategories
{
    enum DisplayType { Tree, List };
    enum SaveType { ToField, ToCategory, Both, ToJoinTable };
    enum FieldSaveType { ID, GUID, CategoryName };

}