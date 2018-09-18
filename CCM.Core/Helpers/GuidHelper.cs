using System;

namespace CCM.Core.Helpers
{
    public static class GuidHelper
    {
        public static string GuidString(Guid? guid) { return guid == null || guid == Guid.Empty ? String.Empty : guid.ToString(); }
    }
}
