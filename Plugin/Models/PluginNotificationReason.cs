namespace MilkyAmiBroker.Plugins.Models
{
    public enum PluginNotificationReason
    {
        DatabaseLoaded = 1,
        DatabaseUnloaded = 2,
        SettingsChange = 4,
        StatusRightClick = 8
    }
}