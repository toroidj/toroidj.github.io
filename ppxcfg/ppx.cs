// PPx のコマンドライン実行・展開を使用する為のクラス

#define x64 // 32bit版にしたいときはコメントアウト

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace PPx
{
	public class api
	{
		private static class NativeMethods
		{
			#if x64
				[DllImport("PPLIB64W.DLL", CharSet = CharSet.Unicode)]
				public static extern uint PP_ExtractMacro(IntPtr hWnd, IntPtr info, IntPtr pos, [In] string param, [Out] System.Text.StringBuilder result, int flags);
			#else
				[DllImport("PPLIB32W.DLL", CharSet = CharSet.Unicode)]
				public static extern uint PP_ExtractMacro(IntPtr hWnd, IntPtr info, IntPtr pos, [In] string param, [Out] System.Text.StringBuilder result, int flags);
			#endif
		}
		// PPx のコマンドラインを実行。戻り値は 0 が成功、0 以外は ERRORCODE
		public uint Execute(string param)
		{
			return NativeMethods.PP_ExtractMacro(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, param, null, 0);
		}

		// PPx のコマンドライン展開。戻り値は展開した文字列
		public string Extract(string param)
		{
			System.Text.StringBuilder buf = new System.Text.StringBuilder(1024);

			NativeMethods.PP_ExtractMacro(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, param, buf, 0);
			return buf.ToString();
		}

	}
}

// 使用例
namespace Main
{
	class Program
	{
		static void Main(string[] args)
		{
			PPx.api ppx = new PPx.api();

			ppx.Execute("%KC\"down down down\""); // アクティブ PPc のキー操作
			Console.Write( ppx.Extract("%E") ); // 一行編集の入力結果を返す
		}
	}
}
