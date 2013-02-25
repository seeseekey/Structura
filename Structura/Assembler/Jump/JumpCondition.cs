using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura.Assembler.Jump
{
	public enum JumpCondition
	{
		None=0,
		Zero=1,
		Positive=2,
		Negative=3,
		Overflow=4
	}
}
