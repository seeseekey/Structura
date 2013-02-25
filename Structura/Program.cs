using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Structura
{
	class Program
	{
		static UInt64 cycles=0;

		static void PrintInternalStates(Structura cpu)
		{
			Console.Clear();

			Console.WriteLine("Structura System v13.02");
			Console.WriteLine("");
			Console.WriteLine("Cycles: {0}", cycles);
			Console.WriteLine("");
			Console.WriteLine("Register:");
			Console.WriteLine("A: {0}, B: {1}, C: {2}, D: {3}, E: {4}, F: {5}, G: {6}, H: {7}", cpu.A, cpu.B, cpu.C, cpu.D, cpu.E, cpu.F, cpu.G, cpu.H);
			Console.WriteLine("I: {0}, J: {1}, K: {2}, L: {3}, M: {4}, N: {5}, O: {6}, P: {7}", cpu.I, cpu.J, cpu.K, cpu.L, cpu.M, cpu.N, cpu.O, cpu.P);
			Console.WriteLine("Q: {0}, R: {1}, S: {2}, T: {3}, U: {4}, V: {5}, W: {6}, X: {7}", cpu.Q, cpu.R, cpu.S, cpu.T, cpu.U, cpu.V, cpu.W, cpu.X);
			Console.WriteLine("Y: {0}, Z: {1}", cpu.Y, cpu.Z);
			Console.WriteLine("");
			Console.WriteLine("Special registers:");
			Console.WriteLine("IC: {0}", cpu.IC);
			Console.WriteLine("");
			Console.WriteLine("Flags:");
			Console.WriteLine("Zero: {0}, Positive: {1}, Negative: {2}, Overflow: {3}", cpu.Zero, cpu.Positive, cpu.Negative, cpu.Overflow);
		}

		static Structura cpu;
		static Graphic graphic;
		static int cycleInterval=1000;
		static bool running=true;

		static void Main(string[] args)
		{
			if(args.Length<1)
			{
				Console.WriteLine("Need assembler file.");
				return;
			}

			if(!File.Exists(args[0]))
			{
				Console.WriteLine("File don't exists.");
				return;
			}

			if(args.Length>1)
			{
				cycleInterval=Convert.ToInt32(args[1]);
			}

			Console.CancelKeyPress+=new ConsoleCancelEventHandler(Console_CancelKeyPress);

			graphic=new Graphic();
			Keyboard keyboard=new Keyboard();

			Memory memory=new Memory();
			memory.AddOverlayDevice(graphic);
			memory.AddOverlayDevice(keyboard);

			cpu=new Structura(memory);

			File.ReadAllText(args[0]);
			string[] asmCode=File.ReadAllLines(args[0]);

			Int64[] machineCode=Assembler.Assemble(asmCode);
			byte[] machineCodeAsByteArray=new byte[machineCode.Length*8];

			//Kopiere Int64 Array in ByteArray
			for(int i=0; i<machineCode.Length; i++)
			{
				byte[] i64=BitConverter.GetBytes(machineCode[i]);
				Array.Copy(i64, 0, machineCodeAsByteArray, i*8, 8);
			}

			memory.WriteData(0, machineCodeAsByteArray);

			//execute system
			Thread thread=new Thread(new ThreadStart(ExecuteSystem));
			thread.Start();

			//Keyboard aktivieren
			while(running)
			{
				ConsoleKeyInfo keyInfo=Console.ReadKey(true);

				byte modifier=(byte)keyInfo.Modifiers;
				byte[] sign=Encoding.UTF32.GetBytes(keyInfo.KeyChar.ToString());

				keyboard.AddKeyEvent(modifier, sign);
			}
		}

		static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			running=false;
			e.Cancel=true; // Event abbrechen
		}

		static void ExecuteSystem()
		{
			while(running)
			{
				PrintInternalStates(cpu);
				cpu.Execute();
				graphic.Display();
				cycles++;
				Thread.Sleep(cycleInterval);
			}
		}
	}
}