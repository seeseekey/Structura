﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Structura.Assembler.Add;

namespace Structura.Hardware
{
    /// <summary>
    /// CPU Structura
    /// </summary>
    public class Structura
    {
        //Register A - Z
        public Int64 A { get; private set; }
        public Int64 B { get; private set; }
        public Int64 C { get; private set; }
        public Int64 D { get; private set; }
        public Int64 E { get; private set; }
        public Int64 F { get; private set; }
        public Int64 G { get; private set; }
        public Int64 H { get; private set; }
        public Int64 I { get; private set; }
        public Int64 J { get; private set; }
        public Int64 K { get; private set; }
        public Int64 L { get; private set; }
        public Int64 M { get; private set; }
        public Int64 N { get; private set; }
        public Int64 O { get; private set; }
        public Int64 P { get; private set; }
        public Int64 Q { get; private set; }
        public Int64 R { get; private set; }
        public Int64 S { get; private set; }
        public Int64 T { get; private set; }
        public Int64 U { get; private set; }
        public Int64 V { get; private set; }
        public Int64 W { get; private set; }
        public Int64 X { get; private set; }
        public Int64 Y { get; private set; }
        public Int64 Z { get; private set; }

        //Spezialregister
        public Int64 IC { get; private set; } //Instruction counter

        //Flags
        public bool Zero { get; private set; }
        public bool Positive { get; private set; }
        public bool Negative { get; private set; }
        public bool Overflow { get; private set; }

        //Memory
        Memory Memory;

        //Konstruktor
        public Structura(Memory memory)
        {
            Memory=memory;
        }

        //Methoden
        public Int64[] Execute()
        {
            List<Int64> instructionLog=new List<Int64>();

            //... IC -> Speicher -> ausführen
            Int64 instructionWord=GetNextInstructionWord(instructionLog);
            ExecuteMachineCode(instructionWord, instructionLog);

            return instructionLog.ToArray();
        }

        Int64 GetNextInstructionWord(List<Int64> instructionLog)
        {
            byte[] bytes=Memory.GetData((int)IC, 8);
            Int64 ret=BitConverter.ToInt64(bytes, 0);
            IC+=8;
            instructionLog.Add(ret);
            return ret;
        }

        Int64 GetRegisterValue(Int64 number)
        {
            switch(number)
            {
                case -1:
                    {
                        return A;
                    }
                case -2:
                    {
                        return B;
                    }
                case -3:
                    {
                        return C;
                    }
                case -4:
                    {
                        return D;
                    }
                case -5:
                    {
                        return E;
                    }
                case -6:
                    {
                        return F;
                    }
                case -7:
                    {
                        return G;
                    }
                case -8:
                    {
                        return H;
                    }
                case -9:
                    {
                        return I;
                    }
                case -10:
                    {
                        return J;
                    }
                case -11:
                    {
                        return K;
                    }
                case -12:
                    {
                        return L;
                    }
                case -13:
                    {
                        return M;
                    }
                case -14:
                    {
                        return N;
                    }
                case -15:
                    {
                        return O;
                    }
                case -16:
                    {
                        return P;
                    }
                case -17:
                    {
                        return Q;
                    }
                case -18:
                    {
                        return R;
                    }
                case -19:
                    {
                        return S;
                    }
                case -20:
                    {
                        return T;
                    }
                case -21:
                    {
                        return U;
                    }
                case -22:
                    {
                        return V;
                    }
                case -23:
                    {
                        return W;
                    }
                case -24:
                    {
                        return X;
                    }
                case -25:
                    {
                        return Y;
                    }
                case -26:
                    {
                        return Z;
                    }
                case -27:
                    {
                        return 0; //ZERO ROM
                    }
                default:
                    {
                        throw new Exception("Unknown register number");
                    }
            }
        }

        void SetRegisterValue(Int64 number, Int64 value)
        {
            switch(number)
            {
                case -1:
                    {
                        A=value;
                        break;
                    }
                case -2:
                    {
                        B=value;
                        break;
                    }
                case -3:
                    {
                        C=value;
                        break;
                    }
                case -4:
                    {
                        D=value;
                        break;
                    }
                case -5:
                    {
                        E=value;
                        break;
                    }
                case -6:
                    {
                        F=value;
                        break;
                    }
                case -7:
                    {
                        G=value;
                        break;
                    }
                case -8:
                    {
                        H=value;
                        break;
                    }
                case -9:
                    {
                        I=value;
                        break;
                    }
                case -10:
                    {
                        J=value;
                        break;
                    }
                case -11:
                    {
                        K=value;
                        break;
                    }
                case -12:
                    {
                        L=value;
                        break;
                    }
                case -13:
                    {
                        M=value;
                        break;
                    }
                case -14:
                    {
                        N=value;
                        break;
                    }
                case -15:
                    {
                        O=value;
                        break;
                    }
                case -16:
                    {
                        P=value;
                        break;
                    }
                case -17:
                    {
                        Q=value;
                        break;
                    }
                case -18:
                    {
                        R=value;
                        break;
                    }
                case -19:
                    {
                        S=value;
                        break;
                    }
                case -20:
                    {
                        T=value;
                        break;
                    }
                case -21:
                    {
                        U=value;
                        break;
                    }
                case -22:
                    {
                        V=value;
                        break;
                    }
                case -23:
                    {
                        W=value;
                        break;
                    }
                case -24:
                    {
                        X=value;
                        break;
                    }
                case -25:
                    {
                        Y=value;
                        break;
                    }
                case -26:
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

        void ExecuteMachineCode(Int64 instructionWord, List<Int64> instructionLog)
        {
            switch(instructionWord)
            {
                case 0: //JUMP
                    {
                        Int64 adressMode=GetNextInstructionWord(instructionLog);
                        Int64 jumpCondition=GetNextInstructionWord(instructionLog);
                        Int64 jumpMode=GetNextInstructionWord(instructionLog);

                        //Calc target
                        Int64 target=GetNextInstructionWord(instructionLog);
                        if(adressMode==1) //Adress contains target adress as value
                        {
                            target=GetRegisterValue(target);
                        }

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
                                    if(Zero)
                                        jumpConditionEntered=true;
                                    break;
                                }
                            case 2: //POS
                                {
                                    if(Positive)
                                        jumpConditionEntered=true;
                                    break;
                                }
                            case 3: //NEG
                                {
                                    if(Negative)
                                        jumpConditionEntered=true;
                                    break;
                                }
                            case 4: //Overflow
                                {
                                    if(Overflow)
                                        jumpConditionEntered=true;
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

                        AddMode addMode=(AddMode)(int)GetNextInstructionWord(instructionLog);
                        Int64 registerA=GetNextInstructionWord(instructionLog);
                        Int64 val=0;

                        if(addMode==AddMode.RegisterAndRegister||addMode==AddMode.RegisterAndNegativeRegister) //RAR oder RANR;
                        {
                            Int64 registerBValue=GetRegisterValue(GetNextInstructionWord(instructionLog));
                            if(addMode==AddMode.RegisterAndNegativeRegister)
                                registerBValue=(-registerBValue);

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
                            Int64 value=GetNextInstructionWord(instructionLog);

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
                        Int64 copyMode=GetNextInstructionWord(instructionLog);
                        Int64 count=GetNextInstructionWord(instructionLog);
                        Int64 targetAdress=GetNextInstructionWord(instructionLog);
                        Int64 sourceAdress=GetNextInstructionWord(instructionLog);

                        byte[] sourceValue;//=new byte[count];

						if(sourceAdress<0) //First value is register
                        {
                            if(copyMode==1||copyMode==3)
                            {
								Int64 registerValue=GetRegisterValue(sourceAdress);
                                sourceValue=Memory.GetData(registerValue, count);
                            }
                            else
                            {
								sourceValue=BitConverter.GetBytes(GetRegisterValue(sourceAdress));
                            }
                        }
                        else
                        {
							sourceValue=Memory.GetData(sourceAdress, count);
                        }

						if(targetAdress<0) //Second value is register
                        {
                            Int64 targetRegister;

                            if(copyMode==2||copyMode==3)
                            {
								targetRegister=GetRegisterValue(targetAdress);
                                Memory.WriteData(targetRegister, sourceValue);
                            }
                            else
                            {
								targetRegister=targetAdress;
                                SetRegisterValue(targetRegister, BitConverter.ToInt64(sourceValue, 0));
                            }
                        }
                        else
                        {
                            Int64 targetMemoryAdress=GetNextInstructionWord(instructionLog);
                            Memory.WriteData(targetMemoryAdress, sourceValue);
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