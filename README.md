# Away Farming, K?
A simple stand-alone WPF project that allows afk "Use Item" action in Minecraft 1.13

This project was the result of a channel discussion on LogicalGeekBoy's discord server, regarding the breakage of most AFK farms that utilize "tricking" the Minecraft Client into thinking the "Use Item" button is being held, in 1.13. The core bits of it were ripped from a testing project I wrote and have been provided as a simple interface. A cross-platform version of this may be in development, as time allows.

Written fast and ugly, but functional.

## Caveats
This program can only function if the following conditions are met:

* This program assumes that "Pause on Lost Focus" is disabled in your minecraft client. If it isn't, this can be toggled by pressing ```F3+P``` in your minecraft client window, while loaded in a world. In multiplayer worlds, this should make little difference as the pause menu does not in fact pause the game. In single player worlds, this will make your game experience more inline with that of a multiplayer world, where games are not paused on lost focus _(redundant statement is redundant)_.  

* This program assumes that you are running the minecraft client with the same system priviledges it's running on; if you run your minecraft client as administrator, this program must be run as administrator as well.  

* Finally, if the minecraft client regains focus while "Use Item" is toggled by this program, the minecraft client appears to check the __actual__ mouse button state to see if it is still down. This will stop the program's "Use Item" event that was sent, and therefore must be toggled off and back on again while the minecraft client is out of focus.  

## Mechanics
This program uses underlying operating system api calls to send event messages to windows. When toggling the "Use Item" state to on, the right mouse button down event is sent once. When toggling the "Use Item" state to off, the right mouse button up event is sent once.

That's it; that's the secret sauce.

## Download
I firmly recommend downloading the source and compiling Away Farming, K yourself. However, if you do not have VS Community (or do not want to learn how to deal with compiling source yourself), you can download the first compiled version from [dropbox](https://www.dropbox.com/s/5a5pacypkbeeari/AwayFarmingK_Executable.zip?dl=0).
