# Makefile for compiling and running the chat room server and client

# Compiler options
CSC = csc
SERVER_SRC = server.cs
CLIENT_SRC = user.cs
SERVER_EXE = server.exe
CLIENT_EXE = user.exe

# Compile server
server:
	$(CSC) $(SERVER_SRC) -out:$(SERVER_EXE)

# Compile client
client:
	$(CSC) $(CLIENT_SRC) -out:$(CLIENT_EXE)

# Clean build files
clean:
	rm -f $(SERVER_EXE) $(CLIENT_EXE)