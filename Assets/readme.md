# Crux

### Wrap Up

Enemy Behaviour
  - ~~Simplify the flight patterns~~
  - ~~Have each ship have an array of valid flight patterns and decide at random~~
  - ~~Slow down the fire rate~~

UI
  - ~~Upon item pickup have a bit of text appear above the player~~
  - Damage indicator?
  - Drop has icon of weapon/item

Progression
  - Decide how to increase difficulty
    - Boost modifiers
    - Increase quantity
    - Increase wave/level/stage duration
    - Add more enemies
  - RNG based drops

Bosses
  - Requires Movement Logic

Core
  - Energy for player ship
  - Setup player ship prefabs

Misc
  - ~~Ship entry and exit~~
  - Ships flash upon hit
  - ~~Accellerate out of stage~~
    - ~~Increase scroll rate of BGManager~~
  - Explosions on enemy deaths
  - Cash drops!? omg get to pick up gold coins with a satisfying souind
  - If the last wave used the same path, choose another
  - ElectroShield gets a sprite that sits on the player, activates animation on fire

Scenes
  - Inter-Stage
    - Weapon swap
    - Skills
  - Hi-Score
    - Persistent data?
  - Game Over

Potential Bugs
  - Stage ends before player spawns
    - There wont be an activeplayership to move

#### InterScene
There are a few ways to accomplish this
  - Fresh scene
    - Encourages music to be swapped out upon new stage
  - Persistent scene
    - Preserves Music
    - Gameplay might feel more fluent
  - Hybrid
    - Upon stage end, show score tally, equips, skills
    - Once finished, fly off and transition into new scene