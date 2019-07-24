using System;
using System.Linq;

using CMS.DataEngine;
using CMS.Helpers;

namespace Demo
{
    /// <summary>
    /// Class providing FooBarInfo management.
    /// </summary>
    public partial class FooBarInfoProvider : AbstractInfoProvider<FooBarInfo, FooBarInfoProvider>
    {
        #region "Public methods"

        /// <summary>
        /// Returns all FooBarInfo bindings.
        /// </summary>
        public static ObjectQuery<FooBarInfo> GetFooBars()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns FooBarInfo binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID</param>
        /// <param name="barId">ObjectType.demo_bar ID</param>  
        public static FooBarInfo GetFooBarInfo(int fooId, int barId)
        {
            return ProviderObject.GetFooBarInfoInternal(fooId, barId);
        }


        /// <summary>
        /// Sets specified FooBarInfo.
        /// </summary>
        /// <param name="infoObj">FooBarInfo to set</param>
        public static void SetFooBarInfo(FooBarInfo infoObj)
        {
            ProviderObject.SetFooBarInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified FooBarInfo binding.
        /// </summary>
        /// <param name="infoObj">FooBarInfo object</param>
        public static void DeleteFooBarInfo(FooBarInfo infoObj)
        {
            ProviderObject.DeleteFooBarInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes FooBarInfo binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID</param>
        /// <param name="barId">ObjectType.demo_bar ID</param>  
        public static void RemoveFooFromBar(int fooId, int barId)
        {
            ProviderObject.RemoveFooFromBarInternal(fooId, barId);
        }


        /// <summary>
        /// Creates FooBarInfo binding. 
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID</param>
        /// <param name="barId">ObjectType.demo_bar ID</param>   
        public static void AddFooToBar(int fooId, int barId)
        {
            ProviderObject.AddFooToBarInternal(fooId, barId);
        }

        #endregion


        #region "Internal methods"

        /// <summary>
        /// Returns the FooBarInfo structure.
        /// Null if binding doesn't exist.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID</param>
        /// <param name="barId">ObjectType.demo_bar ID</param>  
        protected virtual FooBarInfo GetFooBarInfoInternal(int fooId, int barId)
        {
            return ProviderObject.GetObjectQuery().TopN(1)
                .WhereEquals("FooID", fooId)
                .WhereEquals("BarID", barId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Sets specified FooBarInfo binding.
        /// </summary>
        /// <param name="infoObj">FooBarInfo object</param>
        protected virtual void SetFooBarInfoInternal(FooBarInfo infoObj)
        {
            // Customization 1 - On Insert or update, check and set the Order
            if (ValidationHelper.GetInteger(infoObj.GetValue("FooBarOrder"), -1) <= 0)
            {
                infoObj.FooBarOrder = GetFooBars().WhereEquals("FooID", infoObj.FooID).Count + 1;
            }
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified FooBarInfo.
        /// </summary>
        /// <param name="infoObj">FooBarInfo object</param>
        protected virtual void DeleteFooBarInfoInternal(FooBarInfo infoObj)
        {
            DeleteInfo(infoObj);

            // Customization 2, on deletion re-order
            // Initialize Order, the infoObj should still exist in memory and only needed the Generalized portion
            infoObj.Generalized.InitObjectsOrder(null);
        }


        /// <summary>
        /// Deletes FooBarInfo binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID</param>
        /// <param name="barId">ObjectType.demo_bar ID</param>  
        protected virtual void RemoveFooFromBarInternal(int fooId, int barId)
        {
            var infoObj = GetFooBarInfo(fooId, barId);
            if (infoObj != null)
            {
                DeleteFooBarInfo(infoObj);
            }
        }


        /// <summary>
        /// Creates FooBarInfo binding. 
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID</param>
        /// <param name="barId">ObjectType.demo_bar ID</param>   
        protected virtual void AddFooToBarInternal(int fooId, int barId)
        {
            // Create new binding
            var infoObj = new FooBarInfo();
            infoObj.FooID = fooId;
            infoObj.BarID = barId;

            // Save to the database
            SetFooBarInfo(infoObj);
        }

        #endregion
    }
}