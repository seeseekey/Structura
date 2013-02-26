using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structura.Assembler.Jump
{
	public enum AdressInterpretation
	{
		AdressNotContainsTargetAdressAsValue=0,
		AdressContainsTargetAdressAsValue=1,
		RegisterNotContainsTargetAdressAsValue=2,
		RegisterContainsTargetAdressAsValue=3
	}
}