x86_64-w64-mingw32-gcc \
    -O2 \
    -shared \
    -o ./build/seb_x64.dll \
    ./src/seb_x64.c \
    ./src/seb_x64.def \
    -loleaut32