using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura
{
	public class Memory
	{
		public byte[] Data { get; private set; }

		public Memory()
		{
			Data=new byte[8192]; //8 Kilobyte
		}

		public void WriteIntoMemory(UInt64 machineCode, UInt64 offset)
		{
			UInt64[] array=new UInt64[1];
			array[0]=machineCode;
			WriteIntoMemory(array, offset);
		}

		public void WriteIntoMemory(UInt64[] machineCode, UInt64 offset)
		{
			for(int i=0; i<machineCode.Length; i++)
			{
				byte[] i64=BitConverter.GetBytes(machineCode[i]);
				Array.Copy(i64, 0, Data, (int)(offset)+i*8, 8);
			}
		}
	}
}
