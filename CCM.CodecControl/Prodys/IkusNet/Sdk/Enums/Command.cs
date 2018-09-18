namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Enums
{
    public enum Command
    {
        CsAlive = 0,
        CsConnect = 1,
        CsCommandResponse = 3,
        CsConnect2 = 4,
        // 5- 99 Ej definierade nr
        IkusNetSysGetDeviceName = 100,
        IkusNetSysSetDeviceName = 101,
        IkusNetSysRebootDevice = 102,
        IkusNetGetLoadedPresetName = 103,
        IkusNetPresetLoad = 104,
        IkusNetGetVumeters = 105,
        IkusNetSetInputGainLevel = 106,
        IkusNetGetInputGainLevel = 107,
        IkusNetSetInputEnabled = 108,
        IkusNetGetInputEnabled = 109,
        IkusNetCall = 110,
        IkusNetHangUp = 111,
        IkusNetGetGpi = 112,
        IkusNetGetGpo = 113,
        IkusNetSetGpo = 114,
        IkusNetGetLineStatus = 115,
        IkusNetCallV2 = 116,
        IkusNetGetCurrentProfile = 117,
        IkusNetEncoderGetAudioMode = 118,
        // 119-138 Definierade men ej implementerade, se IkusNet SDK User Manual
        IkusNetDecoderGetAudioMode = 139
        // 140-    Definierade men ej implementerade

    }
}