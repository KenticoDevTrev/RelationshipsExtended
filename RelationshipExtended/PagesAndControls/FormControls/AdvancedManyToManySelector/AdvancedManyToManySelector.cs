using System;
using System.Collections.Generic;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using System.Data;
using CMS.CustomTables;
using System.Linq;
using System.Web.UI.WebControls;
using CMS.FormEngine.Web.UI;
using RelationshipsExtended.Enums;
using RelationshipsExtended;
using CMS.DocumentEngine;
using TreeNode = CMS.DocumentEngine.TreeNode;
public partial class Compiled_CMSModules_RelationshipsExtended_FormControls_AdvancedManyToManySelector_AdvancedManyToManySelector : FormEngineUserControl
{

    public Compiled_CMSModules_RelationshipsExtended_FormControls_AdvancedManyToManySelector_AdvancedManyToManySelector()
    {

    }

    #region "Variables"
    private SelectorFieldSaveType RelatedObjectSaveMode;
    #endregion

    #region "Properties - Configuration"

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

    public string RelatedObjectType
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RelatedObjectType"), "");
        }
        set
        {
            SetValue("RelatedObjectType", value);
            if (!UseCustomSelector)
            {
                frmFormControl.SetValue("ObjectType", RelatedObjectType);
            }
        }
    }

    public string RelatedObjectReferenceField
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RelatedObjectReferenceField"), "");
        }
        set
        {
            SetValue("RelatedObjectReferenceField", value);
        }
    }

    public string ReturnType
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ReturnType"), "id");
        }
        set
        {
            SetValue("ReturnType", value);
        }
    }


    public int MinimumRelationships
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MinimumRelationships"), -1);
        }
        set
        {
            SetValue("MinimumRelationships", value);
        }

    }

    public int MaximumRelationships
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaximumRelationships"), -1);
        }
        set
        {
            SetValue("MaximumRelationships", value);
        }

    }

    public string RelatedObjectRestrainingWhere
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RelatedObjectRestrainingWhere"), "");
        }
        set
        {
            SetValue("RelatedObjectRestrainingWhere", value);
        }
    }



    #endregion

    #region "Properties - Selector"

    public string SeparatorCharacter
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SeparatorCharacter"), ";");
        }
        set
        {
            SetValue("SeparatorCharacter", value);
        }
    }

    public bool UseCustomSelector
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseCustomSelector"), false);
        }
        set
        {
            SetValue("UseCustomSelector", value);
        }
    }

    public string FormControlName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FormControlName"), "");
        }
        set
        {
            SetValue("FormControlName", value);
        }
    }

    public string FormControlProperties
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FormControlProperties"), ";");
        }
        set
        {
            SetValue("FormControlProperties", value);
        }
    }

    #endregion

    #region "Properties - Uni Selector"

    public string ReturnColumnName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ReturnColumnName"), "");
        }
        set
        {
            SetValue("ReturnColumnName", value);
            frmFormControl.SetValue("ReturnColumnName", value);
        }
    }

    public string ObjectSiteName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ObjectSiteName"), "");
        }
        set
        {
            SetValue("ObjectSiteName", value);
            frmFormControl.SetValue("ObjectSiteName", value);
        }
    }

    public string DisplayNameFormat
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DisplayNameFormat"), "");
        }
        set
        {
            SetValue("DisplayNameFormat", value);
            frmFormControl.SetValue("DisplayNameFormat", value);
        }
    }

    public int SelectionMode
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectionMode"), 5);
        }
        set
        {
            SetValue("SelectionMode", value);
            frmFormControl.SetValue("SelectionMode", value);
        }
    }

    public string AdditionalColumns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AdditionalColumns"), "");
        }
        set
        {
            SetValue("AdditionalColumns", value);
            frmFormControl.SetValue("AdditionalColumns", value);
        }
    }

    public string WhereCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WhereCondition"), "");
        }
        set
        {
            SetValue("WhereCondition", value);
            frmFormControl.SetValue("WhereCondition", value);
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
            frmFormControl.SetValue("OrderBy", value);
        }
    }
    public string EnabledColumnName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("EnabledColumnName"), "");
        }
        set
        {
            SetValue("EnabledColumnName", value);
            frmFormControl.SetValue("EnabledColumnName", value);
        }
    }

    public bool AllowEditTextBox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowEditTextBox"), false);
        }
        set
        {
            SetValue("AllowEditTextBox", value);
            frmFormControl.SetValue("AllowEditTextBox", value);
        }
    }

    public bool UseAutocomplete
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseAutocomplete"), false);
        }
        set
        {
            SetValue("UseAutocomplete", value);
            frmFormControl.SetValue("UseAutocomplete", value);
        }
    }

    public string Transformation
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Transformation"), "");
        }
        set
        {
            SetValue("Transformation", value);
            frmFormControl.SetValue("Transformation", value);
        }
    }

    public string NoDataTransformation
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NoDataTransformation"), "");
        }
        set
        {
            SetValue("NoDataTransformation", value);
            frmFormControl.SetValue("NoDataTransformation", value);
        }
    }

    public bool EncodeOutput
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EncodeOutput"), true);
        }
        set
        {
            SetValue("EncodeOutput", value);
            frmFormControl.SetValue("EncodeOutput", value);
        }
    }

    public bool AllowEmpty
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowEmpty"), true);
        }
        set
        {
            SetValue("AllowEmpty", value);
            frmFormControl.SetValue("AllowEmpty", value);
        }
    }

    public int MaxDisplayedTotalItems
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxDisplayedTotalItems"), 1000);
        }
        set
        {
            SetValue("MaxDisplayedTotalItems", value);
            frmFormControl.SetValue("MaxDisplayedTotalItems", value);
        }
    }

    public int MaxDisplayedItems
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxDisplayedItems"), 25);
        }
        set
        {
            SetValue("MaxDisplayedItems", value);
            frmFormControl.SetValue("MaxDisplayedItems", value);
        }
    }

    public int ItemsPerPage
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ItemsPerPage"), 25);
        }
        set
        {
            SetValue("ItemsPerPage", value);
            frmFormControl.SetValue("ItemsPerPage", value);
        }
    }

    public string DisabledItems
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DisabledItems"), "");
        }
        set
        {
            SetValue("DisabledItems", value);
            frmFormControl.SetValue("DisabledItems", value);
        }
    }

    public string ButtonImage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ButtonImage"), "");
        }
        set
        {
            SetValue("ButtonImage", value);
            frmFormControl.SetValue("ButtonImage", value);
        }

    }
    public string FilterControl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterControl"), "");
        }
        set
        {
            SetValue("FilterControl", value);
            frmFormControl.SetValue("FilterControl", value);
        }
    }

    public string GridName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("GridName"), "");
        }
        set
        {
            SetValue("GridName", value);
            frmFormControl.SetValue("GridName", value);
        }
    }
    public string DialogGridName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DialogGridName"), "");
        }
        set
        {
            SetValue("DialogGridName", value);
            frmFormControl.SetValue("DialogGridName", value);
        }
    }




    #endregion

    #region "Properties - Join Table Configuration"

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



    #endregion

    #region "Public properties"

    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            // this control does not return anything as it's purely a join table control
            return frmFormControl.Value;
        }
        set
        {
            // Will set from Join Table, not from something passed to this control
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
        if (UIContext != null && UIContext.ResourceName == "CMS.Form")
        {
            pnlPreviewOnly.Visible = true;
            StopProcessing = true;
            return;
        }

        base.OnInit(e);

        // Set Enum Values
        switch (ReturnType.ToLower())
        {
            case "id":
                RelatedObjectSaveMode = SelectorFieldSaveType.ID;
                break;
            case "guid":
                RelatedObjectSaveMode = SelectorFieldSaveType.GUID;
                break;
            case "string":
                RelatedObjectSaveMode = SelectorFieldSaveType.String;
                break;
        }

        this.Form.OnAfterSave += Form_OnAfterSaveJoinTable;
        InitializeFormControl();
    }

    /// <summary>
    /// Sets the category tree before load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !StopProcessing)
        {
            InitializeControl();
        }
    }
    /// <summary>
    /// Sets the where condition for filter on custom table filtering.
    /// </summary>
    public override string GetWhereCondition()
    {

        List<string> SelectedObjectRefIDs = GetSelectedObjects();
        if(SelectedObjectRefIDs.Count > 0)
        {
            var query = new DataQuery(JoinTableName + ".selectall").Column(JoinTableLeftFieldName).WhereIn(JoinTableRightFieldName, SelectedObjectRefIDs).Distinct();
            WhereCondition where = new WhereCondition();

            where.WhereIn(JoinTableLeftFieldName, query);
            return where.ToString();
        }
        else
        {
            return base.GetWhereCondition();
        }
    }

    protected void InitializeFormControl()
    {
        frmFormControl.SetValue("WatermarkText", "Test");

        if (!UseCustomSelector)
        {
            frmFormControl.FormControlName = "Uni_selector";
            frmFormControl.SetValue("ObjectType", RelatedObjectType);
            frmFormControl.SetValue("ReturnColumnName", ReturnColumnName);
            frmFormControl.SetValue("ObjectSiteName", ObjectSiteName);
            frmFormControl.SetValue("DisplayNameFormat", DisplayNameFormat);
            frmFormControl.SetValue("SelectionMode", SelectionMode);
            frmFormControl.SetValue("AdditionalColumns", AdditionalColumns);
            frmFormControl.SetValue("WhereCondition", WhereCondition);
            frmFormControl.SetValue("OrderBy", OrderBy);
            frmFormControl.SetValue("EnabledColumnName", EnabledColumnName);
            frmFormControl.SetValue("AllowEditTextBox", AllowEditTextBox);
            frmFormControl.SetValue("UseAutocomplete", UseAutocomplete);
            frmFormControl.SetValue("ValuesSeparator", SeparatorCharacter);
            frmFormControl.SetValue("Transformation", Transformation);
            frmFormControl.SetValue("NoDataTransformation", NoDataTransformation);
            frmFormControl.SetValue("EncodeOutput", EncodeOutput);
            frmFormControl.SetValue("AllowEmpty", AllowEmpty);
            frmFormControl.SetValue("MaxDisplayedTotalItems", MaxDisplayedTotalItems);
            frmFormControl.SetValue("MaxDisplayedItems", MaxDisplayedItems);
            frmFormControl.SetValue("ItemsPerPage", ItemsPerPage);
            frmFormControl.SetValue("DisabledItems", DisabledItems);
            frmFormControl.SetValue("ButtonImage", ButtonImage);
            frmFormControl.SetValue("FilterControl", FilterControl);
            frmFormControl.SetValue("GridName", string.IsNullOrWhiteSpace(GridName) ? null : GridName);
            frmFormControl.SetValue("DialogGridName", string.IsNullOrWhiteSpace(DialogGridName) ? null : DialogGridName);
        }
        else
        {
            frmFormControl.FormControlName = FormControlName;
            foreach (string propVal in FormControlProperties.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                string prop = propVal.Split('|')[0];
                string val = propVal.Split('|')[1];
                frmFormControl.SetValue(prop, val.Replace("\\n", "\n"));
            }
        }
        //frmFormControl.Reload();
    }
    private List<string> GetSelectedObjects()
    {
        string ObjectRefIDs = ValidationHelper.GetString(frmFormControl.Value, "");
        List<string> SelectedObjects = ObjectRefIDs.Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
        if (!string.IsNullOrWhiteSpace(RelatedObjectRestrainingWhere))
        {
            List<string> AllPossibleObjects = GetPossibleObjects();
            SelectedObjects.RemoveAll(x => !AllPossibleObjects.Contains(x));
        }
        return SelectedObjects;
    }
    private List<string> GetCurrentObjects()
    {
        int totalRecords = 0;
        try
        {
            var CurrentObjectsDS = ConnectionHelper.ExecuteQuery(JoinTableName + ".selectall", null, string.Format("{0} = '{1}'", JoinTableLeftFieldName, CurrentItemIdentification), null, -1, null, -1, -1, ref totalRecords);

            List<string> CurrentRelatedObjectIDs = new List<string>();
            foreach (DataRow dr in CurrentObjectsDS.Tables[0].Rows)
            {
                CurrentRelatedObjectIDs.Add(ValidationHelper.GetString(dr[JoinTableRightFieldName], ""));
            }

            // Apply restraint on current objects if present
            if (!string.IsNullOrWhiteSpace(RelatedObjectRestrainingWhere))
            {
                List<string> AllPossibleObjects = GetPossibleObjects();
                CurrentRelatedObjectIDs.RemoveAll(x => !AllPossibleObjects.Contains(x));
            }
            return CurrentRelatedObjectIDs;
        }
        catch (Exception)
        {
            return new List<string>();
        }
    }

    private List<string> GetPossibleObjects()
    {
        string PossibleObjectWhereCondition = "1=1";
        if (!string.IsNullOrWhiteSpace(RelatedObjectRestrainingWhere))
        {
            PossibleObjectWhereCondition = SqlHelper.AddWhereCondition(PossibleObjectWhereCondition, RelatedObjectRestrainingWhere);
        }
        int totalRecords = 0;
        var CurrentObjectsDS = ConnectionHelper.ExecuteQuery(RelatedObjectType + ".selectall", null, PossibleObjectWhereCondition, null, -1, null, -1, -1, ref totalRecords);

        List<string> CurrentRelatedObjectIDs = new List<string>();
        foreach (DataRow dr in CurrentObjectsDS.Tables[0].Rows)
        {
            CurrentRelatedObjectIDs.Add(ValidationHelper.GetString(dr[RelatedObjectReferenceField], ""));
        }
        return CurrentRelatedObjectIDs;
    }

    /// <summary>
    /// Takes the existing value and sets the Category Tree
    /// </summary>
    protected void InitializeControl()
    {
        // Get initial values from Join Table and set the control's value
        if (!IsPostBack)
        {
            // Setup Form Control
            //InitializeFormControl();
            frmFormControl.Value = string.Join(SeparatorCharacter, GetCurrentObjects()).Trim(SeparatorCharacter.ToCharArray());
        }
        /*else if ()
        {
            // Set from Text value
            string initialCategories = txtValue.Text;
            var CurrentObjectsOfDoc = BaseInfoProvider.GetCategories("CategoryID in ('" + string.Join("','", SplitAndSecure(initialCategories)) + "')", null, -1, null, SiteContext.CurrentSiteID);
            if (CurrentObjectsOfDoc != null)
            {
                CurrentObjects.AddRange(CurrentObjectsOfDoc);
            }
        }*/




    }



    #endregion

    #region "Private methods"

    /// <summary>
    /// Figure out using the Join Table which categories need to be removed and which added
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form_OnAfterSaveJoinTable(object sender, EventArgs e)
    {

        //dont save data for filter forms
        if (sender is CMS.FormEngine.Web.UI.FilterForm)
        {
            return;
        }

        // Add key so it will load the new data even though it is a 'postback' after this.
        DataClassInfo JoinTableClassInfo = DataClassInfoProvider.GetDataClassInfo(JoinTableName);
        if (JoinTableClassInfo != null)
        {
            List<string> SelectedObjectRefIDs = GetSelectedObjects();
            List<string> CurrentObjectRefIDs = GetCurrentObjects();

            List<string> ObjectsToAdd = SelectedObjectRefIDs.Where(x => !CurrentObjectRefIDs.Contains(x)).ToList();
            List<string> ObjectsToRemove = CurrentObjectRefIDs.Where(x => !SelectedObjectRefIDs.Contains(x)).ToList();

            if (ObjectsToAdd.Count > 0)
            {
                foreach (string ObjectToAdd in ObjectsToAdd)
                {
                    // Custom Tables
                    if (JoinTableClassInfo.ClassIsCustomTable)
                    {
                        CustomTableItem newCustomTableItem = CustomTableItem.New(JoinTableName);
                        SetBaseInfoItemValues(newCustomTableItem, GetProperObjectValue(ObjectToAdd), JoinTableClassInfo.ClassName);
                        newCustomTableItem.Insert();
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
                        SetBaseInfoItemValues(newJoinObj, GetProperObjectValue(ObjectToAdd), JoinTableClassInfo.ClassName);
                        InsertObjectHandler(newJoinObj);
                    }
                }
            }
            if (ObjectsToRemove.Count > 0)
            {
                foreach (string ObjectToRemove in ObjectsToRemove)
                {
                    // Custom Table logic
                    if (JoinTableClassInfo.ClassIsCustomTable)
                    {
                        CustomTableItemProvider.GetItems(JoinTableClassInfo.ClassName).WhereEquals(JoinTableLeftFieldName, CurrentItemIdentification)
                            .WhereEquals(JoinTableRightFieldName, GetProperObjectValue(ObjectToRemove))
                            .ToList().ForEach(x => ((CustomTableItem)x).Delete());
                    }
                    else
                    {
                        new ObjectQuery(JoinTableClassInfo.ClassName)
                            .WhereEquals(JoinTableLeftFieldName, CurrentItemIdentification)
                            .WhereEquals(JoinTableRightFieldName, GetProperObjectValue(ObjectToRemove))
                            .ToList().ForEach(x => x.Delete());
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
                            var DocQuery = new DocumentQuery().WhereEquals(JoinTableThisObjectForeignKey, ForiegnKeyValue).Columns("NodeID");
                            if(JoinTableThisObjectForeignKey.ToLower() == "nodealiaspath")
                            {
                                DocQuery.OnCurrentSite();
                            }
                            TreeNode Page = DocQuery.FirstOrDefault();
                            int PrimaryNodeID = RelHelper.GetPrimaryNodeID(Page.NodeID);
                            object NewValue = ForiegnKeyValue;
                            if (PrimaryNodeID != Page.NodeID)
                            {
                                NewValue = new DocumentQuery().WhereEquals("NodeID", PrimaryNodeID).Columns(JoinTableThisObjectForeignKey).FirstOrDefault().GetValue(JoinTableThisObjectForeignKey);
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

    private object GetProperObjectValue(string Value)
    {
        switch (RelatedObjectSaveMode)
        {
            case SelectorFieldSaveType.ID:
                return ValidationHelper.GetInteger(Value, -1);
            case SelectorFieldSaveType.GUID:
                return ValidationHelper.GetGuid(Value, new Guid());
            case SelectorFieldSaveType.String:
            default:
                return Value;
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
        if (MinimumRelationships > -1)
        {
            if (ValidationHelper.GetString(frmFormControl.Value, "").Trim().Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length < MinimumRelationships)
            {
                MinMet = false;
            }
        }
        if (MaximumRelationships > -1)
        {
            if (ValidationHelper.GetString(frmFormControl.Value, "").Trim().Split(SeparatorCharacter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > MaximumRelationships)
            {
                MaxMet = false;
            }
        }

        if (!MinMet || !MaxMet)
        {
            ValidationError = string.Format("{0} {1} {2}",
                (!MinMet ? "Minimum " + MinimumRelationships + " selections allowed" : ""),
                (!MinMet && !MaxMet ? " and " : ""),
                (!MaxMet ? "Maximum " + MaximumRelationships + " selections allowed" : ""));
        }
        return (MinMet && MaxMet);
    }



    #endregion

}

