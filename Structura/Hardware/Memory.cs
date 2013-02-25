using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura.Hardware
{
    public class Memory
    {
        byte[] data;// { get; private set; }
        List<IMemoryOverlay> MemoryOverlays;

        bool IsAreaMemoryOverlayed(Int64 adress, Int64 count, out IMemoryOverlay overlayDevice)
        {
            foreach(IMemoryOverlay overlay in MemoryOverlays)
            {
                if(overlay.OverlayRangeStart>=adress&&overlay.OverlayRangeEnd<=adress+count)
                {
                    //Adresse liegt im Overlayfenster des Gerätes
                    overlayDevice=overlay;
                    return true;
                }
            }

            overlayDevice=null;

            if(adress>Constants.OverlayZoneStart)
                return true; //Adresse liegt im Overlaybereich
            return false;
        }

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
            IMemoryOverlay device;

            if(IsAreaMemoryOverlayed(offset, count, out device))
            {
                if(device!=null)
                {
                    return device.GetData(offset, count);
                }
                else
                {
                    throw new Exception("No assigned adress zone");
                }
            }
            else
            {
                byte[] ret=new byte[count];
                Array.Copy(data, offset, ret, 0, count);
                return ret;
            }
        }

        //TODO Check ob 
        public void WriteData(Int64 offset, byte[] bytes)
        {
            IMemoryOverlay device;

            if(IsAreaMemoryOverlayed(offset, bytes.Length, out device))
            {
                if(device!=null)
                {
                    device.WriteData(offset, bytes);
                }
                else
                {
                    throw new Exception("No assigned adress zone");
                }
            }
            else
            {
                Array.Copy(bytes, 0, data, (int)offset, bytes.Length);
            }
        }
    }
}
