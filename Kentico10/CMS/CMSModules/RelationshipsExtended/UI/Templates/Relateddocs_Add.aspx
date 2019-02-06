<%@ Page Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_UI_Templates_Relateddocs_Add" Theme="Default"  MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/Controls/Relationships/AddRelatedDocument.ascx" TagName="AddRelatedDocument" TagPrefix="cms" %>
<asp:Content runat="server" ID="pnlContent" ContentPlaceHolderID="plcContent">
    <cms:AddRelatedDocument ID="addRelatedDocument" runat="server" IsLiveSite="false" />
</asp:Content>
