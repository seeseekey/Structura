using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura.Assembler.Copy
{
	public enum CopyMode
	{
		NoAdressContainsTargetAdressAsValue=0,
		FirstAdressContainsTargetAdressAsValue=1,
		SecondAdressContainsTargetAdressAsValue=2,
		BothAdressContainsTargetAdressAsValue=3
	}
}
