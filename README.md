# Hotdogs and Magic - User Manual

Please visit our website at https://peter-012.github.io/Hotdogs-Magic-Website/. It includes features like a demo video and a tutorial on how to play. You can find a copy of the website under the folder "Website" within the GitHub repo.

## Setup Guide

### Hardware Requirements
All you need is a VR headset that is linked to a powerful computer and VR controllers to track your hand movements. It is preferable that you have knuckle or wrist straps for your controllers and that you are wearing them since it is quite easy to accidentally throw your controller during play sessions.

### Play Space
This game requires you to move left and right in order to dodge projectiles in the game. Be sure to have ample room to shuffle left and right and some space to extend your arms forward. Also, it is important to tighten the strap of your headset in order to keep it from falling off when playing this game.

### Software Setup
Make sure that you have SteamVR installed to your computer. Under the "Releases" section of GitHub will be a Unity build for the VR game. A copy of the build can be found in the GitHub repo under the folder "Builds". To play the game, just click and run the .exe file (DO NOT click on UnityCrashHander64.exe). It's preferred that you add the game as a non-steam game and make sure to also include the game in your VR Library through the game's properties. That way you can launch the game directly through SteamVR. If you would like to clone the repo and see the code in action within Unity, make sure that you have Unity Editor version 2022.3.17f1 before launching the project and have SteamVR running in the background.

## Description of Implementation

This game was developed using the Unity Editor version 2022.3.17f1

### Special Assets
Other than the projectile (with collsion effects) asset "Fire Ice Projectile - Explosion" which came from the asset store, and the low-poly chair model which came from Sketchfab, all assets have been created by us using Blender and the Unity shader files.

### C# Script Implementation

#### MenuScene/StartMenuScene.cs
This script is attached on the camera rig and is active when the game starts. Since we wanted to avoid attaching components onto the scene manually, we opted to use scripts to initialize and modify object components. 
The code within this script serves as an entry point into initializing other portions of the MenuScene by attaching other scripts and components to objects within the scene.

#### MenuScene/MenuSelection.cs
MenuSelection serves as a event listener and an initializer for the controller objects; It has update and trigger event functions which listen for specific events to occur. 
When the do, the script checks to see whether the object is interactable, and if it is, then it invokes the Select() function from that interactable object, which handles it accordingly.

#### MenuScene/StartDuel.cs
This script listens for an interaction event with the wand in the game, and retrieves data for the player's dominant hand. The script also starts the process of transitioning from the MenuScene to the DuelingScene.

#### MenuScene/BookinteractionHandler.cs
This class handles the interaction logic for the book. It receives a signal from the controllers and uses trigonometry and vector mathematics to compute the angle in which the book should swing.
In the case that the book is opened by the player, a tutorial video is played. If it is closed, the tutorial video stops.

#### MenuScene/Video.cs
This script is attached onto a gameobject and upon doing so, loads the tutorial video from the resources folder and prepares the game object for playing the video. The trigger for starting the video is located in 
the BookInteractionHandler.

#### DuelingScene/StartDuelScene.cs
This script is similar to the StartMenuScene script, but it instead prepares the DuelingScene using precomputed information retrieved from the menuScene. 
The script attaches listeners to the controllers, spawns crates, and initializes the enemy and player.

#### DuelingScene/Wand/SpawnWand.cs
This script instantiates a wand object and prepares the logic for it's interactions by attaching a script to the wand object.
The wand is then translated and rotated such that it fits the controller's position/rotation.

#### DuelingScene/Wand/WandLogic.cs
This script controls the logic for the wand, allowing players to shoot projectiles and adding delay between their shots. The script uses a Coroutine and instantiates projectiles on the tip of the wand before
releasing them in the direction that the wand is aiming towards.

#### DuelingScene/AILogic/EnemyStateManager.cs
This script is a manager for a finite state machine which controls the behaviour of the enemy at different times during the battle. The script initializes the enemy object by attaching an EnemyObject script to it, and 
 updates the state for the enemy by changing and updating the current state which it is in.

#### DuelingScene/AILogic/EnemyStateIdle.cs
This script controls the idle state of the enemy, where it moves randomly to dodge the player's attacks, but does not shoot back at the player. The script uses a random number generator to control where the enemy moves, and updates
the enemy position based on the current time of the game.

#### DuelingScene/AILogic/EnemyStateShoot.cs
The script makes the enemy shoot a projectile at the player by standing still to charge up, and instantiating a projectile object before releasing it in the player's direction. The time between shots is controlled using a
variable which decides whether the enemy should return to the idle state or stay in the shooting state.

#### DuelingScene/Damage/IDamage.cs
This interface is used to collect the projectile types in the game under a common superclass. 
The interface provides an abstract method for the projectiles which is called when a projectile hits another game object which does not ignore collisions.

#### DuelingScene/Damage/PlayerObject.cs
This script handles the data and functions for the player object in the scene; It updates the hitbox of the player when they move, and also handles whenever they take damage or die in the duel.
 the script also adds components to the player object necessary for detecting collisions within the game.


#### DuelingScene/Damage/EnemyObject.cs
 This script provides functionality for the enemy object in the scene. It is similar to the player object in that it handles damage and dying for the enemy, alongside adding components.

#### DuelingScene/Projectile/ProjectileAbstract.cs
This is a superclass for the player and enemy projectiles within the game. It provides base functionality which is common to both the player and enemy projectiles and also serves to 
pass on hit events to the subclass functions which handle respective functionality for both projectiles.

#### DuelingScene/Projectile/ProjectilePlayer.cs
This is a subclass of ProjectileAbstract which further defines specialized functionality for the player projectiles, such as special effects and movement.
It inherits some functionality from the ProjectileAbstract class.

#### DuelingScene/Projectile/ProjectileEnemy.cs
This is a subclass of ProjectileAbstract which is similar to ProjectilePlayer. It defines specialized functionality for the Enemy projectiles which are fired at the player.

#### GameManager.cs
This is a script which contains classes used to store data used to set different difficulty and environment parameters in the game. Some of these parameters change as the player interacts with objects, but a good
number of them are fixed.

#### SpatialAudio.cs
This class manages how in-game sounds should be played to players in different scenes. This script is mainly used to add sound effects to projectiles which are shot from the player and enemy.

#### TransitionScene.cs
This is a helper class which contains functionality used to fade in and out a player's view and transition them to another scene.

#### Other:
We directly modified the SteamVR_Behaviour_Skeleton script in order to add fixes relating to animation and rendering isues on SteamVR's side when running our project. 
