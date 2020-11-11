using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;
using RelationshipsExtended.Interfaces;

[assembly: RegisterObjectType(typeof(FooBarInfo), FooBarInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="FooBarInfo"/>.
    /// </summary>
    [Serializable]
    public partial class FooBarInfo : AbstractInfo<FooBarInfo, IFooBarInfoProvider>, IBindingBaseInfo, IOrderableBaseInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.foobar";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooBarInfoProvider), OBJECT_TYPE, "Demo.Foobar", nameof(FooBarID), null, null, null, null, null, null, nameof(FooBarFooID), FooInfo.OBJECT_TYPE)
        {
            ModuleName = "demo",
            TouchCacheDependencies = true,
            #region "Binding and ordering requirement"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(FooBarBarID), BarInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            OrderColumn = nameof(FooBarOrder),
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.TouchParent,
                IncludeToSynchronizationParentDataSet = IncludeToParentEnum.Complete,
            },
            #endregion
        };

        protected override WhereCondition GetSiblingsWhereCondition()
        {
            return new WhereCondition($"{nameof(FooBarFooID)} = {FooBarFooID}"); ;
        }

        /// <summary>
        /// Foo bar ID
        /// </summary>
        [DatabaseField]
        public virtual int FooBarID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBarID"), 0);
            }
            set
            {
                SetValue("FooBarID", value);
            }
        }


        /// <summary>
        /// Foo bar foo ID
        /// </summary>
        [DatabaseField]
        public virtual int FooBarFooID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBarFooID"), 0);
            }
            set
            {
                SetValue("FooBarFooID", value);
            }
        }


        /// <summary>
        /// Foo bar bar ID
        /// </summary>
        [DatabaseField]
        public virtual int FooBarBarID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBarBarID"), 0);
            }
            set
            {
                SetValue("FooBarBarID", value);
            }
        }


        /// <summary>
        /// Foo bar order
        /// </summary>
        [DatabaseField]
        public virtual int FooBarOrder
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBarOrder"), -1);
            }
            set
            {
                SetValue("FooBarOrder", value);
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
            return nameof(FooBarFooID);
        }

        public string BoundObjectReferenceColumnName()
        {
            return nameof(FooBarBarID);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected FooBarInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="FooBarInfo"/> class.
        /// </summary>
        public FooBarInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="FooBarInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public FooBarInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}