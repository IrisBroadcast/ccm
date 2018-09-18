using System;

namespace CCM.CodecControl.Helpers
{
    /// <summary>
    /// Helper class for encoding and decoding values
    /// </summary>
    public static class ConvertHelper
    {
        public static int EncodeUInt(uint value, byte[] buffer, int offset)
        {
            buffer[offset + 0] = (byte)(value >> 0);
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
            return offset + 4;
        }

        public static int EncodeString(string s, byte[] buffer, int offset, int maxLength)
        {
            // Modify maxLength to take ending null byte in concideration
            Array.Clear(buffer, offset, maxLength);

            var stringBytes = System.Text.Encoding.ASCII.GetBytes(s);
            var length = Math.Min(stringBytes.Length, maxLength-1); // -1 because of null termination

            Array.Copy(stringBytes, 0, buffer, offset, length);
            return offset + maxLength;
        }

        public static uint DecodeUInt(byte[] buffer, int offset)
        {
            return ((uint)buffer[offset + 0] << 0)
                   + ((uint)buffer[offset + 1] << 8)
                   + ((uint)buffer[offset + 2] << 16)
                   + ((uint)buffer[offset + 3] << 24);
        }
    }
}