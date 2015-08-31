using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloomFilters
{
    /// <summary>
    /// Non-cryptographic hash functions.
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// FNV1a hash function.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>An unsigned 32-bit hash value.</returns>
        public static uint Fnv1a(int value)
        {
            unchecked
            {
                const uint fnvPrime = 16777619;
                uint hash = 2166136261;
                var x = (uint)value;
                hash = ((x & 0xFF) ^ hash) * fnvPrime;
                hash = (((x >> 8) & 0xFF) ^ hash) * fnvPrime;
                hash = (((x >> 16) & 0xFF) ^ hash) * fnvPrime;
                hash = (((x >> 24) & 0xFF) ^ hash) * fnvPrime;
                return hash;
            }
        }
    }
}
