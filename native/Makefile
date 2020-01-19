LIBFLAGS=-L/opt/vc/lib -lbrcmEGL -lbrcmGLESv2 -lfreetype
INCLUDEFLAGS=-I/opt/vc/include -I/usr/include/freetype2 -I/opt/vc/include/interface/vmcs_host/linux -I/opt/vc/include/interface/vcos/pthreads -fPIC

all: native

native: api.o oglinit.o font.o ovgwrapper.o
	gcc $(LIBFLAGS) -shared -o pic.so oglinit.o font.o ovgwrapper.o api.o

api.o: api.c 
	gcc -O2 -Wall $(INCLUDEFLAGS) -c api.c

oglinit.o:	oglinit.c eglstate.h
	gcc -O2 -Wall $(INCLUDEFLAGS) -c oglinit.c

font.o: font.c font.h structs.h
	gcc -O2 -Wall $(INCLUDEFLAGS) -c font.c

ovgwrapper.o:	ovgwrapper.c ovgwrapper.h structs.h
	gcc -O2 -Wall $(INCLUDEFLAGS) -c ovgwrapper.c

