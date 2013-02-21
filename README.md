Structura
=========

A 64 bit cpu architecture

Machine code
============

JUMP [UInt64|Befehlswort - 0] [UInt64|Sprungbedingung - 0/NONE 1/ZERO 2/POS 3/NEG 4/OVF] [UInt64|Sprungadressierung - 0/ABS 1/REL] [UInt64|Adresse oder Wert]
ADD [UInt64|Befehlswort - 1] [UInt64|Modus - 0/RAR 1/RAV] [Int64|Register] [Int64|Register oder Wert]

COPY 
  [UInt64|Befehlswort - 2] 
  [UInt64|Modus - 0/NVPAA 1/FPVIS 2/RTM 3/MTR 4/MTM] 
    NVPAA - NO VALUE IS INTERPRET AS ADRESS
    FVIAS - FIRST VALUE IS PARAMETER
  [UInt64|Register oder Speicheradresse] 
  [UInt64|Register oder Speicheradresse]

Assembler code
==============