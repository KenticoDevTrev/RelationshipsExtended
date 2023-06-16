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
using CMS.LicenseProvider;
using CMS.FormEngine.Web.UI;
using CMS.Base.Web.UI;
using RelationshipsExtended.Enums;
using RelationshipsExtended;
using System.Web;
using RelationshipsExtended.Helpers;

public partial class Compiled_CMSModules_RelationshipsExtended_FormControls_AdvancedCategorySelector_AdvancedCategorySelector : FormEngineUserControl
{

    public Compiled_CMSModules_RelationshipsExtended_FormControls_AdvancedCategorySelector_AdvancedCategorySelector()
    {

    }

    #region "Variables"

    private DisplayType DisplayMode;
    private SaveType SaveModeVal;
    private CategoryFieldSaveType FieldSaveModeVal;
    private string AllowableCategoryIDWhere;
    private string DefaultSortOrder;
    private int ExpandCategoryLevel;
    private List<CategoryInfo> PossibleCategories;
    private List<CategoryInfo> CurrentCategories;
    private List<CategoryInfo> _InitialRealCurrentCategoriesHolder;
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

    public string SeparatorCharacter
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SeparatorCharacter"), "|");
        }
        set
        {
            SetValue("SeparatorCharacter", value);
            tbxSeparatorCharacter.Text = value;
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

    public string JoinTableGUIDField
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableGUIDField"), "");
        }
        set
        {
            SetValue("JoinTableGUIDField", value);
        }
    }

    public string JoinTableLastModifiedField
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableLastModifiedField"), "");
        }
        set
        {
            SetValue("JoinTableLastModifiedField", value);
        }
    }

    public string JoinTableCodeNameField
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableCodeNameField"), "");
        }
        set
        {
            SetValue("JoinTableCodeNameField", value);
        }
    }

    public string JoinTableSiteIDField
    {
        get
        {
            return ValidationHelper.GetString(GetValue("JoinTableSiteIDField"), "");
        }
        set
        {
            SetValue("JoinTableSiteIDField", value);
        }
    }

    public string OriginalFieldSaveType
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FieldSaveType"), "CategoryName");
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
            SetValue("FieldSaveType", value);
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

    /// <summary>
    /// Property used to access the parameter of the form control.
    /// </summary>
    public bool AllowManualEntry
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowManualEntry"), false);
        }
        set
        {
            SetValue("AllowManualEntry", value);
        }
    }


    #endregion

    #region "Public properties"

    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {

            // Adjust to return ID, Name, or GUID depending on setting.
            string CategoryValues = txtValue.Text.Trim();
            switch (SaveModeVal)
            {
                case SaveType.ToCategory:
                case SaveType.ToJoinTable:
                    return "";
                case SaveType.ToField:
                    switch (FieldSaveMode)
                    {
                        case "ID":
                            string returnVal1 = "";
                            var Categories1 = CategoryInfoProvider.GetCategories(string.Format("CategoryID in ('{0}')", string.Join("','", SplitAndSecure(CategoryValues))), null, -1, null, SiteContext.CurrentSiteID);
                            foreach (DataRow cdr in Categories1.Execute().Tables[0].Rows)
                            {
                                returnVal1 += SeparatorCharacter + cdr["CategoryID"].ToString();
                            }
                            return (string.IsNullOrWhiteSpace(returnVal1) ? "" : returnVal1.Substring(1));
                        case "GUID":
                            string returnVal2 = "";
                            var Categories2 = CategoryInfoProvider.GetCategories(string.Format("CategoryGUID in ('{0}')", string.Join("','", SplitAndSecure(string.IsNullOrWhiteSpace(CategoryValues) ? new Guid().ToString() : CategoryValues))), null, -1, null, SiteContext.CurrentSiteID);
                            foreach (DataRow cdr in Categories2.Execute().Tables[0].Rows)
                            {
                                returnVal2 += SeparatorCharacter + cdr["CategoryGUID"].ToString();
                            }
                            return (string.IsNullOrWhiteSpace(returnVal2) ? "" : returnVal2.Substring(1));
                        case "CategoryName":
                            string returnVal3 = "";
                            var Categories3 = CategoryInfoProvider.GetCategories(string.Format("CategoryName in ('{0}')", string.Join("','", SplitAndSecure(CategoryValues))), null, -1, null, SiteContext.CurrentSiteID);
                            foreach (DataRow cdr in Categories3.Execute().Tables[0].Rows)
                            {
                                returnVal3 += SeparatorCharacter + cdr["CategoryName"].ToString();
                            }
                            return (string.IsNullOrWhiteSpace(returnVal3) ? "" : returnVal3.Substring(1));
                    }
                    break;
                case SaveType.Both:
                case SaveType.BothNode:
                    var Categories = CategoryInfoProvider.GetCategories(string.Format("CategoryID in ('{0}')", string.Join("','", SplitAndSecure(CategoryValues))), null, -1, null, SiteContext.CurrentSiteID);
                    List<string> CategoryTextVal = new List<string>();
                    foreach (var Category in Categories)
                    {
                        switch (OriginalFieldSaveType)
                        {
                            case "ID":
                                CategoryTextVal.Add(Category.CategoryID.ToString());
                                break;
                            case "GUID":
                                CategoryTextVal.Add(Category.CategoryGUID.ToString());
                                break;
                            case "CategoryName":
                                CategoryTextVal.Add(Category.CategoryName);
                                break;
                        }
                    }
                    return string.Join(SeparatorCharacter, CategoryTextVal.ToArray());
            }
            return "";
        }
        set
        {
            // To Field must save to txtValue, the other will derive from Categories or Join Table
            // This runs before the OnInit so must use actual value
            switch (SaveMode)
            {
                case "ToField":
                    txtValue.Text = ValidationHelper.GetString(value, "");
                    break;
                case "Both":
                case "BothNode":
                    // If both, and it's an insert of the form, then set the txtvalue's default if given
                    if (Form.Mode != FormModeEnum.Update)
                    {
                        txtValue.Text = ValidationHelper.GetString(value, "");
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Gets ClientID of the textbox with path.
    /// </summary>
    public override string ValueElementID
    {
        get
        {
            return txtValue.ClientID;
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
        ScriptHelper.RegisterScriptFile(this.Page, ResolveUrl("~/CMSModules/RelationshipsExtended/FormControls/AdvancedCategorySelector/AdvancedCategorySelector_files/js/jquery.js"));
        ScriptHelper.RegisterScriptFile(this.Page, ResolveUrl("~/CMSModules/RelationshipsExtended/FormControls/AdvancedCategorySelector/AdvancedCategorySelector_files/js/AdvancedCategorySelector.js"));
        Literal cssFile = new Literal() { Text = @"<link href=""" + ResolveUrl("~/CMSModules/RelationshipsExtended/FormControls/AdvancedCategorySelector/AdvancedCategorySelector_files/css/styles.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
        this.Page.Header.Controls.Add(cssFile);

        _InitialRealCurrentCategoriesHolder = new List<CategoryInfo>();
        PossibleCategories = new List<CategoryInfo>();
        tbxSaveMode.Text = SaveMode;
        tbxOnlyLeafSelectable.Text = OnlyLeafSelectable.ToString();
        tbxParentSelectsChildren.Text = (OnlyLeafSelectable ? ParentSelectsChildren.ToString() : "False");
        tbxSeparatorCharacter.Text = SeparatorCharacter;
        string CorrectedFieldSaveMode = FieldSaveMode;
        // Set Enum Values
        switch (SaveMode)
        {
            case "ToField":
                SaveModeVal = SaveType.ToField;
                break;
            case "Both":
                SaveModeVal = SaveType.Both;
                break;
            case "ToCategories":
                SaveModeVal = SaveType.ToCategory;
                break;
            case "BothNode":
                SaveModeVal = SaveType.BothNode;
                // Same as Join table but with presets
                JoinTableThisObjectForeignKey = "NodeID";
                JoinTableName = "CMS.TreeCategory";
                JoinTableLeftFieldName = "NodeID";
                JoinTableRightFieldName = "CategoryID";
                JoinTableGUIDField = "";
                JoinTableLastModifiedField = "";
                JoinTableCodeNameField = "";
                JoinTableSiteIDField = "";
                CorrectedFieldSaveMode = "ID";
                break;
            case "ToNodeCategories":
                // Same as Join table but with presets
                SaveModeVal = SaveType.ToJoinTable;
                JoinTableThisObjectForeignKey = "NodeID";
                JoinTableName = "CMS.TreeCategory";
                JoinTableLeftFieldName = "NodeID";
                JoinTableRightFieldName = "CategoryID";
                JoinTableGUIDField = "";
                JoinTableLastModifiedField = "";
                JoinTableCodeNameField = "";
                JoinTableSiteIDField = "";
                CorrectedFieldSaveMode = "ID";
                break;
            case "ToJoinTable":
                SaveModeVal = SaveType.ToJoinTable;
                break;
        }

        switch (CorrectedFieldSaveMode)
        {
            case "ID":
                FieldSaveModeVal = CategoryFieldSaveType.ID;
                break;
            case "GUID":
                FieldSaveModeVal = CategoryFieldSaveType.GUID;
                break;
            case "CategoryName":
                FieldSaveModeVal = CategoryFieldSaveType.CategoryName;
                break;
        }

        // Set the mode based on the field.
        DisplayMode = (CategoryDisplayMode == "Tree" ? DisplayType.Tree : DisplayType.List);

        if (SaveModeVal == SaveType.ToCategory || SaveModeVal == SaveType.Both)
        {
            this.Form.OnAfterSave += Form_OnAfterSave;
        }

        if (SaveModeVal == SaveType.ToJoinTable || SaveModeVal == SaveType.BothNode)
        {
            this.Form.OnAfterSave += Form_OnAfterSaveJoinTable;
        }

        if (!(AllowManualEntry && SaveModeVal == SaveType.ToField))
        {
            txtValue.Attributes.Add("readonly", "readonly");
        }
        else
        {
            // Have to post back on change to update the listing.
            txtValue.AutoPostBack = true;
        }

        if (SaveModeVal == SaveType.ToField)
        {
            btnSelect.OnClientClick = "$advjQuery('#" + btnSelect.ClientID + "').attr('value', 'Processing...'); $advjQuery('#" + txtValue.ClientID + "').val($advjQuery('#" + tbxCategoryValue.ClientID + "').val());";
        }
        else
        {
            btnSelect.OnClientClick = "$advjQuery('#" + btnSelect.ClientID + "').attr('value', 'Processing...'); $advjQuery('#" + txtValue.ClientID + "').val($advjQuery('#" + tbxCategoryValue.ClientID + "').val()); $advjQuery('#" + tbxDisplayValue.ClientID + "').val($advjQuery('#" + tbxDisplayValueHolder.ClientID + "').val());";
        }
        btnClose.OnClientClick = "$advjQuery('#" + tbxCategoryValue.ClientID + "').val($advjQuery('#" + txtValue.ClientID + "').val());";

        ModalContainerForClass.CssClass = "FormTool_" + this.ID;
        btnSearch.OnClientClick = "SearchCategories('.FormTool_" + this.ID + "');return false;";

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
        if (SaveModeVal == SaveType.ToField)
        {
            tbxDisplayValue.CssClass += " hidden";
            string initialCategories = txtValue.Text;
            switch (FieldSaveModeVal)
            {
                case CategoryFieldSaveType.ID:
                    CurrentCategories.AddRange(CategoryInfoProvider.GetCategories("CategoryID in ('" + string.Join("','", SplitAndSecure(string.IsNullOrWhiteSpace(initialCategories) ? "-1" : initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID));
                    break;
                case CategoryFieldSaveType.GUID:
                    CurrentCategories.AddRange(CategoryInfoProvider.GetCategories("CategoryGUID in ('" + string.Join("','", SplitAndSecure(string.IsNullOrWhiteSpace(initialCategories) ? new Guid().ToString() : initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID));
                    break;
                case CategoryFieldSaveType.CategoryName:
                    CurrentCategories.AddRange(CategoryInfoProvider.GetCategories("CategoryName in ('" + string.Join("','", SplitAndSecure(initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID));
                    break;
            }
        }
        else if (SaveModeVal == SaveType.ToJoinTable || SaveModeVal == SaveType.BothNode)
        {
            tbxDisplayValue.Attributes.Add("readonly", "readonly");
            txtValue.CssClass += " hidden";
            // Get initial categories from Join Table.
            if (!IsPostBack)
            {
                // Get the CategoryNames currently in the join table.
                int totalRecords = 0;
                var currentCategoriesDS = ConnectionHelper.ExecuteQuery(JoinTableName + ".selectall", null, string.Format("{0} = '{1}'", JoinTableLeftFieldName, CurrentItemIdentification), null, -1, null, -1, -1, ref totalRecords);
                string CategoryFieldName = "";
                switch (FieldSaveModeVal)
                {
                    case CategoryFieldSaveType.ID:
                        CategoryFieldName = "CategoryID";
                        break;
                    case CategoryFieldSaveType.GUID:
                        CategoryFieldName = "CategoryGUID";
                        break;
                    case CategoryFieldSaveType.CategoryName:
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
            else
            {
                // Set from Text value
                string initialCategories = txtValue.Text;
                var CurrentCategoriesOfDoc = CategoryInfoProvider.GetCategories("CategoryID in ('" + string.Join("','", SplitAndSecure(initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID);
                if (CurrentCategoriesOfDoc != null)
                {
                    CurrentCategories.AddRange(CurrentCategoriesOfDoc);
                }
            }
        }
        else
        {
            tbxDisplayValue.Attributes.Add("readonly", "readonly");
            txtValue.CssClass += " hidden";
            // Get initial categories from Document listing.
            if (!IsPostBack)
            {
                // Will set the txtValue to the current proper categories after the first setup.
                var CurrentCategoriesOfDoc = DocumentCategoryInfoProvider.GetDocumentCategories(ValidationHelper.GetInteger(Form.GetDataValue("DocumentID"), -1));
                if (CurrentCategoriesOfDoc != null)
                {
                    CurrentCategories.AddRange(CurrentCategoriesOfDoc);
                }
                // Handle default if new form and no categories
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
                    foreach (CategoryInfo catInfo in CategoryInfo.Provider.Get().Where(DefaultValueWhereCondition))
                    {
                        CurrentCategories.Add(catInfo);
                    }
                }
            }
            else
            {
                string initialCategories = txtValue.Text;
                var CurrentCategoriesOfDoc = CategoryInfoProvider.GetCategories("CategoryID in ('" + string.Join("','", SplitAndSecure(initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID);
                if (CurrentCategoriesOfDoc != null)
                {
                    CurrentCategories.AddRange(CurrentCategoriesOfDoc);
                }
            }
        }

        if (SaveModeVal != SaveType.ToField && !IsPostBack)
        {
            // clear the txtValue since it will be overwritten with the IDs anyway
            txtValue.Text = "";
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
        var rootCategory = CategoryInfo.Provider.Get(RootCategory, SiteContext.CurrentSiteID);
        if (rootCategory == null && int.TryParse(RootCategory, out RootCategoryID))
        {
            rootCategory = CategoryInfo.Provider.Get(RootCategoryID);
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

        // If first time for non Save to Field only, then change the CurrentCategories (which has all categories in a document) 
        // to the current list of valid categories that are contained in this form
        // and set the Display textbox.
        if (SaveModeVal != SaveType.ToField && !IsPostBack)
        {
            if (txtValue.Text.Length > 0)
            {
                txtValue.Text = txtValue.Text.Substring(1);
            }
            CurrentCategories = _InitialRealCurrentCategoriesHolder;
            tbxDisplayValue.Text = string.Join(SeparatorCharacter, CurrentCategories.Select(x => GetDataText(x)).ToArray());
        }

        ltrOriginalValues.Text = "<div class=\"ltrOriginalValues\">" + string.Join(SeparatorCharacter, CurrentCategories.Select(x => x.CategoryID).ToArray()) + "</div>";
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
                    case CategoryFieldSaveType.ID:
                        return (theCategory == null ? "0" : theCategory.CategoryID.ToString());
                    case CategoryFieldSaveType.GUID:
                        return (theCategory == null ? new Guid().ToString() : theCategory.CategoryGUID.ToString());
                    case CategoryFieldSaveType.CategoryName:
                        return (theCategory == null ? "" : theCategory.CategoryName);
                }
                break;
            case SaveType.Both:
            case SaveType.BothNode:
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
        if (SaveModeVal != SaveType.ToField)
        {
            treeNode.Checked = CurrentCategories.Exists(x => x.CategoryID == (category == null ? 0 : category.CategoryID));
        }
        else
        {
            switch (FieldSaveModeVal)
            {
                case CategoryFieldSaveType.ID:
                    treeNode.Checked = CurrentCategories.Exists(x => x.CategoryID == (category == null ? 0 : category.CategoryID));
                    break;
                case CategoryFieldSaveType.GUID:
                    treeNode.Checked = CurrentCategories.Exists(x => x.CategoryGUID == (category == null ? new Guid() : category.CategoryGUID));
                    break;
                case CategoryFieldSaveType.CategoryName:
                    treeNode.Checked = CurrentCategories.Exists(x => x.CategoryName == (category == null ? "" : category.CategoryName));
                    break;
            }
        }

        // If initial rendering, add this matching category to the txtValue's ID list.
        if (treeNode.Checked && SaveModeVal != SaveType.ToField && !IsPostBack)
        {
            txtValue.Text += SeparatorCharacter + (category == null ? "0" : category.CategoryID.ToString());
            _InitialRealCurrentCategoriesHolder.Add(category);
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
                case CategoryFieldSaveType.ID:
                    if (category != null)
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.BothNode || SaveModeVal == SaveType.ToJoinTable ? category.CategoryName + " (" + category.CategoryID.ToString() + ")" : category.CategoryID.ToString());
                    }
                    else
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.BothNode || SaveModeVal == SaveType.ToJoinTable ? "Root (0)" : "0");
                    }
                    break;
                case CategoryFieldSaveType.GUID:
                    if (category != null)
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.BothNode || SaveModeVal == SaveType.ToJoinTable ? category.CategoryName : category.CategoryGUID.ToString());
                    }
                    else
                    {
                        dataText = (SaveModeVal == SaveType.Both || SaveModeVal == SaveType.BothNode || SaveModeVal == SaveType.ToJoinTable ? "Root" : new Guid().ToString());
                    }
                    break;
                case CategoryFieldSaveType.CategoryName:
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
                case CategoryFieldSaveType.ID:
                    int.TryParse(e.Node.Value, out categoryID);
                    break;
                case CategoryFieldSaveType.GUID:
                    CategoryInfo temp = CategoryInfoProvider.GetCategories("CategoryGUID = '" + (!string.IsNullOrWhiteSpace(e.Node.Value) ? new Guid().ToString() : e.Node.Value) + "'", null, -1, null, SiteContext.CurrentSiteID).FirstOrDefault();
                    if (temp != null)
                    {
                        categoryID = temp.CategoryID;
                    }
                    break;
                case CategoryFieldSaveType.CategoryName:
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
            e.Node.Text = GetInputDataPrepend(CategoryInfo.Provider.Get(categoryID));
        }
    }

    #endregion

    #region "Private methods"

    protected object CurrentItemIdentification
    {
        get
        {
            object ForiegnKeyValue = Form.GetFieldValue(JoinTableThisObjectForeignKey);
            if (BindOnPrimaryNodeOnly)
            {
                switch (JoinTableThisObjectForeignKey.ToLower())
                {
                    case "nodeguid":
                    case "nodealiaspath":
                        // Get the node
                        return CacheHelper.Cache<object>(cs =>
                        {
                            var DocQuery = new DocumentQuery().WhereEquals(JoinTableThisObjectForeignKey, ForiegnKeyValue).Columns("NodeID").Published(false).LatestVersion(true).CombineWithDefaultCulture().CombineWithAnyCulture();
                            if (JoinTableThisObjectForeignKey.ToLower() == "nodealiaspath")
                            {
                                DocQuery.OnCurrentSite();
                            }
                            CMS.DocumentEngine.TreeNode Page = DocQuery.FirstOrDefault();
                            int PrimaryNodeID = RelHelper.GetPrimaryNodeID(Page.NodeID);
                            object NewValue = ForiegnKeyValue;
                            if (PrimaryNodeID != Page.NodeID)
                            {
                                NewValue = new DocumentQuery().WhereEquals("NodeID", PrimaryNodeID).Columns(JoinTableThisObjectForeignKey).Published(false).LatestVersion(true).CombineWithDefaultCulture().CombineWithAnyCulture().FirstOrDefault().GetValue(JoinTableThisObjectForeignKey);
                            }
                            if (cs.Cached)
                            {
                                cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { "nodeid|" + Page.NodeID, "nodeid|" + PrimaryNodeID });
                            }
                            return NewValue;
                        }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "GetPrimaryNodeValue", ForiegnKeyValue, JoinTableThisObjectForeignKey));
                    case "nodeid":
                        int CurrentNodeID = ValidationHelper.GetInteger(ForiegnKeyValue, 0);
                        return RelHelper.GetPrimaryNodeID(CurrentNodeID);
                    default:
                        return ForiegnKeyValue;
                }
            }
            else
            {
                return ForiegnKeyValue;
            }
        }
    }

    /// <summary>
    /// Figure out using the Join Table which categories need to be removed and which added
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form_OnAfterSaveJoinTable(object sender, EventArgs e)
    {
        SetPossibleCategories();
        string CategoryIDs = txtValue.Text.Trim();
        DataClassInfo JoinTableClassInfo = DataClassInfoProvider.GetDataClassInfo(JoinTableName);
        if (JoinTableClassInfo != null)
        {
            List<int> SelectedCategoryIds = CategoryIDs.Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList(); //CategoryInfoProvider.GetCategories(string.Format("CategoryID in ('{0}')", string.Join("','", CategoryIDs.Split('|'))), null);
            List<int> PossibleCategoryIds = PossibleCategories.Select(x => x.CategoryID).ToList();
            List<int> DocumentCategoryIds = new List<int>();

            // Get all the categories the current entity has
            int totalRecords = 0;
            var currentCategoriesDS = ConnectionHelper.ExecuteQuery(JoinTableName + ".selectall", null, string.Format("{0} = '{1}'", JoinTableLeftFieldName, CurrentItemIdentification), null, -1, null, -1, -1, ref totalRecords);
            CategoryInfo catObject = null;
            string FieldSaveColumnName = "";
            // Convert to CategoryID
            switch (FieldSaveModeVal)
            {
                case CategoryFieldSaveType.ID:
                    FieldSaveColumnName = "CategoryID";
                    break;
                case CategoryFieldSaveType.GUID:
                    FieldSaveColumnName = "CategoryGUID";
                    break;
                case CategoryFieldSaveType.CategoryName:
                    FieldSaveColumnName = "CategoryName";
                    break;
            }

            foreach (DataRow dr in currentCategoriesDS.Tables[0].Rows)
            {
                // Convert to CategoryID
                switch (FieldSaveModeVal)
                {
                    case CategoryFieldSaveType.ID:
                        catObject = CategoryInfo.Provider.Get(ValidationHelper.GetInteger(dr[JoinTableRightFieldName], 0));
                        if (catObject != null)
                        {
                            DocumentCategoryIds.Add(catObject.CategoryID);
                        }
                        break;
                    case CategoryFieldSaveType.GUID:
                        var ClassObject = CategoryInfo.Provider.Get().WhereEquals("CategoryGUID", ValidationHelper.GetGuid(dr[JoinTableRightFieldName], new Guid())).FirstOrDefault();
                        if (ClassObject != null)
                        {
                            DocumentCategoryIds.Add(ValidationHelper.GetInteger(ClassObject["CategoryID"], 0));
                        }
                        break;
                    case CategoryFieldSaveType.CategoryName:
                        catObject = CategoryInfo.Provider.Get(ValidationHelper.GetString(dr[JoinTableRightFieldName], ""), SiteContext.CurrentSiteID);
                        if (catObject != null)
                        {
                            DocumentCategoryIds.Add(catObject.CategoryID);
                        }
                        break;
                }
            }

            // Find IDs we need to add and remove.
            List<int> NotSelectedIds = PossibleCategoryIds.Except(SelectedCategoryIds).ToList();
            List<int> DeselectIds = DocumentCategoryIds.Intersect(NotSelectedIds).ToList();
            List<int> SelectIds = SelectedCategoryIds.Except(DocumentCategoryIds).ToList();

            if (DeselectIds.Count > 0)
            {
                foreach (int DeselectId in DeselectIds)
                {
                    if (JoinTableClassInfo.ClassIsCustomTable)
                    {
                        CustomTableItemProvider.GetItems(JoinTableClassInfo.ClassName).WhereEquals(JoinTableLeftFieldName, CurrentItemIdentification)
                                    .WhereEquals(JoinTableRightFieldName, CategoryInfo.Provider.Get(DeselectId).GetValue(FieldSaveColumnName))
                                    .ToList().ForEach(x => ((CustomTableItem)x).Delete());
                    }
                    else
                    {
                        new ObjectQuery(JoinTableClassInfo.ClassName)
                            .WhereEquals(JoinTableLeftFieldName, CurrentItemIdentification)
                            .WhereEquals(JoinTableRightFieldName, CategoryInfo.Provider.Get(DeselectId).GetValue(FieldSaveColumnName))
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
                        SetBaseInfoItemValues(newCustomTableItem, CategoryInfo.Provider.Get(SelectId).GetValue(FieldSaveColumnName), JoinTableClassInfo.ClassName);
                        newCustomTableItem.Insert();
                    }
                    else
                    {
                        // Create a dynamic BaseInfo object of the right type.
                        var JoinTableClassFactory = new InfoObjectFactory(JoinTableClassInfo.ClassName);
                        if (JoinTableClassFactory.Singleton == null)
                        {
                            if (JoinTableClassInfo.ClassName.ToLower() == "cms.treecategory")
                            {
                                AddError("Class does not have TypeInfo and TypeInfoProvider generated.  Must install RelationshipsExtended module before can bind. (see http://www.devtrev.com/Resources)");
                                return;
                            }
                            else
                            {
                                AddError("Class does not have TypeInfo and TypeInfoProvider generated.  Must generate " + JoinTableClassInfo.ClassName + " Code before can bind.");
                                return;
                            }
                        }
                        BaseInfo newJoinObj = ((BaseInfo)JoinTableClassFactory.Singleton);
                        SetBaseInfoItemValues(newJoinObj, CategoryInfo.Provider.Get(SelectId).GetValue(FieldSaveColumnName), JoinTableClassInfo.ClassName);
                        InsertObjectHandler(newJoinObj);
                    }
                }
            }
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
        string TimeStampCol = (!string.IsNullOrWhiteSpace(JoinTableLastModifiedField) ? JoinTableLastModifiedField : DataHelper.GetNotEmpty(newItem.TypeInfo.TimeStampColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        string GUIDCol = (!string.IsNullOrWhiteSpace(JoinTableGUIDField) ? JoinTableGUIDField : DataHelper.GetNotEmpty(newItem.TypeInfo.GUIDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        string CodeNameCol = (!string.IsNullOrWhiteSpace(JoinTableCodeNameField) ? JoinTableCodeNameField : DataHelper.GetNotEmpty(newItem.TypeInfo.CodeNameColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
        string SiteIDCol = (!string.IsNullOrWhiteSpace(JoinTableSiteIDField) ? JoinTableSiteIDField : DataHelper.GetNotEmpty(newItem.TypeInfo.SiteIDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, ""));
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
    private void Form_OnAfterSave(object sender, EventArgs e)
    {
        SetPossibleCategories();
        int DocumentID = ValidationHelper.GetInteger(Form.GetDataValue("DocumentID"), -1);

        // Can only do Document/Category if there is a DocumentID on the current object.
        if (DocumentID > 0)
        {
            string CategoryIDs = txtValue.Text.Trim();
            List<int> SelectedCategoryIds = (string.IsNullOrWhiteSpace(CategoryIDs) ? new List<int>() : CategoryIDs.Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList()); //CategoryInfoProvider.GetCategories(string.Format("CategoryID in ('{0}')", string.Join("','", CategoryIDs.Split('|'))), null);
            List<int> PossibleCategoryIds = PossibleCategories.Select(x => x.CategoryID).ToList();
            List<int> DocumentCategoryIds = new List<int>();
            foreach (var DocCategory in DocumentCategoryInfoProvider.GetDocumentCategories(DocumentID))
            {
                DocumentCategoryIds.Add(DocCategory.CategoryID);
            }

            // Find IDs we need to add and remove.
            List<int> NotSelectedIds = PossibleCategoryIds.Except(SelectedCategoryIds).ToList();
            List<int> DeselectIds = DocumentCategoryIds.Intersect(NotSelectedIds).ToList();
            List<int> SelectIds = SelectedCategoryIds.Except(DocumentCategoryIds).ToList();
            bool CategoriesChanged = false;
            foreach (int DeselectId in DeselectIds)
            {
                DocumentCategoryInfo.Provider.Remove(DocumentID, DeselectId);
                CategoriesChanged = true;
            }
            foreach (int SelectId in SelectIds)
            {
                DocumentCategoryInfo.Provider.Add(DocumentID, SelectId);
                CategoriesChanged = true;
            }

            // Page changes require custom logic for staging
            if (CategoriesChanged && LicenseHelper.CheckFeature(Request.Url.AbsoluteUri, FeatureEnum.Staging))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

                List<ServerInfo> targetServers = ServerInfo.Provider.Get().Where(x => x.ServerSiteID == SiteContext.CurrentSiteID && x.ServerEnabled).ToList();
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
                        SiteName = docObj.NodeSiteName
                    };

                    if (RelHelper.IsStagingEnabled())
                    {
                        // Logs parent task, which will run through the task on insert event and do the same check.
                        var currentNodeUpdateTask = DocumentSynchronizationHelper.LogDocumentChange(settings);
                    }
                }
            }

        }
    }

    /// <summary>
    /// Rebuilds the "PossibleCategories" list as the list is reset on on load and not re-built if postback.
    /// </summary>
    private void SetPossibleCategories()
    {
        PossibleCategories = new List<CategoryInfo>();
        int RootCategoryID = 0;
        var rootCategory = CategoryInfo.Provider.Get(RootCategory, SiteContext.CurrentSiteID);
        if (rootCategory == null && int.TryParse(RootCategory, out RootCategoryID))
        {
            rootCategory = CategoryInfo.Provider.Get(RootCategoryID);
        }

        // Grab allowable Categories if user sets a WHERE
        string TempWhere = "CategoryNamePath like " + (rootCategory == null ? "'/%'" : "'" + rootCategory.CategoryNamePath + "/%'") + (string.IsNullOrWhiteSpace(WhereFilter) ? "" : " and " + WhereFilter);
        DefaultSortOrder = (string.IsNullOrWhiteSpace(OrderBy) ? (DisplayMode == DisplayType.List ? "CategoryDisplayName" : "CategoryLevel, CategoryOrder") : OrderBy);
        var AllowableCategoryList = CategoryInfoProvider.GetCategories(TempWhere, DefaultSortOrder, -1, null, SiteContext.CurrentSiteID);

        if (DisplayMode == DisplayType.Tree)
        {
            if (rootCategory != null && (rootCategory.Children.Count == 0 || !OnlyLeafSelectable))
            {
                PossibleCategories.Add(rootCategory);
            }
            SetPossibleCategoriesTreeRecursive(rootCategory);
        }
        else if (DisplayMode == DisplayType.List)
        {
            // Just loop through allowable Categories and add to list.
            foreach (CategoryInfo CategoryItem in AllowableCategoryList)
            {
                PossibleCategories.Add(CategoryItem);
            }
        }
    }

    /// <summary>
    /// Helper to Rebuild the Possible Categories recursively for Tree Nodes.
    /// </summary>
    /// <param name="ParentCategoryNode"></param>
    private void SetPossibleCategoriesTreeRecursive(CategoryInfo ParentCategoryNode)
    {
        // Grab all valid child categories
        var ChildCategories = (ParentCategoryNode == null ? CategoryInfoProvider.GetCategories(AllowableCategoryIDWhere, DefaultSortOrder, -1, null, SiteContext.CurrentSiteID).WhereEquals("CategoryLevel", 0) : CategoryInfoProvider.GetChildCategories(ParentCategoryNode.CategoryID, AllowableCategoryIDWhere, DefaultSortOrder, -1, null, SiteContext.CurrentSiteID));
        foreach (CategoryInfo childCategory in ChildCategories)
        {
            // If either all items selectable, or if only leaf selectable and this is a leaf node, save to the possible categories list.
            if (childCategory.Children.Count == 0 || !OnlyLeafSelectable)
            {
                PossibleCategories.Add(childCategory);
            }
            SetPossibleCategoriesTreeRecursive(childCategory);
        }
    }

    /// <summary>
    /// Helper method to split a string and ensure they are escaped.
    /// </summary>
    /// <param name="Values">The Values string</param>
    /// <param name="Seperators">The Delimeter, default is bar |</param>
    /// <returns></returns>
    private string[] SplitAndSecure(string Values, string Seperators = null)
    {
        if (string.IsNullOrWhiteSpace(Seperators))
        {
            Seperators = SeparatorCharacter;
        }

        if (!string.IsNullOrWhiteSpace(Values))
        {
            return Values.Split(Seperators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => SqlHelper.EscapeQuotes(x)).ToArray();
        }
        else
        {
            return new string[] { "" };
        }
    }

    private int[] StringArrayToIntArray(string[] Values)
    {
        List<int> IntArray = new List<int>();
        foreach (string value in Values)
        {
            IntArray.Add(ValidationHelper.GetInteger(value, -1));
        }
        return IntArray.Distinct().ToArray();
    }

    private Guid[] StringArrayToGuidArray(string[] Values)
    {
        List<Guid> GuidArray = new List<Guid>();
        foreach (string value in Values)
        {
            GuidArray.Add(ValidationHelper.GetGuid(value, new Guid()));
        }
        return GuidArray.Distinct().ToArray();
    }

    #endregion

    #region "Other Hooks"

    /// <summary>
    /// Checks the value against the Minimum and Maximum categories
    /// </summary>
    /// <returns>If the entry is valid or not.</returns>
    public override bool IsValid()
    {
        bool MinMet = true;
        bool MaxMet = true;
        if (MinimumCategories > -1)
        {
            if (txtValue.Text.Trim().Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length < MinimumCategories)
            {
                MinMet = false;
            }
        }
        if (MaximumCategories > -1)
        {
            if (txtValue.Text.Trim().Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > MaximumCategories)
            {
                MaxMet = false;
            }
        }

        if (!MinMet || !MaxMet)
        {
            ValidationError = string.Format("{0} {1} {2}",
                (!MinMet ? "Minimum " + MinimumCategories + " selections allowed" : ""),
                (!MinMet && !MaxMet ? " and " : ""),
                (!MaxMet ? "Maximum " + MaximumCategories + " selections allowed" : ""));
        }
        return (MinMet && MaxMet);
    }

    /// <summary>
    /// Hook for when the value of the form changes, to reload the tree so it properly has everything set.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtValue_TextChanged(object sender, EventArgs e)
    {
        tvwCategoryTree.Nodes.Clear();
        setCategoryTree();
    }

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

}


