<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_FormControls_Relationships_SelectRelationshipNames" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/UniSelector/BaseUniSelector.ascx" TagName="UniSelector" TagPrefix="cms" %>
<cms:CMSUpdatePanel ID="pnlUpdate" runat="server">
    <ContentTemplate>
        <cms:UniSelector ID="uniSelector" runat="server" ObjectType="cms.relationshipname"
            AllowAll="false" SelectionMode="SingleDropDownList" DisplayNameFormat="{%RelationshipDisplayName%}" />
    </ContentTemplate>
</cms:CMSUpdatePanel>
