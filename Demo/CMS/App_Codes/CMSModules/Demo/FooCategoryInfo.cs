using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(FooCategoryInfo), FooCategoryInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="FooCategoryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class FooCategoryInfo : AbstractInfo<FooCategoryInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.foocategory";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooCategoryInfoProvider), OBJECT_TYPE, "Demo.FooCategory", "FooCategoryID", null, null, null, null, null, null, "FooID", "demo.foo")
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("CategoryID", "cms.category", ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            LogEvents = false,
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.TouchParent,
                IncludeToSynchronizationParentDataSet = IncludeToParentEnum.Complete,
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Adds the custom class into a new category in the Global objects section of the staging tree
                    new ObjectTreeLocation(GLOBAL, "Demo")
                },
            },
        };


        /// <summary>
        /// Foo category ID
        /// </summary>
        [DatabaseField]
        public virtual int FooCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooCategoryID"), 0);
            }
            set
            {
                SetValue("FooCategoryID", value);
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
        /// Category ID
        /// </summary>
        [DatabaseField]
        public virtual int CategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CategoryID"), 0);
            }
            set
            {
                SetValue("CategoryID", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            FooCategoryInfoProvider.DeleteFooCategoryInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            FooCategoryInfoProvider.SetFooCategoryInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected FooCategoryInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="FooCategoryInfo"/> class.
        /// </summary>
        public FooCategoryInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="FooCategoryInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public FooCategoryInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}