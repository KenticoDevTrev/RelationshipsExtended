using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.UIControls;


public partial class CMSModules_RelationshipExtended_UI_UniSelector_Controls_SelectionDialog : CMSUserControl, IObjectTypeDriven, ICallbackEventHandler
{
    #region "Variables"

    private SelectionModeEnum selectionMode = SelectionModeEnum.SingleButton;

    private string objectType;
    private string returnColumnName;
    private string displayNameFormat;
    private char valuesSeparator = ';';

    private string filterControl;

    private bool useDefaultNameFilter = true;

    private string whereCondition;
    private string orderBy;
    private string siteWhereCondition;

    private int itemsPerPage = 10;

    private GeneralizedInfo iObjectType;

    private string txtClientId;
    private string hdnClientId;
    private string hdnDrpId;
    private string hashId;
    private string callbackValues;

    private CMSAbstractBaseFilterControl searchControl;

    private string emptyReplacement = "&nbsp;";
    private string parentClientId;
    private string dialogGridName = "~/CMSAdminControls/UI/UniSelector/DialogItemList.xml";
    private string additionalColumns;
    private string additionalSearchColumns = String.Empty;
    private string callbackMethod;
    private string disabledItems;
    private string filterMode;

    private bool allowEditTextBox;
    private bool fireOnChanged;
    private bool mHasDependingFields;
    private bool mLocalizeItems = true;
    private bool allRowsChecked = true;

    private string mGlobalObjectSuffix;

    private Hashtable parameters;

    private string mSearchColumns;
    private string mZeroRowsText;
    private string mFilteredZeroRowsText;

    // Custom properties
    private string toolTipFormat;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets the trimmed search text.
    /// </summary>
    private string TrimmedSearchText
    {
        get
        {
            return txtSearch.Text.Trim();
        }
    }


    /// <summary>
    /// Indicates whether localized filtering is allowed.
    /// </summary>
    private bool AllowLocalizedFiltering 
    { 
        get;
        set; 
    }


    /// <summary>
    /// Indicates whether display name column was selected as search column.
    /// </summary>
    private bool DisplayNameSelectedAsSearchColumn
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["DisplayNameSelected"], false);
        }
        set
        {
            ViewState["DisplayNameSelected"] = value;
        }
    }


    /// <summary>
    /// Indicates whether localized filtering can be used for current selection dialog and is enabled.
    /// </summary>
    private bool UseLocalizedFiltering 
    { 
        get
        {
            return ValidationHelper.GetBoolean(ViewState["UseLocalizedFiltering"], false) 
                && DisplayNameSelectedAsSearchColumn
                && AllowLocalizedFiltering 
                && !String.IsNullOrEmpty(TrimmedSearchText);
        }
        set
        {
            ViewState["UseLocalizedFiltering"] = value;
        }
    }


    /// <summary>
    /// Indicates whether to remove multiple commas (can happen when DisplayNameFormat is like {%column1%}, {%column2%}, {column3} and column2 is empty.
    /// </summary>
    public bool RemoveMultipleCommas
    {
        get;
        set;
    }


    /// <summary>
    /// If true, the selector uses the type condition to get the data
    /// </summary>
    public bool UseTypeCondition
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or set the suffix which is added to global objects if AddGlobalObjectSuffix is true. Default is "(global)".
    /// </summary>
    public string GlobalObjectSuffix
    {
        get
        {
            if (string.IsNullOrEmpty(mGlobalObjectSuffix))
            {
                mGlobalObjectSuffix = GetString("general.global");
            }
            return mGlobalObjectSuffix;
        }
        set
        {
            mGlobalObjectSuffix = value;
        }
    }


    /// <summary>
    /// Indicates whether global objects have suffix "(global)" in the grid.
    /// </summary>
    public bool AddGlobalObjectSuffix
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates whether global object name should have prefix '.'
    /// </summary>
    public bool AddGlobalObjectNamePrefix
    {
        get;
        set;
    }


    /// <summary>
    /// Specifies whether the selection dialog should resolve localization macros.
    /// </summary>
    public bool LocalizeItems
    {
        get
        {
            return mLocalizeItems;
        }
        set
        {
            mLocalizeItems = value;
        }
    }


    /// <summary>
    /// Current page index.
    /// </summary>
    public int PageIndex
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["PageIndex"], 0);
        }
        set
        {
            ViewState["PageIndex"] = value;
        }
    }


    /// <summary>
    /// Item prefix.
    /// </summary>
    public string ItemPrefix
    {
        get
        {
            return ValidationHelper.GetString(ViewState["ItemPrefix"], string.Empty);
        }
        set
        {
            ViewState["ItemPrefix"] = value;
        }
    }


    /// <summary>
    /// Contains current where condition used for filtering.
    /// </summary>
    public string FilterWhere
    {
        get
        {
            return ValidationHelper.GetString(ViewState["FilterWhere"], string.Empty);
        }
        set
        {
            ViewState["FilterWhere"] = value;
        }
    }


    /// <summary>
    /// Control's unigrid.
    /// </summary>
    public UniGrid UniGrid
    {
        get
        {
            return uniGrid;
        }
    }


    /// <summary>
    /// Gets or sets current object type.
    /// </summary>
    public string ObjectType
    {
        get
        {
            return ValidationHelper.GetString(parameters["ObjectType"], String.Empty);
        }
        set
        {
            parameters["ObjectType"] = value;
        }
    }

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        LoadParameters();
        if (!RequestHelper.IsCallback())
        {
            LoadCustomFilter();
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (RequestHelper.IsCallback())
        {
            return;
        }

        uniGrid.IsLiveSite = IsLiveSite;
        if (!RequestHelper.IsPostBack())
        {
            uniGrid.Pager.DefaultPageSize = 10;
        }

        // Load data into the unigrid
        LoadControls();

        // Get control IDs from parent window           
        txtClientId = QueryHelper.GetString("txtElem", string.Empty);
        hdnClientId = QueryHelper.GetString("hidElem", string.Empty);
        hdnDrpId = QueryHelper.GetString("selectElem", string.Empty);
        hashId = QueryHelper.GetString("hashElem", string.Empty);
        parentClientId = QueryHelper.GetControlClientId("clientId", string.Empty);
    }


    /// <summary>
    /// Change header title for multiple selection.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        if (!RequestHelper.IsPostBack())
        {
            ChangeSearchCondition();
        }

        // Load the grid data
        ReloadGrid();

        // Register client scripts
        RegisterBaseClientScripts();

        if (uniGrid.GridView.HeaderRow != null)
        {
            switch (selectionMode)
            {
                // Multiple selection
                case SelectionModeEnum.Multiple:
                case SelectionModeEnum.MultipleTextBox:
                case SelectionModeEnum.MultipleButton:
                    {
                        // Add checkbox for selection of all items on the current page
                        CMSCheckBox chkAll = new CMSCheckBox
                        {
                            ID = "chkAll",
                            ToolTip = GetString("UniSelector.CheckAll"),
                            Checked = allRowsChecked
                        };
                        chkAll.Attributes.Add("onclick", "SelectAllItems(this)");

                        uniGrid.GridView.HeaderRow.Cells[0].Controls.Clear();
                        uniGrid.GridView.HeaderRow.Cells[0].Controls.Add(chkAll);
                        uniGrid.GridView.Columns[0].ItemStyle.CssClass = "unigrid-selection";

                        uniGrid.GridView.HeaderRow.Cells[1].Text = GetString("general.itemname");

                        // Register client scripts for multiple modes
                        RegisterMultipleModeClientScripts(chkAll.ClientID);
                    }
                    break;

                // Single selection
                default:
                    {
                        uniGrid.GridView.Columns[0].Visible = false;
                        uniGrid.GridView.HeaderRow.Cells[1].Text = GetString("general.itemname");
                    }
                    break;
            }
        }

        base.OnPreRender(e);
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Unigrid external data bound handler.
    /// </summary>
    protected object uniGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerInvariant())
        {
            case "yesno":
                return UniGridFunctions.ColoredSpanYesNo(parameter);

            case "select":
                {
                    var ti = iObjectType.TypeInfo;

                    DataRowView drv = (DataRowView)parameter;

                    // Get item ID
                    string itemID = drv[returnColumnName].ToString();

                    // Add global object name prefix if required
                    if (AddGlobalObjectNamePrefix && HasSiteIdColumn(ti) && (DataHelper.GetIntValue(drv.Row, ti.SiteIDColumn) == 0))
                    {
                        itemID = "." + itemID;
                    }

                    // Add checkbox for multiple selection
                    switch (selectionMode)
                    {
                        case SelectionModeEnum.Multiple:
                        case SelectionModeEnum.MultipleTextBox:
                        case SelectionModeEnum.MultipleButton:
                        {
                            var itemWithSeparators = string.Format("{0}{1}{0}", valuesSeparator, itemID);

                            string checkBox = string.Format("<span class=\"checkbox\"><input id=\"chk{0}\" type=\"checkbox\" onclick=\"ProcessItem(this,false); UpdateCheckboxAllElement();\" class=\"chckbox\" ", itemID);
                            if (hidItem.Value.IndexOf(itemWithSeparators, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                checkBox += "checked=\"checked\" ";
                            }
                            else
                            {
                                allRowsChecked = false;
                            }

                            if (disabledItems.Contains(itemWithSeparators))
                            {
                                checkBox += "disabled=\"disabled\" ";
                            }

                            checkBox += string.Format("/><label for=\"chk{0}\">&nbsp;</label></span>", itemID);

                            return checkBox;
                        }
                    }
                }
                break;

            case "itemname":
                {
                    DataRowView drv = (DataRowView)parameter;

                    // Get item ID
                    string itemID = drv[returnColumnName].ToString();

                    // Get item name
                    string itemName;

                    // Special formatted user name
                    if (displayNameFormat == UniSelector.USER_DISPLAY_FORMAT)
                    {
                        string userName = DataHelper.GetStringValue(drv.Row, "UserName");
                        string fullName = DataHelper.GetStringValue(drv.Row, "FullName");

                        itemName = Functions.GetFormattedUserName(userName, fullName, IsLiveSite);
                    }
                    else if (displayNameFormat == null)
                    {
                        itemName = drv[iObjectType.DisplayNameColumn].ToString();
                    }
                    else
                    {
                        MacroResolver resolver = MacroResolver.GetInstance();
                        foreach (DataColumn item in drv.Row.Table.Columns)
                        {
                            resolver.SetNamedSourceData(item.ColumnName, drv.Row[item.ColumnName]);
                        }
                        itemName = resolver.ResolveMacros(displayNameFormat);
                    }

                    if (RemoveMultipleCommas)
                    {
                        itemName = TextHelper.RemoveMultipleCommas(itemName);
                    }

                    // Add the prefixes
                    itemName = ItemPrefix + itemName;
                    itemID = ItemPrefix + itemID;

                    var ti = iObjectType.TypeInfo;

                    // Add global object name prefix if required
                    if (AddGlobalObjectNamePrefix && HasSiteIdColumn(ti) && (DataHelper.GetIntValue(drv.Row, ti.SiteIDColumn) == 0))
                    {
                        itemID = "." + itemID;
                    }

                    if (String.IsNullOrEmpty(itemName))
                    {
                        itemName = emptyReplacement;
                    }

                    if (AddGlobalObjectSuffix && HasSiteIdColumn(ti))
                    {
                        itemName += (DataHelper.GetIntValue(drv.Row, ti.SiteIDColumn) > 0 ? string.Empty : " " + GlobalObjectSuffix);
                    }

                    // Link action
                    string onclick = null;
                    bool disabled = disabledItems.Contains(";" + itemID + ";");
                    if (!disabled)
                    {
                        string safeItemID = GetSafe(itemID);
                        string itemHash = ValidationHelper.GetHashString(itemID);
                        switch (selectionMode)
                        {
                            case SelectionModeEnum.Multiple:
                            case SelectionModeEnum.MultipleTextBox:
                            case SelectionModeEnum.MultipleButton:
                                onclick = string.Format("ProcessItem(document.getElementById('chk{0}'),true); UpdateCheckboxAllElement(); return false;", ScriptHelper.GetString(itemID).Trim('\''));
                                break;

                            case SelectionModeEnum.SingleButton:
                                onclick = string.Format("SelectItems({0},'{1}'); return false;", safeItemID, itemHash);
                                break;

                            case SelectionModeEnum.SingleTextBox:
                                if (allowEditTextBox)
                                {
                                    if (!mHasDependingFields)
                                    {
                                        onclick = string.Format("SelectItems({0},{0},{1},{2},{3},'{4}'); return false;", safeItemID, ScriptHelper.GetString(hdnClientId), ScriptHelper.GetString(txtClientId), ScriptHelper.GetString(hashId), itemHash);
                                    }
                                    else
                                    {
                                        onclick = string.Format("SelectItemsReload({0},{0},{1},{2},{3},{4},'{5}'); return false;", safeItemID, ScriptHelper.GetString(hdnClientId), ScriptHelper.GetString(txtClientId), ScriptHelper.GetString(hdnDrpId), ScriptHelper.GetString(hashId), itemHash);
                                    }
                                }
                                else
                                {
                                    if (!mHasDependingFields)
                                    {
                                        onclick = string.Format("SelectItems({0},{1},{2},{3},{4},'{5}'); return false;", safeItemID, GetSafe(itemName), ScriptHelper.GetString(hdnClientId), ScriptHelper.GetString(txtClientId), ScriptHelper.GetString(hashId), itemHash);
                                    }
                                    else
                                    {
                                        onclick = string.Format("SelectItemsReload({0},{1},{2},{3},{4},{5},'{6}'); return false;", safeItemID, GetSafe(itemName), ScriptHelper.GetString(hdnClientId), ScriptHelper.GetString(txtClientId), ScriptHelper.GetString(hdnDrpId), ScriptHelper.GetString(hashId), itemHash);
                                    }
                                }
                                break;

                            default:
                                onclick = string.Format("SelectItemsReload({0},{1},{2},{3},{4},{5},'{6}'); return false;", safeItemID, GetSafe(itemName), ScriptHelper.GetString(hdnClientId), ScriptHelper.GetString(txtClientId), ScriptHelper.GetString(hdnDrpId), ScriptHelper.GetString(hashId), itemHash);
                                break;
                        }

                        onclick = "onclick=\"" + onclick + "\" ";
                    }

                    if (LocalizeItems)
                    {
                        itemName = ResHelper.LocalizeString(itemName);
                    }

                    // Custom Tooltip
                    string tooltip = "";
                    if (!string.IsNullOrWhiteSpace(toolTipFormat))
                    {

                        MacroResolver resolver = MacroResolver.GetInstance();
                        foreach (DataColumn item in drv.Row.Table.Columns)
                        {
                            resolver.SetNamedSourceData(item.ColumnName, drv.Row[item.ColumnName]);
                        }
                        tooltip = resolver.ResolveMacros(toolTipFormat);
                    }
                    if(!string.IsNullOrWhiteSpace(tooltip))
                    { 
                        tooltip = "title=\"" + CMS.Helpers.HTMLHelper.EncodeForHtmlAttribute(tooltip) + "\" ";
                    }

                    return "<div " + (!disabled ? "class=\"SelectableItem\" " : null) + onclick + tooltip + ">" + HTMLHelper.HTMLEncode(TextHelper.LimitLength(itemName, 100)) + "</div>";
                }
        }

        return null;
    }


    protected void uniGrid_OnPageChanged(object sender, EventArgs e)
    {
        // Load the grid data
        ReloadGrid();
    }


    /// <summary>
    /// Button search event handler.
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ChangeSearchCondition();
    }


    /// <summary>
    /// On search condition changed.
    /// </summary>
    private void searchControl_OnFilterChanged()
    {
        ChangeSearchCondition();
    }


    protected void lnkSelectAll_Click(object sender, EventArgs e)
    {
        UpdateHiddenFields(true);
    }


    protected void lnkDeselectAll_Click(object sender, EventArgs e)
    {
        UpdateHiddenFields(false);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Overridden to get the parameters.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    public override object GetValue(string propertyName)
    {
        if ((parameters != null) && parameters.Contains(propertyName))
        {
            return parameters[propertyName];
        }

        return base.GetValue(propertyName);
    }


    /// <summary>
    /// Overridden set value to collect parameters.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="value">Value</param>
    public override bool SetValue(string propertyName, object value)
    {
        // Handle special properties
        if (propertyName.Equals("itemprefix", StringComparison.OrdinalIgnoreCase))
        {
            ItemPrefix = ValidationHelper.GetString(value, string.Empty);
        }

        base.SetValue(propertyName, value);

        // Set parameters for dialog
        parameters[propertyName] = value;

        return true;
    }


    /// <summary>
    /// Loads dynamically custom filter if is defined.
    /// </summary>
    private void LoadCustomFilter()
    {
        // Use user filter
        if (!String.IsNullOrEmpty(filterControl))
        {
            pnlFilter.Controls.Clear();

            searchControl = (CMSAbstractBaseFilterControl)LoadUserControl(filterControl);
            if (searchControl != null)
            {
                searchControl.Parameters = parameters;
                searchControl.FilteredControl = this;
                searchControl.OnFilterChanged += searchControl_OnFilterChanged;
                searchControl.ID = "filterElem";
                searchControl.SelectedValue = hidItem.Value.Replace(valuesSeparator.ToString(), string.Empty);
                searchControl.FilterMode = filterMode;

                pnlFilter.Controls.Add(searchControl);
                pnlFilter.Visible = true;

                // Get init filter where condition
                FilterWhere = SqlHelper.AddWhereCondition(string.Empty, searchControl.WhereCondition);

                // When both filters are rendered, mark the first as followed by another
                if (useDefaultNameFilter)
                {
                    pnlFilter.CssClass += " header-panel-not-last";
                }
            }
        }
    }


    /// <summary>
    /// Loads control parameters.
    /// </summary>
    private void LoadParameters()
    {
        string identifier = QueryHelper.GetString("params", null);

        parameters = (Hashtable)WindowHelper.GetItem(identifier);
        if (parameters != null)
        {
            ResourcePrefix = ValidationHelper.GetString(parameters["ResourcePrefix"], null);

            // Load values from session
            selectionMode = (SelectionModeEnum)parameters["SelectionMode"];
            objectType = ValidationHelper.GetString(parameters["ObjectType"], null);
            returnColumnName = ValidationHelper.GetString(parameters["ReturnColumnName"], null);
            valuesSeparator = ValidationHelper.GetValue(parameters["ValuesSeparator"], ';');
            filterControl = ValidationHelper.GetString(parameters["FilterControl"], null);
            useDefaultNameFilter = ValidationHelper.GetBoolean(parameters["UseDefaultNameFilter"], true);
            whereCondition = ValidationHelper.GetString(parameters["WhereCondition"], null);
            orderBy = ValidationHelper.GetString(parameters["OrderBy"], null);
            itemsPerPage = ValidationHelper.GetInteger(parameters["ItemsPerPage"], 10);
            emptyReplacement = ValidationHelper.GetString(parameters["EmptyReplacement"], "&nbsp;");
            dialogGridName = ValidationHelper.GetString(parameters["DialogGridName"], dialogGridName);
            additionalColumns = ValidationHelper.GetString(parameters["AdditionalColumns"], null);
            callbackMethod = ValidationHelper.GetString(parameters["CallbackMethod"], null);
            allowEditTextBox = ValidationHelper.GetBoolean(parameters["AllowEditTextBox"], false);
            fireOnChanged = ValidationHelper.GetBoolean(parameters["FireOnChanged"], false);
            disabledItems = ";" + ValidationHelper.GetString(parameters["DisabledItems"], String.Empty) + ";";
            GlobalObjectSuffix = ValidationHelper.GetString(parameters["GlobalObjectSuffix"], string.Empty);
            AddGlobalObjectSuffix = ValidationHelper.GetBoolean(parameters["AddGlobalObjectSuffix"], false);
            AddGlobalObjectNamePrefix = ValidationHelper.GetBoolean(parameters["AddGlobalObjectNamePrefix"], false);
            RemoveMultipleCommas = ValidationHelper.GetBoolean(parameters["RemoveMultipleCommas"], false);
            filterMode = ValidationHelper.GetString(parameters["FilterMode"], null);
            displayNameFormat = ValidationHelper.GetString(parameters["DisplayNameFormat"], null);
            additionalSearchColumns = ValidationHelper.GetString(parameters["AdditionalSearchColumns"], String.Empty);
            siteWhereCondition = ValidationHelper.GetString(parameters["SiteWhereCondition"], null);
            UseTypeCondition = ValidationHelper.GetBoolean(parameters["UseTypeCondition"], true);
            AllowLocalizedFiltering = ValidationHelper.GetBoolean(parameters["AllowLocalizedFiltering"], true);
            mZeroRowsText = ValidationHelper.GetString(parameters["ZeroRowsText"], string.Empty);
            mFilteredZeroRowsText = ValidationHelper.GetString(parameters["FilteredZeroRowsText"], string.Empty);
            mHasDependingFields = ValidationHelper.GetBoolean(parameters["HasDependingFields"], false);

            // Custom parameter loaded from session as i can't seem to append my data to the HashTable
            toolTipFormat = ValidationHelper.GetString(SessionHelper.GetValue("ToolTipFormat_" + identifier), "");

            // Set item prefix if it was passed by UniSelector's AdditionalUrlParameters
            var itemPrefix = QueryHelper.GetString("ItemPrefix", null);
            if (!String.IsNullOrEmpty(itemPrefix))
            {
                ItemPrefix = itemPrefix;
            }

            // Init object
            if (!string.IsNullOrEmpty(objectType))
            {
                iObjectType = ModuleManager.GetReadOnlyObject(objectType);
                if (iObjectType == null)
                {
                    throw new Exception("[UniSelector.SelectionDialog]: Object type '" + objectType + "' not found.");
                }

                if (returnColumnName == null)
                {
                    returnColumnName = iObjectType.TypeInfo.IDColumn;
                }
            }

            // Pre-select unigrid values passed from parent window
            if (!RequestHelper.IsPostBack())
            {
                string values = (string)parameters["Values"];
                if (!String.IsNullOrEmpty(values))
                {
                    hidItem.Value = values;
                    parameters["Values"] = null;
                }
            }
        }
    }


    /// <summary>
    /// Loads variables and objects.
    /// </summary>
    private void LoadControls()
    {
        mSearchColumns = GetSearchColumns();

        // Display default name filter only if search columns are specified
        if (useDefaultNameFilter && (!String.IsNullOrEmpty(mSearchColumns) || !String.IsNullOrEmpty(additionalSearchColumns) || (displayNameFormat == UniSelector.USER_DISPLAY_FORMAT)))
        {
            pnlSearch.Visible = true;

            if (!URLHelper.IsPostback())
            {
                ScriptHelper.RegisterStartupScript(this, typeof(string), "Focus", ScriptHelper.GetScript("try{document.getElementById('" + txtSearch.ClientID + "').focus();}catch(err){}"));
            }
        }

        if (!URLHelper.IsPostback())
        {
            uniGrid.Pager.DefaultPageSize = itemsPerPage;
        }

        uniGrid.GridName = dialogGridName;
        uniGrid.GridView.EnableViewState = false;

        // Show the OK button if needed
        switch (selectionMode)
        {
            case SelectionModeEnum.Multiple:
            case SelectionModeEnum.MultipleTextBox:
            case SelectionModeEnum.MultipleButton:
                {
                    pnlAll.Visible = true;
                }
                break;
        }
    }


    /// <summary>
    /// Returns dataset for specified GeneralizedInfo.
    /// </summary>
    private DataSet GetData(bool applyFilter = true)
    {
        int totalRecords = 0;
        return GetData(0, 0, ref totalRecords, true, applyFilter);
    }


    /// <summary>
    /// Returns dataset for specified GeneralizedInfo.
    /// </summary>
    private DataSet GetData(int offset, int maxRecords, ref int totalRecords, bool selection, bool applyFilter = true)
    {
        // If object type is set
        if (iObjectType != null)
        {
            // Init columns
            string columns = null;
            DataSet ds = null;

            if (!selection)
            {
                if (displayNameFormat == UniSelector.USER_DISPLAY_FORMAT)
                {
                    // Ensure columns which are needed for USER_DISPLAY_FORMAT
                    columns = "UserName, FullName";
                }
                // Because the Display name may be from columns we need to 'hack' to pull in, do not include these in its processing, they should be set in the additional columns field.
                /*
                else if (displayNameFormat != null)
                {
                    columns = DataHelper.GetNotEmpty(MacroProcessor.GetMacros(displayNameFormat, true), iObjectType.DisplayNameColumn).Replace(";", ", ");
                }*/
                else
                {
                    columns = iObjectType.DisplayNameColumn;
                }
            }

            // Add return column name
            columns = SqlHelper.MergeColumns(columns, returnColumnName);

            // Add additional columns
            columns = SqlHelper.MergeColumns(columns, additionalColumns);

            // Ensure display name column for query within localized filtering (SelectAll/DeselectAll calls the query with ID column only)
            if (UseLocalizedFiltering)
            {
                columns = SqlHelper.MergeColumns(columns, iObjectType.TypeInfo.DisplayNameColumn);
            }

            var ti = iObjectType.TypeInfo;

            // Get SiteID column if needed
            if (AddGlobalObjectSuffix && HasSiteIdColumn(ti))
            {
                columns = SqlHelper.MergeColumns(columns, ti.SiteIDColumn);
            }

            string where = whereCondition;
            if (applyFilter)
            {
                where = SqlHelper.AddWhereCondition(where, FilterWhere);
            }
            if (!String.IsNullOrEmpty(uniGrid.WhereClause))
            {
                where = SqlHelper.AddWhereCondition(where, uniGrid.WhereClause);
            }

            // Apply site restrictions
            if (!string.IsNullOrEmpty(siteWhereCondition))
            {
                where = SqlHelper.AddWhereCondition(where, siteWhereCondition);
            }

            string order = String.Empty;

            // Order by
            if (String.IsNullOrEmpty(orderBy))
            {
                order += iObjectType.DisplayNameColumn;
            }
            else
            {
                order += orderBy;
            }

            try
            {
                // Get the data query
                var q = iObjectType.GetDataQuery(
                    UseTypeCondition,
                    s => s
                        .Where(where)
                        .OrderBy(order)
                        .Columns(columns),
                    true
                );

                q.IncludeBinaryData = false;

                if (UseLocalizedFiltering)
                {
                    if (!DataHelper.DataSourceIsEmpty(q.Result))
                    {
                        var displayNameColumn = iObjectType.DisplayNameColumn;

                        ds = q.Result;
                        LocalizeAndFilterDataSet(ds, displayNameColumn, TrimmedSearchText);
                        SortDataSetTable(ds, displayNameColumn);

                        totalRecords = ds.Tables[0].Rows.Count;
                        return ds;
                    }

                    totalRecords = 0;
                    return null;
                }

                q.Offset = offset;
                q.MaxRecords = maxRecords;

                ds = q.Result;
                totalRecords = q.TotalRecords;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("UniSelector", "GETDATA", ex);

                uniGrid.ShowError(ex.Message);
            }

            return ds;
        }

        totalRecords = 0;
        return null;
    }


    /// <summary>
    /// Updates the <paramref name="dataSet"/> and adds global object name prefixes.
    /// </summary>
    private void AddGlobalObjectNamePrefixes(DataSet dataSet)
    {
        var typeInfo = iObjectType.TypeInfo;

        if (AddGlobalObjectNamePrefix && HasSiteIdColumn(typeInfo))
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                if (row[typeInfo.SiteIDColumn] == DBNull.Value)
                {
                    row[returnColumnName] = "." + row[returnColumnName];
                }
            }
        }
    }


    /// <summary>
    /// Returns true if given <paramref name="typeInfo"/> has SiteID column name set
    /// </summary>
    private bool HasSiteIdColumn(ObjectTypeInfo typeInfo)
    {
        return !String.IsNullOrEmpty(typeInfo.SiteIDColumn) && !String.Equals(typeInfo.SiteIDColumn, ObjectTypeInfo.COLUMN_NAME_UNKNOWN, StringComparison.OrdinalIgnoreCase);
    }


    /// <summary>
    /// Sorts the dataset table values
    /// </summary>
    private void SortDataSetTable(DataSet ds, string sortColumnName)
    {
        ds.Tables[0].DefaultView.Sort = sortColumnName;
        var sortedTable = ds.Tables[0].DefaultView.ToTable();
        ds.Tables.Clear();
        ds.Tables.Add(sortedTable);
    }


    /// <summary>
    /// Localizes the specified column in a dataset table and removes rows that does not contain the required filter value
    /// </summary>
    private void LocalizeAndFilterDataSet(DataSet ds, string filterColum, string filterValue)
    {
        var rows = ds.Tables[0].Rows;
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            var localizedColumnValue = ResHelper.LocalizeString(Convert.ToString(rows[i][filterColum]));
            if (localizedColumnValue.IndexOf(filterValue, StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                rows[i].Delete();
            }
            else
            {
                rows[i][filterColum] = localizedColumnValue;
            }
        }

        ds.AcceptChanges();
    }


    /// <summary>
    /// Changes ViewState with search condition for UniGrid.
    /// </summary>
    private void ChangeSearchCondition()
    {
        if (iObjectType != null)
        {
            string where = null;

            // Get default filter where
            if ((useDefaultNameFilter) && (!String.IsNullOrEmpty(TrimmedSearchText)))
            {
                // Avoid SQL injection
                string searchText = SqlHelper.EscapeQuotes(TrimmedSearchText);
                
                // Escape like patterns
                searchText = SqlHelper.EscapeLikeText(searchText);
                
                if (displayNameFormat == UniSelector.USER_DISPLAY_FORMAT)
                {
                    // Ensure search in columns which are needed for USER_DISPLAY_FORMAT
                    where = String.Format("UserName LIKE N'%{0}%' OR FullName LIKE N'%{0}%'", searchText);
                }

                // Try enabled localized search
                if (AllowLocalizedFiltering & DisplayNameSelectedAsSearchColumn)
                {
                    UseLocalizedFiltering = true;
                }
                else if (!String.IsNullOrEmpty(mSearchColumns))
                {
                    // Combine main search columns with additional 
                    additionalSearchColumns = additionalSearchColumns.TrimEnd(';') + ";" + mSearchColumns;
                }

                // Append additional columns that should be used for search
                if (!string.IsNullOrEmpty(additionalSearchColumns))
                {
                    string[] columns = additionalSearchColumns.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string column in columns)
                    {
                        where = SqlHelper.AddWhereCondition(where, string.Format("{0} LIKE N'%{1}%'", column.Trim(), searchText), "OR");
                    }
                }
            }

            // Add custom search filter where
            if (searchControl != null)
            {
                where = SqlHelper.AddWhereCondition(where, searchControl.WhereCondition);
            }

            // Save where condition to the view state
            FilterWhere = where;
        }
    }


    /// <summary>
    /// Returns main search columns to filter.
    /// </summary>
    private string GetSearchColumns()
    {
        var ti = iObjectType.TypeInfo;

        if ((ti.DisplayNameColumn != ObjectTypeInfo.COLUMN_NAME_UNKNOWN))
        {
            DisplayNameSelectedAsSearchColumn = true;
            
            // Get column for display name 
            return ti.DisplayNameColumn;
        }
        
        if ((ti.CodeNameColumn != ObjectTypeInfo.COLUMN_NAME_UNKNOWN))
        {
            // Get column for code name
            return ti.CodeNameColumn;
        }
        
        if (!String.IsNullOrEmpty(displayNameFormat))
        {
            // Get columns from display name format if empty don't filter anything
            return MacroProcessor.GetMacros(displayNameFormat);
        }

        return String.Empty;
    }


    /// <summary>
    /// Reloads the grid with given page index.
    /// </summary>
    protected void ReloadGrid()
    {
        int totalRecords = 0;
        int offset = uniGrid.Pager.CurrentPageSize * (uniGrid.Pager.CurrentPage - 1);

        // Reload data set with new page index
        if (uniGrid.DataSource == null)
        {
            uniGrid.DataSource = GetData(offset, uniGrid.Pager.CurrentPageSize, ref totalRecords, false);
            uniGrid.PagerForceNumberOfResults = totalRecords;
        }

        if (string.IsNullOrEmpty(FilterWhere))
        {
            uniGrid.ZeroRowsText = string.IsNullOrEmpty(mZeroRowsText) ? GetString("general.nodatafound") : mZeroRowsText;
        }
        else
        {
            uniGrid.ZeroRowsText = string.IsNullOrEmpty(mFilteredZeroRowsText) ? GetString("general.noitemsfound") : mFilteredZeroRowsText;
        }

        uniGrid.ReloadData();
    }


    /// <summary>
    /// Returns string safe for inserting to javascript as parameter.
    /// </summary>
    /// <param name="param">Parameter</param>    
    private string GetSafe(string param)
    {
        if (String.IsNullOrEmpty(param))
        {
            return param;
        }

        // Replace + char for %20 to make it compatible with client side decodeURIComponent
        return ScriptHelper.GetString(Server.UrlEncode(param).Replace("+", "%20"));
    }


    /// <summary>
    /// Projects currently filtered data to the selection data in hidden fields.
    /// If <paramref name="isSelectAllAction"/> is <c>True</c> new data is added otherwise it is removed.
    /// </summary>
    private void UpdateHiddenFields(bool isSelectAllAction)
    {
        // Get all values
        DataSet ds = GetData();
        if (!DataHelper.DataSourceIsEmpty(ds))
        {
            AddGlobalObjectNamePrefixes(ds);

            var values = hidItem.Value.Split(new[] { valuesSeparator }, StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            // Process all items
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string value = dr[returnColumnName].ToString();

                if (isSelectAllAction)
                {
                    values.Add(value);
                }
                else
                {
                    values.Remove(value);
                }
            }

            hidItem.Value = string.Format("{0}{1}{0}", valuesSeparator, values.Join(valuesSeparator.ToString()));
        }

        pnlHidden.Update();
    }


    private void RegisterBaseClientScripts()
    {
        string stringValuesSeparator = valuesSeparator.ToString();
        string scriptValuesSeparator = ScriptHelper.GetString(stringValuesSeparator);
        string regexEscapedValuesSeparator = ScriptHelper.GetString(Regex.Escape(stringValuesSeparator), false);

        string script;

        switch (selectionMode)
        {
            // Button modes
            case SelectionModeEnum.SingleButton:
            case SelectionModeEnum.MultipleButton:
                {
                    // Register javascript code
                    if (callbackMethod == null)
                    {
                        script = string.Format("function SelectItems(items,hash) {{ wopener.US_SelectItems_{0}(items,hash); CloseDialog(); }}", parentClientId);
                    }
                    else
                    {
                        script = string.Format("function SelectItems(items,hash) {{ wopener.{0}(items.replace(/^{1}+|{1}+$/g, ''),hash); CloseDialog(); }}", callbackMethod, regexEscapedValuesSeparator);
                    }
                }
                break;

            // Selector modes
            default:
                {
                    // Register javascript code
                    script = @"
function SelectItems(items, names, hiddenFieldId, txtClientId, hashClientId, hash) {
    wopener.US_SetItems(items, names, hiddenFieldId, txtClientId, null, hashClientId, hash);" +
(fireOnChanged ? "wopener.US_SelectionChanged_" + parentClientId + "();" : "")
+ @"
    return CloseDialog(); 
}

function SelectItemsReload(items, names, hiddenFieldId, txtClientId, hidValue, hashClientId, hash) {
    wopener.US_SetItems(items, names, hiddenFieldId, txtClientId, hidValue, hashClientId, hash);

    wopener.US_ReloadPage_" + parentClientId + @"();
    return CloseDialog();
}";
                }
                break;
        }

        script += string.Format(@"
function ItemsElem() {{
    return document.getElementById('{0}');
}}

function GetCheckboxValue(chkbox) {{
    return chkbox.id.substr(3);
}}

function ProcessItem(chkbox, changeChecked) {{
    var itemsElem = ItemsElem();
    var items = itemsElem.value;
    if (chkbox != null) {{
        var item = GetCheckboxValue(chkbox);
        if (changeChecked) {{
            chkbox.checked = !chkbox.checked;
        }}
        if (chkbox.checked) {{
            if (items == '') {{
                itemsElem.value = {1} + item + {1};
            }}
            else if (items.toLowerCase().indexOf({1} + item.toLowerCase() + {1}) < 0) {{
                itemsElem.value += item + {1};
            }}
        }}
        else
        {{
            var re = new RegExp('{2}' + item + '{2}', 'i');
            itemsElem.value = items.replace(re, {1});
        }}
    }}
}}
", hidItem.ClientID, scriptValuesSeparator, regexEscapedValuesSeparator);

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "SelectionDialog_" + ClientID, script, true);
    }


    private void RegisterMultipleModeClientScripts(string checkboxAllClientId)
    {
        // Register scripts for multiple modes
        string stringValuesSeparator = valuesSeparator.ToString();
        string regexEscapedValuesSeparator = ScriptHelper.GetString(Regex.Escape(stringValuesSeparator), false);

        string script = "function ProcessResult(items, hash){";

        switch (selectionMode)
        {
            // Button modes
            case SelectionModeEnum.MultipleButton:
                script += "SelectItems(escape(items),hash);";
                break;

            // Textbox modes
            case SelectionModeEnum.MultipleTextBox:
                if (allowEditTextBox && !mHasDependingFields)
                {
                    script += "SelectItems(escape(items), escape(items.replace(/^" + regexEscapedValuesSeparator + "+|" + regexEscapedValuesSeparator + "+$/g, '')), " + ScriptHelper.GetString(hdnClientId) + ", " + ScriptHelper.GetString(txtClientId) + ", " + ScriptHelper.GetString(hashId) + ", hash);";
                }
                else if (mHasDependingFields)
                {
                    script += "SelectItemsReload(escape(items), escape(items.replace(/^" + regexEscapedValuesSeparator + "+|" + regexEscapedValuesSeparator + "+$/g, '')), " + ScriptHelper.GetString(hdnClientId) + ", " + ScriptHelper.GetString(txtClientId) + ", " + ScriptHelper.GetString(hdnDrpId) + ", " + ScriptHelper.GetString(hashId) + ", hash);";
                }
                else
                {
                    script += "SelectItemsReload(escape(items), '', " + ScriptHelper.GetString(hdnClientId) + ", " + ScriptHelper.GetString(txtClientId) + ", " + ScriptHelper.GetString(hdnDrpId) + ", " + ScriptHelper.GetString(hashId) + ", hash);";
                }
                break;

            // Other modes
            default:
                script += "SelectItemsReload(escape(items), '', " + ScriptHelper.GetString(hdnClientId) + ", " + ScriptHelper.GetString(txtClientId) + ", " + ScriptHelper.GetString(hdnDrpId) + ", " + ScriptHelper.GetString(hashId) + ", hash);";
                break;
        }

        script += @"}
function GetCheckboxAllElement() {
    return document.getElementById('" + checkboxAllClientId + @"');
}

function UpdateCheckboxAllElement() {
    var checked = true;
    var checkboxes = document.getElementsByClassName('chckbox');

    for(var i = 0; i < checkboxes.length; i++) {
        var chkbox = checkboxes[i];

        if (!chkbox.checked) {
            checked = false;
            break;
        }
    }

    var chkAll = GetCheckboxAllElement();

    if (chkAll != null && chkAll.checked != checked) {
        chkAll.checked = checked;
    }
}

function SelectAllItems(checkbox) {
    var checkboxes = document.getElementsByClassName('chckbox');
    var checked = checkbox.checked;
    
    for(var i = 0; i < checkboxes.length; i++) {
        var chkbox = checkboxes[i];
        chkbox.checked = checked;

        ProcessItem(chkbox, false);
    }
}

function GetHash() {
    " + Page.ClientScript.GetCallbackEventReference(this, "ItemsElem().value", "UpdateSelection", null) + @";
}

function UpdateSelection(value) {
    var values = value.split('#');    
    if (values.length == 2) {
        ProcessResult(values[0], values[1]);
    }
}

function US_Cancel() {
    CloseDialog();
    return false;
}

function US_Submit() {
    GetHash();
    return false;
}
";
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "SelectionDialogMultipleMode_" + ClientID, script, true);
    }

    #endregion


    #region "ICallbackEventHandler Members"

    string ICallbackEventHandler.GetCallbackResult()
    {
        // Prepare the parameters for dialog
        string result = string.Empty;
        if (!string.IsNullOrEmpty(callbackValues))
        {
            var allowedValues = new List<string>();

            var values = callbackValues.Split(new[] { valuesSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                // Get all (valid) data based on dialog configuration
                var ds = GetData(false);
                if (!DataHelper.DataSourceIsEmpty(ds))
                {
                    AddGlobalObjectNamePrefixes(ds);

                    allowedValues = DataHelper.GetStringValues(ds.Tables[0], returnColumnName);
                }
            }

            // Filter selected items by merging with valid data
            var filteredValues = values.Intersect(allowedValues, StringComparer.OrdinalIgnoreCase);

            // Update selected items and hash on client
            var valuesString = string.Format("{0}{1}{0}", valuesSeparator, filteredValues.Join(valuesSeparator.ToString()));
            result = valuesString + "#" + ValidationHelper.GetHashString(valuesString);
        }

        return result;
    }


    void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
    {
        callbackValues = eventArgument;
    }

    #endregion
}
