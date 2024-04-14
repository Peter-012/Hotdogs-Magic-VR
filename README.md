# Hotdogs and Magic - User Manual

Please visit our website. It includes features like a demo video and a tutorial on how to play. You can find a copy of the website under the folder "Website" within the GitHub repo.

## Setup Guide

### Hardware Requirements
All you need is a VR headset that is linked to a powerful computer and VR controllers to track your hand movements. It is preferable that you have knuckle or wrist straps for your controllers and that you are wearing them since it is quite easy to accidentally throw your controller during play sessions.

### Play Space
This game requires you to move left and right in order to dodge projectiles in the game. Be sure to have ample room to shuffle left and right and some space to extend your arms forward. Also, it is important to tighten the strap of your headset in order to keep it from falling off when playing this game.

### Software Setup
Under the "Releases" section of GitHub will be a Unity build for the VR game. Just click and run the exe file to run the game. A copy of the build can also be found in the GitHub repo under the folder "Builds". If you would like to clone the repo and see the code in action within Unity, make sure that you have Unity Editor version 2022.3.17f1 before launching the project.

## Description of Implementation

This game was developed using the Unity Editor version 2022.3.17f1

### Special Assets
Other than the projectile (with collsion effects) asset "Fire Ice Projectile - Explosion" which came from the asset store, all assets have been created by us using Blender and the Unity shader files.

### C# Script Implementation

#### MenuScene/StartMenuScene.cs
Description

#### MenuScene/MenuSelection.cs
Description

#### MenuScene/StartDuel.cs
Description

#### MenuScene/BookinteractionHandler.cs
Description

#### MenuScene/Video.cs
Description

#### DuelingScene/StartDuelScene.cs
Description

#### DuelingScene/Wand/SpawnWand.cs
Description

#### DuelingScene/Wand/WandLogic.cs
Description

#### DuelingScene/AILogic/EnemyStateManager.cs
Description

#### DuelingScene/AILogic/EnemyStateIdle.cs
Description

#### DuelingScene/AILogic/EnemyStateShoot.cs
Description

#### DuelingScene/Damage/IDamage.cs
Description

#### DuelingScene/Damage/PlayerDamage.cs
Description

#### DuelingScene/Damage/EnemyDamage.cs
Description

#### DuelingScene/Projectile/ProjectileAbstract.cs
Description

#### DuelingScene/Projectile/ProjectilePlayer.cs
Description

#### DuelingScene/Projectile/ProjectileEnemy.cs
Description

#### GameManager.cs
Description

#### SpatialAudio.cs
Description

#### TransitionScene.cs
Description