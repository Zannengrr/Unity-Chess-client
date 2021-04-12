MultiplayerChessNodeJS Unity

Made in Unity version 2020.1.14f1

To start the build there is a Build folder with the last built client app.
To start the app in Unity editor use the SampleScene as the scene.
Everything that is needed to run the client is in that one scene.
The scene consists of Networking Gameobject, Dispatcher and ParentCanvas (parent canvas has all other noteworthy gameobjects and scripts)

---Networking folder---
Has all the data classes that are used across the netowrk.
Network.cs (Networking object)
Network script handles the connection as well as translates incoming messages and their event names into methods that are paired in a NetworkMessageHandles dictonary.

---MainThreadDispatcher folder---
Dispatcher object (Dispatcher)
Since all the messages we receive are not in the main thread, Unity cannot run Monobehaviour actions. To fix that script Dispatcher is used to run any action in the main thread;

---MesagingSystem folder---
MessagingSystem.cs
For easier communication with some objects a MessagingSystem is used. Where Objects subscribe and unsubscribe to messages and then on Dispatching those messages all of the listeners perform their own actions.

---Components folder---
Has different components that are used in the scene. Each folder has its own scripts and prefabs that are used in the component.

---ScriptableObjects folder---
ChessPiecesSo is created here and filled with premade chess game objects.

GameManager.cs
Starts the game, receives move actions from server, handles move actions from server, handles replay, game end,
holds some information messages during play and has references on the ChessBoard and ChessPieceManager, as well as holds the userinfo, turn value and gameId;

Imported assets
Chess pieces png's
Rotary Heart SerializableDiconaryLite (used to show dictonary values in unity)