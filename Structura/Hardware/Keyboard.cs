using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura.Hardware
{
	public class Keyboard : IMemoryOverlay
	{
		byte[] data;// { get; private set; }

		public Keyboard()
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
			lock(data)
			{
				Array.Copy(bytes, 0, data, (int)offset, bytes.Length);
			}
		}

		public long OverlayRangeStart
		{
			get
			{
				return Constants.KeyboardMemoryAdressStart;
			}
		}

		public long OverlayRangeEnd
		{
			get
			{
				return Constants.KeyboardMemoryAdressEnd;
			}
		}

		public void AddKeyEvent(byte modifier, byte[] sign)
		{
			lock(data)
			{
				//Zeichenindex aus Speicher holen erhöhen und zurückschreiben
				byte[] signIndex=new byte[2];
				Array.Copy(data, 0, signIndex, 0, 2);
				UInt16 currentSignIndex=BitConverter.ToUInt16(signIndex, 0);

				if(currentSignIndex>=1638) currentSignIndex=0; //Index zurücksetzen falls er am Ende angekommen ist

				//Zeichenarray zusammenbauen
				byte[] signData=new byte[5];
				signData[0]=modifier;
				Array.Copy(sign, 0, signData, 1, 4);

				//Zeichen in Speicher kopieren
				Array.Copy(signData, 0, data, currentSignIndex*5+2, 5);

				//SIgn Index zurückschreiben
				currentSignIndex++;
				signIndex=BitConverter.GetBytes(currentSignIndex);
				Array.Copy(signIndex, 0, data, 0, 2);
			}
		}
	}
}
