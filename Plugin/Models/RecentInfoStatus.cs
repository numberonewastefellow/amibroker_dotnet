namespace MilkyAmiBroker.Plugins.Models
{
    public enum RecentInfoStatus
    {
        Update = 1 << 0,
        BidAsk = 1 << 1,
        Trade = 1 << 2,
        BarsReady = 1 << 3,
        Incomplete = 1 << 4,
        NewBid = 1 << 5,
        NewAsk = 1 << 6
    }
}