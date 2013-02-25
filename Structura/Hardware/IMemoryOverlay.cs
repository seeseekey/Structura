using System;

namespace Structura.Hardware
{
    public interface IMemoryOverlay
    {
        Int64 OverlayRangeStart { get; }
        Int64 OverlayRangeEnd { get; }

        byte[] GetData(Int64 offset, Int64 count);
        void  WriteData(Int64 offset, byte[] bytes);
    }
}