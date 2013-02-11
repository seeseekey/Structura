﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Structura
{
    /// <summary>
    /// CPU Structura
    /// </summary>
    public class Structura
    {
        //Register A - Z
        public UInt64 A { get; private set; }
        public UInt64 B { get; private set; }
        public UInt64 C { get; private set; }
        public UInt64 D { get; private set; }
        public UInt64 E { get; private set; }
        public UInt64 F { get; private set; }
        public UInt64 G { get; private set; }
        public UInt64 H { get; private set; }
        public UInt64 I { get; private set; }
        public UInt64 J { get; private set; }
        public UInt64 K { get; private set; }
        public UInt64 L { get; private set; }
        public UInt64 M { get; private set; }
        public UInt64 N { get; private set; }
        public UInt64 O { get; private set; }
        public UInt64 P { get; private set; }
        public UInt64 Q { get; private set; }
        public UInt64 R { get; private set; }
        public UInt64 S { get; private set; }
        public UInt64 T { get; private set; }
        public UInt64 U { get; private set; }
        public UInt64 V { get; private set; }
        public UInt64 W { get; private set; }
        public UInt64 X { get; private set; }
        public UInt64 Y { get; private set; }
        public UInt64 Z { get; private set; }

        //Spezialregister
        public UInt64 IC { get; private set; } //Instruction counter

        //Flags
        public bool Zero { get; private set; }
        public bool Positive { get; private set; }
        public bool Negative { get; private set; }
        public bool Overflow { get; private set; }

        //Memory
		Memory Memory;
		Graphic Graphic;

        //Konstruktor
        public Structura(Memory memory, Graphic graphic)
        {
            Memory=memory;
			Graphic=graphic;
        }

        //Methoden
        public void Execute()
        {
            //... IC -> Speicher -> ausführen
            UInt64 instructionWord=GetNextInstructionWord();
            ExecuteMachineCode(instructionWord);
        }

        UInt64 GetNextInstructionWord()
        {
            UInt64 ret=BitConverter.ToUInt64(Memory.Data, (int)IC);
            IC+=8;
            return ret;
        }

        UInt64 GetRegisterValue(UInt64 number)
        {
            switch(number)
            {
                case 0:
                    {
                        return A;
                    }
                case 1:
                    {
                        return B;
                    }
                case 2:
                    {
                        return C;
                    }
                case 3:
                    {
                        return D;
                    }
                case 4:
                    {
                        return E;
                    }
                case 5:
                    {
                        return F;
                    }
                case 6:
                    {
                        return G;
                    }
                case 7:
                    {
                        return H;
                    }
                case 8:
                    {
                        return I;
                    }
                case 9:
                    {
                        return J;
                    }
                case 10:
                    {
                        return K;
                    }
                case 11:
                    {
                        return L;
                    }
                case 12:
                    {
                        return M;
                    }
                case 13:
                    {
                        return N;
                    }
                case 14:
                    {
                        return O;
                    }
                case 15:
                    {
                        return P;
                    }
                case 16:
                    {
                        return Q;
                    }
                case 17:
                    {
                        return R;
                    }
                case 18:
                    {
                        return S;
                    }
                case 19:
                    {
                        return T;
                    }
                case 20:
                    {
                        return U;
                    }
                case 21:
                    {
                        return V;
                    }
                case 22:
                    {
                        return W;
                    }
                case 23:
                    {
                        return X;
                    }
                case 24:
                    {
                        return Y;
                    }
                case 25:
                    {
                        return Z;
                    }
                default:
                    {
                        throw new Exception("Unknown register number");
                    }
            }
        }

        void SetRegisterValue(UInt64 number, UInt64 value)
        {
            switch(number)
            {
                case 0:
                    {
                        A=value;
                        break;
                    }
                case 1:
                    {
                        B=value;
                        break;
                    }
                case 2:
                    {
                        C=value;
                        break;
                    }
                case 3:
                    {
                        D=value;
                        break;
                    }
                case 4:
                    {
                        E=value;
                        break;
                    }
                case 5:
                    {
                        F=value;
                        break;
                    }
                case 6:
                    {
                        G=value;
                        break;
                    }
                case 7:
                    {
                        H=value;
                        break;
                    }
                case 8:
                    {
                        I=value;
                        break;
                    }
                case 9:
                    {
                        J=value;
                        break;
                    }
                case 10:
                    {
                        K=value;
                        break;
                    }
                case 11:
                    {
                        L=value;
                        break;
                    }
                case 12:
                    {
                        M=value;
                        break;
                    }
                case 13:
                    {
                        N=value;
                        break;
                    }
                case 14:
                    {
                        O=value;
                        break;
                    }
                case 15:
                    {
                        P=value;
                        break;
                    }
                case 16:
                    {
                        Q=value;
                        break;
                    }
                case 17:
                    {
                        R=value;
                        break;
                    }
                case 18:
                    {
                        S=value;
                        break;
                    }
                case 19:
                    {
                        T=value;
                        break;
                    }
                case 20:
                    {
                        U=value;
                        break;
                    }
                case 21:
                    {
                        V=value;
                        break;
                    }
                case 22:
                    {
                        W=value;
                        break;
                    }
                case 23:
                    {
                        X=value;
                        break;
                    }
                case 24:
                    {
                        Y=value;
                        break;
                    }
                case 25:
                    {
                        Z=value;
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown register number");
                    }
            }
        }

        void ExecuteMachineCode(UInt64 instructionWord)
        {
            switch(instructionWord)
            {
                case 0: //JUMP
                    {
                        UInt64 jumpCondition=GetNextInstructionWord();
                        UInt64 jumpMode=GetNextInstructionWord();
                        UInt64 target=GetNextInstructionWord();
                        bool jumpConditionEntered=false;

						switch(jumpCondition)
						{
							case 0: //NONE
								{
									jumpConditionEntered=true;
									break;
								}
							case 1: //ZERO
								{
									if(Zero)jumpConditionEntered=true;
									break;
								}
							case 2: //POS
								{
									if(Positive) jumpConditionEntered=true;
									break;
								}
							case 3: //NEG
								{
									if(Negative) jumpConditionEntered=true;
									break;
								}
							case 4: //Overflow
								{
									if(Overflow) jumpConditionEntered=true;
									break;
								}
						}

                        if(jumpConditionEntered)
                        {

                            switch(jumpMode)
                            {
                                case 0: //absolute
                                    {
                                        IC=target;
                                        break;
                                    }
                                case 1: //relativ
                                    {
                                        IC+=target;
                                        break;
                                    }
                            }
                        }
                        break;
                    }
				case 1: //ADD
					{
						Overflow=false;

						UInt64 addMode=GetNextInstructionWord();
						UInt64 registerA=GetNextInstructionWord();
						UInt64 val=0;

						if(addMode==0) //RAR
						{
							UInt64 registerBValue=GetRegisterValue(GetNextInstructionWord());

							checked
							{
								try
								{
									val=GetRegisterValue(registerA)+registerBValue;
								}
								catch(OverflowException)
								{
									Overflow=true;
								}
							}

							val=GetRegisterValue(registerA)+registerBValue;
							SetRegisterValue(registerA, val);
						}
						else //RAV
						{
							UInt64 value=GetNextInstructionWord();

							checked
							{
								try
								{
									val=GetRegisterValue(registerA)+value;
								}
								catch(OverflowException)
								{
									Overflow=true;
								}
							}

							val=GetRegisterValue(registerA)+value;
							SetRegisterValue(registerA, val);
						}

						Zero=val==0;
						Positive=val>0;
						Negative=val<0;

						break;
					}
                case 2: //COPY
                    {
                        UInt64 copyMode=GetNextInstructionWord();

                        UInt64 sourceValue;

                        if(copyMode==0||copyMode==1) //First value is register
                        {
                            sourceValue=GetRegisterValue(GetNextInstructionWord());
                        }
                        else
                        {
                            sourceValue=Memory.Data[GetNextInstructionWord()];
                        }

                        if(copyMode==0||copyMode==2) //Second value is register
                        {
                            UInt64 targetRegister=GetNextInstructionWord();
                            SetRegisterValue(targetRegister, sourceValue);
                        }
                        else
                        {
                            UInt64 targetMemoryAdress=GetNextInstructionWord();
							Memory.WriteIntoMemory(sourceValue, targetMemoryAdress);
                        }

                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown opcode");
                    }
            }
        }
    }
}
