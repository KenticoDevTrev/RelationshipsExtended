using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Demo;

[assembly: RegisterObjectType(typeof(FooInfo), FooInfo.OBJECT_TYPE)]
    
namespace Demo
{
    /// <summary>
    /// FooInfo data container class.
    /// </summary>
	[Serializable]
    public partial class FooInfo : AbstractInfo<FooInfo>
    {
        #region "Type information"

        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "demo.foo";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooInfoProvider), OBJECT_TYPE, "Demo.Foo", "FooID", "FooLastModified", "FooGuid", "FooCodeName", "FooDisplayName", null, null, null, null)
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
        /// Foo display name
        /// </summary>
        [DatabaseField]
        public virtual string FooDisplayName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("FooDisplayName"), String.Empty);
            }
            set
            {
                SetValue("FooDisplayName", value);
            }
        }


        /// <summary>
        /// Foo code name
        /// </summary>
        [DatabaseField]
        public virtual string FooCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("FooCodeName"), String.Empty);
            }
            set
            {
                SetValue("FooCodeName", value);
            }
        }


        /// <summary>
        /// Foo guid
        /// </summary>
        [DatabaseField]
        public virtual Guid FooGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("FooGuid"), Guid.Empty);
            }
            set
            {
                SetValue("FooGuid", value);
            }
        }


        /// <summary>
        /// Foo last modified
        /// </summary>
        [DatabaseField]
        public virtual DateTime FooLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("FooLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("FooLastModified", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            FooInfoProvider.DeleteFooInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            FooInfoProvider.SetFooInfo(this);
        }

        #endregion


        #region "Constructors"

		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected FooInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty FooInfo object.
        /// </summary>
        public FooInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new FooInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public FooInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}