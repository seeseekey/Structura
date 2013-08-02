using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using CSCL.Imaging;

namespace Structura.Hardware
{
	public class Graphic : IMemoryOverlay
	{
		byte[] data;// { get; private set; }

		Int64 width=500;
		Int64 height=500;

		public Graphic()
		{
			data=new byte[8192]; //8 Kilobyte

			MemoryStream stream=new MemoryStream(data);
			BinaryWriter writer=new BinaryWriter(stream);
			writer.Write(width); //Bildschirmbreite
			writer.Write(height); //Bildschirmhöhe 
			writer.Flush();
			writer.Close();
		}

		public byte[] GetData(Int64 offset, Int64 count)
		{
			byte[] ret=new byte[count];
			Array.Copy(data, offset, ret, 0, count);
			return ret;
		}

		public void WriteData(Int64 offset, byte[] bytes)
		{
			//Check ob Speicher groß genug
			if(data.Length-offset<bytes.Length)
			{
				throw new OutOfMemoryException();
			}

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

		public void Display()
		{
			CSCL.Imaging.Graphic image=new CSCL.Imaging.Graphic((uint)width, (uint)height, Format.RGB);

			for(Int64 i=Constants.GraphicMemoryDisplayAdressStart; i<width*height; i++)
			{
				Int64 x=i%width;
				Int64 y=i/width;

				byte r=data[i+0];
				byte g=data[i+1];
				byte b=data[i+2];
				byte a=data[i+3];

				image.SetPixel((int)x, (int)y, Color.FromArgb(a, r, g, b));
			}

			image.SaveToPNG("display.png");
		}
	}
}