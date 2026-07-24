#include <windows.h>

BOOL __cdecl IsRemoteSession(void)
{
    return FALSE;
}

BOOL __cdecl IsVirtualMachine(BSTR* manufacturer, int* probability)
{
    if (manufacturer != NULL)
        *manufacturer = NULL;

    if (probability != NULL)
        *probability = 0;

    return FALSE;
}

BOOL WINAPI DllMain(
    HINSTANCE instance,
    DWORD reason,
    LPVOID reserved
)
{
    (void)instance;
    (void)reserved;

    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            DisableThreadLibraryCalls(instance);
            break;

        case DLL_PROCESS_DETACH:
            break;
    }

    return TRUE;
}