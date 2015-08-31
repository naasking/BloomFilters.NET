using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloomFilters
{
    /// <summary>
    /// A 32-bit Bloom filter.
    /// </summary>
    /// <remarks>
    /// Bloom filters normally require you to specify the false positive rate you desire,
    /// and then infer the number of bits needed to satisfy that demand. Bloom32 instead
    /// provides a more compact abstraction of fixed bit length, which assumes you've
    /// already done the work to compute an upper bound on the false positive rate given
    /// the data sets you're likely to encounter.
    /// </remarks>
    public struct Bloom32 : IEquatable<Bloom32>
    {
        uint bits;

        /// <summary>
        /// Calculates the false positive rate given the number of items stored.
        /// </summary>
        /// <param name="count">The number of items.</param>
        /// <returns>The false positive probability.</returns>
        public double FalsePositiveRate(int count)
        {
            return Math.Pow(Math.E, -32 * Math.Pow(Math.Log(2), 2) / count);
        }

        /// <summary>
        /// Compute the Bloom filter that includes the new hash code.
        /// </summary>
        /// <param name="hashCode">The hash code to include.</param>
        /// <returns>A Bloom filter with the new hash code included.</returns>
        public Bloom32 Add(int hashCode)
        {
            return new Bloom32 { bits = bits | Mask(hashCode) };
        }

        /// <summary>
        /// Take the union of two Bloom filters.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Bloom32 Union(Bloom32 other)
        {
            return new Bloom32 { bits = bits | other.bits };
        }

        /// <summary>
        /// Take the union of two Bloom filters.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Bloom32 Intersect(Bloom32 other)
        {
            return new Bloom32 { bits = bits ^ other.bits };
        }

        /// <summary>
        /// Check whether the given hash code is in the set.
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns>True if the hash code may be in the set, false if it is definitely not in the set.</returns>
        public bool Contains(int hashCode)
        {
            return 0 != (bits & Mask(hashCode));
        }

        /// <summary>
        /// Check for equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Bloom32 other)
        {
            return bits == other.bits;
        }

        /// <summary>
        /// Check for equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Bloom32 && Equals((Bloom32)obj);
        }

        /// <summary>
        /// Compute a 32-bit hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return unchecked((int)bits);
        }

        /// <summary>
        /// Compare for equality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Bloom32 left, Bloom32 right)
        {
            return left.bits == right.bits;
        }

        /// <summary>
        /// Compare for inequality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Bloom32 left, Bloom32 right)
        {
            return left.bits != right.bits;
        }

        /// <summary>
        /// Compute the bitmask for a given hash code.
        /// </summary>
        static uint Mask(int hashCode)
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
