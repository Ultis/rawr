using System;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Rawr.Forms.Utilities
{
	/// <summary>
	/// Summary description for InvokeHelper.
	/// </summary>
	public class InvokeHelper
	{

		private static ModuleBuilder builder;
		private static AssemblyBuilder myAsmBuilder;
		private static Hashtable typeLookup;

		
		static InvokeHelper()
		{
			AssemblyName name = new AssemblyName();
			name.Name = "InvokeHelper";
			myAsmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
			builder = myAsmBuilder.DefineDynamicModule("InvokeHelperModule");
			typeLookup = new Hashtable();
		}

		private static Delegate LookupDelegate(Control control, string methodName, params object[] parameters)
		{
			string key = control.GetType().Name + "." + methodName;
			Type type = typeLookup[key] as Type;
			if (type == null)
			{
				Type[] paramList = new Type[control.GetType().GetMethod(methodName,BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetParameters().Length];
				int count = 0;
				foreach (ParameterInfo pi in control.GetType().GetMethod(methodName,BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetParameters())
				{
					paramList[count++] = pi.ParameterType;
				}
				TypeBuilder typeBuilder = builder.DefineType("Del_" + control.GetType().Name + "_" + methodName, TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.Public | TypeAttributes.Sealed, typeof (MulticastDelegate));
				ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] {typeof (object), typeof (IntPtr)});
				constructorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime);
				MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, control.GetType().GetMethod(methodName,BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).ReturnType, paramList);
				methodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime);
				type = typeBuilder.CreateType();
				typeLookup.Add(key, type);
			}
			return MulticastDelegate.CreateDelegate(type, control, methodName);;
		}

		/// <summary>
		/// Executes BeginInvoke on the underlying control
		/// </summary>
		/// <param name="control">Control to execute the methond on</param>
		/// <param name="methodName">Method to execute</param>
		/// <param name="parameters">Parameters for Method</param>
		/// <returns></returns>
		public static void BeginInvoke(Control control, string methodName, params object[] parameters)
		{
			Delegate del = LookupDelegate(control,methodName,parameters);
			if (control.Created) control.BeginInvoke(del, parameters);
		}

		/// <summary>
		/// Executes Invoke on the underlying control
		/// </summary>
		/// <param name="control">Control to execute the methond on</param>
		/// <param name="methodName">Method to execute</param>
		/// <param name="parameters">Parameters for Method</param>
		/// <returns></returns>
		public static object Invoke(Control control, string methodName, params object[] parameters)
		{
			Delegate del = LookupDelegate(control,methodName,parameters);
			if (control.Created) return control.Invoke(del, parameters);
			return null;
		}
	}
}
