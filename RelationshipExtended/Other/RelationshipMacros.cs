using CMS;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Relationships;
using RelationshipsExtended;
using System;
using CMS.DataEngine;
using CMS.SiteProvider;
using System.Linq;
using CMS.Core;

[assembly: RegisterExtension(typeof(RelationshipMacroMethods), typeof(RelationshipsExtendedMacroNamespace))]
namespace RelationshipsExtended
{
    public class RelationshipMacroMethods : MacroMethodContainer
    {

        [MacroMethod(typeof(string), "Returns the URL to create a new page of the specified type at the specified location.", 2)]
        [MacroMethodParam(0, "ClassName", typeof(string), "The class name of the page type that will be created.")]
        [MacroMethodParam(1, "ParentNodeAlias", typeof(string), "The parent node alias that the page will be inserted at.")]
        [MacroMethodParam(2, "CurrentCulture", typeof(string), "The document culture, will default to en-US if not provided.")]
        [MacroMethodParam(3, "CurrentSiteName", typeof(string), "The Site Name selection for the Related Node Site.")]
        public static object GetNewPageLink(EvaluationContext context, params object[] parameters)
        {
            try
            {
                
                if (parameters.Length >= 2)
                {
                    string ClassName = ValidationHelper.GetString(parameters[0], "");
                    string ParentNodeAlias = ValidationHelper.GetString(parameters[1], "");
                    string Culture = ValidationHelper.GetString(parameters.Length > 2 ? parameters[2] : "en-US", "en-US");
                    string SiteName = ValidationHelper.GetString(parameters.Length > 3 ? parameters[3] : SiteContext.CurrentSiteName, SiteContext.CurrentSiteName);
                    string SiteDomain = "";
                    if(SiteName.Equals("#currentsite", StringComparison.InvariantCultureIgnoreCase))
                    {
                        SiteName = SiteContext.CurrentSiteName;
                    }
                    if(!string.IsNullOrWhiteSpace(SiteName) && !SiteName.Equals(SiteContext.CurrentSiteName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        SiteDomain = (System.Web.HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://") + SiteInfo.Provider.Get(SiteName).DomainName.Trim('/');
                    }
                    if (!string.IsNullOrWhiteSpace(ClassName) && !string.IsNullOrWhiteSpace(ParentNodeAlias))
                    {
                        return CacheHelper.Cache<string>(cs =>
                        {
                            int ClassID = DataClassInfoProvider.GetDataClassInfo(ClassName).ClassID;
                            int NodeID = new DocumentQuery().Path(ParentNodeAlias, PathTypeEnum.Single).FirstOrDefault().NodeID;
                            return SiteDomain+URLHelper.ResolveUrl(string.Format("~/CMSModules/Content/CMSDesk/Edit/Edit.aspx?action=new&classid={0}&parentnodeid={1}&parentculture={2}", ClassID, NodeID, Culture));
                        }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), ClassName, ParentNodeAlias, Culture, SiteName));
                    }
                }
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("RelationshipMacros", "GetNewPageLinkError", ex);
            }
            return "#";
        }

        [MacroMethod(typeof(bool), "Determines if the current relationship tab should be visible (if the current document is either on the left or right side of this relationship).", 0)]
        public static object RelationshipTabIsVisible(EvaluationContext context, params object[] parameters)
        {
            string LeftSideMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsLeftSideMacro"), "");
            string RightSideMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsRightSideMacro"), "");
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                var NewResolver = MacroContext.CurrentResolver.CreateChild();
                NewResolver.SetNamedSourceData("CurrentDocument", currentNode);
                bool IsLeft = ValidationHelper.GetBoolean(NewResolver.ResolveMacros(LeftSideMacro), false);
                bool IsRight = ValidationHelper.GetBoolean(NewResolver.ResolveMacros(RightSideMacro), false);
                return IsLeft || IsRight;
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Determines if the selector should be visible (is left or is right on a switchable non adhoc relationship).", 0)]
        public static object RelationshipSelectorIsVisible(EvaluationContext context, params object[] parameters)
        {
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                bool AllowSwitchSides = ValidationHelper.GetBoolean(UIContext.Current.Data.GetValue("AllowSwitchSides"), false);
                return IsLeft() || (!IsAdHocRelationship() && AllowSwitchSides && IsRight());
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Determines if the Relationship Listing (Editable) should be visible (if the current document is a Left side, or is Right Side and Is not an ad-hoc relationship).", 0)]
        public static object RelationshipListingEditableIsVisible(EvaluationContext context, params object[] parameters)
        {
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                return (IsLeft() || (IsRight() && !IsAdHocRelationship()));
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Determines if the Relationship Listing (View Only) should be visible (if the current document is a Left side, or is Right Side and Is not an ad-hoc relationship).", 0)]
        public static object RelationshipListingReadOnlyIsVisible(EvaluationContext context, params object[] parameters)
        {
            if (CurrentNode() != null)
            {
                bool AllowSwitchSides = ValidationHelper.GetBoolean(UIContext.Current.Data.GetValue("AllowSwitchSides"), false);
                return !((IsLeft() || (IsRight() && !IsAdHocRelationship())));
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Returns if the current document is allowed on the Left side of the relationship (using the Left Side Macro).", 0)]
        public static object CurrentNodeIsLeft(EvaluationContext context, params object[] parameters)
        {
            return IsLeft();
        }

        [MacroMethod(typeof(bool), "Returns if the current document is allowed on the Right side of the relationship (using the Right Side Macro).", 0)]
        public static object CurrentNodeIsRight(EvaluationContext context, params object[] parameters)
        {
            return IsRight();
        }

        [MacroMethod(typeof(bool), "Returns if the current relationship (RelationshipName) is an AdHoc Relationship", 0)]
        public static object IsAdHocRelationship(EvaluationContext context, params object[] parameters)
        {
            return IsAdHocRelationship();
        }

        /// <summary>
        /// Determines if the current page fits the IsLeftSideMacro for the UI Page.
        /// </summary>
        /// <returns>True if it is a Left side relationship</returns>
        private static bool IsLeft()
        {
            string LeftSideMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsLeftSideMacro"), "");
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                var NewResolver = MacroContext.CurrentResolver.CreateChild();
                NewResolver.SetNamedSourceData("CurrentDocument", currentNode);
                return ValidationHelper.GetBoolean(NewResolver.ResolveMacros(LeftSideMacro), false);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the current page fits the IsRightSideMacro for the UI Page.
        /// </summary>
        /// <returns>True if it is a Right side relationship</returns>
        private static bool IsRight()
        {
            string LeftRightMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsRightSideMacro"), "");
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                var NewResolver = MacroContext.CurrentResolver.CreateChild();
                NewResolver.SetNamedSourceData("CurrentDocument", currentNode);
                return ValidationHelper.GetBoolean(NewResolver.ResolveMacros(LeftRightMacro), false);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the current Tree Node based on the NodeID parameter passed to UI elements
        /// </summary>
        /// <returns>The Tree Node that the current page belongs to</returns>
        private static TreeNode CurrentNode()
        {
            int NodeID = QueryHelper.GetInteger("NodeID", -1);
            string Culture = QueryHelper.GetString("culture", "en-US");
            if (NodeID > 0)
            {
                return CacheHelper.Cache<TreeNode>(cs =>
                {
                    TreeNode currentNode = new DocumentQuery().WhereEquals("NodeID", NodeID).Culture(Culture).CombineWithDefaultCulture(true).FirstOrDefault();
                    if (currentNode != null && cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency("nodeid|" + NodeID);
                    }
                    return currentNode;
                }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "RelationshipMacro", "CurrentNode", NodeID, Culture));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if the current relationship is an AdHoc relationship based on the UI Property RelationshipName
        /// </summary>
        /// <returns>True if the current relationship is an ad hoc relationship</returns>
        private static bool IsAdHocRelationship()
        {
            string RelationshipName = ValidationHelper.GetString(UIContext.Current.Data.GetValue("RelationshipName"), "");
            return CacheHelper.Cache<bool>(cs =>
            {
                RelationshipNameInfo relationshipObj = RelationshipNameInfo.Provider.Get(RelationshipName);

                if (relationshipObj != null && cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("cms.relationshipname|byid|" + relationshipObj.RelationshipNameId);
                }
                return relationshipObj != null ? relationshipObj.RelationshipNameIsAdHoc : false;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "RelationshipMacro", "IsAdHocRelationship", RelationshipName));
        }

    }

}
