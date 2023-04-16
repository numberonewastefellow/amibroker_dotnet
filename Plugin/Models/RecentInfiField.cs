namespace MilkyAmiBroker.Plugins.Models
{
    public enum RecentInfiField
    {
        Last = 1 << 0,
        Open = 1 << 1,
        HighLow = 1 << 2,
        TradeVol = 1 << 3,
        TotalVol = 1 << 4,
        OpenInt = 1 << 5,
        PrevChange = 1 << 6,
        Bid = 1 << 7,
        Ask = 1 << 8,
        EPS = 1 << 9,
        Dividend = 1 << 10,
        Shares = 1 << 11,
        Week52 = 1 << 12,
        DateUpdated = 1 << 13,
        DateChanged = 1 << 14
    }
}