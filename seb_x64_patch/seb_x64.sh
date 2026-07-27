x86_64-w64-mingw32-gcc \
    -O2 \
    -shared \
    -o ./build/seb_x64.dll \
    ./test/poc_exploit_01//seb_x64.c \
    ./test/poc_exploit_01/seb_x64.def \
    -loleaut32