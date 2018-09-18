namespace CCM.Core.CodecControl.Entities
{
    public class VuValues
    {
        public int TxLeft { get; set; } // Sändning vänster
        public int TxRight { get; set; } // Sändning höger
        public int RxLeft { get; set; } // Mottagning vänster
        public int RxRight { get; set; } // Mottagning höger

        public override string ToString()
        {
            return string.Format("Tx: {0} {1} Rx: {2} {3}", TxLeft, TxRight, RxLeft, RxRight);
        }

    }
}