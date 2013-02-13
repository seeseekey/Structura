using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura
{
    public class Assembler
    {
		#region Common
        static bool IsRegister(string registerOrAdress)
        {
            if(registerOrAdress.ToUpper()[0]>='A'&&registerOrAdress.ToUpper()[0]<='Z') return true;
            else return false;
        }

        static Int64 GetRegisterNumber(string register)
        {
            switch(register.ToUpper())
            {
                case "A":
                    {
                        return -1;
                    }
                case "B":
                    {
                        return -2;
                    }
                case "C":
                    {
                        return -3;
                    }
                case "D":
                    {
                        return -4;
                    }
                case "E":
                    {
                        return -5;
                    }
                case "F":
                    {
                        return -6;
                    }
                case "G":
                    {
                        return -7;
                    }
                case "H":
                    {
                        return -8;
                    }
                case "I":
                    {
                        return -9;
                    }
                case "J":
                    {
                        return -10;
                    }
                case "K":
                    {
                        return -11;
                    }
                case "L":
                    {
                        return -12;
                    }
                case "M":
                    {
                        return -13;
                    }
                case "N":
                    {
                        return -14;
                    }
                case "O":
                    {
                        return -15;
                    }
                case "P":
                    {
                        return -16;
                    }
                case "Q":
                    {
                        return -17;
                    }
                case "R":
                    {
                        return -18;
                    }
                case "S":
                    {
                        return -19;
                    }
                case "T":
                    {
                        return -20;
                    }
                case "U":
                    {
                        return -21;
                    }
                case "V":
                    {
                        return -22;
                    }
                case "W":
                    {
                        return -23;
                    }
                case "X":
                    {
                        return -24;
                    }
                case "Y":
                    {
                        return -25;
                    }
                case "Z":
                    {
                        return -26;
                    }
				case "ZERO":
					{
						return -27;
					}
                default:
                    {
                        throw new Exception("Unknown register");
                    }
            }
        }

        const Int64 GraphicMemoryAdress=9000000000000000000;
        const Int64 KeyboardMemoryAdress=9100000000000000000;
        #endregion

        #region JUMP
		enum AdressInterpretation
		{
			AdressNotContainsTargetAdressAsValue=0,
			AdressContainsTargetAdressAsValue=1
		}

		enum JumpCondition
		{
			None=0,
			Zero=1,
			Positive=2,
			Negative=3,
			Overflow=4
		}

		enum JumpMode
		{
			Absolute=0,
			Relative=1
		}

		static JumpMode GetJumpMode(string mode)
        {
            switch(mode.ToUpper())
            {
                case "ABS":
                    {
						return JumpMode.Absolute;
                    }
                case "REL":
                    {
						return JumpMode.Relative;
                    }
                default:
                    {
						throw new Exception("Unkown jump mode");
                    }
            }
        }

		static JumpCondition GetJumpCondition(string condition)
        {
            switch(condition.ToUpper())
            {
                case "NONE":
                    {
						return JumpCondition.None;
                    }
                case "ZERO":
                    {
						return JumpCondition.Zero;
                    }
                case "POS":
                    {
						return JumpCondition.Positive;
                    }
                case "NEG":
                    {
						return JumpCondition.Negative;
                    }
                case "OVF":
                    {
						return JumpCondition.Overflow;
                    }
                default:
                    {
						throw new Exception("Unkown jump condition");
                    }
            }
        }

		static Int64[] GetJumpInstruction(AdressInterpretation adressInterpretation, JumpCondition jumpCondition, JumpMode jumpMode, Int64 target)
		{
			Int64[] instruction=new Int64[5];
			instruction[0]=0;
			instruction[1]=(Int64)adressInterpretation;
			instruction[2]=(Int64)jumpCondition;
			instruction[3]=(Int64)jumpMode;
			instruction[4]=target;
			return instruction;
		}
        #endregion

        #region ADD
		enum AddMode
		{
			RegisterAndRegister=0,
			RegisterAndValue=1
		}

		static Int64[] GetAddInstruction(AddMode addMode, Int64 registerNumber, Int64 target)
        {
            Int64[] instruction=new Int64[4];
            instruction[0]=1;
            instruction[1]=(Int64)addMode;
            instruction[2]=registerNumber;
            instruction[3]=target;
            return instruction;
        }
        #endregion

        #region COPY
		enum CopyMode
		{
			NoAdressContainsTargetAdressAsValue=0,
			FirstAdressContainsTargetAdressAsValue=1,
			SecondAdressContainsTargetAdressAsValue=2,
			BothAdressContainsTargetAdressAsValue=3
		}

		static Int64[] GetCopyInstruction(CopyMode copyMode, Int64 count, Int64 source, Int64 target)
        {
            Int64[] instruction=new Int64[5];
            instruction[0]=2;
            instruction[1]=(Int64)copyMode;
			instruction[2]=count;
            instruction[3]=source;
            instruction[4]=target;

            return instruction;
        }
        #endregion

        public static Int64[] Assemble(string[] assembler)
        {
#if! DEBUG
            try
            {
#endif
                List<Int64> ret=new List<Int64>();

				//Kommentare entfernen und assembler code zusammenbauen
				string preprocessedCode="";
				for(int i=0; i<assembler.Length; i++)
				{
					if(assembler[i].IndexOf("//")!=-1)
					{
						preprocessedCode+=assembler[i].Substring(0, assembler[i].IndexOf("//")).Trim();
					}
					else
					{
						preprocessedCode+=assembler[i].Trim();
					}
				}

				string[] lines=preprocessedCode.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string line in lines)
                {
					string[] token=line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(token.Length==0) continue;

                    string instructionWord=token[0].ToUpper();

                    switch(instructionWord)
                    {
                        case "JUMP":
                            {
								bool adressContainsTargetAdressAsValue=token[3].StartsWith("*");
								AdressInterpretation adressInterpretation=adressContainsTargetAdressAsValue?AdressInterpretation.AdressContainsTargetAdressAsValue:AdressInterpretation.AdressNotContainsTargetAdressAsValue;

								token[3]=token[3].TrimStart('*');

                                JumpCondition jumpCondition=GetJumpCondition(token[1]);
                                JumpMode jumpMode=GetJumpMode(token[2]);
								Int64 target;

								if(IsRegister(token[3]))
								{
									target=GetRegisterNumber(token[3]);
								}
								else //Normale adresse
								{
									target=Convert.ToInt64(token[3]);
								}

								ret.AddRange(GetJumpInstruction(adressInterpretation, jumpCondition, jumpMode, target));

                                break;
                            }
                        case "NOOP":
                            {
								ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.None, JumpMode.Relative, 1));
                                break;
                            }
                        case "ADD":
                            {
                                Int64 target=0;
								AddMode addMode;

								if(IsRegister(token[2])) //Register and register
                                {
                                    target=Convert.ToInt64(GetRegisterNumber(token[2]));
									addMode=AddMode.RegisterAndRegister;
                                }
                                else //Register and value
                                {
                                    target=Convert.ToInt64(token[2]);
									addMode=AddMode.RegisterAndValue;
                                }

								ret.AddRange(GetAddInstruction(addMode, GetRegisterNumber(token[1]), target));
                                break;
                            }
						case "MUL":
							{
								Int64 target=0;
								AddMode addMode;

								if(IsRegister(token[2])) //Register and register
								{
									target=Convert.ToInt64(GetRegisterNumber(token[2]));
									addMode=AddMode.RegisterAndRegister;
								}
								else //Register and value
								{
									target=Convert.ToInt64(token[2]);
									addMode=AddMode.RegisterAndValue;
								}

								if(addMode==AddMode.RegisterAndValue)
								{
									ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("ZERO"), GetRegisterNumber("Z")));
									ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), target));
								}
								else //Register and Register
								{
									ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, target, GetRegisterNumber("Z"))); //Kopiere nach Z
								}

								ret.AddRange(GetAddInstruction(addMode, GetRegisterNumber(token[1]), GetRegisterNumber(token[1]))); //Source + Source
								ret.AddRange(GetAddInstruction(addMode, GetRegisterNumber("Z"), -1)); //DEC Z 1 //Breite bis hier -80
								ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, -80)); //Bedingter Sprung wenn Z>0;

								break;
							}
                        case "COPY":
                            {
								Int64 count=Convert.ToInt64(token[1]);

								bool firstAdressContainsTargetAdressAsValue=token[2].StartsWith("*");
								bool secondAdressContainsTargetAdressAsValue=token[3].StartsWith("*");

								token[2]=token[2].TrimStart('*');
								token[3]=token[3].TrimStart('*');

                                Int64 source;
                                Int64 target;

                                if(IsRegister(token[2])) source=GetRegisterNumber(token[2]);
                                else source=Convert.ToInt64(token[2]);

                                if(IsRegister(token[3])) target=GetRegisterNumber(token[3]);
                                else target=Convert.ToInt64(token[3]);

                                if(firstAdressContainsTargetAdressAsValue==false&&secondAdressContainsTargetAdressAsValue==false)
                                {
									ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, count, source, target)); //none
                                }
                                else if(firstAdressContainsTargetAdressAsValue==true&&secondAdressContainsTargetAdressAsValue==false)
                                {
									ret.AddRange(GetCopyInstruction(CopyMode.FirstAdressContainsTargetAdressAsValue, count, source, target)); //first adress contains adress
                                }
                                else if(firstAdressContainsTargetAdressAsValue==false&&secondAdressContainsTargetAdressAsValue==true)
                                {
									ret.AddRange(GetCopyInstruction(CopyMode.SecondAdressContainsTargetAdressAsValue, count, source, target)); 
                                }
                                else if(firstAdressContainsTargetAdressAsValue==true&&secondAdressContainsTargetAdressAsValue==true)
                                {
									ret.AddRange(GetCopyInstruction(CopyMode.BothAdressContainsTargetAdressAsValue, count, source, target)); 
                                }

                                break;
                            }
                        case "DEC":
                            {
                                ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber(token[1]), -1)); //RAV Register Value
                                break;
                            }
                        case "INC":
                            {
								ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber(token[1]), 1)); //RAV Register Value
                                break;
                            }
                        case "LOAD":
                            {
								ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, Convert.ToInt64(token[1]), GetRegisterNumber(token[2]))); //MTR Memory Register
                                break;
                            }
                        case "WRITE":
                            {
								ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(token[1]), Convert.ToInt64(token[2]))); //RTM Register Memory
                                break;
                            }
                        default:
                            {
                                throw new Exception("Unknown mnemonic");
                            }
                    }
                }

                return ret.ToArray();
#if! DEBUG
            }
            catch(Exception ex)
            {
                throw ex;
            }
#endif
        }
    }
}
