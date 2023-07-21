using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class HashUtils
    {
        public static void Combine(ref uint hashA, uint hashB)
        {
            hashA ^= hashB + 0x9e3779b9 + (hashA << 6) + (hashA >> 2);
        }
        public static void Combine(ref uint hashA, ulong hashB)
        {
            hashA ^= (uint)(hashB + 0x9e3779b97f4a7c15U + (hashA << 6) + (hashA >> 2));
        }
        public static uint Hash(string input)
        {
            ulong hash = FNVHash(input);
            return FoldHash(hash);
        }

        private static ulong FNVHash(string input)
        {
            uint result = 2166136261U;
            int idx = 0;
            while(idx != input.Length)
            {
                result = (result * 16777619) ^ (byte)input.ElementAt(idx);
                ++idx;
            }
            return (ulong)result;
        }
        private static uint FoldHash(ulong value)
        {
            uint lowValue = (uint)value;
            uint highValue = (uint)(value >> 32);

            if (highValue == 0)
                return lowValue;

            uint result = lowValue;
            Combine(ref result, highValue);
            return result;
        }
    }
}
