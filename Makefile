all: managed-exe native-so

managed-exe:
	$(MAKE) -C managed
	cp ./managed/bin/Debug/OpenVg.exe OpenVg.exe 

native-so:
	$(MAKE) -C native
	cp ./native/pic.so pic.so

