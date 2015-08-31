# BloomFilters.NET

A set of simple Bloom filters for .NET:

 * Bloom32: an efficient 32-bit Bloom filter that uses a single System.UInt32 for the bitmap.
 * Bloom64: an efficient 64-bit Bloom filter that uses a single System.UInt64 for the bitmap.

Bloom filters normally require you to specify the false positive rate you desire,
and then infer the number of bits needed to satisfy that demand. Bloom64 instead
provides a more compact abstraction of fixed bit length, which assumes you've
already done the work to compute an upper bound on the false positive rate given
the data sets you're likely to encounter.

# LICENSE

LGPL version 2.
