namespace Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Models.Api20180815Preview
{
    using static Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Runtime.Extensions;

    /// <summary>
    /// The storage configuration provides the connection details that allows the Time Series Insights service to connect to the
    /// customer storage account that is used to store the environment's data.
    /// </summary>
    public partial class LongTermStorageConfigurationInput :
        Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Models.Api20180815Preview.ILongTermStorageConfigurationInput,
        Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Models.Api20180815Preview.ILongTermStorageConfigurationInputInternal
    {

        /// <summary>Backing field for <see cref="AccountName" /> property.</summary>
        private string _accountName;

        /// <summary>
        /// The name of the storage account that will hold the environment's long term data.
        /// </summary>
        [Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Origin(Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.PropertyOrigin.Owned)]
        public string AccountName { get => this._accountName; set => this._accountName = value; }

        /// <summary>Backing field for <see cref="ManagementKey" /> property.</summary>
        private string _managementKey;

        /// <summary>
        /// The value of the management key that grants the Time Series Insights service write access to the storage account. This
        /// property is not shown in environment responses.
        /// </summary>
        [Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Origin(Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.PropertyOrigin.Owned)]
        public string ManagementKey { get => this._managementKey; set => this._managementKey = value; }

        /// <summary>Creates an new <see cref="LongTermStorageConfigurationInput" /> instance.</summary>
        public LongTermStorageConfigurationInput()
        {

        }
    }
    /// The storage configuration provides the connection details that allows the Time Series Insights service to connect to the
    /// customer storage account that is used to store the environment's data.
    public partial interface ILongTermStorageConfigurationInput :
        Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Runtime.IJsonSerializable
    {
        /// <summary>
        /// The name of the storage account that will hold the environment's long term data.
        /// </summary>
        [Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Runtime.Info(
        Required = true,
        ReadOnly = false,
        Description = @"The name of the storage account that will hold the environment's long term data.",
        SerializedName = @"accountName",
        PossibleTypes = new [] { typeof(string) })]
        string AccountName { get; set; }
        /// <summary>
        /// The value of the management key that grants the Time Series Insights service write access to the storage account. This
        /// property is not shown in environment responses.
        /// </summary>
        [Microsoft.Azure.PowerShell.Cmdlets.TimeSeriesInsights.Runtime.Info(
        Required = true,
        ReadOnly = false,
        Description = @"The value of the management key that grants the Time Series Insights service write access to the storage account. This property is not shown in environment responses.",
        SerializedName = @"managementKey",
        PossibleTypes = new [] { typeof(string) })]
        string ManagementKey { get; set; }

    }
    /// The storage configuration provides the connection details that allows the Time Series Insights service to connect to the
    /// customer storage account that is used to store the environment's data.
    internal partial interface ILongTermStorageConfigurationInputInternal

    {
        /// <summary>
        /// The name of the storage account that will hold the environment's long term data.
        /// </summary>
        string AccountName { get; set; }
        /// <summary>
        /// The value of the management key that grants the Time Series Insights service write access to the storage account. This
        /// property is not shown in environment responses.
        /// </summary>
        string ManagementKey { get; set; }

    }
}