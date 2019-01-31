<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="OrderableBindingEditItemWithNodeSupport.ascx.cs"
    Inherits="CMSModules_RelationshipExtended_Controls_UIControls_NodeBindingEditItem" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/OrderableUniSelector/UniSelector.ascx" TagPrefix="RelationshipsExtended" TagName="OrderableUniSelector" %>
<RelationshipsExtended:OrderableUniSelector runat="server" ID="editElem" IsLiveSite="false" SelectionMode="Multiple"  />
<style>
.cms-bootstrap .btn.icon-only.js-custom_move {
    cursor: move;
}
</style>