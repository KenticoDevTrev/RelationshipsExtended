using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.Relationships;
using CMS.SiteProvider;
using CMS.UIControls;
using RelationshipsExtended;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeNode = CMS.DocumentEngine.TreeNode;
public partial class Compiled_CMSModules_RelationshipsExtended_Controls_Relateddocs_List : CMSAbstractUIWebpart
{
    public Compiled_CMSModules_RelationshipsExtended_Controls_Relateddocs_List() { }
    #region "Properties"

    public string RelationshipName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RelationshipName"), "");
        }
        set
        {
            UIContext.SetValue("RelationshipName", value);
        }
    }

    public string DirectionMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DirectionMode"), "");
        }
        set
        {
            SetValue("DirectionMode", value);
        }
    }

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

    public int CurrentNodeID
    {
        get
        {

            int NodeID = ValidationHelper.GetInteger(GetValue("CurrentNodeID"), 0);
            if (BindOnPrimaryNodeOnly)
            {
                return RelHelper.GetPrimaryNodeID(NodeID);
            }
            else
            {
                return NodeID;
            }
        }
        set
        {
            SetValue("CurrentNodeID", value);
        }
    }

    public string CurrentCulture
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CurrentCulture"), "");
        }
        set
        {
            SetValue("CurrentCulture", value);
        }
    }

    public bool AllowSwitchSides
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowSwitchSides"), false);
        }
        set
        {
            SetValue("AllowSwitchSides", value);
        }
    }
    public bool DisableSort
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisableSort"), false);
        }
        set
        {
            SetValue("DisableSort", value);
        }
    }

    public TreeNode Node
    {
        get
        {
            return new DocumentQuery().WhereEquals("NodeID", (CurrentNodeID > 0 ? CurrentNodeID : QueryHelper.GetInteger("nodeid", 0))).Culture((!string.IsNullOrWhiteSpace(CurrentCulture) ? CurrentCulture : QueryHelper.GetString("culture", "en-US"))).CombineWithDefaultCulture(true).FirstOrDefault();
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
            relatedDocuments.AllowedPageTypes = value;
        }
    }

    public string ToolTipFormat
    {
        get
        {
            // Sometimes it tries to processes the Macros instead of just passing the value.
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(UIContext.Data.GetValue("ToolTipFormat"), ""), ValidationHelper.GetString(GetValue("ToolTipFormat"), ""));
        }
        set
        {
            SetValue("ToolTipFormat", value);
            relatedDocuments.ToolTipFormat = value;
        }
    }

    public string DisplayNameFormat
    {
        get
        {
            // Sometimes it tries to processes the Macros instead of just passing the value.
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(UIContext.Data.GetValue("DisplayNameFormat"), ""), ValidationHelper.GetString(GetValue("DisplayNameFormat"), ""));
        }
        set
        {
            SetValue("DisplayNameFormat", value);
            relatedDocuments.DisplayNameFormat = value;
        }
    }

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMessages;
        }
    }

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {

        base.OnInit(e);

        // Initialize node
        relatedDocuments.TreeNode = Node;
        relatedDocuments.AllowSwitchSides = AllowSwitchSides;
        relatedDocuments.RelationshipName = RelationshipName;
        relatedDocuments.DisableSort = DisableSort;
        // Odd as it is, the DefaultSide should be false if the current node is left...
        relatedDocuments.DefaultSide = (DirectionMode == "RightNode");
        relatedDocuments.AllowedPageTypes = AllowedPageTypes;
        relatedDocuments.DisplayNameFormat = DisplayNameFormat;
        relatedDocuments.ToolTipFormat = ToolTipFormat;
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        // Check if any relationship exists
        DataSet dsRel = RelationshipNameInfoProvider.GetRelationshipNames("RelationshipAllowedObjects LIKE '%" + ObjectHelper.GROUP_DOCUMENTS + "%' AND RelationshipNameID IN (SELECT RelationshipNameID FROM CMS_RelationshipNameSite WHERE SiteID = " + SiteContext.CurrentSiteID + ")", null, 1, "RelationshipNameID");
        if (DataHelper.DataSourceIsEmpty(dsRel))
        {
            relatedDocuments.Visible = false;
            ShowInformation(ResHelper.GetString("relationship.norelationship"));
        }
        else
        {
            if (Node != null)
            {
                bool enabled = true;

                // Check modify permissions
                if (!DocumentUIHelper.CheckDocumentPermissions(Node, PermissionsEnum.Modify))
                {
                    relatedDocuments.Enabled = enabled = false;
                }
            }
        }

        pnlContent.Enabled = !DocumentManager.ProcessingAction;
    }

    #endregion
}