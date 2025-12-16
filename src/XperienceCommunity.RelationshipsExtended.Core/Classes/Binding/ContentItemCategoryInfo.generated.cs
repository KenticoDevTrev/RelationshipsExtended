using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using RelationshipsExtended;
using RelationshipsExtended.Interfaces;
using RelationshipsExtended.Enums;
using CMS.ContentEngine;

[assembly: RegisterObjectType(typeof(ContentItemCategoryInfo), ContentItemCategoryInfo.OBJECT_TYPE)]

namespace RelationshipsExtended
{
    /// <summary>
    /// Data container class for <see cref="ContentItemCategoryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class ContentItemCategoryInfo : AbstractInfo<ContentItemCategoryInfo, IContentItemCategoryInfoProvider>, IInfoWithId, IBindingInfo
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "relationshipsextended.contentitemcategory";


        public string ParentClassReferenceColumn() => nameof(ContentItemFields.ContentItemID);

        public string ChildClassReferenceColumn() => nameof(TagInfo.TagID);

        public IdentityType ParentReferenceType() => IdentityType.ID;

        public IdentityType ChildReferenceType() => IdentityType.ID;

        public string ParentClassName() => "CMS.ContentItem";

        public string ChildClassName() => TagInfo.OBJECT_TYPE;

        public string BindingTableName() => "RelationshipsExtended_ContentItemCategory";

        public string OrderColumn() => string.Empty;

        string IBindingBaseInfo.ObjectClassName() => OBJECT_TYPE;

        public string ParentObjectReferenceColumnName() => nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID);

        public string ChildObjectReferenceColumnName() => nameof(ContentItemCategoryInfo.ContentItemCategoryTagID);


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ContentItemCategoryInfoProvider), OBJECT_TYPE, "RelationshipsExtended.ContentItemCategory", "ContentItemCategoryID", null, null, null, null, null, "ContentItemCategoryContentItemID", "cms.contentitem")
        {
            TouchCacheDependencies = true,
            IsBinding = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("ContentItemCategoryTagID", "cms.tag", ObjectDependencyEnum.Binding),
            },
            LogEvents = false,
            RequiredObject = false,
            ContinuousIntegrationSettings =
            {
                Enabled = true
            }
            /*SynchronizationSettings =
            {
                // Logging is handled separately
                LogSynchronization = SynchronizationTypeEnum.None
            },*/

        };


        /// <summary>
        /// Content item category ID
        /// </summary>
        [DatabaseField]
        public virtual int ContentItemCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ContentItemCategoryID"), 0);
            }
            set
            {
                SetValue("ContentItemCategoryID", value);
            }
        }


        /// <summary>
        /// Content item category content item ID
        /// </summary>
        [DatabaseField]
        public virtual int ContentItemCategoryContentItemID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ContentItemCategoryContentItemID"), 0);
            }
            set
            {
                SetValue("ContentItemCategoryContentItemID", value);
            }
        }


        /// <summary>
        /// Content item category tag ID
        /// </summary>
        [DatabaseField]
        public virtual int ContentItemCategoryTagID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ContentItemCategoryTagID"), 0);
            }
            set
            {
                SetValue("ContentItemCategoryTagID", value);
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
        //protected ContentItemCategoryInfo(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}


        /// <summary>
        /// Creates an empty instance of the <see cref="ContentItemCategoryInfo"/> class.
        /// </summary>
        public ContentItemCategoryInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="ContentItemCategoryInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ContentItemCategoryInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}