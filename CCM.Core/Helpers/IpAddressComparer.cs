using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CCM.Core.Helpers
{
    public class IpAddressComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            IPAddress xAddress;
            IPAddress yAddress;
            IPAddress.TryParse(x, out xAddress);
            IPAddress.TryParse(y, out yAddress);

            if (xAddress == null && yAddress == null) { return 0; }
            if (xAddress == null) { return -1; } 
            if (yAddress == null) { return 1; } 

            byte[] first = xAddress.GetAddressBytes();
            byte[] second = yAddress.GetAddressBytes();
            return first.Zip(second, (a, b) => a.CompareTo(b)).FirstOrDefault(c => c != 0);
        }
    }
}