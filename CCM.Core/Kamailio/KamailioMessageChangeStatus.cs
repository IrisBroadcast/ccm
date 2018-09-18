namespace CCM.Core.Kamailio
{
    public enum KamailioMessageChangeStatus
    {
        NothingChanged = 0,
        CallStarted,
        CallClosed,
        CodecAdded,
        CodecUpdated,
        CodecRemoved
    }
}