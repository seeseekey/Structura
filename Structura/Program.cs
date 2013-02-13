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
        static void PrintInternalStates(Structura cpu)
        {
            Console.Clear();

            Console.WriteLine("Structura System v13.02");
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
            Console.WriteLine("");
        }

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

            Memory memory=new Memory();
			Graphic graphic=new Graphic();

			Structura cpu=new Structura(memory, graphic);

			File.ReadAllText(args[0]);
			string[] asmCode=File.ReadAllLines(args[0]);
				
            Int64[] machineCode=Assembler.Assemble(asmCode);
			memory.WriteData(0, machineCode);

            while(true)
            {
                PrintInternalStates(cpu);
                cpu.Execute();
                Thread.Sleep(1000);
            }
        }
    }
}