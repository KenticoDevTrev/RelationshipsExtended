using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;
using RelationshipsExtended.Interfaces;
[assembly: RegisterObjectType(typeof(NodeFooInfo), NodeFooInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// NodeFooInfo data container class.
    /// </summary>
    [Serializable]
    public partial class NodeFooInfo : AbstractInfo<NodeFooInfo>, IOrderableBaseInfo, IBindingBaseInfo
    {
        #region "Type information"
		
        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.nodefoo";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeFooInfoProvider), OBJECT_TYPE, "Demo.NodeFoo", "NodeFooID", null, null, null, null, null, null, "NodeID", "cms.node")
        {
			ModuleName = "Demo",
			TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>() 
			{
			    new ObjectDependency("FooID", "demo.foo", ObjectDependencyEnum.Binding), 
            },
            IsBinding = true,
            OrderColumn = "NodeFooOrder",
            SynchronizationSettings =
            {
                // Logging is handled separately
                LogSynchronization = SynchronizationTypeEnum.None
            },
        };

        #endregion


        #region "Properties"

        /// <summary>
        /// Node foo ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeFooID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeFooID"), 0);
            }
            set
            {
                SetValue("NodeFooID", value);
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
        /// Foo ID
        /// </summary>
        [DatabaseField]
        public virtual int FooID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooID"), 0);
            }
            set
            {
                SetValue("FooID", value);
            }
        }


        /// <summary>
        /// Node foo order
        /// </summary>
        [DatabaseField]
        public virtual int NodeFooOrder
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeFooOrder"), 0);
            }
            set
            {
                SetValue("NodeFooOrder", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            NodeFooInfoProvider.DeleteNodeFooInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            NodeFooInfoProvider.SetNodeFooInfo(this);
        }


        public void SetObjectOrder(int Order)
        {
            Generalized.SetObjectOrder(Order);
            SetObject();
        }

        public void SetObjectOrderRelative(int PositionChange)
        {
            Generalized.SetObjectOrder(PositionChange, true);
            SetObject();
        }

        public void MoveObjectUp()
        {
            Generalized.MoveObjectUp();
            SetObject();
        }

        public void MoveObjectDown()
        {
            Generalized.MoveObjectDown();
            SetObject();
        }

        public string ParentObjectReferenceColumnName()
        {
            return "NodeID";
        }

        public string BoundObjectReferenceColumnName()
        {
            return "FooID";
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected NodeFooInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty NodeFooInfo object.
        /// </summary>
        public NodeFooInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new NodeFooInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public NodeFooInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}