using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;
using RelationshipsExtended.Interfaces;

[assembly: RegisterObjectType(typeof(NodeBazInfo), NodeBazInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="NodeBazInfo"/>.
    /// </summary>
    [Serializable]
    public partial class NodeBazInfo : AbstractInfo<NodeBazInfo, INodeBazInfoProvider>, IBindingBaseInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.nodebaz";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(NodeBazInfoProvider), OBJECT_TYPE, "Demo.NodeBaz", nameof(NodeBazID), null, null, null, null, null, null, nameof(NodeBazNodeID), "cms.node")
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            #region "Binding requirement"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(NodeBazBazID), BazInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            SynchronizationSettings =
            {
                // Logging is handled separately
                LogSynchronization = SynchronizationTypeEnum.None
            },
            #endregion
        };


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
        /// Node baz node ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeBazNodeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeBazNodeID"), 0);
            }
            set
            {
                SetValue("NodeBazNodeID", value);
            }
        }


        /// <summary>
        /// Node baz baz ID
        /// </summary>
        [DatabaseField]
        public virtual int NodeBazBazID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("NodeBazBazID"), 0);
            }
            set
            {
                SetValue("NodeBazBazID", value);
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
            return nameof(NodeBazNodeID);
        }

        public string BoundObjectReferenceColumnName()
        {
            return nameof(NodeBazBazID);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected NodeBazInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="NodeBazInfo"/> class.
        /// </summary>
        public NodeBazInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="NodeBazInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public NodeBazInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}