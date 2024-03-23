// PPx のコマンドライン実行・展開を使用する為の関数
#define UNICODE
#include <windows.h>

typedef DWORD (WINAPI *impPP_ExtractMacro)(HWND hWnd, void *ParentInfo, POINT *pos, const WCHAR *param, WCHAR *extract, int flags);

HMODULE hDLL = NULL;
impPP_ExtractMacro PP_ExtractMacro;

BOOL PPx_InitDLL(void)
{
	if ( hDLL != NULL ) return TRUE;

#ifdef _WIN64
	hDLL = LoadLibrary(L"PPLIB64W.DLL");
#else
	hDLL = LoadLibrary(L"PPLIB32W.DLL");
#endif
	if ( hDLL == NULL ) return FALSE;

	PP_ExtractMacro = (impPP_ExtractMacro)GetProcAddress(hDLL, "PP_ExtractMacro");
#ifndef _WIN64
	if ( PP_ExtractMacro == NULL ){
		PP_ExtractMacro = (impPP_ExtractMacro)GetProcAddress(hDLL, "_PP_ExtractMacro");
	}
#endif
	if ( PP_ExtractMacro != NULL ) return TRUE;
	FreeLibrary(hDLL);
	hDLL = NULL;
	return FALSE;
}

void PPx_FreeDLL(void)
{
	if ( hDLL == NULL ) return;
	FreeLibrary(hDLL);
	hDLL = NULL;
}

DWORD PPx_Execute(const WCHAR *param)
{
	if ( PPx_InitDLL() == FALSE ) return ERROR_FILE_NOT_FOUND;

	return PP_ExtractMacro(GetActiveWindow(), NULL, NULL, param, NULL, 0);
}

// 結果は 1024文字確保する必要がある
DWORD PPx_Extract(const WCHAR *param, WCHAR *result1024)
{
	if ( PPx_InitDLL() == FALSE ) return ERROR_FILE_NOT_FOUND;

	return PP_ExtractMacro(GetActiveWindow(), NULL, NULL, param, result1024, 0);
}

// 使用例
int main(void)
{
	WCHAR result1024[1024];

	if ( PPx_InitDLL() == FALSE ){
		printf("PPlib64W.DLL / PPlib32W.DLL load error.\n");
		return -1;
	}

	PPx_Execute( L"%KC\"down down down\"" ); // アクティブ PPc のキー操作

	PPx_Extract( L"%E", result1024 ); // 一行編集の入力結果を返す
	MessageBoxW(GetActiveWindow(), result1024, NULL, MB_OK);
	return 0;
}
