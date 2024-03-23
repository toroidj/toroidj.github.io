// PPx �̃R�}���h���C�����s�E�W�J���g�p����ׂ̃N���X

#define x64 // 32bit�łɂ������Ƃ��̓R�����g�A�E�g

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
		// PPx �̃R�}���h���C�������s�B�߂�l�� 0 �������A0 �ȊO�� ERRORCODE
		public uint Execute(string param)
		{
			return NativeMethods.PP_ExtractMacro(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, param, null, 0);
		}

		// PPx �̃R�}���h���C���W�J�B�߂�l�͓W�J����������
		public string Extract(string param)
		{
			System.Text.StringBuilder buf = new System.Text.StringBuilder(1024);

			NativeMethods.PP_ExtractMacro(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, param, buf, 0);
			return buf.ToString();
		}

	}
}

// �g�p��
namespace Main
{
	class Program
	{
		static void Main(string[] args)
		{
			PPx.api ppx = new PPx.api();

			ppx.Execute("%KC\"down down down\""); // �A�N�e�B�u PPc �̃L�[����
			Console.Write( ppx.Extract("%E") ); // ��s�ҏW�̓��͌��ʂ�Ԃ�
		}
	}
}
