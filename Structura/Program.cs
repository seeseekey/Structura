using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using CSCL;
using Structura.Hardware;
using Structura.Assembler;

namespace Structura
{
    class Program
    {
        static UInt64 cycles=0;

        static Hardware.Structura cpu;
        static Graphic graphic;
        static int cycleInterval=1000;
        static bool running=true;
        static bool traceExecution=false;
		static string traceExecutionFilename="trace.txt";

        static void PrintInternalStates(Hardware.Structura cpu)
        {
            Console.Clear();

            Console.WriteLine("Structura System v13.03");
            Console.WriteLine("");
            Console.WriteLine("Cycles: {0}", cycles);
            Console.WriteLine("");
            Console.WriteLine("Register:");
			Console.WriteLine("A: {0}, B: {1}, C: {2}, D: {3}, E: {4}, F: {5}, G: {6}, H: {7}, I: {8}, J: {9},", cpu.A, cpu.B, cpu.C, cpu.D, cpu.E, cpu.F, cpu.G, cpu.H, cpu.I, cpu.J);
			Console.WriteLine("K: {0}, L: {1}, M: {2}, N: {3}, O: {4}, P: {5}, Q: {6}, R: {7}, S: {8}, T: {9},", cpu.K, cpu.L, cpu.M, cpu.N, cpu.O, cpu.P, cpu.Q, cpu.R, cpu.S, cpu.T);
			Console.WriteLine("U: {0}, V: {1}, W: {2}, X: {3}, Y: {4}, Z: {5}", cpu.U, cpu.V, cpu.W, cpu.X, cpu.Y, cpu.Z);
            Console.WriteLine("");
            Console.WriteLine("Special registers:");
            Console.WriteLine("IC: {0}", cpu.IC);
            Console.WriteLine("");
            Console.WriteLine("Flags:");
            Console.WriteLine("Zero: {0}, Positive: {1}, Negative: {2}, Overflow: {3}", cpu.Zero, cpu.Positive, cpu.Negative, cpu.Overflow);
        }

        static void ShowHelp()
        {
            Console.Clear();

            Console.WriteLine("Structura System Help");
            Console.WriteLine("");
            Console.WriteLine("Structura.exe program.asm");
            Console.WriteLine("Structura.exe program.asm -cycleInterval:1000");
            Console.WriteLine("Structura.exe program.asm -disassemble");
            Console.WriteLine("");
            Console.WriteLine("Parameter:");
            Console.WriteLine("  -file:<filename>");
            Console.WriteLine("  -cycleInterval:<timeInMilliSeconds>");
            Console.WriteLine("  -disassemble <-withIC>");
            Console.WriteLine("  -traceExecution:<filename>");
        }

        static void Main(string[] args)
        {
            Parameters arguments=Parameters.InterpretCommandLine(args);

            if(!arguments.Contains("file000")||arguments.Contains("h")||arguments.Contains("help")||arguments.Contains("?"))
            {
                ShowHelp();
            }

            string filename=arguments.GetString("file000");

            if(!File.Exists(filename))
            {
                Console.WriteLine("File don't exists.");
                return;
            }

            if(arguments.Contains("cycleInterval"))
            {
                cycleInterval=Convert.ToInt32(arguments.GetString("cycleInterval"));
            }

            Console.CancelKeyPress+=new ConsoleCancelEventHandler(Console_CancelKeyPress);

            //Assemblieren
            string[] asmCode=File.ReadAllLines(filename);
            Int64[] machineCode=Assembler.Assembler.Assemble(asmCode);
            byte[] machineCodeAsByteArray=new byte[machineCode.Length*8];

            //Kopiere Int64 Array in ByteArray
            for(int i=0;i<machineCode.Length;i++)
            {
                byte[] i64=BitConverter.GetBytes(machineCode[i]);
                Array.Copy(i64, 0, machineCodeAsByteArray, i*8, 8);
            }

            //Prüfe auf Disassembler
            if(arguments.Contains("disassemble"))
            {
                bool withIC=arguments.GetBool("withIC", false);
                List<string> disassembly=Disassembler.Disassemble(machineCodeAsByteArray, withIC);

                foreach(string line in disassembly)
                {
                    Console.WriteLine(line);
                }

                return;
            }

            //System initialisieren
            graphic=new Graphic();
            Keyboard keyboard=new Keyboard();

            Memory memory=new Memory();
            memory.AddOverlayDevice(graphic);
            memory.AddOverlayDevice(keyboard);
            memory.WriteData(0, machineCodeAsByteArray);

            cpu=new Hardware.Structura(memory);

            //Trace execution
            traceExecution=arguments.Contains("traceExecution");
			if(traceExecution)
			{
				traceExecutionFilename=arguments.GetString("traceExecution");
			}

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
			StreamWriter writer=null;
            List<Int64> processedInstructions=new List<Int64>();

            if(traceExecution)
            {
				writer=new StreamWriter(traceExecutionFilename);
            }

            while(running)
            {
                PrintInternalStates(cpu);
				
                Int64[] processedInstruction;

                try
                {
                    processedInstruction=cpu.Execute();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    running=false;
                    break; //aus runnig ausbrechen
                }

                if(traceExecution)
                {
                    processedInstructions.AddRange(processedInstruction);
                }

                graphic.Display();

                cycles++;
                Thread.Sleep(cycleInterval);
            }

            if(traceExecution)
            {
				if(writer!=null)
				{
					List<string> lines=Disassembler.Disassemble(processedInstructions.ToArray(), false); //IC Nummern stimmen beim trace nicht (kein reiner Disassemble)

					foreach(string line in lines)
					{
						writer.WriteLine(line);
					}

					writer.Close();
				}
            }
        }
    }
}