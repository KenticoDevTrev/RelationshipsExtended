using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(FooBazInfo), FooBazInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="FooBazInfo"/>.
    /// </summary>
    [Serializable]
    public partial class FooBazInfo : AbstractInfo<FooBazInfo, IFooBazInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.foobaz";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooBazInfoProvider), OBJECT_TYPE, "Demo.FooBaz", nameof(FooBazID), null, null, null, null, null, null, nameof(FooBazFooID), FooInfo.OBJECT_TYPE)
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            #region "Binding Properties"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(FooBazBazID), BazInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
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
        /// Foo baz ID
        /// </summary>
        [DatabaseField]
        public virtual int FooBazID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBazID"), 0);
            }
            set
            {
                SetValue("FooBazID", value);
            }
        }


        /// <summary>
        /// Foo baz foo ID
        /// </summary>
        [DatabaseField]
        public virtual int FooBazFooID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBazFooID"), 0);
            }
            set
            {
                SetValue("FooBazFooID", value);
            }
        }


        /// <summary>
        /// Foo baz baz ID
        /// </summary>
        [DatabaseField]
        public virtual int FooBazBazID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooBazBazID"), 0);
            }
            set
            {
                SetValue("FooBazBazID", value);
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


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected FooBazInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="FooBazInfo"/> class.
        /// </summary>
        public FooBazInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="FooBazInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public FooBazInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}