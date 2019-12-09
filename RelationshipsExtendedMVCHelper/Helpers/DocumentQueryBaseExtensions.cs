using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationshipsExtended
{
    /// <summary>
    /// 
    /// </summary>
    public static class DocumentQueryExtensions
    {
        /// <summary>
        /// Allows for Related Pages lookup using Ordering on non-MultpleDocumentQuery queries.  The given Node must be on the "left" hand side in this case for ordering.
        /// </summary>
        /// <param name="baseQuery">The Base Document Query</param>
        /// <param name="nodeGuid">The NodeGuid</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        public static void InRelationWithOrder(this DocumentQuery baseQuery, Guid nodeGuid, string relationshipName = null)
        {
            // Get the RelationshipID and NodeID
            int? RelationshipNameID = GetRelationshipNameID(relationshipName);
            int? NodeID = GetNodeID(nodeGuid);
            if (!NodeID.HasValue)
            {
                return;
            }

            // Add the Inner Join with proper alias formatting
            if (RelationshipNameID.HasValue)
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("RelationshipNameID", RelationshipNameID.Value).WhereEquals("LeftNodeID", NodeID.Value)));
            }
            else
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("LeftNodeID", NodeID.Value)));
            }

            // add the by the Relationship Order
            baseQuery.OrderBy("RelationshipOrder");
        }

        /// <summary>
        /// Allows for Related Pages lookup using Ordering on non-MultpleDocumentQuery queries.  The given Node must be on the "left" hand side in this case for ordering.
        /// </summary>
        /// <param name="baseQuery">The Base Document Query</param>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        public static void InRelationWithOrder(this DocumentQuery baseQuery, int nodeID, string relationshipName = null)
        {
            // Get the RelationshipID and NodeID
            int? RelationshipNameID = GetRelationshipNameID(relationshipName);

            // Add the Inner Join with proper alias formatting
            if (RelationshipNameID.HasValue)
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("RelationshipNameID", RelationshipNameID.Value).WhereEquals("LeftNodeID", nodeID)));
            }
            else
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("LeftNodeID", nodeID)));
            }

            // add the by the Relationship Order
            baseQuery.OrderBy("RelationshipOrder");
        }


        /// <summary>
        /// Allows for Related Pages lookup using Ordering on non-MultpleDocumentQuery queries.  The given Node must be on the "left" hand side in this case for ordering.
        /// </summary>
        /// <param name="baseQuery">The Base Document Query</param>
        /// <param name="nodeGuid">The NodeGuid</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        public static void InRelationWithOrder(this MultiDocumentQuery baseQuery, Guid nodeGuid, string relationshipName = null)
        {
            // Get the RelationshipID and NodeID
            int? RelationshipNameID = GetRelationshipNameID(relationshipName);
            int? NodeID = GetNodeID(nodeGuid);
            if (!NodeID.HasValue)
            {
                return;
            }

            // Add the Inner Join with proper alias formatting
            if (RelationshipNameID.HasValue)
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("RelationshipNameID", RelationshipNameID.Value).WhereEquals("LeftNodeID", NodeID.Value)));
            }
            else
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("LeftNodeID", NodeID.Value)));
            }

            // add the by the Relationship Order
            baseQuery.OrderBy("RelationshipOrder");
        }

        /// <summary>
        /// Allows for Related Pages lookup using Ordering on non-MultpleDocumentQuery queries.  The given Node must be on the "left" hand side in this case for ordering.
        /// </summary>
        /// <param name="baseQuery">The Base Document Query</param>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        public static void InRelationWithOrder(this MultiDocumentQuery baseQuery, int nodeID, string relationshipName = null)
        {
            // Get the RelationshipID and NodeID
            int? RelationshipNameID = GetRelationshipNameID(relationshipName);

            // Add the Inner Join with proper alias formatting
            if (RelationshipNameID.HasValue)
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("RelationshipNameID", RelationshipNameID.Value).WhereEquals("LeftNodeID", nodeID)));
            }
            else
            {
                baseQuery.Source((QuerySource s) => s.InnerJoin(new QuerySourceTable("CMS_Relationship"), new WhereCondition("NodeID = RightNodeID").WhereEquals("LeftNodeID", nodeID)));
            }

            // add the by the Relationship Order
            baseQuery.OrderBy("RelationshipOrder");
        }

        /// <summary>
        /// Gets the NodeID from NodeGuid
        /// </summary>
        /// <param name="nodeGuid"></param>
        /// <returns>The NodeID</returns>
        private static int? GetNodeID(Guid nodeGuid)
        {
            return CacheHelper.Cache(cs =>
            {
                TreeNode node = DocumentHelper.GetDocuments()
                .WhereEquals("NodeGuid", nodeGuid)
                .CombineWithAnyCulture()
                .CombineWithDefaultCulture()
                .Columns("NodeID")
                .FirstOrDefault();

                if (node != null)
                {
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency($"nodeid|{node.NodeID}");
                    }
                }

                return node?.NodeID;
            }, new CacheSettings(1440, "RelExtendedGetNodeID", nodeGuid));
        }

        private static int? GetRelationshipNameID(string RelationshipName)
        {
            return CacheHelper.Cache(cs =>
            {
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency($"cms.relationshipname|byname|{RelationshipName}");
                }
                var Relationship = RelationshipNameInfoProvider.GetRelationshipNameInfo(RelationshipName);
                return Relationship?.RelationshipNameId;
            }, new CacheSettings(1440, "RelExtendedGetRelationshipNameID", RelationshipName));
        }
    }
}
