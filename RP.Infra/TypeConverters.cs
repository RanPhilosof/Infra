using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RP.Infra.TypeConverters
{
#if !NET6_0
    public static class GuidInt128Converter
    {
        public static Int128 GuidToInt128(Guid guid)
        {
            Span<byte> guidBytes = stackalloc byte[16];
            guid.TryWriteBytes(guidBytes);
            return MemoryMarshal.Read<Int128>(guidBytes);
        }

        public static Guid Int128ToGuid(Int128 value)
        {
            Span<byte> int128Bytes = stackalloc byte[16];
            MemoryMarshal.Write(int128Bytes, ref value);

            return new Guid(int128Bytes);
        }
    }
#endif
}
