using System;
using System.Data;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Relationships;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class Compiled_CMSModules_RelationshipsExtended_UI_Templates_Relateddocs_List : CMSPropertiesPage
{
    public Compiled_CMSModules_RelationshipsExtended_UI_Templates_Relateddocs_List() { }
    #region "Properties"

    public string RelationshipName
    {
        get
        {
            return ValidationHelper.GetString(UIContext.GetValue("RelationshipName"), "");
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
            return ValidationHelper.GetString(UIContext.GetValue("DirectionMode"), "");
        }
        set
        {
            UIContext.SetValue("DirectionMode", value);
        }
    }

    public bool AllowSwitchSides
    {
        get
        {
            return ValidationHelper.GetBoolean(UIContext.GetValue("AllowSwitchSides"), false);
        }
        set
        {
            UIContext.SetValue("AllowSwitchSides", value);
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

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Content", "Properties.RelatedDocs"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.RelatedDocs");
        }

        // Initialize node
        relatedDocuments.TreeNode = Node;
        relatedDocuments.AllowSwitchSides = AllowSwitchSides;
        relatedDocuments.RelationshipName = RelationshipName;
        relatedDocuments.DefaultSide = (DirectionMode == "LeftNode");
        CurrentMaster.PanelContent.CssClass = "";
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SetPropertyTab(TAB_RELATEDDOCS);

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

                menuElem.AddExtraAction(new HeaderAction()
                {
                    Enabled = enabled,
                    Text = GetString("relationship.addrelateddocs"),
                    RedirectUrl = "~/CMSModules/Content/CMSDesk/Properties/Relateddocs_Add.aspx?nodeid=" + NodeID
                });
            }
        }

        pnlContent.Enabled = !DocumentManager.ProcessingAction;
    }

    #endregion
}