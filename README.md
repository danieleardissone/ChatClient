# Chat Client
A simple .NET chat client that connects to a server on TCP port 10000.

## Project organization
The project is organized into different forders:
* ```Consts```: contains classes of constants
* ```Enums```: contains classes of enumerations
* ```Exceptions```: constains custom exceptions
* ```Models```: contains model classes, such as ChatMessage or User
* ```Services```: contains service classes injected as dependencies by LightInject library
* ```ChatForm.cs```: main chat UI form
* ```Program.cs```: app launcher, contains the injection of services

## How it works
The client allows user to choose a chat name and connect to the server using that name.
After connection, the user can send simple text messages and the server broadcast them to all connected users.
When user wants to leave the chat, he can disconnect or simply close che UI form.

To connect to the server, the client opens a TCP connection on port 10000.
If the connection opens, it creates a thread timer that runs a method every 500 milliseconds to check if there are new messages to receive.
Network connection and interactions with server is managed by the ```TcpNetworkService```, that is injected when the client starts.

![image](https://user-images.githubusercontent.com/20296719/215570663-dc8b845c-90c9-4a97-962c-2e32141810fc.png)

![PagoPaChatClient](https://user-images.githubusercontent.com/20296719/215570343-41e40add-b8e0-4569-b3d4-27aa49fd20d9.PNG)

# Chat Server
A simple Node.js server that listen on TCP port 10000 using net module.
It's available at https://gist.github.com/danieleardissone/02e4a5481c638c29c818ae9137317134
