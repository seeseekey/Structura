﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structura.Assembler.Jump;
using Structura.Assembler.Add;
using Structura.Assembler.Copy;

namespace Structura.Assembler
{
    public class Assembler
    {
		#region Common
        static bool IsRegister(string registerOrAdress)
        {
            if(registerOrAdress.ToUpper()[0]>='A'&&registerOrAdress.ToUpper()[0]<='Z')
                return true;
            else
                return false;
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
        #endregion

        #region JUMP
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
        static Int64[] GetCopyInstruction(CopyMode copyMode, Int64 count, Int64 target, Int64 source)
        {
            Int64[] instruction=new Int64[5];
            instruction[0]=2;
            instruction[1]=(Int64)copyMode;
            instruction[2]=count;
			instruction[3]=target;
			instruction[4]=source;

            return instruction;
        }
        #endregion

		#region MAKRO
        static List<Int64> GetCopy(string countAsString, string val1, string val2)
        {
            List<Int64> ret=new List<Int64>();

            Int64 count=Convert.ToInt64(countAsString);

            bool firstAdressContainsTargetAdressAsValue=val1.StartsWith("*");
            bool secondAdressContainsTargetAdressAsValue=val2.StartsWith("*");

            val1=val1.TrimStart('*');
            val2=val2.TrimStart('*');

            Int64 target;
            Int64 source;

            if(IsRegister(val1))
                target=GetRegisterNumber(val1);
            else
                target=Convert.ToInt64(val1);

            if(IsRegister(val2))
                source=GetRegisterNumber(val2);
            else
                source=Convert.ToInt64(val2);

            if(firstAdressContainsTargetAdressAsValue==false&&secondAdressContainsTargetAdressAsValue==false)
            {
                ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, count, target, source)); //none
            }
            else if(firstAdressContainsTargetAdressAsValue==true&&secondAdressContainsTargetAdressAsValue==false)
            {
                ret.AddRange(GetCopyInstruction(CopyMode.FirstAdressContainsTargetAdressAsValue, count, target, source)); //first adress contains adress
            }
            else if(firstAdressContainsTargetAdressAsValue==false&&secondAdressContainsTargetAdressAsValue==true)
            {
                ret.AddRange(GetCopyInstruction(CopyMode.SecondAdressContainsTargetAdressAsValue, count, target, source));
            }
            else if(firstAdressContainsTargetAdressAsValue==true&&secondAdressContainsTargetAdressAsValue==true)
            {
                ret.AddRange(GetCopyInstruction(CopyMode.BothAdressContainsTargetAdressAsValue, count, target, source));
            }

            return ret;
        }

		static List<Int64> GetNeg(string val1, string workRegister)
		{
			List<Int64> ret=new List<Int64>();
			ret.AddRange(GetCopy("8", workRegister, val1)); //Kopiere Register auf Z
			ret.AddRange(GetAdd(val1, "-"+workRegister));
			ret.AddRange(GetAdd(val1, "-"+workRegister));
			ret.AddRange(GetCopy("8", workRegister, "ZERO")); //setze z auf 0
			return ret;
		}

        static List<Int64> GetMultiplication(string val1, string val2)
        {
            List<Int64> ret=new List<Int64>();

            Int64 target=0;
            AddMode addMode;

            if(IsRegister(val2)) //Register and register
            {
                target=Convert.ToInt64(GetRegisterNumber(val2));
                addMode=AddMode.RegisterAndRegister;
            }
            else //Register and value
            {
                target=Convert.ToInt64(val2);
                addMode=AddMode.RegisterAndValue;
            }

            ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Y"), GetRegisterNumber(val1))); //kopiere Register auf Y

            //AddMode auswerten
            if(addMode==AddMode.RegisterAndValue)
            {
                ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), GetRegisterNumber("ZERO")));
                ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), target));
            }
            else //Register and Register
            {
                ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), target)); //Kopiere nach Z
            }

            //Vorzeichen zählen
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("X"), GetRegisterNumber("ZERO")));

            //Y
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Y"), 0));
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 72)); //Überspringe Abs Block
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 32)); //Überspringe Abs Block
            ret.AddRange(GetAdd("X", "1"));

            //Z
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), 0));
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 72)); //Überspringe Abs Block
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 32)); //Überspringe Abs Block
            ret.AddRange(GetAdd("X", "1"));

            //Zähler absolut machen
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("ZERO")));
            ret.AddRange(GetAbs("Y", "W")); 
            ret.AddRange(GetAbs("Z", "W")); 

            //Multiplikationsschleife
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndRegister, GetRegisterNumber(val1), GetRegisterNumber("Y"))); //Source + Source
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), -1)); //DEC Z 1 //Breite bis hier -80
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, -104)); //Bedingter Sprung wenn Z>0;

            //Vorzeichen wieder antragen

            //Ist X==2?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), -2));
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 368)); //springe zum register leeren

            //Ist X==1?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), -1));
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 112)); //springe zum vorzeichen umdrehen

            //Ist X==0?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
            ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), 0));
            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 144)); //springe zum register leeren

            //Vorzeichenanzahl ist 1, damit ist Vorzeichen negativ
            ret.AddRange(GetNeg(val1, "W"));

            //Bereinige Register X, Y und Z
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("X"), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Y"), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), GetRegisterNumber("ZERO")));

            return ret;
        }

		static List<Int64> GetDivision(string val1, string val2)
		{
			List<Int64> ret=new List<Int64>();

			Int64 target=0;
			AddMode addMode;

			if(IsRegister(val2)) //Register and register
			{
				target=Convert.ToInt64(GetRegisterNumber(val2));
				addMode=AddMode.RegisterAndRegister;
			}
			else //Register and value
			{
				target=Convert.ToInt64(val2);
				addMode=AddMode.RegisterAndValue;
			}

			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Y"), GetRegisterNumber(val1))); //kopiere Register auf Y

			//AddMode auswerten
			if(addMode==AddMode.RegisterAndValue)
			{
				ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), GetRegisterNumber("ZERO")));
				ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), target));
			}
			else //Register and Register
			{
				ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), target)); //Kopiere nach Z
			}

			//Feststellen ob Division durch 0 vorliegt
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 1624)); //Überspringe restlichen DIV Block

			//Vorzeichen zählen
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("X"), GetRegisterNumber("ZERO")));

			//Y
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Y"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 72)); //Überspringe Abs Block
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 32)); //Überspringe Abs Block
			ret.AddRange(GetAdd("X", "1"));

			//Z
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 72)); //Überspringe Abs Block
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 32)); //Überspringe Abs Block
			ret.AddRange(GetAdd("X", "1"));

			//Zähler absolut machen
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("ZERO")));
			ret.AddRange(GetAbs("Y", "W"));
			ret.AddRange(GetAbs("Z", "W"));

			//Zielregister leeren um Ergebnis zu speichern
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("ZERO")));

			//Divisionsschleife
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndNegativeRegister, GetRegisterNumber("Y"), GetRegisterNumber("Z"))); //Source + Source
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 152)); //Bedingter Sprung wenn Z>0;
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Negative, JumpMode.Relative, 112)); //Bedingter Sprung wenn Z>0;
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber(val1), 1)); //wenn wert nicht negativ oder null, zähle hoch
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.None, JumpMode.Relative, -184)); //Sprung zurück

			//Vorzeichen wieder antragen

			//Ist X==2?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), -2));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 368)); //springe zum register leeren

			//Ist X==1?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), -1));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 112)); //springe zum vorzeichen umdrehen

			//Ist X==0?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 144)); //springe zum register leeren

			//Vorzeichenanzahl ist 1, damit ist Vorzeichen negativ
			ret.AddRange(GetNeg(val1, "W"));

			//Bereinige Register X, Y und Z
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("X"), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Y"), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), GetRegisterNumber("ZERO")));

			return ret;
		}

		static List<Int64> GetModulo(string val1, string val2)
		{
			List<Int64> ret=new List<Int64>();

			Int64 target=0;
			AddMode addMode;

			if(IsRegister(val2)) //Register and register
			{
				target=Convert.ToInt64(GetRegisterNumber(val2));
				addMode=AddMode.RegisterAndRegister;
			}
			else //Register and value
			{
				target=Convert.ToInt64(val2);
				addMode=AddMode.RegisterAndValue;
			}

			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Y"), GetRegisterNumber(val1))); //kopiere Register auf Y

			//AddMode auswerten
			if(addMode==AddMode.RegisterAndValue)
			{
				ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), GetRegisterNumber("ZERO")));
				ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), target));
			}
			else //Register and Register
			{
				ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), target)); //Kopiere nach Z
			}

			//Feststellen ob Division durch 0 vorliegt
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 1624)); //Überspringe restlichen DIV Block

			//Vorzeichen zählen
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("X"), GetRegisterNumber("ZERO")));

			//Y
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Y"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 72)); //Überspringe Abs Block
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 32)); //Überspringe Abs Block
			ret.AddRange(GetAdd("X", "1"));

			//Z
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("Z"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 72)); //Überspringe Abs Block
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 32)); //Überspringe Abs Block
			ret.AddRange(GetAdd("X", "1"));

			//Zähler absolut machen
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("ZERO")));
			ret.AddRange(GetAbs("Y", "W"));
			ret.AddRange(GetAbs("Z", "W"));

			//Zielregister leeren um Ergebnis zu speichern
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("Y"))); //ersten Rest zuweisen

			//Divisionsschleife
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndNegativeRegister, GetRegisterNumber("Y"), GetRegisterNumber("Z"))); //Source + Source
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 120)); //Bedingter Sprung wenn Z>0;
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Negative, JumpMode.Relative, 80)); //Bedingter Sprung wenn Z>0;
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber(val1), GetRegisterNumber("Y"))); //Rest zuweisen
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.None, JumpMode.Relative, -192)); //Sprung zurück

			//Vorzeichen wieder antragen

			//Ist X==2?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), -2));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 368)); //springe zum register leeren

			//Ist X==1?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), -1));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 112)); //springe zum vorzeichen umdrehen

			//Ist X==0?
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("W"), GetRegisterNumber("X")));
			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("W"), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 144)); //springe zum register leeren

			//Vorzeichenanzahl ist 1, damit ist Vorzeichen negativ
			ret.AddRange(GetNeg(val1, "W"));

			//Bereinige Register X, Y und Z
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("X"), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Y"), GetRegisterNumber("ZERO")));
			ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("Z"), GetRegisterNumber("ZERO")));

			return ret;
		}

		static List<Int64> GetAbs(string val1, string workRegister)
		{
			List<Int64> ret=new List<Int64>();

			ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber(val1), 0));
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, 184)); //Überspringe Abs Block
			ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Zero, JumpMode.Relative, 144)); //Überspringe Abs Block

			//Abs Block
			ret.AddRange(GetCopy("8", workRegister, val1)); //Kopiere Register auf Z
			ret.AddRange(GetAdd(val1, "-"+workRegister));
			ret.AddRange(GetAdd(val1, "-"+workRegister));
			ret.AddRange(GetCopy("8", workRegister, "ZERO")); //setze zw auf 0

			return ret;
		}

        static List<Int64> GetAdd(string val1, string val2)
        {
            List<Int64> ret=new List<Int64>();

            Int64 target=0;
            AddMode addMode;

            if(IsRegister(val2.TrimStart('-'))) //Register and register
            {
                target=Convert.ToInt64(GetRegisterNumber(val2.TrimStart('-')));

                if(val2.StartsWith("-"))
                    addMode=AddMode.RegisterAndNegativeRegister;
                else
                    addMode=AddMode.RegisterAndRegister;
            }
            else //Register and value
            {
                target=Convert.ToInt64(val2);
                addMode=AddMode.RegisterAndValue;
            }

            ret.AddRange(GetAddInstruction(addMode, GetRegisterNumber(val1), target));

            return ret;
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
            for(int i=0;i<assembler.Length;i++)
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
                if(token.Length==0)
                    continue;

                string instructionWord=token[0].ToUpper();

                switch(instructionWord)
                {
                    case "JUMP":
                        {
                            bool valueContainsTargetAdressAsValue=token[3].StartsWith("*");
                            token[3]=token[3].TrimStart('*');

                            bool valueIsRegister=IsRegister(token[3]);

                            AdressInterpretation adressInterpretation;

                            if(valueContainsTargetAdressAsValue)
                            {
                                if(valueIsRegister)
                                    adressInterpretation=AdressInterpretation.RegisterContainsTargetAdressAsValue;
                                else
                                    adressInterpretation=AdressInterpretation.AdressContainsTargetAdressAsValue;
                            }
                            else
                            {
                                if(valueIsRegister)
                                    adressInterpretation=AdressInterpretation.RegisterNotContainsTargetAdressAsValue;
                                else
                                    adressInterpretation=AdressInterpretation.AdressNotContainsTargetAdressAsValue;
                            }

                            JumpCondition jumpCondition=GetJumpCondition(token[1]);
                            JumpMode jumpMode=GetJumpMode(token[2]);
                            Int64 target;

                            if(valueIsRegister)
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
                            ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.None, JumpMode.Relative, 0));
                            break;
                        }
                    case "ABS":
                        {
                            ret.AddRange(GetAbs(token[1], "Z"));
                            break;
                        }
					case "NEG":
						{
							ret.AddRange(GetCopy("8", "Z", token[1])); //Kopiere Register auf Z
							ret.AddRange(GetAdd(token[1], "-Z"));
							ret.AddRange(GetAdd(token[1], "-Z"));
							ret.AddRange(GetCopy("8", "Z", "ZERO")); //setze z auf 0
							break;
						}
                    case "ADD":
                        {
                            ret.AddRange(GetAdd(token[1], token[2]));
                            break;
                        }
                    case "MUL":
                        {
                            ret.AddRange(GetMultiplication(token[1], token[2]));
                            break;
                        }
					case "DIV":
						{
							ret.AddRange(GetDivision(token[1], token[2]));
							break;
						}
					case "MOD":
						{
							ret.AddRange(GetModulo(token[1], token[2]));
							break;
						}
                    case "SHIFTL":
						{
							//Vorbereitung für SHIFTL
							Int64 target;
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

							//Register W X Y Z vorbereiten
							ret.AddRange(GetCopy("8", "U", "ZERO")); //setze w auf 0
							ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("U"), 2)); //setze w auf 2

							if(addMode==AddMode.RegisterAndRegister)
								ret.AddRange(GetCopy("8", "V", token[2]));
							else
							{
								//RegisterAndValue
								ret.AddRange(GetCopy("8", "V", "ZERO")); //setze w auf 0
								ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("V"), Convert.ToInt64(token[2]))); //Wert setzen
							}

							//DEC X um 1
							ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("V"), -1));

							//Wert aufmultiplizieren
							ret.AddRange(GetMultiplication("U", "2"));
							ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("V"), -1)); //DEC X 1
							ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, -400)); //Bedingter Sprung wenn X>0;

							//In W ist nun der 2^(x) Wert enthalten / Nun Multiplikation mit dem ersten Wert
							ret.AddRange(GetMultiplication(token[1], "U"));

							//W bereinigen
							ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("U"), GetRegisterNumber("ZERO")));

							break;
						}
					case "SHIFTR":
						{
							//Vorbereitung für SHIFTR
							Int64 target;
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

							//Register W X Y Z vorbereiten
							ret.AddRange(GetCopy("8", "U", "ZERO")); //setze w auf 0
							ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("U"), 2)); //setze w auf 2

							if(addMode==AddMode.RegisterAndRegister)
								ret.AddRange(GetCopy("8", "V", token[2]));
							else
							{
								//RegisterAndValue
								ret.AddRange(GetCopy("8", "V", "ZERO")); //setze w auf 0
								ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("V"), Convert.ToInt64(token[2]))); //Wert setzen
							}

							//DEC X um 1
							ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("V"), -1));

							//Wert aufmultiplizieren
							ret.AddRange(GetMultiplication("U", "2"));
							ret.AddRange(GetAddInstruction(AddMode.RegisterAndValue, GetRegisterNumber("V"), -1)); //DEC X 1
							ret.AddRange(GetJumpInstruction(AdressInterpretation.AdressNotContainsTargetAdressAsValue, JumpCondition.Positive, JumpMode.Relative, -400)); //Bedingter Sprung wenn X>0;

							//In W ist nun der 2^(x) Wert enthalten / Nun Multiplikation mit dem ersten Wert
							ret.AddRange(GetDivision(token[1], "U"));

							//W bereinigen
							ret.AddRange(GetCopyInstruction(CopyMode.NoAdressContainsTargetAdressAsValue, 8, GetRegisterNumber("U"), GetRegisterNumber("ZERO")));

							break;
						}
                    case "COPY":
                        {
                            ret.AddRange(GetCopy(token[1], token[2], token[3]));
                            break;
                        }
                    case "CLR":
                        {
                            if(token[1]=="ALL")
                            {
								ret.AddRange(GetCopy("8", "A", "ZERO"));
								ret.AddRange(GetCopy("8", "B", "ZERO"));
								ret.AddRange(GetCopy("8", "C", "ZERO"));
								ret.AddRange(GetCopy("8", "D", "ZERO"));
								ret.AddRange(GetCopy("8", "E", "ZERO"));
								ret.AddRange(GetCopy("8", "F", "ZERO"));
								ret.AddRange(GetCopy("8", "G", "ZERO"));
								ret.AddRange(GetCopy("8", "H", "ZERO"));
								ret.AddRange(GetCopy("8", "I", "ZERO"));
								ret.AddRange(GetCopy("8", "J", "ZERO"));
								ret.AddRange(GetCopy("8", "K", "ZERO"));
								ret.AddRange(GetCopy("8", "L", "ZERO"));
								ret.AddRange(GetCopy("8", "M", "ZERO"));
								ret.AddRange(GetCopy("8", "N", "ZERO"));
								ret.AddRange(GetCopy("8", "O", "ZERO"));
								ret.AddRange(GetCopy("8", "P", "ZERO"));
								ret.AddRange(GetCopy("8", "Q", "ZERO"));
								ret.AddRange(GetCopy("8", "R", "ZERO"));
								ret.AddRange(GetCopy("8", "S", "ZERO"));
								ret.AddRange(GetCopy("8", "T", "ZERO"));
								ret.AddRange(GetCopy("8", "U", "ZERO"));
								ret.AddRange(GetCopy("8", "V", "ZERO"));
								ret.AddRange(GetCopy("8", "W", "ZERO"));
								ret.AddRange(GetCopy("8", "X", "ZERO"));
								ret.AddRange(GetCopy("8", "Y", "ZERO"));
								ret.AddRange(GetCopy("8", "Z", "ZERO"));
                            }
                            else
                            {
                                ret.AddRange(GetCopy("8", token[1], "ZERO"));
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
