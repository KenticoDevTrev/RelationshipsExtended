﻿using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using CMS;
using CMS.DataEngine;
using CMS.Helpers;

[assembly: RegisterObjectType(typeof(TreeCategoryInfo), TreeCategoryInfo.OBJECT_TYPE)]

namespace CMS
{
    /// <summary>
    /// Data container class for <see cref="TreeCategoryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class TreeCategoryInfo : AbstractInfo<TreeCategoryInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "cms.treecategory";

        private static ObjectTypeInfo GetTypeInfo()
        {
            if (DataHelper.GetNotEmpty(SettingsKeyInfoProvider.GetValue(new SettingsKeyName("NodeCategoryStagingMode")), "WithDocument") == "WithDocument")
            {
                return new ObjectTypeInfo(typeof(TreeCategoryInfoProvider), OBJECT_TYPE, "CMS.TreeCategory", "TreeCategoryID", null, null, null, null, null, null, "NodeID", "cms.node")
                {
                    ModuleName = "RelationshipsExtended",
                    TouchCacheDependencies = true,
                    IsBinding = true,
                    DependsOn = new List<ObjectDependency>()
                    {
                        new ObjectDependency("CategoryID", "cms.category", ObjectDependencyEnum.Binding),
                    },
                            ImportExportSettings =
                    {
                        IsExportable = false
                    },
                            LogEvents = true,
                            RequiredObject = false,
                            SynchronizationSettings =
                    {
                        // Logging is handled separately
                        LogSynchronization = SynchronizationTypeEnum.None
                    },
                            ContinuousIntegrationSettings =
                    {
                        Enabled = true
                    }
                };
            }
            else
            {
                return new ObjectTypeInfo(typeof(TreeCategoryInfoProvider), OBJECT_TYPE, "CMS.TreeCategory", "TreeCategoryID", null, null, null, null, null, null, null, null)
                {
                    ModuleName = "RelationshipsExtended",
                    TouchCacheDependencies = true,
                    IsBinding = false,
                    DependsOn = new List<ObjectDependency>()
                    {
                        new ObjectDependency("CategoryID", "cms.category", ObjectDependencyEnum.Required),
                        new ObjectDependency("NodeID", "cms.node", ObjectDependencyEnum.Required)
                    },
                        ImportExportSettings =
                    {
                        IsExportable = false
                    },
                        LogEvents = true,
                        RequiredObject = false,
                        SynchronizationSettings =
                    {
                        // Logging is handled separately
                        LogSynchronization = SynchronizationTypeEnum.LogSynchronization,
                        ObjectTreeLocations = new List<ObjectTreeLocation>()
                        {
                            // Adds the custom class into a new category in the Global objects section of the staging tree
                            new ObjectTreeLocation(GLOBAL, "TreeCategories")
                        },
                    },
                        ContinuousIntegrationSettings =
                    {
                        Enabled = true
                    }
                };
            }
        }
        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = GetTypeInfo();

        /// <summary>
        /// Tree category ID
        /// </summary>
		[DatabaseField]
        public virtual int TreeCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("TreeCategoryID"), 0);
            }
            set
            {
                SetValue("TreeCategoryID", value);
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
            TreeCategoryInfoProvider.DeleteTreeCategoryInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            TreeCategoryInfoProvider.SetTreeCategoryInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected TreeCategoryInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="TreeCategoryInfo"/> class.
        /// </summary>
        public TreeCategoryInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="TreeCategoryInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public TreeCategoryInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}