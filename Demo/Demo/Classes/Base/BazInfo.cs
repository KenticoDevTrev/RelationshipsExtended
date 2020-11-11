using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(BazInfo), BazInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="BazInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BazInfo : AbstractInfo<BazInfo, IBazInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.baz";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BazInfoProvider), OBJECT_TYPE, "Demo.Baz", nameof(BazID), nameof(BazLastModified), nameof(BazGuid), nameof(BazCodeName), nameof(BazName), null, null, null, null)
        {
            ModuleName = "Demo",
            TouchCacheDependencies = true,
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization,
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Adds the custom class into a new category in the Global objects section of the staging tree
                    new ObjectTreeLocation(GLOBAL, "Demo")
                },
            },
        };


        /// <summary>
        /// Baz ID.
        /// </summary>
        [DatabaseField]
        public virtual int BazID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BazID"), 0);
            }
            set
            {
                SetValue("BazID", value);
            }
        }


        /// <summary>
        /// Baz name.
        /// </summary>
        [DatabaseField]
        public virtual string BazName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BazName"), String.Empty);
            }
            set
            {
                SetValue("BazName", value);
            }
        }


        /// <summary>
        /// Baz code name.
        /// </summary>
        [DatabaseField]
        public virtual string BazCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BazCodeName"), String.Empty);
            }
            set
            {
                SetValue("BazCodeName", value);
            }
        }


        /// <summary>
        /// Baz guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BazGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BazGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BazGuid", value);
            }
        }


        /// <summary>
        /// Baz last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BazLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BazLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BazLastModified", value);
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
        protected BazInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BazInfo"/> class.
        /// </summary>
        public BazInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BazInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BazInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}