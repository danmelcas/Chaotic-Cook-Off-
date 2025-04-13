CS390 Class
# Unity Game Jam Quest

## Game Overview

Game Designer: Dani Castillo

Game Title: Chaotic Cook-Off!

GitLab Repository Link: https://coursework.cs.duke.edu/computer-game-design-spring-2025/danielaerica-castillo/unitygamejam

### Elevator Pitch

Provide a concise 1-3 sentence summary of your game, highlighting its core concept and what makes it unique.

Chaotic Cook-Off is a cooking simulator. 

### Theme Interpretation

Explain how your game aligns with the given theme. Describe your creative interpretation and how it is reflected through gameplay mechanics, narrative, or visuals.

I took the "strange powerups" theme literally and had that as a key mechanic in my game. Powerups and debuffs, while common in many games, aren't as common in cooking simulator type games, so their addition is strange in this context, but also I tried to make the powerups themselves somewhat absurd or strange as well. For instance, while there are some pretty typical powerups like doubled points, there are also some odd ones like having a giant knife powerup that makes the regularly shaped knife extremely large but able to cut salami in one quick slice, and the "high steaks" debuff causes cookable meat objects to float up and down. 

### Controls

List the controls for your game (e.g., arrow keys for movement, Z for action).

Use WASD to move around, mouse movement to look around, e to interact with objects (picking up, dropping), p to pause the game, and left click while holding the fire extinguisher to use the fire extinguisher. 

## MDA Framework

### Aesthetics

Identify the **primary** aesthetic(s) of your game and explain how the elements contribute to it. Consider aspects such as visuals, sound, and gameplay elements that support the intended player experience.

The primary aesthetic of the game is challenge. The goal of the game is to try to get 1-3 stars by assembling dishes correctly and quickly before they expire while managing challenges like preventing or putting out fires, having to deal with different dishes at once, and random powerups and debuffs that can make cooking harder or easier. The powerups and debuffs contribute to the challenge because they provide obstacles and benefits that make getting stars easier or harder depending on your luck. Sound also gives a sense of urgency and feedback for the user, telling them if they did well (as the cash register noise does) or diverts their attention to new obstacles like the fire sound when fires begin and spread. 

### Dynamics

Outline the core interactions and player experiences that emerge from the mechanics. How does the game encourage specific behaviors, strategies, or engagement over time?

Players are encouraged to plan and time their actions carefully as a result from the challenges that the mechanics and the limited time present. This is because if they don't keep track of the orders and what's currently on the stove, obstacles like fire popping up and spreading can happen, which takes more time since they need to use the fire extinguisher. If they are sloppy with their cooking, they actually lose more than they earn; for instance, a raw or burnt dish will always cause a penalty, so players must also keep in mind quality. 

Player behavior is also influenced by the random powerups and debuffs that happen. "High Steaks", for instance, is a debuff that causes all cookable meat objects to begin floating away for a short duration, which hurts the player, and from there players can either focus on something else (like chopping) or brute force their way into cooking the meat. Some players might notice though that this only affects meat that aren't on plates, so players might try quickly plating the dishes. "Timer Freeze" allows the opportunity for players to quickly get pressing orders done since they have some forgiveness, "Order Mix-up" and "Double Points" encourages players to quickly submit dishes, "Flash Fry" makes meat instantly cook when placed on the stove so encourages players to cook as much meat as possible in that short duration, and so on. Players' actions are likely quite responsive to the powerups and debuffs at hand. Players may need to anticipate and prepare for these situations and may take actions to do so, like preparing dishes other than the ones shown currently in case an "Order Mix-Up" debuff happens. 

Players might also notice that points are awarded differently based on the complexity of the dish and the timing of the dish, so players could think about prioritizing the completion of higher rewarding dishes. 

### Mechanics

Detail the core mechanics that define your game, such as movement, player actions, obstacles, and unique features. Explain how these mechanics work together to create an engaging gameplay experience.

The core mechanics of the game include moving around, chopping, picking up objects, moving them and dropping them, using the fire extinguisher, cooking objects by placing them on the stove, and ringing the bell to submit an order. Players get bonuses for completing dishes quickly, get penalized if they miss an order or deliver them in poor states (like raw or inedible), and must also manage random powerups and debuffs that happen during the game. The gameplay experience could be considered as engaging because each play of the game is different and even one play could take a turn for the worse or better because of the random powerups/debuffs. Players could get lucky and get many powerups, or get the more helpful ones. This, coupled with the random orders, makes each game slightly different and could encourage players to keep trying to get 3 stars since they could get lucky in the next game. The gameplay experience could also be considered engaging because of its demand, explained by the time pressure coupled with the different requirements for dishes like chopping and cooking, as there is always something that needs to be done with orders spawning very frequently.   

## External Resources

### Assets

List any external assets used (e.g., sprite graphics, sound effects, music) and their sources. Provide proper attribution.

Scripts: 
- Quick Outline asset: script that adds an outline taken from Unity Store at link https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488
- Player movement code adapted from "First person Movement in 10 Minutes - Unity Tutorial" https://youtu.be/f473C43s8nE?si=fwLLHuvtgNaQZ1I4

Assets/Design: 
-Hands and their animations are by Daniel Stringer, taken from SpaceShooter2022 a VR tutorial on Udemy at https://www.udemy.com/course/unityspaceshooter/.
Models from the kitchen environment taken from https://assetstore.unity.com/packages/3d/props/furniture/kitchen-furniture-starterpack-209331 and https://assetstore.unity.com/packages/3d/props/furniture/kitchen-set-interior-263284h
-Player model: https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/free-low-poly-human-rpg-character-219979    
-Food pack: https://assetstore.unity.com/packages/3d/props/food/food-pack-free-demo-225294 and https://assetstore.unity.com/packages/3d/props/food/food-pack-low-poly-fast-food-drinks-221388  
-"Table Bell" (https://skfb.ly/6YJwF) by gla_bot is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
-Cartoon Fire by ABitOfGameDev: https://github.com/abitofgamedev/CartoonFire/tree/master
-Fire extinguisher model: "Low-poly Fire extinguisher (free)" (https://skfb.ly/oAxJR) by szaw is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/). 

Fonts: 
Bitshow by khuranstudio.com 
UT Breado by Universitype 

Sound effects and music: 
- Sizzle by WrightBrandBacon -- https://freesound.org/s/618146/ -- License: Creative Commons 0
- Knife chopping sound effect by Floraphonic from Pixabay https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=211702
- Negative game over SFX found at https://uppbeat.io/sfx/cartoon-fail-music-sting/12432/31046
- Positive game over SFX (achievement sound) from https://freesound.org/people/LittleRobotSoundFactory/sounds/270404/ 
- UI Click Sound: Mouth_22.wav by LittleRobotSoundFactory -- https://freesound.org/s/290489/ -- License: Attribution 4.0
- Fire sound effect: Sound Effect by Max Hammarbäck from Pixabay at https://pixabay.com/users/maxhammarb%C3%A4ck-25559203/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=21991
- Fire extinguisher Sound Effect from https://www.youtube.com/watch?v=3E69S13c84w

Music by Ben Sound
- Main menu music: "Cute" by Ben Sound https://www.bensound.com/royalty-free-music/track/cute
- In game music: "Jazzy French" by Ben Sound https://www.bensound.com/royalty-free-music/track/jazzy-frenchy-upbeat-funny 

### Code


## Code Documentation

You do not need to modify the Code Documentation section of the readme. This seciton serves as a reminder to make sure that your in-code documentation is clear and informative. Important sections such as function or files should be accompanied by comments describing their purpose and functionality.
