typedef struct MsvcString {
    union {
        char inlineBuffer[16];
        char *heapBuffer;
    } storage;

    unsigned long long size;
    unsigned long long capacity;
} MsvcString;

typedef struct WString32 {
    union {
        wchar_t inlineBuffer[8];
        wchar_t *heapBuffer;
    } storage;

    unsigned long long size;
    unsigned long long capacity;
} WString32;

typedef struct VendorList {
    unsigned long long count;
    MsvcString vendors[5];
} VendorList;

typedef struct VmContext {
    uint64_t bits[2];
} VmContext;