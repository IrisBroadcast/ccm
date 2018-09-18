using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class GpoViewModel : CodecViewModelBase
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}