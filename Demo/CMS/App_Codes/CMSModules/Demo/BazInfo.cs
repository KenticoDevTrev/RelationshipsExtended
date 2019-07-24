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
    /// BazInfo data container class.
    /// </summary>
	[Serializable]
    public partial class BazInfo : AbstractInfo<BazInfo>
    {
        #region "Type information"

        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.baz";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BazInfoProvider), OBJECT_TYPE, "Demo.Baz", "BazID", "BazLastModified", "BazGuid", "BazCodeName", "BazDisplayName", null, null, null, null)
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
        /// Baz ID
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
        /// Baz display name
        /// </summary>
        [DatabaseField]
        public virtual string BazDisplayName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BazDisplayName"), String.Empty);
            }
            set
            {
                SetValue("BazDisplayName", value);
            }
        }


        /// <summary>
        /// Baz code name
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
        /// Baz guid
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
        /// Baz last modified
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

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BazInfoProvider.DeleteBazInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BazInfoProvider.SetBazInfo(this);
        }

        #endregion


        #region "Constructors"

		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected BazInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty BazInfo object.
        /// </summary>
        public BazInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new BazInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public BazInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}