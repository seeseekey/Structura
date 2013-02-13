using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura
{
    public class Graphic: IMemoryOverlay
    {
        byte[] data;// { get; private set; }

        public Graphic()
        {
            data=new byte[8192]; //8 Kilobyte
        }

        public byte[] GetData(Int64 offset, Int64 count)
        {
            byte[] ret=new byte[count];
            Array.Copy(data, offset, ret, 0, count);
            return ret;
        }
        
        public void WriteData(Int64 offset, byte[] bytes)
        {
            Array.Copy(bytes, 0, data, (int)offset, bytes.Length);
        }

        public long OverlayRangeStart
        {
            get
            {
                return Constants.GraphicMemoryAdressStart;
            }
        }
        
        public long OverlayRangeEnd
        {
            get
            {
                return Constants.GraphicMemoryAdressEnd;
            }
        }
    }
}