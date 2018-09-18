using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class ResponseHeader
    {
        public Command Command { get; set; }
        public int Length { get; set; }

        public override string ToString()
        {
            return string.Format("Command={0}, Length={1}", Command, Length);
        }
    }
}