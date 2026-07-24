

#include <windows.h>


void init_loader() { HMODULE hModule = LoadLibraryA("seb_x64_"); return; }

BOOL WINAPI DllMain(
    HINSTANCE hinstDLL,
    DWORD reason,
    LPVOID reserved
)
{
    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            DisableThreadLibraryCalls(hinstDLL);
            break;
    }

    return TRUE;
}
