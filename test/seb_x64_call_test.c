#include <windows.h>
#include <oleauto.h>
#include <stdio.h>

typedef BOOL (__cdecl *IsVirtualMachine_t)(
    BSTR *manufacturer,
    int *probability
);

int main(void)
{
    HMODULE module;
    IsVirtualMachine_t IsVirtualMachine;

    BSTR manufacturer = NULL;
    int probability = 0x12345678;
    BOOL result;

    module = LoadLibraryA("seb_x64.dll");
    if (!module) {
        printf("LoadLibrary failed: %lu\n", GetLastError());
        return 1;
    }

    IsVirtualMachine = (IsVirtualMachine_t)
        GetProcAddress(module, "IsVirtualMachine");

    if (!IsVirtualMachine) {
        printf("GetProcAddress failed: %lu\n", GetLastError());
        FreeLibrary(module);
        return 1;
    }

    printf("Function: %p\n", (void *)IsVirtualMachine);

    result = IsVirtualMachine(
        &manufacturer,
        &probability
    );

    printf("Result raw  : 0x%08lX\n",
           (unsigned long)result);

    printf("Result      : %s\n",
           result ? "true" : "false");

    printf("Probability : %d (0x%08X)\n",
           probability,
           (unsigned int)probability);

    printf("BSTR ptr    : %p\n",
           (void *)manufacturer);

    if (manufacturer) {
        printf("BSTR length : %u\n",
               (unsigned int)SysStringLen(manufacturer));

        printf("Manufacturer: %ls\n",
               manufacturer);

        SysFreeString(manufacturer);
    } else {
        printf("Manufacturer: (null)\n");
    }

    FreeLibrary(module);
    return 0;
}