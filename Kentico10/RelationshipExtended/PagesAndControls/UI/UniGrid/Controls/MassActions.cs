using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.UIControls;


/// <summary>
/// Control for displaying and handling the UniGrid mass actions.
/// </summary>
public partial class Compiled_CMSModules_RelationshipsExtended_UI_UniGrid_Controls_MassActions : MassActions
{
    public Compiled_CMSModules_RelationshipsExtended_UI_UniGrid_Controls_MassActions() { }
    protected override CMSDropDownList ScopeDropDown
    {
        get
        {
            return drpScope;
        }
    }


    protected override CMSDropDownList ActionDropDown
    {
        get
        {
            return drpAction;
        }
    }


    protected override Control Messages
    {
        get
        {
            return divMessages;
        }
    }


    protected override Control ConfirmationButton
    {
        get
        {
            return btnOk;
        }
    }
}
