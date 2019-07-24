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
    /// NodeRegionInfo data container class.
    /// </summary>
	[Serializable]
    public partial class NodeRegionInfo : AbstractInfo<NodeRegionInfo>
    {
        #region "Type information"

        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.noderegion";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeRegionInfoProvider), OBJECT_TYPE, "Demo.NodeRegion", "NodeRegionID", null, null, null, null, null, null, "NodeID", "cms.node")
        {
			ModuleName = "Demo",
			TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>() 
			{
			    new ObjectDependency("RegionCategoryID", "cms.category", ObjectDependencyEnum.Binding), 
            },
            SynchronizationSettings =
            {
                // Logging is handled separately
                LogSynchronization = SynchronizationTypeEnum.None
            },
        };

        #endregion


        #region "Properties"

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

        #endregion


        #region "Type based properties and methods"

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

        #endregion


        #region "Constructors"

		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected NodeRegionInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty NodeRegionInfo object.
        /// </summary>
        public NodeRegionInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new NodeRegionInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public NodeRegionInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}