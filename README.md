Structura
=========

Structura is a 64 bit cpu architecture.

Machine code
============

JUMP (Width: 40 Byte)
  [Int64|Instruction word - 0]
  [Int64|Adress interpretation - 0/ANCTAAV 1/ACTAAV 2/RNCTAAV 3/RCTAAV]
      ANCTAAV - Adress not contains target adress as value
      ACTAAV - Adress contains target adress as value
      RNCTAAV - Register not contains target adress as value
      RCTAAV - Register contains target adress as value
  [Int64|Jump condition - 0/NONE 1/ZERO 2/POS 3/NEG 4/OVF]
  [Int64|Jump addressing - 0/ABS 1/REL] 
  [Int64|Adress or value]

ADD (Width: 32 Byte)
  [Int64|Instruction word - 1]
  [Int64|Mode - 0/RAR 1/RANR 2/RAV] 
      RAR - Register and register
      RANR - Register and negative register
      RAV - Register and value
  [Int64|Register] 
  [Int64|Register or value]

COPY (Width: 40 Byte)
  [Int64|Instruction word - 2] 
  [Int64|Mode - 0/NACTAAV 1/FACTAAV 2/SACTAAV 3/BACTAAV]
    NACTAAV - No adress contains target adress as value
    FACTAAV - First adress contains target adress as value
    SACTAAV - Second adress contains target adress as value
    BACTAAV - Both adress contains target adress as value
  [Int64|Amount of copied data in bytes]
  [Int64|Register or memory adress] 
  [Int64|Register, memory adress or ZERO]

Assembler description
=====================

ABS [Register]
  * Type: complex command
  * Effect: set the value in register to his absolute
  * Flags: none
  * Register used: Z
  * Example: ABS A;
  
ADD [Register] [Register or value]
  - Type: basic command
  - Effect: Adds a value to the register on the left
  - Flags: none
  - Register used: Z
  - Example: ABS A;

CLR [Register or ALL]
  # Type: complex command
  # Effect: Set specified register to zero
  # Flags: none
  # Register used: none
  # Example: CLR A;

COPY [Int64|Menge an kopierenden Daten in Byte] [Register or memory adress] [Register, memory adress or ZERO]
  > Type: basic command
  > Effect: Copy data from register to memory, and reverse, in any combination
  > Flags: none
  > Register used: none
  > Example: COPY 8 A 2100;

DIV [Register] [Register or value]
  > Type: complex command
  > Effect: Divide the first value through the second value
  > Flags: Zero (if division by 0)
  > Register used: W, X, Y, Z
  > Example: DIV A 3;

LOAD [Register] [Memory adress] 
  > Type: basic command
  > Effect: Load 8 bytes from a memory adress to the specified register
  > Flags: none
  > Register used: none
  > Example: LOAD 1234 A;

DEC [Register]
  > Type: basic command
  > Effect: Decrement the value in the specified register
  > Flags: none
  > Register used: none
  > Example: DEC A;

INC [Register]
  > Type: basic command
  > Effect: Increment the value in the specified register
  > Flags: none
  > Register used: none
  > Example: INC A;

JUMP [Jump condition|NONE|ZERO|POS|NEG|OVL] [Jump adressing|ABS|REL] [Adress or value]
  > Type: basic command
  > Effect: Jump to specified adress under the specified condition
  > Flags: none
  > Register used: none
  > Example: JUMP POS REL -100;
  
MOD [Register] [Register or value]
  > Type: complex command
  > Effect: Write the remainder the of the division in the first value
  > Flags: Zero (if division by 0)
  > Register used: W, X, Y, Z
  > Example: MOD A 3;

MUL [Register] [Register or value]
  > Type: complex command
  > Effect: Multiply the first value through the second value
  > Flags: none
  > Register used: W, X, Y, Z
  > Example: MUL A 3;

NEG [Register]
  > Type: complex command
  > Effect: Negate the value in the specified register
  > Flags: none
  > Register used: Z
  > Example: NEG A 3;

NOOP
  > Type: basic command
  > Effect: Increase the IC by 8 and needs one CPU cycle
  > Flags: none
  > Register used: none
  > Example: NOOP;

SHIFTL [Register] [Register or value]
  > Type: complex command
  > Effect: Left shift the value in the specified register by the second value
  > Flags: none
  > Register used: U, V, W, X, Y, Z
  > Example: SHIFTL A 3;

SHIFTR [Register] [Register or value]
  > Type: complex command
  > Effect: Right shift the value in the specified register by the second value
  > Flags: none
  > Register used: U, V, W, X, Y, Z
  > Example: SHIFTR A 3;

WRITE [Memory address] [Register]
  > Type: basic command
  > Effect: Write 8 bytes from a register to the specified memory adress
  > Flags: none
  > Register used: none
  > Example: WRITE A 1234;