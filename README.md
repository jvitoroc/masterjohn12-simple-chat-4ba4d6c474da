
# simple-chat
Chat de texto e voz.

### Obs
Desenvolvi esse projeto para colocar em prática meu conhecimento de [network programming](https://en.wikipedia.org/wiki/Computer_network_programming) e aprender além da teoria. Com ele, entrei em contato mais ainda com o eccossistema .NET.

 - Usei TPL (biblioteca de tarefas paralelas) para deixar ambas aplicações, servidor e cliente, multithreaded.
 - Utilizei a library [NAudio](https://github.com/naudio/NAudio) para adicionar a funcionalidade de comunicação com voz.
 
 #### Alguns outros remarks:
	
 - Criei meu próprio protocolo em cima do TCP - que lembra muito o HTTP - para a comunicação entre o cliente e servidor para a troca de mensagens (comandos, mensagens de texto, etc.).
 - Já que o UDP é um protocolo connectionless, desenvolvi uma library que cria uma camada em cima do protocolo que o torna connection-oriented, assim facilitando a implementação da comunicação via aúdio.
 - Aprendi mais sobre controle de concorrência enquanto aplicava o uso de tasks.
 - **Pretendo adicionar mais funcionalidades e refatorações no código!**
