using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structura.Assembler.Jump;
using Structura.Assembler.Add;
using Structura.Assembler.Copy;

namespace Structura.Assembler
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

		static string GetRegisterName(Int64 number)
		{
			switch(number)
			{
				case -1:
					{
						return "A";
					}
				case -2:
					{
						return "B";
					}
				case -3:
					{
						return "C";
					}
				case -4:
					{
						return "D";
					}
				case -5:
					{
						return "E";
					}
				case -6:
					{
						return "F";
					}
				case -7:
					{
						return "G";
					}
				case -8:
					{
						return "H";
					}
				case -9:
					{
						return "I";
					}
				case -10:
					{
						return "J";
					}
				case -11:
					{
						return "K";
					}
				case -12:
					{
						return "L";
					}
				case -13:
					{
						return "M";
					}
				case -14:
					{
						return "N";
					}
				case -15:
					{
						return "O";
					}
				case -16:
					{
						return "P";
					}
				case -17:
					{
						return "Q";
					}
				case -18:
					{
						return "R";
					}
				case -19:
					{
						return "S";
					}
				case -20:
					{
						return "T";
					}
				case -21:
					{
						return "U";
					}
				case -22:
					{
						return "V";
					}
				case -23:
					{
						return "W";
					}
				case -24:
					{
						return "X";
					}
				case -25:
					{
						return "Y";
					}
				case -26:
					{
						return "Z";
					}
				case -27:
					{
						return "ZERO"; //ZERO ROM
					}
				default:
					{
						throw new Exception("Unknown register number");
					}
			}
		}

		public static List<string> Disassemble(byte[] machineCodeAsByteArray, bool withIC)
		{
			List<string> ret=new List<string>();
			Int64 IC=0;

			while(IC<machineCodeAsByteArray.Length)
			{
				Int64 preIC=IC;

				//Get instruction word
				Int64 instructionWord=GetNextInstructionWord(machineCodeAsByteArray, ref IC);

				//Auswerten
				switch(instructionWord)
				{
					case 0: //JUMP
						{
							string jump="JUMP ";

							AdressInterpretation adressInterpretation=(AdressInterpretation)GetNextInstructionWord(machineCodeAsByteArray, ref IC);
							jump+=GetJumpMode(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+" ";
							jump+=GetJumpAdressing(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+" ";

							switch(adressInterpretation)
							{
								case AdressInterpretation.AdressNotContainsTargetAdressAsValue:
									{
										jump+=GetNextInstructionWord(machineCodeAsByteArray, ref IC)+";";
										break;
									}
								case AdressInterpretation.AdressContainsTargetAdressAsValue:
									{
										jump+="*"+GetNextInstructionWord(machineCodeAsByteArray, ref IC)+";";
										break;
									}
								case AdressInterpretation.RegisterNotContainsTargetAdressAsValue:
									{
										jump+=GetRegisterName(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+";";
										break;
									}
								case AdressInterpretation.RegisterContainsTargetAdressAsValue:
									{
										jump+="*"+GetRegisterName(GetNextInstructionWord(machineCodeAsByteArray, ref IC))+";";
										break;
									}
							}

							if(withIC)
							{
								int padLength=16-(preIC.ToString().Length+IC.ToString().Length);
								string padding="".PadRight(padLength, ' ');
								ret.Add(String.Format("({0}/{1}) {2} {3}", preIC, IC, padding, jump));
							}
							else ret.Add(jump);
							break;
						}
					case 1: //ADD
						{
							string add="ADD ";
							
							AddMode addMode=(AddMode)GetNextInstructionWord(machineCodeAsByteArray, ref IC);
							add+=GetRegisterName(GetNextInstructionWord(machineCodeAsByteArray, ref IC)) + " ";

							switch(addMode)
							{
								case AddMode.RegisterAndRegister:
									{
										add+=GetRegisterName(GetNextInstructionWord(machineCodeAsByteArray, ref IC)) + ";";
										break;
									}
									case AddMode.RegisterAndValue:
									{
										add+=GetNextInstructionWord(machineCodeAsByteArray, ref IC) + ";";
										break;
									}
							}

							if(withIC)
							{
								int padLength=16-(preIC.ToString().Length+IC.ToString().Length);
								string padding="".PadRight(padLength, ' ');
								ret.Add(String.Format("({0}/{1}) {2} {3}", preIC, IC, padding, add));
							}
							else ret.Add(add);
							break;
						}
					case 2: //COPY
						{
							string copy="COPY ";

							CopyMode copyMode=(CopyMode)GetNextInstructionWord(machineCodeAsByteArray, ref IC);

							copy+=GetNextInstructionWord(machineCodeAsByteArray, ref IC)+" "; //CopyLength

							//FirstValue
							Int64 registerOrMemoryAdress=GetNextInstructionWord(machineCodeAsByteArray, ref IC);
							string registerOrMemoryAdressAsString;
							if(registerOrMemoryAdress<0)
							{
								registerOrMemoryAdressAsString=GetRegisterName(registerOrMemoryAdress)+" ";
							}
							else
							{
								registerOrMemoryAdressAsString=registerOrMemoryAdress+" ";
							}

							if(copyMode==CopyMode.FirstAdressContainsTargetAdressAsValue||copyMode==CopyMode.BothAdressContainsTargetAdressAsValue)
							{
								registerOrMemoryAdressAsString="*"+registerOrMemoryAdressAsString;
							}

							copy+=registerOrMemoryAdressAsString;

							//Second
							registerOrMemoryAdress=GetNextInstructionWord(machineCodeAsByteArray, ref IC);
							if(registerOrMemoryAdress<0)
							{
								registerOrMemoryAdressAsString=GetRegisterName(registerOrMemoryAdress)+";";
							}
							else
							{
								registerOrMemoryAdressAsString=registerOrMemoryAdress+";";
							}

							if(copyMode==CopyMode.SecondAdressContainsTargetAdressAsValue||copyMode==CopyMode.BothAdressContainsTargetAdressAsValue)
							{
								registerOrMemoryAdressAsString="*"+registerOrMemoryAdressAsString;
							}

							copy+=registerOrMemoryAdressAsString;

							if(withIC)
							{
								int padLength=16-(preIC.ToString().Length+IC.ToString().Length);
								string padding="".PadRight(padLength, ' ');
								ret.Add(String.Format("({0}/{1}) {2} {3}", preIC, IC, padding, copy));
							}
							else ret.Add(copy);
							break;
						}
				}
			}

			return ret;
		}
	}
}
