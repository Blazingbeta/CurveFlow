using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MyDLL
{
    public class Class1
    {
		[DllExport("GetDomain", CallingConventions = CallingConvention.StdCall)]
		static public string GetDomainName(bool result)
		{
			System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
			return currentUser.Name.Split('\\')[0];
		}
	}
}
