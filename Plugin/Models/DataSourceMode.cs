namespace MilkyAmiBroker.Plugins.Models
{
    public enum DataSourceMode
    {
        /// <summary>
        /// Use data storage mode from preferences
        /// </summary>
        Default,

        /// <summary>
        /// Store data locally
        /// </summary>
        LocalStorage,

        /// <summary>
        /// No local data storage
        /// </summary>
        NoLocalStorage
    }
}