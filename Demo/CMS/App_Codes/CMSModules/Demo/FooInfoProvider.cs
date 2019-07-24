using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace Demo
{    
    /// <summary>
    /// Class providing FooInfo management.
    /// </summary>
    public partial class FooInfoProvider : AbstractInfoProvider<FooInfo, FooInfoProvider>
    {
        #region "Constructors"

        /// <summary>
        /// Constructor
        /// </summary>
        public FooInfoProvider()
            : base(FooInfo.TYPEINFO)
        {
        }

        #endregion


        #region "Public methods - Basic"

        /// <summary>
        /// Returns a query for all the FooInfo objects.
        /// </summary>
        public static ObjectQuery<FooInfo> GetFoos()
        {
            return ProviderObject.GetFoosInternal();
        }


        /// <summary>
        /// Returns FooInfo with specified ID.
        /// </summary>
        /// <param name="id">FooInfo ID</param>
        public static FooInfo GetFooInfo(int id)
        {
            return ProviderObject.GetFooInfoInternal(id);
        }


        /// <summary>
        /// Returns FooInfo with specified name.
        /// </summary>
        /// <param name="name">FooInfo name</param>
        public static FooInfo GetFooInfo(string name)
        {
            return ProviderObject.GetFooInfoInternal(name);
        }


        /// <summary>
        /// Returns FooInfo with specified GUID.
        /// </summary>
        /// <param name="guid">FooInfo GUID</param>                
        public static FooInfo GetFooInfo(Guid guid)
        {
            return ProviderObject.GetFooInfoInternal(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified FooInfo.
        /// </summary>
        /// <param name="infoObj">FooInfo to be set</param>
        public static void SetFooInfo(FooInfo infoObj)
        {
            ProviderObject.SetFooInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified FooInfo.
        /// </summary>
        /// <param name="infoObj">FooInfo to be deleted</param>
        public static void DeleteFooInfo(FooInfo infoObj)
        {
            ProviderObject.DeleteFooInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes FooInfo with specified ID.
        /// </summary>
        /// <param name="id">FooInfo ID</param>
        public static void DeleteFooInfo(int id)
        {
            FooInfo infoObj = GetFooInfo(id);
            DeleteFooInfo(infoObj);
        }

        #endregion


        #region "Internal methods - Basic"
	
        /// <summary>
        /// Returns a query for all the FooInfo objects.
        /// </summary>
        protected virtual ObjectQuery<FooInfo> GetFoosInternal()
        {
            return GetObjectQuery();
        }    


        /// <summary>
        /// Returns FooInfo with specified ID.
        /// </summary>
        /// <param name="id">FooInfo ID</param>        
        protected virtual FooInfo GetFooInfoInternal(int id)
        {	
            return GetInfoById(id);
        }


        /// <summary>
        /// Returns FooInfo with specified name.
        /// </summary>
        /// <param name="name">FooInfo name</param>        
        protected virtual FooInfo GetFooInfoInternal(string name)
        {
            return GetInfoByCodeName(name);
        } 


        /// <summary>
        /// Returns FooInfo with specified GUID.
        /// </summary>
        /// <param name="guid">FooInfo GUID</param>
        protected virtual FooInfo GetFooInfoInternal(Guid guid)
        {
            return GetInfoByGuid(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified FooInfo.
        /// </summary>
        /// <param name="infoObj">FooInfo to be set</param>        
        protected virtual void SetFooInfoInternal(FooInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified FooInfo.
        /// </summary>
        /// <param name="infoObj">FooInfo to be deleted</param>        
        protected virtual void DeleteFooInfoInternal(FooInfo infoObj)
        {
            DeleteInfo(infoObj);
        }	

        #endregion
    }
}