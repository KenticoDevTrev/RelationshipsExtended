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
    /// Data container class for <see cref="FooInfo"/>.
    /// </summary>
    [Serializable]
    public partial class FooInfo : AbstractInfo<FooInfo, IFooInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "demo.foo";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(FooInfoProvider), OBJECT_TYPE, "Demo.Foo", nameof(FooID), nameof(FooLastModified), nameof(FooGuid), nameof(FooCodeName), nameof(FooName), null, null, null, null)
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
        /// Foo ID.
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
        /// Foo name.
        /// </summary>
        [DatabaseField]
        public virtual string FooName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("FooName"), String.Empty);
            }
            set
            {
                SetValue("FooName", value);
            }
        }


        /// <summary>
        /// Foo code name.
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
        /// Foo guid.
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
        /// Foo last modified.
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
        protected FooInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="FooInfo"/> class.
        /// </summary>
        public FooInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="FooInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public FooInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}