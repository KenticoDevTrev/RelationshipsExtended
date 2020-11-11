using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;
using CMS.Taxonomy;
using RelationshipsExtended.Interfaces;

[assembly: RegisterObjectType(typeof(NodeRegionInfo), NodeRegionInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="NodeRegionInfo"/>.
    /// </summary>
    [Serializable]
    public partial class NodeRegionInfo : AbstractInfo<NodeRegionInfo, INodeRegionInfoProvider>, IBindingBaseInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.noderegion";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeRegionInfoProvider), OBJECT_TYPE, "Demo.NodeRegion", nameof(NodeRegionID), null, null, null, null, null, null, nameof(NodeRegionNodeID), "cms.node")
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            #region "Binding requirement"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(NodeRegionCategoryID), CategoryInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            SynchronizationSettings =
            {
                // Handled separately
                LogSynchronization = SynchronizationTypeEnum.None
            }
            #endregion
        };


        /// <summary>
        /// Node region ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeRegionID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeRegionID"), 0);
            }
            set
            {
                SetValue("NodeRegionID", value);
            }
        }


        /// <summary>
        /// Node region node ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeRegionNodeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeRegionNodeID"), 0);
            }
            set
            {
                SetValue("NodeRegionNodeID", value);
            }
        }


        /// <summary>
        /// Node region category ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeRegionCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeRegionCategoryID"), 0);
            }
            set
            {
                SetValue("NodeRegionCategoryID", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }

        public string ParentObjectReferenceColumnName()
        {
            return nameof(NodeRegionNodeID);
        }

        public string BoundObjectReferenceColumnName()
        {
            return nameof(NodeRegionCategoryID);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected NodeRegionInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="NodeRegionInfo"/> class.
        /// </summary>
        public NodeRegionInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="NodeRegionInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public NodeRegionInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}