using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCL.Graphic;
using System.IO;
using System.Drawing;

namespace Structura
{
    public class Graphic: IMemoryOverlay
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
            gtImage image=new gtImage(500, 500, gtImage.Format.RGB);

			for(Int64 i=Constants.GraphicMemoryDisplayAdressStart; i<width*height; i++)
			{
				Int64 x=0;
				Int64 y=0;

				byte r=data[i+0];
				byte g=data[i+1];
				byte b=data[i+2];
				byte a=data[i+3];

				image.SetPixel((int)x, (int)y, Color.FromArgb(a,r,g,b));
			}

            image.SaveToPNGGDI("display.png");
        }
    }
}