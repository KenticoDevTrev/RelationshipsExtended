<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/CMSFormControls/Custom/AdvancedManyToManySelector/AdvancedManyToManySelector.ascx.cs" Inherits="CMSFormControls_Custom_AdvancedManyToManySelector_AdvancedManyToManySelector" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <asp:Panel runat="server" ID="pnlPreviewOnly" Visible="false">This tool cannot be viewed in Preview since it requires configuration to operate.</asp:Panel>
    <!-- Uni selctor -->
    <cms:FormControl runat="server" ID="frmFormControl" FormControlName="Uni_selector" />
