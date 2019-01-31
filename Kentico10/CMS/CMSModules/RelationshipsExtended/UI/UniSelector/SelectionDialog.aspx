<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_RelationshipExtended_UI_UniSelector_SelectionDialog" Title="Selection dialog"
    ValidateRequest="false" Theme="default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"  CodeFile="SelectionDialog.aspx.cs" %>

<%@ Register Src="Controls/SelectionDialog.ascx" TagName="CustomSelectionDialog" TagPrefix="cms" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
    <cms:CustomSelectionDialog runat="server" ID="selectionDialog" IsLiveSite="false" />
</asp:Content>