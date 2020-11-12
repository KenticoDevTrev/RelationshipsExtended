using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Taxonomy;
using DocumentFormat.OpenXml.Drawing;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelationshipsExtended
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryExtensions
    {
        #region "Related Pages"

        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeGuid">The NodeGuid that the documents must be related to</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        public static DocumentQuery InRelationWithOrder(this DocumentQuery baseQuery, Guid nodeGuid, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InRelationWithOrder(baseQuery, nodeGuid, relationshipName, Ordered, AscendingOrder, ReverseRelationship);
        }


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        public static DocumentQuery InRelationWithOrder(this DocumentQuery baseQuery, int nodeID, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InRelationWithOrder(baseQuery, nodeID, relationshipName, Ordered, AscendingOrder, ReverseRelationship);
        }


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeGuid">The NodeGuid</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        public static DocumentQuery<TDocument> InRelationWithOrder<TDocument>(this DocumentQuery<TDocument> baseQuery, Guid nodeGuid, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InRelationWithOrder(baseQuery, nodeGuid, relationshipName, Ordered, AscendingOrder, ReverseRelationship);
        }


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        public static DocumentQuery<TDocument> InRelationWithOrder<TDocument>(this DocumentQuery<TDocument> baseQuery, int nodeID, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InRelationWithOrder(baseQuery, nodeID, relationshipName, Ordered, AscendingOrder, ReverseRelationship);
        }


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="baseQuery">The Base Document Query</param>
        /// <param name="nodeGuid">The NodeGuid</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        public static MultiDocumentQuery InRelationWithOrder(this MultiDocumentQuery baseQuery, Guid nodeGuid, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InRelationWithOrder(baseQuery, nodeGuid, relationshipName, Ordered, AscendingOrder, ReverseRelationship);
        }


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        public static MultiDocumentQuery InRelationWithOrder(this MultiDocumentQuery baseQuery, int nodeID, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false)
        {
            // Get the RelationshipID and NodeID
            return Service.Resolve<IRelationshipExtendedHelper>().InRelationWithOrder(baseQuery, nodeID, relationshipName, Ordered, AscendingOrder, ReverseRelationship);
        }

        #endregion

        #region "Custom Relationships (full params)"

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>        
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Bar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: BarID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the Document identity value. Ex: BarNodeNodeID (from Demo.BarNode)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: BarNodeBarID (from Demo.BarNode)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        /// <returns></returns>
        public static DocumentQuery InCustomRelationshipWithOrder(this DocumentQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, PrimaryClass, InRelationshipWithValue, ObjectIDFieldName, LeftFieldName, RightFieldName, OrderColumn, OrderAsc, Identity);
        }

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Bar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: BarID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the Document identity value. Ex: BarNodeNodeID (from Demo.BarNode)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: BarNodeBarID (from Demo.BarNode)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        public static DocumentQuery<TDocument> InCustomRelationshipWithOrder<TDocument>(this DocumentQuery<TDocument> baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, PrimaryClass, InRelationshipWithValue, ObjectIDFieldName, LeftFieldName, RightFieldName, OrderColumn, OrderAsc, Identity);
        }

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Bar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: BarID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the Document identity value. Ex: BarNodeNodeID (from Demo.BarNode)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: BarNodeBarID (from Demo.BarNode)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        public static MultiDocumentQuery InCustomRelationshipWithOrder(this MultiDocumentQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, PrimaryClass, InRelationshipWithValue, ObjectIDFieldName, LeftFieldName, RightFieldName, OrderColumn, OrderAsc, Identity);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Foo</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the the primary class's identity value. Ex: FooBarFooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBarBarID (from Demo.FooBar)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order. Ex: FooBarOrder</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        public static ObjectQuery InCustomRelationshipWithOrder(this ObjectQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, PrimaryClass, InRelationshipWithValue, ObjectIDFieldName, LeftFieldName, RightFieldName, OrderColumn, OrderAsc, Identity);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Foo</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that Fis used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the the primary class's identity value. Ex: FooBarFooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBarBarID (from Demo.FooBar)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order. Ex: FooBarOrder</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        public static ObjectQuery<TInfo> InCustomRelationshipWithOrder<TInfo>(this ObjectQuery<TInfo> baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID) where TInfo : BaseInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, PrimaryClass, InRelationshipWithValue, ObjectIDFieldName, LeftFieldName, RightFieldName, OrderColumn, OrderAsc, Identity);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Foo</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the the primary class's identity value. Ex: FooBarFooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBarBarID (from Demo.FooBar)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order. Ex: FooBarOrder</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        public static MultiObjectQuery InCustomRelationshipWithOrder(this MultiObjectQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, PrimaryClass, InRelationshipWithValue, ObjectIDFieldName, LeftFieldName, RightFieldName, OrderColumn, OrderAsc, Identity);
        }

        #endregion

        #region "Custom Relationships (IBaseBinding)"

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>        
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <returns></returns>
        public static DocumentQuery InCustomRelationshipWithOrder(this DocumentQuery baseQuery, IBindingInfo BindingClass, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, BindingType, InRelationshipWithValue, OrderAsc);
        }
        
        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>        
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <returns></returns>
        public static DocumentQuery InCustomRelationshipWithOrder<TBindingInfo>(this DocumentQuery baseQuery, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static DocumentQuery<TDocument> InCustomRelationshipWithOrder<TDocument>(this DocumentQuery<TDocument> baseQuery, IBindingInfo BindingClass, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static DocumentQuery<TDocument> InCustomRelationshipWithOrder<TDocument, TBindingInfo>(this DocumentQuery<TDocument> baseQuery, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TDocument : TreeNode, new() where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static MultiDocumentQuery InCustomRelationshipWithOrder(this MultiDocumentQuery baseQuery, IBindingInfo BindingClass, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static MultiDocumentQuery InCustomRelationshipWithOrder<TBindingInfo>(this MultiDocumentQuery baseQuery, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static ObjectQuery InCustomRelationshipWithOrder(this ObjectQuery baseQuery, IBindingInfo BindingClass, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static ObjectQuery InCustomRelationshipWithOrder<TBindingInfo>(this ObjectQuery baseQuery, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that Fis used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static ObjectQuery<TInfo> InCustomRelationshipWithOrder<TInfo>(this ObjectQuery<TInfo> baseQuery, IBindingInfo BindingClass, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TInfo : BaseInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="InRelationshipWithValue">The value of the primary class that Fis used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static ObjectQuery<TInfo> InCustomRelationshipWithOrder<TInfo, TBindingInfo>(this ObjectQuery<TInfo> baseQuery, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TInfo : BaseInfo, new() where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static MultiObjectQuery InCustomRelationshipWithOrder(this MultiObjectQuery baseQuery, IBindingInfo BindingClass, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, BindingClass, BindingType, InRelationshipWithValue, OrderAsc);
        }

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        public static MultiObjectQuery InCustomRelationshipWithOrder<TBindingInfo>(this MultiObjectQuery baseQuery, BindingQueryType BindingType, object InRelationshipWithValue, bool OrderAsc = true) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().InCustomRelationshipWithOrder(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingType, InRelationshipWithValue, OrderAsc);
        }

        #endregion

        #region "Document Category Filter"

        /// <summary>
        /// Adds Document Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        public static DocumentQuery DocumentCategoryCondition(this DocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document")
        {
            return Service.Resolve<IRelationshipExtendedHelper>().DocumentCategoryCondition(baseQuery, Values, Condition, DocumentIDTableName);
        }

        /// <summary>
        /// Adds Document Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        public static DocumentQuery<TDocument> DocumentCategoryCondition<TDocument>(this DocumentQuery<TDocument> baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document") where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().DocumentCategoryCondition(baseQuery, Values, Condition, DocumentIDTableName);
        }

        /// <summary>
        /// Adds Document Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        public static MultiDocumentQuery DocumentCategoryCondition(this MultiDocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document")
        {
            return Service.Resolve<IRelationshipExtendedHelper>().DocumentCategoryCondition(baseQuery, Values, Condition, DocumentIDTableName);
        }

        #endregion

        #region "Node Category Filter"

        /// <summary>
        /// Adds Tree Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        public static DocumentQuery TreeCategoryCondition(this DocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree")
        {
            return Service.Resolve<IRelationshipExtendedHelper>().TreeCategoryCondition(baseQuery, Values, Condition, NodeIDTableName);
        }

        /// <summary>
        /// Adds Tree Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        public static DocumentQuery<TDocument> TreeCategoryCondition<TDocument>(this DocumentQuery<TDocument> baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree") where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().TreeCategoryCondition(baseQuery, Values, Condition, NodeIDTableName);
        }

        /// <summary>
        /// Adds Tree Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        public static MultiDocumentQuery TreeCategoryCondition(this MultiDocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree")
        {
            return Service.Resolve<IRelationshipExtendedHelper>().TreeCategoryCondition(baseQuery, Values, Condition, NodeIDTableName);
        }

        #endregion

        #region "Binding Category Filter"

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.NodeRegion)</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: NodeRegionNodeID (from Demo.NodeRegion)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: NodeRegionCategoryID (from Demo.NodeRegion) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeRegion</param>
        public static DocumentQuery BindingCategoryCondition(this DocumentQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.NodeRegion)</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: NodeRegionNodeID (from Demo.NodeRegion)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: NodeRegionCategoryID (from Demo.NodeRegion) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeRegion</param>
        public static DocumentQuery<TDocument> BindingCategoryCondition<TDocument>(this DocumentQuery<TDocument> baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.NodeRegion)</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: NodeRegionNodeID (from Demo.NodeRegion)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: NodeRegionCategoryID (from Demo.NodeRegion) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeRegion</param>
        public static MultiDocumentQuery BindingCategoryCondition(this MultiDocumentQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.FooCategory)</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooCategoryFooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: FooCategoryCategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooCategory</param>
        public static ObjectQuery BindingCategoryCondition(this ObjectQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.FooCategory)</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooCategoryFooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: FooCategoryCategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooCategory</param>
        public static ObjectQuery<TInfo> BindingCategoryCondition<TInfo>(this ObjectQuery<TInfo> baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TInfo : BaseInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.FooCategory)</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooCategoryFooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: FooCategoryCategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooCategory</param>
        public static MultiObjectQuery BindingCategoryCondition(this MultiObjectQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        #endregion

        #region "Binding Category Filter with IBindingInfo"

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new NodeRegionInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery BindingCategoryCondition(this DocumentQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery BindingCategoryCondition<TBindingInfo>(this DocumentQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new NodeRegionInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery<TDocument> BindingCategoryCondition<TDocument>(this DocumentQuery<TDocument> baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery<TDocument> BindingCategoryCondition<TDocument, TBindingInfo>(this DocumentQuery<TDocument> baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TDocument : TreeNode, new() where TBindingInfo: IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new NodeRegionInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiDocumentQuery BindingCategoryCondition(this MultiDocumentQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiDocumentQuery BindingCategoryCondition<TBindingInfo>(this MultiDocumentQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new FooCategoryInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery BindingCategoryCondition(this ObjectQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery BindingCategoryCondition<TBindingInfo>(this ObjectQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }


        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new FooCategoryInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery<TObject> BindingCategoryCondition<TObject>(this ObjectQuery<TObject> baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TObject : BaseInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery<TObject> BindingCategoryCondition<TObject, TBindingInfo>(this ObjectQuery<TObject> baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TObject : BaseInfo, new() where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new FooCategoryInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiObjectQuery BindingCategoryCondition(this MultiObjectQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiObjectQuery BindingCategoryCondition<TBindingInfo>(this MultiObjectQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCategoryCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        #endregion

        #region "Custom Binding Filter"

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.NodeBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: NodeBazNodeID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: NodeBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeBaz</param>
        public static DocumentQuery BindingCondition(this DocumentQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.NodeBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: NodeBazNodeID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: NodeBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeBaz</param>
        public static DocumentQuery<TDocument> BindingCondition<TDocument>(this DocumentQuery<TDocument> baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.NodeBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: NodeBazNodeID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: NodeBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeBaz</param>
        public static MultiDocumentQuery BindingCondition(this MultiDocumentQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: FooBazFooID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames) (ex 'BazA', 'BazB')</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBaz</param>
        public static ObjectQuery BindingCondition(this ObjectQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: FooBazFooID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames) (ex 'BazA', 'BazB')</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBaz</param>
        public static ObjectQuery<TInfo> BindingCondition<TInfo>(this ObjectQuery<TInfo> baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TInfo : BaseInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: FooBazFooID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames) (ex 'BazA', 'BazB')</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBaz</param>
        public static MultiObjectQuery BindingCondition(this MultiObjectQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, Identity, Condition, ObjectIDTableName);
        }

        #endregion

        #region "Custom Binding Filter with IBindingInfo"

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new NodeBazInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery BindingCondition(this DocumentQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery BindingCondition<TBindingInfo>(this DocumentQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new NodeBazInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery<TDocument> BindingCondition<TDocument>(this DocumentQuery<TDocument> baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TDocument : TreeNode, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static DocumentQuery<TDocument> BindingCondition<TDocument, TBindingInfo>(this DocumentQuery<TDocument> baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TDocument : TreeNode, new() where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new NodeBazInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiDocumentQuery BindingCondition(this MultiDocumentQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiDocumentQuery BindingCondition<TBindingInfo>(this MultiDocumentQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new FooBazInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery BindingCondition(this ObjectQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }
        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery BindingCondition<TBindingInfo>(this ObjectQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new FooBazInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery<TObject> BindingCondition<TObject>(this ObjectQuery<TObject> baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TObject : BaseInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static ObjectQuery<TObject> BindingCondition<TObject, TBindingInfo>(this ObjectQuery<TObject> baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TObject : BaseInfo, new() where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">An instance of the Binding Class (ex: new FooBazInfo())</param>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiObjectQuery BindingCondition(this MultiObjectQuery baseQuery, IBindingInfo BindingClass, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any)
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, BindingClass, BindingCondition, Values, Condition);
        }

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="Values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        public static MultiObjectQuery BindingCondition<TBindingInfo>(this MultiObjectQuery baseQuery, BindingConditionType BindingCondition, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any) where TBindingInfo : IBindingInfo, new()
        {
            return Service.Resolve<IRelationshipExtendedHelper>().BindingCondition(baseQuery, (IBindingInfo)Activator.CreateInstance(typeof(TBindingInfo)), BindingCondition, Values, Condition);
        }

        #endregion
    }
}
