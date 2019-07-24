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
    /// FooBarInfo data container class.
    /// </summary>
    [Serializable]
    public partial class FooBarInfo : AbstractInfo<FooBarInfo>, IOrderableBaseInfo, IBindingBaseInfo
    {
        #region "Type information"
		
        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.foobar";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooBarInfoProvider), OBJECT_TYPE, "Demo.FooBar", "FooBarID", null, null, null, null, null, null, "FooID", "demo.foo")
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("BarID", "demo.bar", ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            OrderColumn = "FooBarOrder",
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.TouchParent,
                IncludeToSynchronizationParentDataSet = IncludeToParentEnum.Complete, 
            },
        };

        protected override WhereCondition GetSiblingsWhereCondition()
        {
            return new WhereCondition(string.Format("FooID = {0}", this.FooID));
        }

        #endregion


        #region "Properties"

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
        /// BarID
        /// </summary>
        [DatabaseField]
        public virtual int BarID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BarID"), 0);
            }
            set
            {
                SetValue("BarID", value);
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
                return ValidationHelper.GetInteger(GetValue("FooBarOrder"), 0);
            }
            set
            {
                SetValue("FooBarOrder", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            FooBarInfoProvider.DeleteFooBarInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            FooBarInfoProvider.SetFooBarInfo(this);
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
            return "FooID";
        }

        public string BoundObjectReferenceColumnName()
        {
            return "BarID";
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected FooBarInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty FooBarInfo object.
        /// </summary>
        public FooBarInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new FooBarInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public FooBarInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}