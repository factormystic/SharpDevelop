// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;

namespace Debugger
{
	public class VariableEventArgs: DebuggerEventArgs
	{
		Variable variable;
		
		public Variable Variable {
			get {
				return variable;
			}
		}
		
		public VariableEventArgs(Variable variable): base(variable.Process)
		{
			this.variable = variable;
		}
	}
}
