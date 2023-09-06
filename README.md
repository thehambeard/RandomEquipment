## Random Equipment 2

 [WrathModMaker](https://github.com/cabarius/WrathModMaker) is required to build.
 [ModMenu 1.3.1+](https://github.com/WittleWolfie/ModMenu/releases) is required to build and required to play.

 This is the second version of Random Equipment. The entire mod has been rewritten from the ground up. While not a whole lot has changed on the front end
 lots have changed on the back end of things. This opens up a options for user customizations in the future.  This mod is save safe and can be installed 
 during a playthrough or removed. It will break nothing.
 
 Updates include:
 * New settings menu using WittleWolfie's [ModMenu](https://github.com/WittleWolfie/ModMenu/releases). The lets you customize your random item experience!
	Each type of container can have it's own chances of containing loot and the level modifiers that get applied.
 * Containers can now add some usables to the container for a little extra spice.
 * Option to see your roll each time you open that container. Rolls use the base dice system from the game so no blaming me for your bad luck!

 Settings (found under the games settings menu under the tab Mods:
 * There are 3 sections:
	* Standard: These are the settings for  normal every day container you find
	* Hidden: These are the settings for containers you require a perception check to find
	* Locked: These are the settings for conatainers that are locked.
 * Chance of Random Loot
	* This setting is the base chance that there is random loot in the container. If you miss this check you get nothing.
 * Chance of Random Usables
	* This setting is the chance that a container contains random usables in addition to whatever else is in the chest.
 * Minimum/Maximum Level Modifier
	* These settings determine the range of the modifiers applied to the CR/CL of the loot in the container. If you get lucky your loot might be a level or two
	  higher than your current level or the reverse if you are unlucky. The default settings mostly have the extreme ends at a lower chance of being rolled than
	  the middle of the range (bell curve). The approximate chances of getting any specific modifier is listed in the description. 
 * Random Modifier Curve Shift
	* This setting will adjust were the middle of the bell curve mentioned above sits. Increasing it will make the Maximum modified side more likely to be rolled.
	  Decreasing it will move the chances closer to the Minimum Modifier side of the curve.
 * Random Modifier Curve Scale
	* This setting will adjust the peak of the curve. Increasing the scale will make all the numbers in the range move closer to being similar in the chances to roll.
	  Decreasing will make the middle of the range much more likely to be rolled making the ends of the range less likely. I suggest playing around with the sliders
	  to see how the effect the chances and select one that you would most want to see in your game.
 
 Notes:
 * The list of random loot that is possible is 3985 items from the game. This is a dump of the all the items in the game over 500gp (800gp for weapons). I have filtered out notable items, broken items, and most of the junk I believe.
   If there is anything I have missed and shouldn't be part of items given do let me know and I'll add it to the list.
	* 1583 weapons
	* 479 armors
	* 98 shields
	* 696 equipment pieces
	* 96 potions
	* 645 scrolls
	* 227 wands
	* 8 utility usables
	* 156 other usables
 * Random items are only ever given once so you shouldn't see any duplicates. This does not include basic usables.
 * You may see that you were successful in your roll but not get an item. This happens when a valid item in the level range can't be found. This is more common in the lower levels as I have filtered out
   most of the junk items. If you crank the chances to 100% this could also happen due to getting all the items in your level range. When this happens the game will attempt to find a valid item again but
   with a +1 level modifier. If that fails then no item is given. 
 * The level range is your modified level -1 and your modified level.
 * There just isn't a lot of level 1 gear so seeing the same items during each play through or not getting any items despite success rolls is not uncommon during the start of the game. 
	 
 Future plans:
 * I would like to add the option for user defined loot tables! The framework is there to make this doable as I built it with that in mind. An example of the loot tables could be something like:
	* 50% chance of container containing loot
	  * 30% chance of a usable (roll on usable table)
		* 30% 1d4 Potions
		* 30% 1d4 Scrolls
		* 30% Wand
		* 10% Special usable
	  * 30% chance of a weapon (roll on weapons table)
		* *list of weapons types...*
	  * *more tables...*
    You can see that tables can contain subtables making your random item experience very unique! I hope to add this if I get time...