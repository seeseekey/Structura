using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura
{
    public class Assembler
    {
        #region Common
        static Int64 GetRegisterNumber(string number)
        {
            switch(number.ToUpper())
            {
                case "A":
                    {
                        return 0;
                    }
                case "B":
                    {
                        return 1;
                    }
                case "C":
                    {
                        return 2;
                    }
                case "D":
                    {
                        return 3;
                    }
                case "E":
                    {
                        return 4;
                    }
                case "F":
                    {
                        return 5;
                    }
                case "G":
                    {
                        return 6;
                    }
                case "H":
                    {
                        return 7;
                    }
                case "I":
                    {
                        return 8;
                    }
                case "J":
                    {
                        return 9;
                    }
                case "K":
                    {
                        return 10;
                    }
                case "L":
                    {
                        return 11;
                    }
                case "M":
                    {
                        return 12;
                    }
                case "N":
                    {
                        return 13;
                    }
                case "O":
                    {
                        return 14;
                    }
                case "P":
                    {
                        return 15;
                    }
                case "Q":
                    {
                        return 16;
                    }
                case "R":
                    {
                        return 17;
                    }
                case "S":
                    {
                        return 18;
                    }
                case "T":
                    {
                        return 19;
                    }
                case "U":
                    {
                        return 20;
                    }
                case "V":
                    {
                        return 21;
                    }
                case "W":
                    {
                        return 22;
                    }
                case "X":
                    {
                        return 23;
                    }
                case "Y":
                    {
                        return 24;
                    }
                case "Z":
                    {
                        return 25;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

		const Int64 GraphicMemoryAdress=9000000000000000000;
		const Int64 KeyboardMemoryAdress=9100000000000000000;
        #endregion

        #region JUMP
        static Int64 GetJumpMode(string mode)
        {
            switch(mode.ToUpper())
            {
                case "ABS":
                    {
                        return 0;
                    }
                case "REL":
                    {
                        return 1;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        static Int64 GetJumpCondition(string condition)
        {
            switch(condition.ToUpper())
            {
                case "NONE":
                    {
                        return 0;
                    }
                case "ZERO":
                    {
                        return 1;
                    }
                case "POS":
                    {
                        return 2;
                    }
                case "NEG":
                    {
                        return 3;
                    }
				case "OVF":
					{
						return 4;
					}
                default:
                    {
                        return 0;
                    }
            }
        }
        #endregion

        #region ADD
        static Int64 GetAddMode(string mode)
        {
            switch(mode.ToUpper())
            {
                case "RAR":
                    {
                        return 0;
                    }
                case "RAV":
                    {
                        return 1;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

		static Int64[] GetAddInstruction(Int64 addMode, Int64 registerNumber, Int64 target)
		{
			Int64[] instruction=new Int64[4];
			instruction[0]=1;
			instruction[1]=addMode;
			instruction[2]=registerNumber;
			instruction[3]=target;
			return instruction;
		}
        #endregion

        #region COPY
        static Int64 GetCopyMode(string mode)
        {
            switch(mode.ToUpper())
            {
                case "RTR":
                    {
                        return 0;
                    }
                case "RTM":
                    {
                        return 1;
                    }
                case "MTR":
                    {
                        return 2;
                    }
                case "MTM":
                    {
                        return 3;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

		static Int64[] GetCopyInstruction(Int64 copyMode, Int64 source, Int64 target)
		{
			Int64[] instruction=new Int64[4];
			instruction[0]=2;
			instruction[1]=copyMode;
			instruction[2]=source;
			instruction[3]=target;

			return instruction;
		}
        #endregion

        public static Int64[] Assemble(string assembler)
        {
            try
            {
				List<Int64> ret=new List<Int64>();
                string[] lines=assembler.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string line in lines)
                {
                    Int64[] instruction=null;

                    string[] token=line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string instructionWord=token[0].ToUpper();

                    switch(instructionWord)
                    {
                        case "JUMP":
                            {
                                instruction=new Int64[4];
                                instruction[0]=0;
                                instruction[1]=GetJumpCondition(token[1]);
                                instruction[2]=GetJumpMode(token[2]);
                                instruction[3]=Convert.ToInt64(token[3]);

                                break;
                            }
						case "NOOP":
							{
								instruction=new Int64[4];
								instruction[0]=0;
								instruction[1]=0; //Jumpcondition NONE
								instruction[2]=1; //Jumpmode REL
								instruction[3]=1; //Set IC==IC+1

								break;
							}
                        case "ADD":
                            {
								Int64 target=0;

                                if(token[1].ToUpper()=="RAR") //Register and register
                                {
                                    target=Convert.ToInt64(GetRegisterNumber(token[3]));
                                }
                                else //Register and value
                                {
                                    target=Convert.ToInt64(token[3]);
                                }

								instruction=GetAddInstruction(GetAddMode(token[1]), GetRegisterNumber(token[2]), target);

                                break;
                            }
                        case "COPY":
                            {
                                Int64 source;
								Int64 target;

                                if(token[1].ToUpper()=="RTR"||token[1].ToUpper()=="RTM") //First value is register
                                {
                                    source=GetRegisterNumber(token[2]);
                                }
                                else //First value is memory adress
                                {
                                    source=GetRegisterNumber(token[2]);
                                }
                            
                                if(token[1].ToUpper()=="RTR"||token[1].ToUpper()=="MTR") //Second value is register
                                {
                                    target=Convert.ToInt64(GetRegisterNumber(token[3]));
                                }
                                else //Second value is memory adress
                                {
                                    target=Convert.ToInt64(token[3]);
                                }

								instruction=GetCopyInstruction(GetCopyMode(token[1]), source, target);
                            
                                break;
                            }
						case "DEC":
							{
								instruction=GetAddInstruction(1, GetRegisterNumber(token[1]), -1); //RAV Register Value
								break;
							}
						case "INC":
							{
								instruction=GetAddInstruction(1, GetRegisterNumber(token[1]), 1); //RAV Register Value
								break;
							}
						case "LOAD":
							{
								instruction=GetCopyInstruction(2, Convert.ToInt64(token[1]), GetRegisterNumber(token[2])); //MTR Memory Register
								break;
							}
						case "WRITE":
							{
								instruction=GetCopyInstruction(1, GetRegisterNumber(token[1]), Convert.ToInt64(token[2])); //RTM Register Memory
								break;
							}
                        default:
                            {
                                throw new Exception("Unknown opcode");
                            }
                    }

                    ret.AddRange(instruction);
                }

                return ret.ToArray();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
