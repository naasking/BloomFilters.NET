using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloomFilters
{
    /// <summary>
    /// A Bloom filter of arbitrary length.
    /// </summary>
    public struct BloomFilter : IEquatable<BloomFilter>
    {
        ulong[] bits;

        public BloomFilter(int bitCount)
        {
            bits = new ulong[(int)Math.Ceiling(bitCount / 64.0)];
        }

        /// <summary>
        /// Calculates the false positive rate given the number of items stored.
        /// </summary>
        /// <param name="count">The number of items stored.</param>
        /// <returns>The false positive probability.</returns>
        public double FalsePositiveRate(int count)
        {
            return Math.Pow(Math.E, -BitCount * Math.Pow(Math.Log(2), 2) / count);
        }

        public int BitCount
        {
            get { return 64 * bits.Length; }
        }

        public void Add(int hashCode)
        {

        }

        static int BitIndex(int fnvHash, int block)
        {
            return Math.Loo
        }

        static void Mask(int hashCode)
        {
            unchecked
            {
                // We hash here solely because integers on the CLR simply use their values as their
                // hash code. Due to Benford's law, chances are most integral values used will
                // be small numbers, which means only the low-order bits of of the Bloom filter
                // will ever be used and we'll have many collisions. We thus mix up the bits
                // using FNV so small numbers are (ideally) distributed evenly across higher numbers as well.
                var x = Hash.Fnv1a(hashCode);
                return (uint)1 << ((int)x & 0x1F)
                     | (uint)1 << ((int)(x >> 8) & 0x1F)
                     | (uint)1 << ((int)(x >> 16) & 0x1F)
                     | (uint)1 << ((int)(x >> 24) & 0x1F);
            }
        }
    }
}
