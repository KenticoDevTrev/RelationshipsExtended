using System.Web.UI.WebControls;

using CMS.UIControls;


public partial class Compiled_CMSModules_RelationshipsExtended_UI_PageElements_BreadCrumbs : Breadcrumbs
{
    public Compiled_CMSModules_RelationshipsExtended_UI_PageElements_BreadCrumbs() { }
    #region "Properties"

    /// <summary>
    /// Help control.
    /// </summary>
    public HelpControl Help
    {
        get
        {
            return helpBreadcrumbs;
        }
    }


    /// <summary>
    /// Placeholder into which the breadcrumb items will be generated.
    /// </summary>
    protected override PlaceHolder BreadcrumbsContainer
    {
        get
        {
            return plcBreadcrumbs;
        }
    }

    #endregion
}