using System.ComponentModel;

namespace CCM.Core.CodecControl.Enums
{
    public enum LineStatusCode
    {
        [Description("Ingen tillgänglig förbindelse möjlig")]NoPhysicalLine = 0,
        [Description("Nedkopplad")]Disconnected,
        [Description("Kopplar ned")]Disconnecting,
        [Description("Ringer")]Calling, // Ringer upp
        [Description("Mottagning av samtal")]ReceivingCall,
        [Description("Samtal uppringt")]ConnectedCalled, // Uppkopplad. Ringde upp samtalet.
        [Description("Samtal mottaget")]ConnectedReceived, // Uppkopplad. Tog emot samtalet.
        [Description("Ej tillgänglig")]NotAvailable,
        [Description("Förhandlar om dynamisk IP-adresstilldelning")]NegotiatingDhcp,
        [Description("Återansluter")]Reconnecting,
        [Description("Testar förbindelsen")]ConnectedTestingLine,
        [Description("Laddar upp filen")]ConnectedUploadingFile,
        [Description("Laddar ner filen")]ConnectedDownloadingFile,
        [Description("Initiering")]Initializing,
        [Description("Kan inte läsa status")]ErrorGettingStatus = 333
    }
    
}