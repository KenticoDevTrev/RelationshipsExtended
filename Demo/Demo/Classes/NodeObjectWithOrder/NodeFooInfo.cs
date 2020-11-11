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
    /// Data container class for <see cref="NodeFooInfo"/>.
    /// </summary>
    [Serializable]
    public partial class NodeFooInfo : AbstractInfo<NodeFooInfo, INodeFooInfoProvider>, IOrderableBaseInfo, IBindingBaseInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.nodefoo";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeFooInfoProvider), OBJECT_TYPE, "Demo.NodeFoo", nameof(NodeFooID), null, null, null, null, null, null, nameof(NodeFooNodeID), "cms.node")
        {
            ModuleName = "demo",
            TouchCacheDependencies = true,
            #region "Binding and ordering requirement"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(NodeFooFooID), FooInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            OrderColumn = nameof(NodeFooOrder),
            SynchronizationSettings =
            {
                // Logging is handled separately since it's a Node-binding
                LogSynchronization = SynchronizationTypeEnum.None
            },
            #endregion
        };


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
        /// Node foo node ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeFooNodeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeFooNodeID"), 0);
            }
            set
            {
                SetValue("NodeFooNodeID", value);
            }
        }


        /// <summary>
        /// Node foo foo ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeFooFooID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeFooFooID"), 0);
            }
            set
            {
                SetValue("NodeFooFooID", value);
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
                return ValidationHelper.GetInteger(GetValue("NodeFooOrder"), -1);
            }
            set
            {
                SetValue("NodeFooOrder", value);
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

        #region "Relationship Extended Required Overrides"

        protected override WhereCondition GetSiblingsWhereCondition()
        {
            return new WhereCondition($"{nameof(NodeFooNodeID)} = {NodeFooNodeID}");
        }

        public string ParentObjectReferenceColumnName()
        {
            return nameof(NodeFooNodeID);
        }

        public string BoundObjectReferenceColumnName()
        {
            return nameof(NodeFooFooID);
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

        #endregion

        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected NodeFooInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="NodeFooInfo"/> class.
        /// </summary>
        public NodeFooInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="NodeFooInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public NodeFooInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}