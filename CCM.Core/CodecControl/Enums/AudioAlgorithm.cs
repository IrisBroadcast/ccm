using System.ComponentModel;

namespace CCM.Core.CodecControl.Enums
{
    /// <summary>
    /// Ljudkodningsalgoritm, baserat på Prodys implementation
    /// </summary>
    public enum AudioAlgorithm
    {
        [Description("Okänd/saknad kodning")] Error = 0,
        [Description("MPEG 4 AAC-HE")] Mpeg4AacHe = 1,
        [Description("MPEG 4 AAC-LC")] Mpeg4AacLc = 2,
        [Description("MPEG 4 AAC-ELD")] Mpeg4AacEld = 3,
        [Description("E-aptX")] Eaptx = 4,
        [Description("OPUS")] Opus = 5,
        [Description("PCM")] Pcm = 6,
        [Description("G711")] G711 = 7,
        [Description("G722")] G722 = 8,
        [Description("MPEG 4 AAC-LD")] Mpeg4AacLd = 9,
        [Description("MPEG L2")] MpegL2 = 10
    }
}