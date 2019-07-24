using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace Demo
{    
    /// <summary>
    /// Class providing BarInfo management.
    /// </summary>
    public partial class BarInfoProvider : AbstractInfoProvider<BarInfo, BarInfoProvider>
    {
        #region "Constructors"

        /// <summary>
        /// Constructor
        /// </summary>
        public BarInfoProvider()
            : base(BarInfo.TYPEINFO)
        {
        }

        #endregion


        #region "Public methods - Basic"

        /// <summary>
        /// Returns a query for all the BarInfo objects.
        /// </summary>
        public static ObjectQuery<BarInfo> GetBars()
        {
            return ProviderObject.GetBarsInternal();
        }


        /// <summary>
        /// Returns BarInfo with specified ID.
        /// </summary>
        /// <param name="id">BarInfo ID</param>
        public static BarInfo GetBarInfo(int id)
        {
            return ProviderObject.GetBarInfoInternal(id);
        }


        /// <summary>
        /// Returns BarInfo with specified name.
        /// </summary>
        /// <param name="name">BarInfo name</param>
        public static BarInfo GetBarInfo(string name)
        {
            return ProviderObject.GetBarInfoInternal(name);
        }


        /// <summary>
        /// Returns BarInfo with specified GUID.
        /// </summary>
        /// <param name="guid">BarInfo GUID</param>                
        public static BarInfo GetBarInfo(Guid guid)
        {
            return ProviderObject.GetBarInfoInternal(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified BarInfo.
        /// </summary>
        /// <param name="infoObj">BarInfo to be set</param>
        public static void SetBarInfo(BarInfo infoObj)
        {
            ProviderObject.SetBarInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified BarInfo.
        /// </summary>
        /// <param name="infoObj">BarInfo to be deleted</param>
        public static void DeleteBarInfo(BarInfo infoObj)
        {
            ProviderObject.DeleteBarInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes BarInfo with specified ID.
        /// </summary>
        /// <param name="id">BarInfo ID</param>
        public static void DeleteBarInfo(int id)
        {
            BarInfo infoObj = GetBarInfo(id);
            DeleteBarInfo(infoObj);
        }

        #endregion


        #region "Internal methods - Basic"
	
        /// <summary>
        /// Returns a query for all the BarInfo objects.
        /// </summary>
        protected virtual ObjectQuery<BarInfo> GetBarsInternal()
        {
            return GetObjectQuery();
        }    


        /// <summary>
        /// Returns BarInfo with specified ID.
        /// </summary>
        /// <param name="id">BarInfo ID</param>        
        protected virtual BarInfo GetBarInfoInternal(int id)
        {	
            return GetInfoById(id);
        }


        /// <summary>
        /// Returns BarInfo with specified name.
        /// </summary>
        /// <param name="name">BarInfo name</param>        
        protected virtual BarInfo GetBarInfoInternal(string name)
        {
            return GetInfoByCodeName(name);
        } 


        /// <summary>
        /// Returns BarInfo with specified GUID.
        /// </summary>
        /// <param name="guid">BarInfo GUID</param>
        protected virtual BarInfo GetBarInfoInternal(Guid guid)
        {
            return GetInfoByGuid(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified BarInfo.
        /// </summary>
        /// <param name="infoObj">BarInfo to be set</param>        
        protected virtual void SetBarInfoInternal(BarInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified BarInfo.
        /// </summary>
        /// <param name="infoObj">BarInfo to be deleted</param>        
        protected virtual void DeleteBarInfoInternal(BarInfo infoObj)
        {
            DeleteInfo(infoObj);
        }	

        #endregion
    }
}