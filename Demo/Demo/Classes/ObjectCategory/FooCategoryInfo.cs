using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;
using CMS.Taxonomy;
using RelationshipsExtended.Interfaces;

[assembly: RegisterObjectType(typeof(FooCategoryInfo), FooCategoryInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="FooCategoryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class FooCategoryInfo : AbstractInfo<FooCategoryInfo, IFooCategoryInfoProvider>, IBindingBaseInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.foocategory";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooCategoryInfoProvider), OBJECT_TYPE, "Demo.FooCategory", nameof(FooCategoryID), null, null, null, null, null, null, nameof(FooCategoryFooID), FooInfo.OBJECT_TYPE)
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            #region "Binding requirement"
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(FooCategoryCategoryID), CategoryInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
            },
            IsBinding = true,
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.TouchParent,
                IncludeToSynchronizationParentDataSet = IncludeToParentEnum.Complete
            },
            #endregion
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
        /// Foo category foo ID
        /// </summary>
        [DatabaseField]
        public virtual int FooCategoryFooID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooCategoryFooID"), 0);
            }
            set
            {
                SetValue("FooCategoryFooID", value);
            }
        }


        /// <summary>
        /// Foo category category ID
        /// </summary>
        [DatabaseField]
        public virtual int FooCategoryCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("FooCategoryCategoryID"), 0);
            }
            set
            {
                SetValue("FooCategoryCategoryID", value);
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
            return nameof(FooCategoryFooID);
        }

        public string BoundObjectReferenceColumnName()
        {
            return nameof(FooCategoryCategoryID);
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