# English Draughts
 
C# implementation of [English Draughts](https://en.wikipedia.org/wiki/English_draughts) for playing against AI.    

<img src="https://i.imgur.com/gRCk0km.png">
 
### Player
 
- Chooses side (Black/White) before starting the game.
- Configures AI turn time limit during their turn.
- May use unlimited depth Undo/Redo functionality.
 
### AI 
 
- Computes its turn parallelly.    
- Uses all available time for choosing the best turn.
 
### Project structure
 
- Core - Game models and controllers.  
- Robot - An AI implementation.
- NUnitTests - Tests using NUnit framework.                             
- WPF - GUI.               
 
### Based on
 
.Net Core 3.1, Windows Presentation Foundation (WPF).
