using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura
{
	public static class Disassembler
	{
		static Int64 GetNextInstructionWord(byte[] data, ref Int64 IC)
		{
			//Get instruction word
			byte[] instructionWordAsBytes=new byte[8];
			Array.Copy(data, IC, instructionWordAsBytes, 0, 8);
			IC+=8;
			return BitConverter.ToInt64(instructionWordAsBytes, 0);
		}

		static string GetJumpAdressInterpretation(Int64 adressInterpretation)
		{
			switch(adressInterpretation)
			{
				case 0:
					{
						return "ANCTAAV";
					}
				case 1:
					{
						return "ACTAAV";
					}
			}

			throw new Exception("Unkown opcode");
		}

		static string GetJumpMode(Int64 jumpMode)
		{
			switch(jumpMode)
			{
				case 0:
					{
						return "NONE";
					}
				case 1:
					{
						return "ZERO";
					}
				case 2:
					{
						return "POS";
					}
				case 3:
					{
						return "NEG";
					}
				case 4:
					{
						return "OVF";
					}
			}

			throw new Exception("Unkown opcode");
		}

		static string GetJumpAdressing(Int64 adressing)
		{
			switch(adressing)
			{
				case 0:
					{
						return "ABS";
					}
				case 1:
					{
						return "REL";
					}
			}

			throw new Exception("Unkown opcode");
		}

		static string GetJumpAdressOrValue(Int64 adressOrValue)
		{
			switch(adressOrValue)
			{
				case 0:
					{
						return "ABS";
					}
				case 1:
					{
						return "REL";
					}
			}

			throw new Exception("Unkown opcode");
		}

		public static List<string> Disassemble(byte[] machineCodeAsByteArray)
		{
			List<string> ret=new List<string>();
			Int64 IC=0;

			while(IC<machineCodeAsByteArray.Length)
			{
				//Get instruction word
				Int64 instructionWord=GetNextInstructionWord(machineCodeAsByteArray, ref IC);

				//Auswerten
				switch(instructionWord)
				{
					case 0: //JUMP
						{
							string jump="JUMP ";
							jump+=GetJumpAdressInterpretation(GetNextInstructionWord(machineCodeAsByteArray, ref IC)) + " ";
							jump+=GetJumpMode(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+" ";
							jump+=GetJumpAdressing(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+" ";
							jump+=GetJumpAdressOrValue(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+";";

							ret.Add(jump);

							break;
						}
					case 1: //ADD
						{
							break;
						}
					case 2: //COPY
						{
							break;
						}
				}
			}

			return ret;
		}
	}
}
