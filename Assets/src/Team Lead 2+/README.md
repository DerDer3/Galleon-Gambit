# MapTransitions prefab

The `MapTransitions` prefab is used for transitioning between the map and other levels.
It is designed to work within a single scene that has root objects that represent different states of the game.
It is a singleton, so there need not be more than one instance instantiated at a time.
`MapTransitions` has the following serialized fields:

* `Menu` - the root object for the main menu
* `Treasure` - the root object for the treasure event
* `Unknown` - the root object for the randomized event
* `Map` - the root object for the map
* `Game` - the root object for the battle scene

Transitions are envoked by calling one of the `TransitionTo` methods.
The following is the list of transition functions:

* `TransitionToMenu` - transitions to the main menu
* `TransitionToMap` - transitions to the map
* `TransitionToUnknown` - transitions to the randomized event
* `TransitionToTreasure` - transitions to the treasure event
* `TransitionLevel` - calls the transition function that corresponds to the passed level

Once a transition method is called, the screen fades to black, switches active root objects, and then the screen fades from black.
This requires that all root objects already be instantiated and ready.
This method of switching produces quicker loading times then switching scenes.

This prefab handles the correct music track and sound effects for switching between levels.
