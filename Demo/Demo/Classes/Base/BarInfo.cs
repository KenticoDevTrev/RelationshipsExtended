using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(BarInfo), BarInfo.OBJECT_TYPE)]

namespace Demo
{
    /// <summary>
    /// Data container class for <see cref="BarInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BarInfo : AbstractInfo<BarInfo, IBarInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.bar";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BarInfoProvider), OBJECT_TYPE, "Demo.Bar", nameof(BarID), nameof(BarLastModified), nameof(BarGuid), nameof(BarCodeName), nameof(BarName), null, null, null, null)
        {
            ModuleName = "demo",
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
        /// Bar ID.
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
        /// Bar name.
        /// </summary>
        [DatabaseField]
        public virtual string BarName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BarName"), String.Empty);
            }
            set
            {
                SetValue("BarName", value);
            }
        }


        /// <summary>
        /// Bar code name.
        /// </summary>
        [DatabaseField]
        public virtual string BarCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BarCodeName"), String.Empty);
            }
            set
            {
                SetValue("BarCodeName", value);
            }
        }


        /// <summary>
        /// Bar guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BarGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BarGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BarGuid", value);
            }
        }


        /// <summary>
        /// Bar last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BarLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BarLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BarLastModified", value);
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
        protected BarInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BarInfo"/> class.
        /// </summary>
        public BarInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BarInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BarInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}