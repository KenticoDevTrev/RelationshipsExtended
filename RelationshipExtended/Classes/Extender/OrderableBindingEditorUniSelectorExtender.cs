using CMS;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.UIControls;
using CMS.UIControls.UniGridConfig;
using RelationshipsExtended;
using RelationshipsExtended.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Action = CMS.UIControls.UniGridConfig.Action;

[assembly: RegisterCustomClass("OrderableBindingEditorUniSelectorExtender", typeof(OrderableBindingEditorUniSelectorExtender))]

namespace RelationshipsExtended
{
    public class OrderableBindingEditorUniSelectorExtender : ControlExtender<UniSelector>
    {
        public string BindingObjectType
        {
            get
            {
                return ValidationHelper.GetString(Control.Attributes["BindingObjectType"], "");
            }
        }

        public string ObjectType
        {
            get
            {
                return ValidationHelper.GetString(Control.Attributes["ObjectType"], "");
            }
        }

        public int CorrectObjectID
        {
            get
            {
                return ValidationHelper.GetInteger(Control.Attributes["CorrectObjectID"], -1);
            }
        }

        /// <summary>
        /// Initializes the control, this looks to see if the Binding Object type is of type IOrderableBaseInfo and IBindingBaseInfo 
        /// </summary>
        public override void OnInit()
        {
            if (!string.IsNullOrWhiteSpace(BindingObjectType))
            {
                // Get the Binding and base Object so we can detect if they are orderable or not.
                var BindingObjectFactory = new InfoObjectFactory(BindingObjectType);
                var BindingObjectSingleton = (BindingObjectFactory != null && BindingObjectFactory.Singleton != null ? BindingObjectFactory.Singleton : null);

                var ObjectFactory = new InfoObjectFactory(ObjectType);
                var ObjectSingleton = (ObjectFactory != null && ObjectFactory.Singleton != null ? ObjectFactory.Singleton : null);

                if (BindingObjectSingleton != null && ObjectSingleton != null && BindingObjectSingleton is IOrderableBaseInfo && BindingObjectSingleton is IBindingBaseInfo)
                {
                    // Set the new Grid with actions
                    Control.GridName = "~/CMSModules/RelationshipsExtended/Grids/ControlItemList.xml";
                    Control.UniGrid.GridName = "~/CMSModules/RelationshipsExtended/Grids/ControlItemList.xml";
                    Control.UniGrid.LoadXmlConfiguration();

                    // Add the ordering to the DataSet's Table, then use a TableView to order by that.  Hacky but works.
                    Control.UniGrid.OnAfterRetrieveData += UniGrid_OnAfterRetrieveData;
                    // Ensures that it retrieves all the rows found so it can order them manually
                    Control.UniGrid.ApplyPageSize = false;

                    // Ensure no sorting.
                    if (Control.UniGrid.GridOptions == null)
                    {
                        Control.UniGrid.GridOptions = new UniGridOptions();
                    }
                    Control.UniGrid.GridOptions.AllowSorting = false;

                    // Catch the Move events
                    Control.UniGrid.OnAction += UniGrid_OnAction;

                    // Get ColumnID of Object it's referencing for the Grid Actions at various points in the rendering, since this seems to get lost frequently.
                    SetActions();
                    Control.UniGrid.PreRender += UniGrid_PreRender;
                    Control.UniGrid.Load += UniGrid_Load;
                    Control.UniGrid.OnAfterDataReload += UniGrid_OnAfterDataReload;

                    // Setup Field and Script for Move command.
                    AddCustomMoveHiddenField();
                    RegisterCmdScripts();

                    // Handle current page and size if in URL parameter
                    if (!Control.Page.IsPostBack && !string.IsNullOrWhiteSpace(URLHelper.GetUrlParameter(RequestContext.RawURL, "OrderableUniPage")))
                    {
                        int CurrentPage = ValidationHelper.GetInteger(URLHelper.GetUrlParameter(RequestContext.RawURL, "OrderableUniPage"), 1);
                        int CurrentPageSize = ValidationHelper.GetInteger(URLHelper.GetUrlParameter(RequestContext.RawURL, "OrderableUniPageSize"), -1);
                        if (Control.UniGrid.Pager != null)
                        {
                            Control.UniGrid.Pager.CurrentPage = CurrentPage;
                            if (CurrentPageSize > 0)
                            {
                                Control.UniGrid.Pager.CurrentPageSize = CurrentPageSize;
                            }
                        }
                    }
                }
            }
        }

        

        #region "Ordering Methods"

        /// <summary>
        /// Handles the Grid Actions, sadly can't use the default #move #moveup and #movedown because this is technically displaying the Object, not the binding object.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="actionArgument"></param>
        private void UniGrid_OnAction(string actionName, object actionArgument)
        {
            var BindingObjectFactory = new InfoObjectFactory(BindingObjectType);
            var BindingObjectSingleton = BindingObjectFactory.Singleton;
            var BindingObj = new ObjectQuery(BindingObjectType)
                           .WhereEquals(((IBindingBaseInfo)BindingObjectSingleton).ParentObjectReferenceColumnName(), CorrectObjectID)
                           .WhereEquals(((IBindingBaseInfo)BindingObjectSingleton).ChildObjectReferenceColumnName(), actionArgument).FirstOrDefault();

            var ObjectFactory = new InfoObjectFactory(ObjectType);
            var ObjectSingleton = (ObjectFactory != null && ObjectFactory.Singleton != null ? ObjectFactory.Singleton : null);

            bool RefreshData = false;
            switch (actionName)
            {
                case "custom_move":
                    // Get Custom Move Hidden Field value
                    foreach (HiddenField hidField in GetControlsOfType<HiddenField>(Control.UniGrid))
                    {
                        if (hidField.ID == "hdnCustomMoveArgs")
                        {
                            string[] Values = hidField.Value.Split(':');
                            if (Values.Length == 3)
                            {
                                int ObjectID = ValidationHelper.GetInteger(Values[0], 0);
                                int OrigPosition = ValidationHelper.GetInteger(Values[1], 0);
                                int NewPosition = ValidationHelper.GetInteger(Values[2], 0);
                                BindingObj = new ObjectQuery(BindingObjectType)
                                    .WhereEquals(((IBindingBaseInfo)BindingObjectSingleton).ParentObjectReferenceColumnName(), CorrectObjectID)
                                    .WhereEquals(((IBindingBaseInfo)BindingObjectSingleton).ChildObjectReferenceColumnName(), ObjectID).FirstOrDefault();
                                ((IOrderableBaseInfo)BindingObj).SetObjectOrderRelative(NewPosition - OrigPosition);
                            }
                        }
                    }
                    break;
                case "custom_moveup":
                    ((IOrderableBaseInfo)BindingObj).MoveObjectUp();
                    try
                    {
                        // If the current item is the "first" item on the current page, refresh as it may now be on another page
                        var UpRows = ((DataSet)Control.UniGrid.DataSource).Tables[0].Rows;
                        if (ValidationHelper.GetInteger(UpRows[0][((BaseInfo)ObjectSingleton).TypeInfo.IDColumn], -1) == ValidationHelper.GetInteger(actionArgument, -1)) {
                            RefreshData = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Service.Resolve<IEventLogService>().LogException("OrderableBindingEditorUniSelectorExtender", "ActionRefreshError", ex, additionalMessage: "Something went wrong trying to detect if binding UI should be refreshed, please ensure both Binding Object and Bound Object types are properly set.");
                    }
                    break;
                case "custom_movedown":
                    ((IOrderableBaseInfo)BindingObj).MoveObjectDown();
                    try
                    {
                        // If the current item is the "last" item on the current page, refresh as it may now be on another page
                        var DownRows = ((DataSet)Control.UniGrid.DataSource).Tables[0].Rows;
                        if (ValidationHelper.GetInteger(DownRows[DownRows.Count - 1][((BaseInfo)ObjectSingleton).TypeInfo.IDColumn], -1) == ValidationHelper.GetInteger(actionArgument, -1)) {
                            RefreshData = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Service.Resolve<IEventLogService>().LogException("OrderableBindingEditorUniSelectorExtender", "ActionRefreshError", ex, additionalMessage: "Something went wrong trying to detect if binding UI should be refreshed, please ensure both Binding Object and Bound Object types are properly set.");
                    }
                    break;
            }

            // If an element is moved off the current page, the grid needs to be refreshed, and the only way to seem to get the UI to update is to totally refresh the page
            if (RefreshData)
            {
                string Url = RequestContext.RawURL;
                if(Control.UniGrid.Pager != null) { 
                    int CurrentPage = Control.UniGrid.Pager.CurrentPage;
                    int CurrentPageSize = Control.UniGrid.Pager.CurrentPageSize;
                    Url = URLHelper.AddParameterToUrl(Url, "OrderableUniPage", CurrentPage.ToString());
                    Url = URLHelper.AddParameterToUrl(Url, "OrderableUniPageSize", CurrentPageSize.ToString());
                }
                URLHelper.ResponseRedirect(Url, true);
            }

        }

        /// <summary>
        /// Dynamically adds the hidden field that is used for the custom move, reduces need to duplicate the UniGrid control.
        /// </summary>
        private void AddCustomMoveHiddenField()
        {
            // Find the other hidden fields in the UniSelector and add this one next to it
            HiddenField hdnCustomMoveArgs = new HiddenField()
            {
                ID = "hdnCustomMoveArgs",
                Value = ""
            };
            foreach (HiddenField hidField in GetControlsOfType<HiddenField>(Control.UniGrid))
            {
                if (hidField.ID == "hidCmdArg")
                {
                    hidField.Parent.Controls.Add(hdnCustomMoveArgs);
                    return;
                }
            }
        }

        /// <summary>
        /// Custom logic to get the onmousedown InitMove to work on the custom moving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniGrid_PreRender(object sender, EventArgs e)
        {
            foreach (var cmsActionButtonControl in GetControlsOfType<CMSGridActionButton>(Control.UniGrid))
            {
                if (cmsActionButtonControl.CommandName == "custom_move")
                {
                    IButtonControl buttonControl = (IButtonControl)cmsActionButtonControl;
                    if (buttonControl != null)
                    {
                        UniGrid uniGridControl = Control.UniGrid;
                        AttributeCollection attributes = ((WebControl)buttonControl).Attributes;
                        attributes["onmousedown"] = string.Concat(uniGridControl.GetJSModule(), ".initMove(", ScriptHelper.GetString(buttonControl.CommandArgument), "); return false;");
                        attributes["onclick"] = "return false;";
                        cmsActionButtonControl.OnClientClick = "return false;";
                        cmsActionButtonControl.CssClass += " no-click";
                    }
                }
            }

        }

        /// <summary>
        /// If actions exist, make sure the Command Argument is set to the Object's ID Column Name so it properly renders the ID value
        /// </summary>
        private void SetActions()
        {
            if (Control.UniGrid.GridActions != null)
            {
                var ObjectFactory = new InfoObjectFactory(ObjectType);
                var ObjectSingleton = (ObjectFactory != null && ObjectFactory.Singleton != null ? ObjectFactory.Singleton : null);

                Control.UniGrid.GridActions.Parameters = ((BaseInfo)ObjectSingleton).TypeInfo.IDColumn;
                foreach (Action action in Control.UniGrid.GridActions.Actions)
                {
                    action.CommandArgument = ((BaseInfo)ObjectSingleton).TypeInfo.IDColumn;
                }
            }
        }

        /// <summary>
        /// Register the Move Command Script, this leverages a customized UniGrid.js module to work with the new custom_move property
        /// </summary>
        protected void RegisterCmdScripts()
        {
            List<HiddenField> HiddenFields = GetControlsOfType<HiddenField>(Control.UniGrid).ToList();
            bool MoveActionFound = (Control.UniGrid.GridActions == null ? false : Control.UniGrid.GridActions.Actions.OfType<Action>().Any<Action>((Action action) => action.Name.Equals("custom_move", StringComparison.OrdinalIgnoreCase)));
            object variable = new
            {
                id = Control.UniGrid.ClientID,
                uniqueId = Control.UniGrid.UniqueID,
                hdnCmdNameId = HiddenFields.Where(x => x.ID == "hidCmdName").First().ClientID,
                hdnCmdArgId = HiddenFields.Where(x => x.ID == "hidCmdArg").First().ClientID,
                hdnSelHashId = HiddenFields.Where(x => x.ID == "hidSelectionHash").First().ClientID,
                hdnSelId = HiddenFields.Where(x => x.ID == "hidSelection").First().ClientID,
                // This one is a custom field for our custom move logic
                hdnCustomMoveArgs = HiddenFields.Where(x => x.ID == "hdnCustomMoveArgs").First().ClientID,

                gridId = Control.UniGrid.GridView.ClientID,
                resetSelection = false,
                allowSorting = true
            };
            ScriptHelper.EnsurePostbackMethods(Control.UniGrid);
            ScriptHelper.RegisterModule(Control.UniGrid, "RelationshipsExtended/UniGrid", variable);
            if (MoveActionFound)
            {
                ScriptHelper.RegisterJQueryUI(Control.Page, true);
            }

            ScriptHelper.RegisterStartupScript(Control.Page, Control.UniGrid.GetType(), "KillMoveClick", "$cmsj(document).ready(function() { $cmsj('.no-click').unbind('click'); });", true);
        }

        /// <summary>
        /// Since the normal Binding UniGrid only shows a summary of the Object that is bound, it does nto have the built in
        /// Order that would be on the Binding Object.  So I'm programatically adding on an Order Column, then returning the 
        /// Items in that order using a Table View.
        /// </summary>
        /// <param name="ds">The Dataset</param>
        /// <returns>The ordered data set</returns>
        private DataSet UniGrid_OnAfterRetrieveData(DataSet ds)
        {
            // Get the object and binding object's information (typeinfo) and their referencing column names.
            var BindingObjectFactory = new InfoObjectFactory(BindingObjectType);
            var BindingObjectSingleton = (BindingObjectFactory != null && BindingObjectFactory.Singleton != null ? BindingObjectFactory.Singleton : null);

            var ObjectFactory = new InfoObjectFactory(ObjectType);
            var ObjectSingleton = (ObjectFactory != null && ObjectFactory.Singleton != null ? ObjectFactory.Singleton : null);

            string ParentObjectReference = ((IBindingBaseInfo)BindingObjectSingleton).ParentObjectReferenceColumnName();
            string BoundObjectReference = ((IBindingBaseInfo)BindingObjectSingleton).ChildObjectReferenceColumnName();
            string ObjectIDColumn = ((BaseInfo)ObjectSingleton).TypeInfo.IDColumn;
            string OrderColumn = ((BaseInfo)BindingObjectSingleton).TypeInfo.OrderColumn;
            string DisplayNameColumn = ((BaseInfo)BindingObjectSingleton).TypeInfo.DisplayNameColumn;

            // Just sort the data by the order
            if (!ds.Tables[0].Columns.Contains("OrderableObjectUniSelector_Order"))
            {
                ds.Tables[0].Columns.Add("OrderableObjectUniSelector_Order", typeof(int));
            }

            // Go through each entry and find the proper Order and assign it.
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var OrderItem = new ObjectQuery(BindingObjectType)
                    .WhereEquals(ParentObjectReference, CorrectObjectID)
                    .WhereEquals(BoundObjectReference, dr[ObjectIDColumn])
                    .FirstOrDefault();
                if (OrderItem != null)
                {
                    dr["OrderableObjectUniSelector_Order"] = ValidationHelper.GetInteger(OrderItem.GetValue(OrderColumn), 0);
                }
                else
                {
                    dr["OrderableObjectUniSelector_Order"] = 999999999;
                }
            }

            // Sort by the Order and return the new table, now sorted properly.
            DataView DV = new DataView(ds.Tables[0]);
            DV.Sort = "[OrderableObjectUniSelector_Order] asc";
            DataTable dt = DV.ToTable();

            // Now that the items are ordered, we need to limit the results to the Page and Page Size
            // Get the current Page and page size, adjusting if that page no longer exists due to record deletion
            int PageSize = 10;
            int CurrentPage = 1;
            if (Control.UniGrid.Pager != null)
            {
                PageSize = Control.UniGrid.Pager.CurrentPageSize;
                CurrentPage = Control.UniGrid.Pager.CurrentPage;
            }
            DataTable newDT = dt.Clone();
            if (dt.Rows.Count > 0)
            {
                while (CurrentPage > 1 && dt.Rows.Count <= (CurrentPage - 1) * PageSize)
                {
                    CurrentPage--;
                }
            }
            // This event triggers twice, once on the full data, and once on the page-only data.  If page-only data, do NOT reset the page (unless the count is 0)
            if (Control.UniGrid.Pager != null && (ds.Tables[0].Rows.Count == Control.UniGrid.PagerForceNumberOfResults || ds.Tables[0].Rows.Count == 0))
            {
                Control.UniGrid.Pager.CurrentPage = CurrentPage;
            }

            // Now remove any records that are not in the current page
            int LowerBound = (CurrentPage - 1) * PageSize;
            int UpperBound = LowerBound + PageSize;
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                if (r >= LowerBound && r < UpperBound)
                {
                    newDT.ImportRow(dt.Rows[r]);
                }
            }

            // Lastly add this new sorted and 'trimmed' table to the results.
            ds.Tables.Remove(ds.Tables[0]);
            ds.Tables.Add(newDT);
            return ds;
        }

        private void UniGrid_OnAfterDataReload()
        {
            SetActions();
        }

        private void UniGrid_Load(object sender, EventArgs e)
        {
            SetActions();
        }

        #endregion



        /// <summary>
        /// Helper Method to get recursively find controls of a given type, using this to get various Control IDs and other items on the UniGrid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetControlsOfType<T>(Control root)
     where T : Control
        {
            var t = root as T;
            if (t != null)
                yield return t;

            var container = root as Control;
            if (container != null)
                foreach (Control c in container.Controls)
                    foreach (var i in GetControlsOfType<T>(c))
                        yield return i;
        }

    }
}
