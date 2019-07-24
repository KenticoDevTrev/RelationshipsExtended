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
    /// BarInfo data container class.
    /// </summary>
	[Serializable]
    public partial class BarInfo : AbstractInfo<BarInfo>
    {
        #region "Type information"

        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.bar";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BarInfoProvider), OBJECT_TYPE, "Demo.Bar", "BarID", "BarLastModified", "BarGuid", "BarCodeName", "BarDisplayName", null, null, null, null)
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

        #endregion


        #region "Properties"

        /// <summary>
        /// Bar ID
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
        /// Bar display name
        /// </summary>
        [DatabaseField]
        public virtual string BarDisplayName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BarDisplayName"), String.Empty);
            }
            set
            {
                SetValue("BarDisplayName", value);
            }
        }


        /// <summary>
        /// Bar code name
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
        /// Bar guid
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
        /// Bar last modified
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

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BarInfoProvider.DeleteBarInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BarInfoProvider.SetBarInfo(this);
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected BarInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty BarInfo object.
        /// </summary>
        public BarInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new BarInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public BarInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}