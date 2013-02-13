using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura
{
    public class Memory
    {
        byte[] data;// { get; private set; }
        List<IMemoryOverlay> MemoryOverlays;

        public Memory()
        {
            data=new byte[8192]; //8 Kilobyte
            MemoryOverlays=new List<IMemoryOverlay>();
        }

        public void AddOverlayDevice(IMemoryOverlay overlay)
        {
            MemoryOverlays.Add(overlay);
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

        //public void WriteData(Int64 machineCode, Int64 offset)
        //{
        //    Int64[] array=new Int64[1];
        //    array[0]=machineCode;
        //    WriteData(offset, array);
        //}

        public void WriteData(Int64 offset, Int64[] machineCode)
        {
            for(int i=0;i<machineCode.Length;i++)
            {
                byte[] i64=BitConverter.GetBytes(machineCode[i]);
                Array.Copy(i64, 0, data, (int)(offset)+i*8, 8);
            }
        }
    }
}
