using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.PortalEngine.Web.UI;
using CMS.Relationships;
using CMS.SiteProvider;
using CMS.UIControls;
using RelationshipsExtended;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeNode = System.Web.UI.WebControls.TreeNode;

public partial class Compiled_CMSModules_RelationshipsExtended_Controls_RelatedPage_Tree : CMSAbstractWebPart
{
    public Compiled_CMSModules_RelationshipsExtended_Controls_RelatedPage_Tree() { }
    public string AllowedPageTypes
    {
        get
        {
            return SqlHelper.EscapeQuotes(ValidationHelper.GetString(GetValue("AllowedPageTypes"), ""));
        }
        set
        {
            SetValue("AllowedPageTypes", value);
        }
    }

    public string WhereCondition
    {
        get
        {
            // Sometimes an error is thrown by passing a where condition, if so use the UI context.
            try
            {
                return ValidationHelper.GetString(GetValue("WhereCondition"), "");
            }
            catch (InvalidOperationException)
            {
                return ValidationHelper.GetString(UIContext.Data.GetValue("WhereConditionSelector"), "");
            }
        }
        set
        {
            SetValue("WhereCondition", value);
        }
    }

    public string RelationshipName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RelationshipName"), "");
        }
        set
        {
            SetValue("RelationshipName", value);
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

    private string RelatedNodeSite
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RelatedNodeSite"), "");
        }
        set
        {
            SetValue("RelatedNodeSite", value);
        }
    }

    private string RelatedNodeSiteName
    {
        get
        {
            switch (RelatedNodeSite)
            {
                case "#currentsite":
                    return SiteContext.CurrentSiteName;
                default:
                    return RelatedNodeSite;
            }
        }
    }


    public string DirectionMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DirectionMode"), "LeftNode");
        }
        set
        {
            SetValue("DirectionMode", value);
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

    public int MaxRelationships
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxRelationships"), -1);
        }
        set
        {
            SetValue("MaxRelationships", value);
        }
    }

    public string StartingPaths
    {
        get
        {
            return ValidationHelper.GetString(GetValue("StartingPaths"), "");
        }
        set
        {
            SetValue("StartingPath", value);
        }
    }

    public bool RemoveUnselectableChildTrees
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("RemoveUnselectableChildTrees"), true);
        } set
        {
            SetValue("RemoveUnselectableChildTrees", value);
        }
    }

    private string[] StartingPathArray
    {
        get
        {
            return StartingPaths.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimEnd('%').TrimEnd('/')).ToArray();
        }
    }

    public string SelectionMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SelectionMode"), "Checkbox");
        }
        set
        {
            SetValue("SelectionMode", value);
        }
    }

    public string ToolTipFormat
    {
        get
        {
            // Use UI Context as this resolvse the macro for some reason
            return ValidationHelper.GetString(UIContext.Data.GetValue("ToolTipFormat"), "");
        }
        set
        {
            SetValue("ToolTipFormat", value);
        }
    }

    public string DisplayNameFormat
    {
        get
        {
            return ValidationHelper.GetString(UIContext.Data.GetValue("DisplayNameFormat"), "");
        }
        set
        {
            SetValue("DisplayNameFormat", value);
        }
    }

    private List<string> _ClassNames;
    private List<string> ClassNames
    {
        get
        {
            if (_ClassNames != null)
            {
                return _ClassNames;
            }
            _ClassNames = new List<string>(AllowedPageTypes.Split(";,|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            return _ClassNames;
        }
    }

    private bool AllowAllTypes
    {
        get
        {
            return ClassNames.Count() == 0;
        }
    }

    private List<int> AlreadySelectedNodes = new List<int>();
    private List<int> SelectableSelectedNodes = new List<int>();
    private bool FilterSelectableNodes = false;

    protected override void OnInit(EventArgs e)
    {
        // set direction initially if unset
        if (SessionHelper.GetValue("RelatedPageTreeDirection_" + CurrentNodeID + "_" + UIContext.ElementGuid) == null || !AllowSwitchSides)
        {
            SessionHelper.SetValue("RelatedPageTreeDirection_" + CurrentNodeID + "_" + UIContext.ElementGuid, DirectionMode);
            ddlCurrentNodeDirection.SelectedValue = "LeftNode";
        }
        ddlCurrentNodeDirection.SelectedValue = (string)SessionHelper.GetValue("RelatedPageTreeDirection_" + CurrentNodeID + "_" + UIContext.ElementGuid);
        ddlCurrentNodeDirection.Visible = AllowSwitchSides;

        SetupControl();

        base.OnInit(e);
    }

    private void SetupControl()
    {
        pageTree.LineImagesFolder = "~/CMSModules/RelationshipsExtended/Controls/RelatedCategories_Files";
        pageTree.NodeStyle.CssClass = "InputNode";
        pageTree.ShowLines = true;

        // Build a list of the pages
        var docQuery = new DocumentQuery().OrderBy("NodeLevel, NodeOrder");
        foreach (string Path in StartingPathArray)
        {
            docQuery.Path(Path, PathTypeEnum.Section);
        }
        if (!string.IsNullOrWhiteSpace(RelatedNodeSiteName))
        {
            docQuery.OnSite(RelatedNodeSiteName);
        }
        List<CMS.DocumentEngine.TreeNode> Nodes = docQuery.TypedResult.ToList();

        // Get existing selected nodes
        string where = string.Format("NodeClassID in (select ClassID from CMS_Class where ClassName in ('{0}'))",
            string.Join("','", AllowedPageTypes.Split(";| ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));

        // Split off Where condition for if they provide a Where to filter out other items.
        string AdditionalWhere = where;

        // Filter to show items not already selected
        if (ddlCurrentNodeDirection.SelectedValue == "LeftNode")
        {
            where = SqlHelper.AddWhereCondition(where,
            string.Format("({0})",
                string.Format("NodeID in (Select RightNodeID from CMS_Relationship where LeftNodeID = {1} and RelationshipNameID in (Select RelationshipNameID from CMS_RelationshipName where RelationshipName = '{0}'))",
            RelationshipName, CurrentNodeID)));
        }
        else
        {
            where = SqlHelper.AddWhereCondition(where,
            string.Format("({0})",
                string.Format("NodeID in (Select LeftNodeID from CMS_Relationship where RightNodeID = {1} and RelationshipNameID in (Select RelationshipNameID from CMS_RelationshipName where RelationshipName = '{0}'))",
            RelationshipName, CurrentNodeID)));
        }

        where = SqlHelper.AddWhereCondition(where, string.Format("NodeID <> {0}", CurrentNodeID));

        AlreadySelectedNodes = new DocumentQuery().Where(where).Columns("NodeID").Select(x => x.NodeID).ToList();

        // Exclude the current node, can't relate a node to itself.
        AlreadySelectedNodes.Add(CurrentNodeID);

        // If the WhereCondition is set, also add any Nodes that match this to "Already selected" so they can't be selected
        // Filter on the where condition if given
        FilterSelectableNodes = false;
        List<int> VisibleNodes = new List<int>();
        if (!string.IsNullOrWhiteSpace(WhereCondition))
        {
            AdditionalWhere = SqlHelper.AddWhereCondition(AdditionalWhere, WhereCondition);
            FilterSelectableNodes = true;
            SelectableSelectedNodes.AddRange(new DocumentQuery().Where(AdditionalWhere).Columns("NodeID").Select(x => x.NodeID).ToList());
        }

        pageTree.Nodes.Clear();

        TreeNode RootNode = new TreeNode("[Tree Root]", "0")
        {
            SelectAction = TreeNodeSelectAction.None
        };
        Dictionary<int, TreeNode> NodeIDToTreeNode = new Dictionary<int, TreeNode>();
        NodeIDToTreeNode.Add(0, RootNode);

        // Build the tree
        for (int i = 0; i < Nodes.Count(); i++)
        {
            var Node = Nodes[i];

            // Skip Root node
            if (string.IsNullOrWhiteSpace(Node.NodeName))
            {
                continue;
            }

            TreeNode newNode = CreateTreeNode(Node);
            if (!NodeIDToTreeNode.ContainsKey(Node.NodeID))
            {
                NodeIDToTreeNode.Add(Node.NodeID, newNode);
            }

            // Add to the parent if it exists, if it doesn't then add to root.
            if (NodeIDToTreeNode.ContainsKey(Node.NodeParentID))
            {
                NodeIDToTreeNode[Node.NodeParentID].ChildNodes.Add(newNode);
            }
            else
            {
                NodeIDToTreeNode[0].ChildNodes.Add(newNode);
            }
        }

        if(RemoveUnselectableChildTrees)
        {
            HideUnselectableChildren(RootNode);
        }

        // Add root
        pageTree.Nodes.Add(RootNode);

        if (SelectionMode == "Checkbox")
        {
            btnAdd.Visible = true;
        }
        else
        {
            btnAdd.Visible = false;
            pageTree.SelectedNodeChanged += PageTree_SelectedNodeChanged;
        }

    }

    private void HideUnselectableChildren(TreeNode RootNode)
    {
        List<TreeNode> ChildrenToRemove = new List<TreeNode>();
        foreach(TreeNode Child in RootNode.ChildNodes)
        {
            if(!HasSelectableChild(Child))
            {
                ChildrenToRemove.Add(Child);
            }
        }
        ChildrenToRemove.ForEach(x => RootNode.ChildNodes.Remove(x));
    }

    private bool HasSelectableChild(TreeNode ParentNode)
    {
        if(ParentNode.ChildNodes.Count == 0)
        {
            return ((RelatedPage_Tree_CustomTreeNode)ParentNode).PossiblySelectable;
        } else
        {
            bool SelectableChildFound = false;
            List<TreeNode> ChildrenToRemove = new List<TreeNode>();

            foreach (TreeNode Child in ParentNode.ChildNodes)
            {
                if (!HasSelectableChild(Child))
                {
                    ChildrenToRemove.Add(Child);
                } else { 
                    SelectableChildFound = true;
                }
            }
            // Remove children with no selections
            ChildrenToRemove.ForEach(x => ParentNode.ChildNodes.Remove(x));

            return SelectableChildFound || ((RelatedPage_Tree_CustomTreeNode)ParentNode).PossiblySelectable;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void PageTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        AddPagesToRelationship(new int[] { int.Parse(pageTree.SelectedNode.Value) });
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        AddPagesToRelationship(pageTree.CheckedNodes.Cast<TreeNode>().Select(x => int.Parse(x.Value)).ToArray());
    }

    protected void ddlCurrentNodeDirection_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Save new direction
        SessionHelper.SetValue("RelatedPageTreeDirection_" + CurrentNodeID + "_" + UIContext.ElementGuid, ddlCurrentNodeDirection.SelectedValue);

        // Preserve tree's selection level
        List<string> ExpandedTreeNodes = GetExpandedNodeValues(pageTree.Nodes[0]);
        ExpandedTreeNodes = ExpandedTreeNodes.Distinct().ToList();
        // Rebuild tree
        SetupControl();

        // Restore expansion
        foreach (TreeNode treeNode in pageTree.Nodes)
        {
            SetExpandedNode(ExpandedTreeNodes, treeNode);
        }
    }

    private List<string> GetExpandedNodeValues(TreeNode node)
    {
        List<string> NodeValues = new List<string>();
        if (node.Expanded.HasValue && node.Expanded.Value)
        {
            NodeValues.Add(node.Value);
        }
        foreach (TreeNode subNode in node.ChildNodes)
        {
            NodeValues.AddRange(GetExpandedNodeValues(subNode));
        }
        return NodeValues;
    }

    private void SetExpandedNode(List<string> ExpandedValues, TreeNode node)
    {
        if (ExpandedValues.Contains(node.Value))
        {
            node.Expanded = true;
        }
        foreach (TreeNode subNode in node.ChildNodes)
        {
            SetExpandedNode(ExpandedValues, subNode);
        }
    }

    private TreeNode CreateTreeNode(CMS.DocumentEngine.TreeNode Node)
    {
        string tooltip = null;
        string customName = null;
        if ((!string.IsNullOrWhiteSpace(ToolTipFormat) || !string.IsNullOrWhiteSpace(DisplayNameFormat)) && AllowedPageTypes.ToLower().Split(";,|".ToCharArray()).Contains(Node.NodeClassName.ToLower()))
        {
            MacroResolver NodeResolver = GetNodeMacroResolver(Node);
            if (!string.IsNullOrWhiteSpace(ToolTipFormat))
            {
                tooltip = NodeResolver.ResolveMacros(ToolTipFormat);
            }
            if (!string.IsNullOrWhiteSpace(DisplayNameFormat))
            {
                customName = NodeResolver.ResolveMacros(DisplayNameFormat);
            }
        }

        RelatedPage_Tree_CustomTreeNode newNode = new RelatedPage_Tree_CustomTreeNode((!string.IsNullOrWhiteSpace(customName) ? customName : Node.NodeName), Node.NodeID.ToString(), tooltip);
        if (AlreadySelectedNodes.Contains(Node.NodeID) || (FilterSelectableNodes && !SelectableSelectedNodes.Contains(Node.NodeID)))
        {
            newNode.SelectAction = TreeNodeSelectAction.None;
            newNode.ShowCheckBox = false;
            newNode.Text = newNode.Text;
            newNode.CssClass = "AlreadySelected";
            newNode.PossiblySelectable = AlreadySelectedNodes.Contains(Node.NodeID);
        }
        else
        {
            if ((AllowAllTypes || ClassNames.Contains(Node.ClassName)))
            {
                newNode.PossiblySelectable = true;
                newNode.CssClass = "Selectable";
                if (SelectionMode == "Checkbox")
                {
                    newNode.ShowCheckBox = true;
                    newNode.SelectAction = TreeNodeSelectAction.None;
                }
                else
                {
                    newNode.SelectAction = TreeNodeSelectAction.Select;
                }
            }
            else
            {
                newNode.SelectAction = TreeNodeSelectAction.None;
                newNode.CssClass = "NotSelectable";
            }
        }
        return newNode;
    }

    private MacroResolver GetNodeMacroResolver(CMS.DocumentEngine.TreeNode Node)
    {
        string Culture = URLHelper.GetQueryValue(Request.RawUrl, "culture");
        return CacheHelper.Cache<MacroResolver>(cs =>
        {
            MacroResolver resolver = MacroResolver.GetInstance();

            List<string> Columns = new List<string>();

            if (!string.IsNullOrWhiteSpace(ToolTipFormat))
            {
                Columns.AddRange(DataHelper.GetNotEmpty(MacroProcessor.GetMacros(ToolTipFormat, true), "NodeName").Split(';'));
            }
            if (!string.IsNullOrWhiteSpace(DisplayNameFormat))
            {
                Columns.AddRange(DataHelper.GetNotEmpty(MacroProcessor.GetMacros(DisplayNameFormat, true), "NodeName").Split(';'));
            }
            // Get data for this node and render it out
            DataSet FullData = new DocumentQuery(Node.ClassName)
                .WhereEquals("NodeID", Node.NodeID)
                .Columns(Columns)
                .Culture(Culture)
                .CombineWithDefaultCulture(true).Result;

            foreach (DataColumn item in FullData.Tables[0].Columns)
            {
                resolver.SetNamedSourceData(item.ColumnName, FullData.Tables[0].Rows[0][item.ColumnName]);
            }

            if (cs.Cached)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency("nodeid|" + Node.NodeID);
            }
            return resolver;
        }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "RelationshipTree_GetNodeMacroResolver", Node.NodeID, Culture, ToolTipFormat, DisplayNameFormat));
    }

    protected void AddPagesToRelationship(int[] SelectedNodeIDs)
    {
        int RelationshipNameID = RelationshipNameInfoProvider.GetRelationshipNameInfo(RelationshipName).RelationshipNameId;

        if (MaxRelationships > -1 && GetRelationshipCount() + SelectedNodeIDs.Length > MaxRelationships)
        {
            AddMessage(CMS.Base.Web.UI.MessageTypeEnum.Error, "Too many relationships, max allowed is " + MaxRelationships);
            return;
        }

        foreach (int NodeID in SelectedNodeIDs)
        {
            if (NodeID > 0)
            {
                if (ddlCurrentNodeDirection.SelectedValue == "LeftNode")
                {
                    RelationshipInfoProvider.AddRelationship(CurrentNodeID, NodeID, RelationshipNameID);
                }
                else
                {
                    RelationshipInfoProvider.AddRelationship(NodeID, CurrentNodeID, RelationshipNameID);
                }
            }
        }
        // Save direction
        SessionHelper.SetValue("RelatedPageTreeDirection_" + CurrentNodeID + "_" + UIContext.ElementGuid, ddlCurrentNodeDirection.SelectedValue);
        URLHelper.RefreshCurrentPage();
    }

    private int GetRelationshipCount()
    {
        int RelationshipNameID = RelationshipNameInfoProvider.GetRelationshipNameInfo(RelationshipName).RelationshipNameId;
        if (AllowSwitchSides)
        {
            return RelationshipInfoProvider.GetRelationships()
                                           .WhereEquals("RelationshipNameID", RelationshipNameID)
                                           .Where(string.Format("(LeftNodeID = {0} or RightNodeID = {0})", CurrentNodeID))
                                           .Count;
        }
        else
        {
            return RelationshipInfoProvider.GetRelationships()
                                           .WhereEquals("RelationshipNameID", RelationshipNameID)
                                           .WhereEquals(DirectionMode == "LeftNode" ? "LeftNodeID" : "RightNodeID", CurrentNodeID)
                                           .Count;
        }
    }
}

public class RelatedPage_Tree_CustomTreeNode : TreeNode
{
    public string CssClass;
    public string Style;
    public string ToolTip;
    public bool PossiblySelectable;
    public NameValueCollection Attributes { get; set; }

    public RelatedPage_Tree_CustomTreeNode(string text, string value, string ToolTip = null)
    {
        Text = text;
        Value = value;
        this.ToolTip = ToolTip;
        Attributes = new NameValueCollection();
    }
    protected override void RenderPreText(HtmlTextWriter writer)
    {
        writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
        writer.AddAttribute(HtmlTextWriterAttribute.Style, Style);
        if (!string.IsNullOrWhiteSpace(ToolTip))
        {
            // the HTML Writer already encodes attributes
            writer.AddAttribute("title", ToolTip);
        }
        foreach (string AttributeKey in Attributes.AllKeys)
        {
            writer.AddAttribute(AttributeKey, Attributes[AttributeKey]);
        }
        writer.RenderBeginTag(HtmlTextWriterTag.Div);
        base.RenderPreText(writer);
    }

    protected override void RenderPostText(HtmlTextWriter writer)
    {
        writer.RenderEndTag();
        base.RenderPostText(writer);
    }

}