﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connected.Inbound.DataAccessLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ConnectedConfEntities : DbContext
    {
        public ConnectedConfEntities()
            : base("name=ConnectedConfEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdapterBasic> AdapterBasic { get; set; }
        public virtual DbSet<AdapterMessageType> AdapterMessageType { get; set; }
        public virtual DbSet<AdapterTypeDIM> AdapterTypeDIM { get; set; }
        public virtual DbSet<ConnectedSettings> ConnectedSettings { get; set; }
        public virtual DbSet<MessageSubscriptionDetails> MessageSubscriptionDetails { get; set; }
        public virtual DbSet<MessageType> MessageType { get; set; }
        public virtual DbSet<ReceiveAdapterDetails> ReceiveAdapterDetails { get; set; }
        public virtual DbSet<AdaptersView> AdaptersView { get; set; }
        public virtual DbSet<ConnectedSettingsView> ConnectedSettingsView { get; set; }
    
        public virtual int usp_Add_Adapter(string adapterName, string adapterType, string messageType, string messageSchema, Nullable<bool> isReceiveAdapter, string adapterServiceURI, string adapterServiceOperation, Nullable<bool> isWCFService, Nullable<int> timesToRetry, Nullable<int> retryInterval)
        {
            var adapterNameParameter = adapterName != null ?
                new ObjectParameter("AdapterName", adapterName) :
                new ObjectParameter("AdapterName", typeof(string));
    
            var adapterTypeParameter = adapterType != null ?
                new ObjectParameter("AdapterType", adapterType) :
                new ObjectParameter("AdapterType", typeof(string));
    
            var messageTypeParameter = messageType != null ?
                new ObjectParameter("MessageType", messageType) :
                new ObjectParameter("MessageType", typeof(string));
    
            var messageSchemaParameter = messageSchema != null ?
                new ObjectParameter("MessageSchema", messageSchema) :
                new ObjectParameter("MessageSchema", typeof(string));
    
            var isReceiveAdapterParameter = isReceiveAdapter.HasValue ?
                new ObjectParameter("IsReceiveAdapter", isReceiveAdapter) :
                new ObjectParameter("IsReceiveAdapter", typeof(bool));
    
            var adapterServiceURIParameter = adapterServiceURI != null ?
                new ObjectParameter("AdapterServiceURI", adapterServiceURI) :
                new ObjectParameter("AdapterServiceURI", typeof(string));
    
            var adapterServiceOperationParameter = adapterServiceOperation != null ?
                new ObjectParameter("AdapterServiceOperation", adapterServiceOperation) :
                new ObjectParameter("AdapterServiceOperation", typeof(string));
    
            var isWCFServiceParameter = isWCFService.HasValue ?
                new ObjectParameter("IsWCFService", isWCFService) :
                new ObjectParameter("IsWCFService", typeof(bool));
    
            var timesToRetryParameter = timesToRetry.HasValue ?
                new ObjectParameter("TimesToRetry", timesToRetry) :
                new ObjectParameter("TimesToRetry", typeof(int));
    
            var retryIntervalParameter = retryInterval.HasValue ?
                new ObjectParameter("RetryInterval", retryInterval) :
                new ObjectParameter("RetryInterval", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Add_Adapter", adapterNameParameter, adapterTypeParameter, messageTypeParameter, messageSchemaParameter, isReceiveAdapterParameter, adapterServiceURIParameter, adapterServiceOperationParameter, isWCFServiceParameter, timesToRetryParameter, retryIntervalParameter);
        }
    
        public virtual int usp_Delete_Adapter(Nullable<int> adapterId)
        {
            var adapterIdParameter = adapterId.HasValue ?
                new ObjectParameter("AdapterId", adapterId) :
                new ObjectParameter("AdapterId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Delete_Adapter", adapterIdParameter);
        }
    
        public virtual ObjectResult<usp_Get_Adapters_Result> usp_Get_Adapters(string sortColumn, string sortOrder)
        {
            var sortColumnParameter = sortColumn != null ?
                new ObjectParameter("SortColumn", sortColumn) :
                new ObjectParameter("SortColumn", typeof(string));
    
            var sortOrderParameter = sortOrder != null ?
                new ObjectParameter("SortOrder", sortOrder) :
                new ObjectParameter("SortOrder", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Get_Adapters_Result>("usp_Get_Adapters", sortColumnParameter, sortOrderParameter);
        }
    
        public virtual int usp_Get_SettingByID(Nullable<int> settingsId)
        {
            var settingsIdParameter = settingsId.HasValue ?
                new ObjectParameter("SettingsId", settingsId) :
                new ObjectParameter("SettingsId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Get_SettingByID", settingsIdParameter);
        }
    
        public virtual int usp_Get_Settings(Nullable<int> rowStartIndex, Nullable<int> pageSize, string searchText, string sortColumn, string sortOrder)
        {
            var rowStartIndexParameter = rowStartIndex.HasValue ?
                new ObjectParameter("RowStartIndex", rowStartIndex) :
                new ObjectParameter("RowStartIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var searchTextParameter = searchText != null ?
                new ObjectParameter("SearchText", searchText) :
                new ObjectParameter("SearchText", typeof(string));
    
            var sortColumnParameter = sortColumn != null ?
                new ObjectParameter("SortColumn", sortColumn) :
                new ObjectParameter("SortColumn", typeof(string));
    
            var sortOrderParameter = sortOrder != null ?
                new ObjectParameter("SortOrder", sortOrder) :
                new ObjectParameter("SortOrder", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Get_Settings", rowStartIndexParameter, pageSizeParameter, searchTextParameter, sortColumnParameter, sortOrderParameter);
        }
    
        public virtual ObjectResult<usp_Get_Subscribers_Result> usp_Get_Subscribers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Get_Subscribers_Result>("usp_Get_Subscribers");
        }
    
        public virtual ObjectResult<usp_GetAdaptersByID_Result> usp_GetAdaptersByID(Nullable<int> adapterId)
        {
            var adapterIdParameter = adapterId.HasValue ?
                new ObjectParameter("AdapterId", adapterId) :
                new ObjectParameter("AdapterId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetAdaptersByID_Result>("usp_GetAdaptersByID", adapterIdParameter);
        }
    
        public virtual int usp_Update_Adapter(Nullable<int> adapterId, string adapterName, Nullable<int> messageTypeId, string messageType, string messageSchema, Nullable<bool> isReceiveAdapter, string adapterServiceURI, string adapterServiceOperation, Nullable<bool> isWCFService, Nullable<int> timesToRetry, Nullable<int> retryInterval)
        {
            var adapterIdParameter = adapterId.HasValue ?
                new ObjectParameter("AdapterId", adapterId) :
                new ObjectParameter("AdapterId", typeof(int));
    
            var adapterNameParameter = adapterName != null ?
                new ObjectParameter("AdapterName", adapterName) :
                new ObjectParameter("AdapterName", typeof(string));
    
            var messageTypeIdParameter = messageTypeId.HasValue ?
                new ObjectParameter("MessageTypeId", messageTypeId) :
                new ObjectParameter("MessageTypeId", typeof(int));
    
            var messageTypeParameter = messageType != null ?
                new ObjectParameter("MessageType", messageType) :
                new ObjectParameter("MessageType", typeof(string));
    
            var messageSchemaParameter = messageSchema != null ?
                new ObjectParameter("MessageSchema", messageSchema) :
                new ObjectParameter("MessageSchema", typeof(string));
    
            var isReceiveAdapterParameter = isReceiveAdapter.HasValue ?
                new ObjectParameter("IsReceiveAdapter", isReceiveAdapter) :
                new ObjectParameter("IsReceiveAdapter", typeof(bool));
    
            var adapterServiceURIParameter = adapterServiceURI != null ?
                new ObjectParameter("AdapterServiceURI", adapterServiceURI) :
                new ObjectParameter("AdapterServiceURI", typeof(string));
    
            var adapterServiceOperationParameter = adapterServiceOperation != null ?
                new ObjectParameter("AdapterServiceOperation", adapterServiceOperation) :
                new ObjectParameter("AdapterServiceOperation", typeof(string));
    
            var isWCFServiceParameter = isWCFService.HasValue ?
                new ObjectParameter("IsWCFService", isWCFService) :
                new ObjectParameter("IsWCFService", typeof(bool));
    
            var timesToRetryParameter = timesToRetry.HasValue ?
                new ObjectParameter("TimesToRetry", timesToRetry) :
                new ObjectParameter("TimesToRetry", typeof(int));
    
            var retryIntervalParameter = retryInterval.HasValue ?
                new ObjectParameter("RetryInterval", retryInterval) :
                new ObjectParameter("RetryInterval", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Update_Adapter", adapterIdParameter, adapterNameParameter, messageTypeIdParameter, messageTypeParameter, messageSchemaParameter, isReceiveAdapterParameter, adapterServiceURIParameter, adapterServiceOperationParameter, isWCFServiceParameter, timesToRetryParameter, retryIntervalParameter);
        }
    
        public virtual int usp_Update_Setting(Nullable<int> settingsId, string settingsKey, string settingsValue, string description, string modifiedUserId)
        {
            var settingsIdParameter = settingsId.HasValue ?
                new ObjectParameter("SettingsId", settingsId) :
                new ObjectParameter("SettingsId", typeof(int));
    
            var settingsKeyParameter = settingsKey != null ?
                new ObjectParameter("SettingsKey", settingsKey) :
                new ObjectParameter("SettingsKey", typeof(string));
    
            var settingsValueParameter = settingsValue != null ?
                new ObjectParameter("SettingsValue", settingsValue) :
                new ObjectParameter("SettingsValue", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var modifiedUserIdParameter = modifiedUserId != null ?
                new ObjectParameter("ModifiedUserId", modifiedUserId) :
                new ObjectParameter("ModifiedUserId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Update_Setting", settingsIdParameter, settingsKeyParameter, settingsValueParameter, descriptionParameter, modifiedUserIdParameter);
        }
    }
}