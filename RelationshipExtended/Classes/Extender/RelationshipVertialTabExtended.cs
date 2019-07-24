using CMS;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.UIControls;
using System.Xml;
using CMS.MacroEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using RelationshipsExtended;
[assembly: RegisterCustomClass("RelationshipVerticalTabExtender", typeof(RelationshipVerticalTabExtender))]

namespace RelationshipsExtended
{
    /// <summary>
    /// Content edit tabs control extender, will automatically hide Edit Relationship UIs that are neither LeftSide nor RightSide
    /// </summary>
    public class RelationshipVerticalTabExtender : UITabsExtender
    {
        /// <summary>
        /// Document manager
        /// </summary>
        public ICMSDocumentManager DocumentManager
        {
            get;
            set;
        }


        /// <summary>
        /// Initialization of tabs.
        /// </summary>
        public override void OnInitTabs()
        {
            var page = (CMSPage)Control.Page;

            // Setup the document manager
            DocumentManager = page.DocumentManager;

            ScriptHelper.RegisterScriptFile(Control.Page, "~/CMSModules/Content/CMSDesk/Properties/PropertiesTabs.js");

            Control.OnTabCreated += OnTabCreated;
        }


        protected void OnTabCreated(object sender, TabCreatedEventArgs e)
        {
            if (e.Tab == null)
            {
                return;
            }

            var tab = e.Tab;
            var element = e.UIElement;
            PageTemplateInfo UITemplate = PageTemplateInfoProvider.GetPageTemplateInfo(e.UIElement.ElementPageTemplateID);
            var manager = DocumentManager;
            var node = manager.Node;

            bool splitViewSupported = PortalContext.ViewMode != ViewModeEnum.EditLive;

            string elementName = element.ElementName.ToLowerCSafe();

            if (UITemplate.CodeName.ToLower().Contains("editrelationship"))
            {
                XmlDocument properties = new XmlDocument();
                properties.LoadXml(e.UIElement.ElementProperties);
                XmlNode LeftSideMacro = properties.SelectSingleNode("/Data[1]/IsLeftSideMacro[1]");
                XmlNode RightSideMacro = properties.SelectSingleNode("/Data[1]/IsRightSideMacro[1]");
                XmlNode AutoHide = properties.SelectSingleNode("/Data[1]/AutoHide[1]");

                if (AutoHide != null && ValidationHelper.GetBoolean(AutoHide.InnerText, false) && LeftSideMacro != null && RightSideMacro != null)
                {
                    MacroResolver pageResolver = MacroResolver.GetInstance();
                    // Get current node's class, then full document so it has related data.
                    int NodeID = ValidationHelper.GetInteger(URLHelper.GetQueryValue(RequestContext.RawURL, "nodeid"), 1);
                    string Culture = DataHelper.GetNotEmpty(URLHelper.GetQueryValue(RequestContext.RawURL, "culture"), "en-US");
                    TreeNode CurrentDocument = CacheHelper.Cache<TreeNode>(cs =>
                    {
                        TreeNode Document = new DocumentQuery().WhereEquals("NodeID", NodeID).Columns("ClassName").FirstObject;
                        Document = new DocumentQuery(Document.ClassName).WhereEquals("NodeID", NodeID).Culture(Culture).FirstObject;
                        if (cs.Cached)
                        {
                            cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { string.Format("node|{0}|{1}|{2}", Document.NodeSiteName, Document.NodeAliasPath, Culture,
                                PageTemplateInfo.OBJECT_TYPE + "|byid|" + e.UIElement.ElementPageTemplateID )});
                        }
                        return Document;
                    }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), NodeID, Culture, e.UIElement.ElementPageTemplateID));
                    pageResolver.SetNamedSourceData("CurrentDocument", CurrentDocument);
                    if (!(ValidationHelper.GetBoolean(pageResolver.ResolveMacros(LeftSideMacro.InnerText), true) || ValidationHelper.GetBoolean(pageResolver.ResolveMacros(RightSideMacro.InnerText), true)))
                    {
                        e.Tab = null;
                    }
                }
            }

            if (DocumentUIHelper.IsElementHiddenForNode(element, node))
            {
                e.Tab = null;
                return;
            }

            // Ensure split view mode
            if (splitViewSupported && PortalUIHelper.DisplaySplitMode)
            {
                tab.RedirectUrl = DocumentUIHelper.GetSplitViewUrl(tab.RedirectUrl);
            }
        }
    }
}