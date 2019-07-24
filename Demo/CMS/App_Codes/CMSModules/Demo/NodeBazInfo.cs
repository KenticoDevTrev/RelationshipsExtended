using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(NodeBazInfo), NodeBazInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// NodeBazInfo data container class.
    /// </summary>
    [Serializable]
    public partial class NodeBazInfo : AbstractInfo<NodeBazInfo>
    {
        #region "Type information"
		
        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.nodebaz";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeBazInfoProvider), OBJECT_TYPE, "Demo.NodeBaz", "NodeBazID", null, null, null, null, null, null, "NodeID", "cms.node")
        {
			ModuleName = "Demo",
			TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>() 
			{
			    new ObjectDependency("BazID", "demo.baz", ObjectDependencyEnum.Binding), 
            },
            IsBinding = true,
            SynchronizationSettings =
            {
                // Logging is handled separately
                LogSynchronization = SynchronizationTypeEnum.None
            },
        };

        #endregion


        #region "Properties"

        /// <summary>
        /// Node baz ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeBazID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeBazID"), 0);
            }
            set
            {
                SetValue("NodeBazID", value);
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
        /// Baz ID
        /// </summary>
        [DatabaseField]
        public virtual int BazID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BazID"), 0);
            }
            set
            {
                SetValue("BazID", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            NodeBazInfoProvider.DeleteNodeBazInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            NodeBazInfoProvider.SetNodeBazInfo(this);
        }

        #endregion


        #region "Constructors"
		
		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected NodeBazInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty NodeBazInfo object.
        /// </summary>
        public NodeBazInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new NodeBazInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public NodeBazInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}