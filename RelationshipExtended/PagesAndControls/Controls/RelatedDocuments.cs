using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Relationships;
using CMS.SiteProvider;
using CMS.UIControls;
using RelationshipsExtended;
using Action = CMS.UIControls.UniGridConfig.Action;
using MessageTypeEnum = CMS.Base.Web.UI.MessageTypeEnum;
using TreeNode = CMS.DocumentEngine.TreeNode;


public partial class Compiled_CMSModules_RelationshipsExtended_Controls_RelatedDocuments : ReadOnlyFormEngineUserControl
{
    public Compiled_CMSModules_RelationshipsExtended_Controls_RelatedDocuments() { }
    #region "Constants"

    private const string ADHOC_RELATIONSHIP_NAME = "##AUTO##";

    #endregion


    #region "Variables"

    private TreeProvider mTreeProvider;
    private bool mShowAddRelation = true;
    private DialogConfiguration mConfig;
    private string mAdHocRelationshipName;

    #endregion


    #region "Private properties"

    /// <summary>
    /// Control is used as form field of Page data type (as ad hoc relationship).
    /// </summary>
    private bool? _IsAdhocRelationship;
    private bool IsAdHocRelationship
    {
        get
        {
            if (!_IsAdhocRelationship.HasValue)
            {
                _IsAdhocRelationship = RelationshipNameInfo.Provider.Get(RelationshipName).RelationshipNameIsAdHoc;
            }
            return _IsAdhocRelationship.Value;
        }
    }


    /// <summary>
    /// Ad-hoc relationship name based on document type class name and field GUID.
    /// </summary>
    private string AdHocRelationshipName
    {
        get
        {
            if (mAdHocRelationshipName != null)
            {
                return mAdHocRelationshipName;
            }

            if (!IsAdHocRelationship)
            {
                throw new NotSupportedException("Control can be used only for a Page data type of a form field.");
            }

            if (TreeNode != null)
            {
                mAdHocRelationshipName = RelationshipNameInfoProvider.GetAdHocRelationshipNameCodeName(TreeNode.NodeClassName, FieldInfo);
            }

            return mAdHocRelationshipName;
        }
    }


    /// <summary>
    /// Indicates if relationship name is created as ad-hoc within first relation created.
    /// </summary>
    private bool UseAdHocRelationshipName
    {
        get
        {
            return false;
            //return String.Equals(RelationshipName, ADHOC_RELATIONSHIP_NAME, StringComparison.InvariantCultureIgnoreCase);
        }
    }


    /// <summary>
    /// Gets the configuration for Copy and Move dialog.
    /// </summary>
    private DialogConfiguration Config
    {
        get
        {
            if (mConfig != null)
            {
                return mConfig;
            }

            mConfig = new DialogConfiguration();
            mConfig.HideLibraries = true;
            mConfig.ContentSelectedSite = SiteContext.CurrentSiteName;
            mConfig.HideAnchor = true;
            mConfig.HideAttachments = true;
            mConfig.HideContent = false;
            mConfig.HideEmail = true;
            mConfig.HideLibraries = true;
            mConfig.HideWeb = true;
            mConfig.ContentSelectedSite = SiteContext.CurrentSiteName;
            mConfig.OutputFormat = OutputFormatEnum.Custom;
            mConfig.CustomFormatCode = "relationship";
            mConfig.ContentSites = AvailableSitesEnum.OnlyCurrentSite;
            mConfig.AdditionalQueryParameters = "contentchanged=false";
            return mConfig;
        }
    }

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets relationship name.
    /// </summary>
    public string RelationshipName
    {
        get
        {
            return GetValue("RelationshipName", String.Empty);
        }
        set
        {
            SetValue("RelationshipName", value);
        }
    }


    /// <summary>
    /// Indicates if allow switch sides.
    /// </summary>
    public bool AllowSwitchSides
    {
        get
        {
            return GetValue("AllowSwitchSides", true);
        }
        set
        {
            SetValue("AllowSwitchSides", value);
        }
    }


    /// <summary>
    /// Default side (False - left, True - right).
    /// </summary>
    public bool DefaultSide
    {
        get
        {
            return GetValue("DefaultSide", true);
        }
        set
        {
            SetValue("DefaultSide", value);
        }
    }


    /// <summary>
    /// Default page size.
    /// </summary>
    public int DefaultPageSize
    {
        get
        {
            return GetValue("DefaultPageSize", 5);
        }
        set
        {
            SetValue("DefaultPageSize", value);
        }
    }


    /// <summary>
    /// Number of related pages which can be added to page. 
    /// </summary>
    public int RelatedPagesLimit
    {
        get
        {
            return GetValue("RelatedPagesLimit", 0);
        }
        set
        {
            SetValue("RelatedPagesLimit", value);
        }
    }


    /// <summary>
    /// Page size values separated with comma.
    /// </summary>
    public string PageSize
    {
        get
        {
            return GetValue("PageSize", "5,10,25,50,100,##ALL##");
        }
        set
        {
            SetValue("PageSize", value);
        }
    }


    /// <summary>
    /// Indicates id show link 'Add relation'.
    /// </summary>
    public bool ShowAddRelation
    {
        get
        {
            return mShowAddRelation;
        }
        set
        {
            mShowAddRelation = value;
        }
    }


    /// <summary>
    /// Path where content tree in document selection dialog will start.
    /// </summary>
    public string StartingPath
    {
        get
        {
            return GetValue("StartingPath", "/");
        }
        set
        {
            SetValue("StartingPath", value);
        }
    }

    public string RelatedNodeSite
    {
        get
        {
            return GetValue("RelatedNodeSite", "");
        }
        set
        {
            SetValue("RelatedNodeSite", value);
        }
    }


    private string RelatedNodeSiteName
    {
        get
        {
            switch (RelatedNodeSite)
            {
                case "#currentsite":
                    return SiteContext.CurrentSiteName;
                default:
                    return RelatedNodeSite;
            }
        }
    }


    /// <summary>
    /// Gets or sets the document;.
    /// </summary>
    public TreeNode TreeNode
    {
        get;
        set;
    }


    /// <summary>
    /// Gets tree provider.
    /// </summary>
    public TreeProvider TreeProvider
    {
        get
        {
            return mTreeProvider ?? (mTreeProvider = new TreeProvider(MembershipContext.AuthenticatedUser));
        }
    }


    /// <summary>
    /// Enables or disables control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }
        set
        {
            base.Enabled = value;
            UniGridRelationship.GridView.Enabled = value;
        }
    }


    /// <summary>
    /// Messages placeholder.
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }

    public string AllowedPageTypes
    {
        get
        {
            return SqlHelper.EscapeQuotes(ValidationHelper.GetString(GetValue("AllowedPageTypes"), ""));
        }
        set
        {
            SetValue("AllowedPageTypes", value);
        }
    }

    public string ToolTipFormat
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ToolTipFormat"), "");
        }
        set
        {
            SetValue("ToolTipFormat", value);
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
        }
    }

    /// <summary>
    /// Path where content tree in document selection dialog will start.
    /// </summary>
    public bool DisableSort { get; set; }

    #endregion


    #region "Page events"

    /// <summary>
    /// OnInit event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        // Set default items on Grid
        InitUniGrid();

        // Allow ControlExtender to overwrite if needed
        base.OnInit(e);

        // Paging
        UniGridRelationship.PageSize = PageSize;
        UniGridRelationship.Pager.DefaultPageSize = DefaultPageSize;

    }


    /// <summary>
    /// PageLoad event handler.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        UniGridRelationship.Attributes.Add("AllowedPageTypes", AllowedPageTypes);
        UniGridRelationship.Attributes.Add("DisplayNameFormat", DisplayNameFormat);
        UniGridRelationship.Attributes.Add("ToolTipFormat", ToolTipFormat);

        // Object type cannot be defined in xml definition -> it would ignore code behind configuration
        UniGridRelationship.ObjectType = (IsAdHocRelationship) ? RelationshipInfo.OBJECT_TYPE_ADHOC : RelationshipInfo.OBJECT_TYPE;

        //UniGridRelationship.ObjectType = RelationshipInfo.OBJECT_TYPE_ADHOC;

        if (StopProcessing)
        {
            UniGridRelationship.StopProcessing = StopProcessing;
            return;
        }

        // Set tree node from Form object
        if ((TreeNode == null) && (Form != null) && (Form.EditedObject != null))
        {
            var node = Form.EditedObject as TreeNode;
            if ((node != null) && (Form.Mode == FormModeEnum.Update))
            {
                TreeNode = node;
            }
            else
            {
                ShowInformation(GetString("relationship.editdocumenterror"));
            }
        }

        if (TreeNode != null)
        {
            //InitUniGrid();

            int nodeId = TreeNode.NodeID;

            // Add relationship name condition
            var condition = new WhereCondition()
                .WhereIn("RelationshipNameID", new IDQuery<RelationshipNameInfo>().Where(GetRelationshipNameCondition()));

            // Switch sides is disabled
            condition.WhereEquals(DefaultSide ? "RightNodeID" : "LeftNodeID", nodeId);

            if (!string.IsNullOrWhiteSpace(RelatedNodeSiteName))
            {
                condition.Where(string.Format("{0} in (Select NodeID from View_CMS_Tree_Joined where NodeSiteID = {1})", (DefaultSide ? "LeftNodeID" : "RightNodeID"), SiteInfoProvider.GetSiteID(RelatedNodeSiteName)));
            }

            InitFilterVisibility();

            UniGridRelationship.WhereCondition = condition.ToString(true);

        }
        else
        {
            UniGridRelationship.StopProcessing = true;
            UniGridRelationship.Visible = false;
        }

        if (RequestHelper.IsPostBack())
        {
            string target = Request[Page.postEventSourceID];
            if ((target != pnlUpdate.ClientID) && (target != pnlUpdate.UniqueID))
            {
                return;
            }

            string action = Request[Page.postEventArgumentID];
            if (string.IsNullOrEmpty(action))
            {
                return;
            }

            switch (action.ToLowerInvariant())
            {
                // Insert from 'Select document' dialog
                case "insertfromselectdocument":
                    SaveRelationship();
                    break;
            }
        }
        else
        {
            bool inserted = QueryHelper.GetBoolean("inserted", false);
            if (inserted)
            {
                ShowConfirmation(GetString("relationship.wasadded"));
            }
        }


        if (
            (!string.IsNullOrWhiteSpace(RelatedNodeSiteName) && !RelatedNodeSiteName.Equals(SiteContext.CurrentSiteName, StringComparison.InvariantCultureIgnoreCase)
            || (string.IsNullOrWhiteSpace(RelatedNodeSiteName) && SiteInfo.Provider.Get().Count > 1))
            )
        {
            ltrStyleHide.Visible = true;
        }
        else
        {
            ltrStyleHide.Visible = false;
        }
    }


    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected void Page_PreRender(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// Initializes the filter visibility
    /// </summary>
    private void InitFilterVisibility()
    {
        if (!AllowSwitchSides)
        {
            // Hide filter for the default side (always current document)
            var hideField = DefaultSide ? "RightNode" : "LeftNode";
            UniGridRelationship.FilterForm.FieldsToHide.Add(hideField);
        }

        if (!String.IsNullOrEmpty(RelationshipName))
        {
            // Hide filter for relationship name (always the same)
            UniGridRelationship.FilterForm.FieldsToHide.Add("RelationshipName");
        }
    }


    /// <summary>
    /// Gets where condition for selecting relationship names
    /// </summary>
    private WhereCondition GetRelationshipNameCondition()
    {
        var relationshipNameCondition = new WhereCondition();

        if (!UseAdHocRelationshipName)
        {
            relationshipNameCondition.Where(new WhereCondition());

            if (!String.IsNullOrEmpty(RelationshipName))
            {
                relationshipNameCondition.WhereEquals("RelationshipName", RelationshipName);
            }
        }

        return relationshipNameCondition;
    }


    private void InitUniGrid()
    {
        var zeroRowsText = GetString("relationship.nodatafound");

        if (RelatedPagesLimit > 0)
        {
            zeroRowsText = zeroRowsText + " " + String.Format(GetString("relationship.pagelimit.info"), RelatedPagesLimit);
        }

        // Set unigrid items, for events only set if not already set.
        UniGridRelationship.OnExternalDataBound += UniGridRelationship_OnExternalDataBound;
        UniGridRelationship.OnBeforeDataReload += UniGridRelationship_OnBeforeDataReload;
        UniGridRelationship.OnAction += UniGridRelationship_OnAction;
        UniGridRelationship.OnLoadColumns += UniGridRelationship_OnLoadColumns;
        UniGridRelationship.ZeroRowsText = zeroRowsText;
        UniGridRelationship.ShowActionsMenu = !IsLiveSite;
        UniGridRelationship.ShowExportMenu = true;
        UniGridRelationship.OrderBy = "RelationshipOrder";
    }

    #endregion


    #region "Event handlers"

    /// <summary>
    /// Performs actions before reload grid.
    /// </summary>
    private void UniGridRelationship_OnBeforeDataReload()
    {
        DataControlField rightTypeColumn = UniGridRelationship.NamedColumns["RightClassID"];
        DataControlField leftTypeColumn = UniGridRelationship.NamedColumns["LeftClassID"];

        // Hide columns
        if (!string.IsNullOrEmpty(RelationshipName))
        {
            string headerText = GetString("relationship.relateddocument");
            DataControlField leftColumn = UniGridRelationship.NamedColumns["LeftNodeName"];
            DataControlField relationshipNameColumn = UniGridRelationship.NamedColumns["RelationshipDisplayName"];
            DataControlField rightColumn = UniGridRelationship.NamedColumns["RightNodeName"];

            if (DefaultSide)
            {
                leftColumn.HeaderText = headerText;
                leftTypeColumn.HeaderStyle.Width = new Unit("100%");
                rightColumn.Visible = false;
                rightTypeColumn.Visible = false;
            }
            else
            {
                DataControlField leftColumnType = UniGridRelationship.NamedColumns["LeftClassID"];

                rightColumn.HeaderText = headerText;
                rightTypeColumn.HeaderStyle.Width = new Unit("100%");
                leftColumn.Visible = false;
                leftColumnType.Visible = false;
            }

            // Hide relationship name column
            relationshipNameColumn.Visible = false;
        }
        else
        {
            rightTypeColumn.HeaderStyle.Width = new Unit("100%");
        }
    }


    /// <summary>
    /// Fires on the grid action.
    /// </summary>
    /// <param name="actionName">Action name</param>
    /// <param name="actionArgument">Action argument</param>
    private void UniGridRelationship_OnAction(string actionName, object actionArgument)
    {
        // Check modify permissions
        if (MembershipContext.AuthenticatedUser.IsAuthorizedPerDocument(TreeNode, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied)
        {
            return;
        }

        switch (actionName.ToLowerInvariant())
        {
            case "delete":
                var relationshipId = actionArgument.ToInteger(0);

                // If parameters are valid
                if (relationshipId > 0)
                {
                    // Remove relationship
                    RelationshipInfoProvider.RemoveRelationship(relationshipId);
                    if (RelHelper.IsStagingEnabled())
                    {
                        // Log synchronization
                        DocumentSynchronizationHelper.LogDocumentChange(TreeNode.NodeSiteName, TreeNode.NodeAliasPath, TaskTypeEnum.UpdateDocument, TreeProvider);
                    }
                    ShowConfirmation(GetString("relationship.wasdeleted"));
                    URLHelper.RefreshCurrentPage();
                }

                break;
        }
    }

    private void UniGridRelationship_OnLoadColumns()
    {
        List<string> actionsToRemove = new List<string>();

        if (DisableSort)
        {
            actionsToRemove.AddRange(new[] { "#moveup", "#movedown", "#move" });
        }
        else
        {
            if (!IsAdHocRelationship || DefaultSide)
            {
                actionsToRemove.AddRange(new[] { "#moveup", "#movedown", "#move" });
            }
            else
            {
                actionsToRemove.AddRange(new[] { "#moveup", "#movedown" });
            }
        }

        // Only show edit on proper page
        if (!DefaultSide)
        {
            actionsToRemove.Add("GoToPageLeft");
        }
        else
        {
            actionsToRemove.Add("GoToPageRight");
        }

        UniGridRelationship.GridActions.Actions.RemoveAll(a =>
        {
            var action = a as Action;
            if (action == null)
            {
                return false;
            }
            return actionsToRemove.Contains(action.Name);
        });
    }

    /// <summary>
    /// Binds the grid columns.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="sourceName">Source name</param>
    /// <param name="parameter">Parameter</param>
    private object UniGridRelationship_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerInvariant())
        {
            case "lefnodename":
            case "rightnodename":
                string tooltip = null;
                string customName = null;
                int NodeID = ValidationHelper.GetInteger(parameter, 0);
                var NodeObj = new DocumentQuery().WhereEquals("NodeID", NodeID).Columns("NodeID, NodeName, NodeLevel, ClassName").FirstOrDefault();
                // Not root and is in the allowed page types
                if (NodeObj.NodeLevel != 0 && (!string.IsNullOrWhiteSpace(ToolTipFormat) || !string.IsNullOrWhiteSpace(DisplayNameFormat)) && AllowedPageTypes.ToLower().Split(";,|".ToCharArray()).Contains(NodeObj.NodeClassName.ToLower()))
                {
                    ValidationHelper.GetInteger(parameter, 0);
                    MacroResolver NodeResolver = GetNodeMacroResolver(NodeObj.NodeID, NodeObj.ClassName);
                    if (!string.IsNullOrWhiteSpace(ToolTipFormat))
                    {
                        tooltip = NodeResolver.ResolveMacros(ToolTipFormat);
                    }
                    if (!string.IsNullOrWhiteSpace(DisplayNameFormat))
                    {
                        customName = NodeResolver.ResolveMacros(DisplayNameFormat);
                    }
                    return string.Format("<div title=\"{0}\">{1}</div>", HTMLHelper.EncodeForHtmlAttribute(tooltip), (!string.IsNullOrWhiteSpace(customName) ? customName : NodeObj.NodeName));
                }
                else
                {
                    var tr = new ObjectTransformation(PredefinedObjectType.NODE, NodeID);
                    tr.EncodeOutput = false;
                    tr.Transformation = "{%NodeName|(default)" + GetString("general.root") + "|(encode)%}";
                    return tr;
                }
            case "delete":
                var btn = ((CMSGridActionButton)sender);
                btn.PreRender += imgDelete_PreRender;
                break;
        }

        return parameter;
    }

    private MacroResolver GetNodeMacroResolver(int NodeID, string ClassName)
    {
        string Culture = URLHelper.GetQueryValue(Request.RawUrl, "culture");
        return CacheHelper.Cache<MacroResolver>(cs =>
        {
            MacroResolver resolver = MacroResolver.GetInstance();

            List<string> Columns = new List<string>();

            if (!string.IsNullOrWhiteSpace(ToolTipFormat))
            {
                Columns.AddRange(DataHelper.GetNotEmpty(MacroProcessor.GetMacros(ToolTipFormat, true), "NodeName").Split(';'));
            }
            if (!string.IsNullOrWhiteSpace(DisplayNameFormat))
            {
                Columns.AddRange(DataHelper.GetNotEmpty(MacroProcessor.GetMacros(DisplayNameFormat, true), "NodeName").Split(';'));
            }
            // Get data for this node and render it out
            DataSet FullData = new DocumentQuery(ClassName)
                    .WhereEquals("NodeID", NodeID)
                    .Columns(Columns)
                    .Culture(Culture)
                    .CombineWithDefaultCulture(true).Result;

            foreach (DataColumn item in FullData.Tables[0].Columns)
            {
                resolver.SetNamedSourceData(item.ColumnName, FullData.Tables[0].Rows[0][item.ColumnName]);
            }

            if (cs.Cached)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency("nodeid|" + NodeID);
            }
            return resolver;
        }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "RelationshipTree_GetNodeMacroResolver", NodeID, Culture, ToolTipFormat, DisplayNameFormat));
    }

    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected void imgDelete_PreRender(object sender, EventArgs e)
    {
        CMSGridActionButton imgDelete = (CMSGridActionButton)sender;
        if (!Enabled)
        {
            // Disable delete icon in case that editing is not allowed
            imgDelete.Enabled = false;
            imgDelete.Style.Add("cursor", "default");
        }
    }

    #endregion

    #region "Code for custom OnExternalDataBound"
    // USE THE BELOW FOR CREATING OVERRIDES FOR THE OnExternalDataBound of this control so you do not lose the custom display and tooltip!
    /*
     * using CMS;
    using CMS.Base.Web.UI;
    using CMS.DataEngine;
    using CMS.DocumentEngine;
    using CMS.Helpers;
    using CMS.Localization;
    using CMS.MacroEngine;
    using CMS.SiteProvider;
    using CMS.UIControls;
    using My.Classes.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI;

    [assembly: RegisterCustomClass("RelatedPagesCustomUniGridExtension", typeof(RelatedPagesCustomUniGridExtension))]
    namespace My.Classes.Extensions
    {
    public class RelatedPagesCustomUniGridExtension : ControlExtender<UniGrid>
    {
        private string AllowedPageTypes;
        private string DisplayNameFormat;
        private string ToolTipFormat;

        public override void OnInit()
        {
            Control.OnExternalDataBound += Control_OnExternalDataBound;
        }

        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            switch (sourceName.ToLowerInvariant())
            {
                case "lefnodename":
                case "rightnodename":
                    UniGrid GridObj = GetUniGridControl((Control)sender);
                    // Retrieve properties from grid's attributes
                    if(GridObj != null) { 
                        AllowedPageTypes = ValidationHelper.GetString(GridObj.Attributes["AllowedPageTypes"], "");
                        DisplayNameFormat = ValidationHelper.GetString(GridObj.Attributes["DisplayNameFormat"], "");
                        ToolTipFormat = ValidationHelper.GetString(GridObj.Attributes["ToolTipFormat"], "");
                    }
                    int NodeID = ValidationHelper.GetInteger(parameter, 0);
                    var NodeObj = new DocumentQuery().WhereEquals("NodeID", NodeID).Columns("NodeID, NodeLevel, NodeName, ClassName").FirstObject;
                    // Not root and is in the allowed page types
                    if (GridObj != null && NodeObj.NodeLevel != 0 && (!string.IsNullOrWhiteSpace(ToolTipFormat) || !string.IsNullOrWhiteSpace(DisplayNameFormat)) && AllowedPageTypes.ToLower().Split(";,|".ToCharArray()).Contains(NodeObj.NodeClassName.ToLower()))
                    {
                        string tooltip = null;
                        string customName = null;
                        ValidationHelper.GetInteger(parameter, 0);
                        MacroResolver NodeResolver = GetNodeMacroResolver(NodeObj.NodeID, NodeObj.ClassName);
                        if (!string.IsNullOrWhiteSpace(ToolTipFormat))
                        {
                            tooltip = NodeResolver.ResolveMacros(ToolTipFormat);
                        }
                        if (!string.IsNullOrWhiteSpace(DisplayNameFormat))
                        {
                            customName = NodeResolver.ResolveMacros(DisplayNameFormat);
                        }
                        return string.Format("<div title=\"{0}\" class=\"CustomUniGridEntry\">{1}</div>", HTMLHelper.EncodeForHtmlAttribute(tooltip), (!string.IsNullOrWhiteSpace(customName) ? customName : NodeObj.NodeName));
                    }
                    else
                    {
                        var tr = new ObjectTransformation(PredefinedObjectType.NODE, NodeID);
                        tr.EncodeOutput = false;
                        tr.Transformation = "{%NodeName|(default)" + LocalizationHelper.LocalizeExpression("{$ general.root $}") + "|(encode)%}";
                        return tr;
                    }
                case "delete":
                    var btn = ((CMSGridActionButton)sender);
                    btn.PreRender += imgDelete_PreRender;
                    break;
                case "mycustomthing":
                    return "the custom stuff";
            }
            return parameter;
        }

        /// <summary>
        /// Recursively goes up the current control until it finds a control of type UniGrid
        /// </summary>
        /// <param name="control">the starting control, usually the "sender" object</param>
        /// <returns>The UniGrid Control, or null if not found</returns>
        private UniGrid GetUniGridControl(Control control)
        {
            while(!(control is UniGrid))
            {
                if(control.Parent != null)
                {
                    control = control.Parent;
                } else
                {
                    return null;
                }
            }
            return (UniGrid)control;
        }

        /// <summary>
        /// Renders the Delete Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgDelete_PreRender(object sender, EventArgs e)
        {
            CMSGridActionButton imgDelete = (CMSGridActionButton)sender;
            if (!Control.GridView.Enabled)
            {
                // Disable delete icon in case that editing is not allowed
                imgDelete.Enabled = false;
                imgDelete.Style.Add("cursor", "default");
            }
        }

        /// <summary>
        /// Gets macro resolver for the Node with the classes needed to render the Tooltip / display name format
        /// </summary>
        /// <param name="NodeID"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        private MacroResolver GetNodeMacroResolver(int NodeID, string ClassName)
        {
            string Culture = URLHelper.GetQueryValue(HttpContext.Current.Request.RawUrl, "culture");
            return CacheHelper.Cache<MacroResolver>(cs =>
            {
                MacroResolver resolver = MacroResolver.GetInstance();

                List<string> Columns = new List<string>();

                if (!string.IsNullOrWhiteSpace(ToolTipFormat))
                {
                    Columns.AddRange(DataHelper.GetNotEmpty(MacroProcessor.GetMacros(ToolTipFormat, true), "NodeName").Split(';'));
                }
                if (!string.IsNullOrWhiteSpace(DisplayNameFormat))
                {
                    Columns.AddRange(DataHelper.GetNotEmpty(MacroProcessor.GetMacros(DisplayNameFormat, true), "NodeName").Split(';'));
                }
                // Get data for this node and render it out
                DataSet FullData = new DocumentQuery(ClassName)
                    .WhereEquals("NodeID", NodeID)
                    .Columns(Columns)
                    .Culture(Culture)
                    .CombineWithDefaultCulture(true).Result;

                foreach (DataColumn item in FullData.Tables[0].Columns)
                {
                    resolver.SetNamedSourceData(item.ColumnName, FullData.Tables[0].Rows[0][item.ColumnName]);
                }

                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("nodeid|" + NodeID);
                }
                return resolver;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "RelationshipTree_GetNodeMacroResolver", NodeID, Culture, ToolTipFormat, DisplayNameFormat));
        }
    }
    }
    */
    #endregion



    #region "Methods"

    /// <summary>
    /// Saves relationship.
    /// </summary>
    public void SaveRelationship()
    {
        if (TreeNode == null)
        {
            return;
        }

        // Check modify permissions
        if (MembershipContext.AuthenticatedUser.IsAuthorizedPerDocument(TreeNode, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied)
        {
            return;
        }

        bool currentNodeIsOnLeftSide = !DefaultSide;

        // Selected node Id
        int selectedNodeId = ValidationHelper.GetInteger(hdnSelectedNodeId.Value, 0);

        // Get relationshipNameId
        var relationshipNameId = GetRelationshipNameId();

        if ((selectedNodeId <= 0) || (relationshipNameId <= 0))
        {
            return;
        }

        try
        {
            // Left side
            if (currentNodeIsOnLeftSide)
            {
                RelationshipInfo.Provider.Add(TreeNode.NodeID, selectedNodeId, relationshipNameId);
            }
            // Right side
            else
            {
                RelationshipInfo.Provider.Add(selectedNodeId, TreeNode.NodeID, relationshipNameId);
            }

            if (RelHelper.IsStagingEnabled())
            {
                // Log synchronization
                DocumentSynchronizationHelper.LogDocumentChange(TreeNode.NodeSiteName, TreeNode.NodeAliasPath, TaskTypeEnum.UpdateDocument, TreeProvider);
            }

            ShowConfirmation(GetString("relationship.wasadded"));
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }


    /// <summary>
    /// Returns Javascript used for invoking 'add related document' dialog.
    /// </summary>
    public string GetAddRelatedDocumentScript()
    {
        string postbackArgument;
        if (!AllowSwitchSides && !string.IsNullOrEmpty(RelationshipName))
        {
            postbackArgument = "insertfromselectdocument";

            // Register javascript 'postback' function
            string script = "function RefreshRelatedPanel(elementId) { if (elementId != null) { __doPostBack(elementId, '" + postbackArgument + "'); } } \n";
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "RefreshRelatedPanel", ScriptHelper.GetScript(script));

            // Dialog 'Select document'
            Config.EditorClientID = pnlUpdate.ClientID + ";" + hdnSelectedNodeId.ClientID;

            // Set dialog starting path
            if (!string.IsNullOrEmpty(StartingPath))
            {
                Config.ContentStartingPath = StartingPath;
            }

            string url = CMSDialogHelper.GetDialogUrl(Config, false, null, false);

            return string.Format("modalDialog('{0}', 'contentselectnode', '90%', '85%');", url);
        }
        else
        {
            postbackArgument = "insert";

            // Register javascript 'postback' function
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "RefreshUpdatePanel_" + ClientID, ScriptHelper.GetScript(
                "function RefreshUpdatePanel_" + ClientID + "(){ " + Page.ClientScript.GetPostBackEventReference(pnlUpdate, postbackArgument) + "; } \n"));

            // Dialog 'Add related document'
            string query = "?nodeid=" + TreeNode.NodeID;
            query = URLHelper.AddUrlParameter(query, "defaultside", DefaultSide.ToString());
            query = URLHelper.AddUrlParameter(query, "allowswitchsides", AllowSwitchSides.ToString());
            query = URLHelper.AddUrlParameter(query, "relationshipname", RelationshipName);
            query = URLHelper.AddUrlParameter(query, "externalControlID", ClientID);
            query = URLHelper.AddUrlParameter(query, "startingpath", StartingPath ?? "");

            query = query.Replace("%", "%25").Replace("/", "%2F");

            query = URLHelper.AddUrlParameter(query, "hash", QueryHelper.GetHash(query));

            string url;
            if (IsLiveSite)
            {
                url = ResolveUrl("~/CMSFormControls/LiveSelectors/RelatedDocuments.aspx" + query);
            }
            else
            {
                url = ResolveUrl("~/CMSFormControls/Selectors/RelatedDocuments.aspx" + query);
            }

            return string.Format("modalDialog('{0}', 'AddRelatedDocument', '900', '315');", url);
        }
    }


    private int GetRelationshipNameId()
    {
        var relationshipName = UseAdHocRelationshipName ? RelationshipNameInfoProvider.GetAdHocRelationshipNameCodeName(TreeNode.ClassName, FieldInfo) : RelationshipName;
        var relationshipNameInfo = RelationshipNameInfo.Provider.Get(relationshipName);

        if (relationshipNameInfo == null)
        {
            throw new NullReferenceException("Missing relationship name to use for relation.");
        }

        return relationshipNameInfo.RelationshipNameId;
    }


    private int GetRelationshipCount()
    {
        return RelationshipInfo.Provider.Get()
                                       .WhereEquals("RelationshipNameID", GetRelationshipNameId())
                                       .WhereEquals("LeftNodeID", TreeNode.NodeID).Count;
    }

    #endregion
}
