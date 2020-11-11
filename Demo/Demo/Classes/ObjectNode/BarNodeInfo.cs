using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;
using RelationshipsExtended.Interfaces;

[assembly: RegisterObjectType(typeof(BarNodeInfo), BarNodeInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="BarNodeInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BarNodeInfo : AbstractInfo<BarNodeInfo, IBarNodeInfoProvider>, IBindingBaseInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.barnode";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BarNodeInfoProvider), OBJECT_TYPE, "Demo.BarNode", nameof(BarNodeID), null, null, null, null, null, null, nameof(BarNodeBarID), BarInfo.OBJECT_TYPE)
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            #region "Binding Properties"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(BarNodeNodeID), "cms.node", ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.TouchParent,
                IncludeToSynchronizationParentDataSet = IncludeToParentEnum.Complete,
            }
            #endregion
        };


        /// <summary>
        /// Bar node ID
        /// </summary>
        [DatabaseField]
        public virtual int BarNodeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BarNodeID"), 0);
            }
            set
            {
                SetValue("BarNodeID", value);
            }
        }


        /// <summary>
        /// Bar node bar ID
        /// </summary>
        [DatabaseField]
        public virtual int BarNodeBarID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BarNodeBarID"), 0);
            }
            set
            {
                SetValue("BarNodeBarID", value);
            }
        }


        /// <summary>
        /// Bar node node ID
        /// </summary>
        [DatabaseField]
        public virtual int BarNodeNodeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BarNodeNodeID"), 0);
            }
            set
            {
                SetValue("BarNodeNodeID", value);
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
            return nameof(BarNodeBarID);
        }

        public string BoundObjectReferenceColumnName()
        {
            return nameof(BarNodeNodeID);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BarNodeInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BarNodeInfo"/> class.
        /// </summary>
        public BarNodeInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="BarNodeInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BarNodeInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}