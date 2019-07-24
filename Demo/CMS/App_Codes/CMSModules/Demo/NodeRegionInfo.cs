using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(NodeRegionInfo), NodeRegionInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="NodeRegionInfo"/>.
    /// </summary>
    [Serializable]
    public partial class NodeRegionInfo : AbstractInfo<NodeRegionInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.noderegion";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeRegionInfoProvider), OBJECT_TYPE, "Demo.NodeRegion", "NodeRegionID", null, null, null, null, null, null, "NodeID", "cms.node")
        {
			ModuleName = "Demo",
			TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>() 
			{
			    new ObjectDependency("RegionCategoryID", "cms.category", ObjectDependencyEnum.Binding), 
            },
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.None
            }
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
        /// Node ID
        /// </summary>
		[DatabaseField]
        public virtual int NodeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeID"), 0);
            }
            set
            {
                SetValue("NodeID", value);
            }
        }


        /// <summary>
        /// Region category ID
        /// </summary>
		[DatabaseField]
        public virtual int RegionCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("RegionCategoryID"), 0);
            }
            set
            {
                SetValue("RegionCategoryID", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            NodeRegionInfoProvider.DeleteNodeRegionInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            NodeRegionInfoProvider.SetNodeRegionInfo(this);
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