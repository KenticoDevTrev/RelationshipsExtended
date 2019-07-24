using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace Demo
{    
    /// <summary>
    /// Class providing BazInfo management.
    /// </summary>
    public partial class BazInfoProvider : AbstractInfoProvider<BazInfo, BazInfoProvider>
    {
        #region "Constructors"

        /// <summary>
        /// Constructor
        /// </summary>
        public BazInfoProvider()
            : base(BazInfo.TYPEINFO)
        {
        }

        #endregion


        #region "Public methods - Basic"

        /// <summary>
        /// Returns a query for all the BazInfo objects.
        /// </summary>
        public static ObjectQuery<BazInfo> GetBazes()
        {
            return ProviderObject.GetBazesInternal();
        }


        /// <summary>
        /// Returns BazInfo with specified ID.
        /// </summary>
        /// <param name="id">BazInfo ID</param>
        public static BazInfo GetBazInfo(int id)
        {
            return ProviderObject.GetBazInfoInternal(id);
        }


        /// <summary>
        /// Returns BazInfo with specified name.
        /// </summary>
        /// <param name="name">BazInfo name</param>
        public static BazInfo GetBazInfo(string name)
        {
            return ProviderObject.GetBazInfoInternal(name);
        }


        /// <summary>
        /// Returns BazInfo with specified GUID.
        /// </summary>
        /// <param name="guid">BazInfo GUID</param>                
        public static BazInfo GetBazInfo(Guid guid)
        {
            return ProviderObject.GetBazInfoInternal(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified BazInfo.
        /// </summary>
        /// <param name="infoObj">BazInfo to be set</param>
        public static void SetBazInfo(BazInfo infoObj)
        {
            ProviderObject.SetBazInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified BazInfo.
        /// </summary>
        /// <param name="infoObj">BazInfo to be deleted</param>
        public static void DeleteBazInfo(BazInfo infoObj)
        {
            ProviderObject.DeleteBazInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes BazInfo with specified ID.
        /// </summary>
        /// <param name="id">BazInfo ID</param>
        public static void DeleteBazInfo(int id)
        {
            BazInfo infoObj = GetBazInfo(id);
            DeleteBazInfo(infoObj);
        }

        #endregion


        #region "Internal methods - Basic"
	
        /// <summary>
        /// Returns a query for all the BazInfo objects.
        /// </summary>
        protected virtual ObjectQuery<BazInfo> GetBazesInternal()
        {
            return GetObjectQuery();
        }    


        /// <summary>
        /// Returns BazInfo with specified ID.
        /// </summary>
        /// <param name="id">BazInfo ID</param>        
        protected virtual BazInfo GetBazInfoInternal(int id)
        {	
            return GetInfoById(id);
        }


        /// <summary>
        /// Returns BazInfo with specified name.
        /// </summary>
        /// <param name="name">BazInfo name</param>        
        protected virtual BazInfo GetBazInfoInternal(string name)
        {
            return GetInfoByCodeName(name);
        } 


        /// <summary>
        /// Returns BazInfo with specified GUID.
        /// </summary>
        /// <param name="guid">BazInfo GUID</param>
        protected virtual BazInfo GetBazInfoInternal(Guid guid)
        {
            return GetInfoByGuid(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified BazInfo.
        /// </summary>
        /// <param name="infoObj">BazInfo to be set</param>        
        protected virtual void SetBazInfoInternal(BazInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified BazInfo.
        /// </summary>
        /// <param name="infoObj">BazInfo to be deleted</param>        
        protected virtual void DeleteBazInfoInternal(BazInfo infoObj)
        {
            DeleteInfo(infoObj);
        }	

        #endregion
    }
}