# PPx のコマンドライン実行・展開を使用する為のクラス

import ctypes

class PPx:
	dll = 0

	def __init__(self):
		from ctypes.wintypes import HWND

		if self.dll == 0:
			try:
				self.dll = ctypes.WinDLL("./PPLIB64W.DLL")
			except OSError:
				try:
					self.dll = ctypes.WinDLL("./PPLIB32W.DLL")
				except OSError:
					raise Exception('PPx: PPLIB32W.DLL / PPLIB64W.DLL not find.')
			self.dll.PP_ExtractMacro.restype = ctypes.c_uint
			self.dll.PP_ExtractMacro.argtypes = [HWND, ctypes.c_void_p, ctypes.c_void_p, ctypes.c_wchar_p, ctypes.c_void_p, ctypes.c_uint32]

	# PPx のコマンドラインを実行。戻り値は 0 が成功、0 以外は ERRORCODE
	def execute(self, params):
		return self.dll.PP_ExtractMacro(0, 0, 0, params, None, 0)

	# PPx のコマンドライン展開。戻り値は展開した文字列
	def extract(self, params):
		resultstr = ctypes.create_unicode_buffer(1024)
		if self.dll.PP_ExtractMacro(0, 0, 0, params, resultstr, 0) == 0:
			return resultstr.value
		return ''

# 使用例
ppx = PPx()

ppx.execute("%\"PPx execute test\" %: %I\"%E\" ")
print( ppx.extract("%\"PPx extract test\" %E") )

