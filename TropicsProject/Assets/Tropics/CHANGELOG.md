# 1.1.0
* Art pass:
  * Replaced the skybox islands and added rock spires, floating islands, rings, and clouds opposite from the sun. Overall, it's a lot fuller and hopefully feels more natural
  * Replaced the wicker boxes with crab traps and wooden barrels to better fit the seaside village theme
  * Added rock spires to the clifftops near the playable space, to match the new spires in the skybox. Gives the map a much more unique and interesting shape I think
  * Added Titanic Plains style rings throughout the map
  * Added dry grass to the beaches and sea grass clusters underwater
  * Remodeled and retextured the wooden plank balconies throughout the map
  * Replaced the blue bazaar lights marking the legendary chests with the firebowls recently added to Wetland Downpour
  * Increased the poly count of the holes carved into the sea shell houses (sea shells are also now caked with moss)
  * Added more hanging moss clusters throughout the map where appropriate
  * Adjusted the map's fog colors. Distant fog is more saturated, mid fog is a bit more pinkish
  * Added ambient noise
* Gameplay changes:
  * Replaced a thin plank bridge between two nautilus shell houses with a small floating island. Also redirected a nearby cleyser to launch stuff onto it
  * Slightly expanded a beach on the outskirts of the map, near the aforementioned floating island. Teleporters can spawn there now  
  * Added a new little beach area on the tiny island with the little pond. Accessible via a new wooden ramp, shell house doorway, and ledge in the terrain
  * Removed a couple randomly scattered houses and replaced them with rings that provide cover in the same spots
  * Moved most of the legendary chest lights to make them much more noticeable from further away
  * Underwater fog in regular Sunset Tropics is a bit clearer
  * Added LOD groups to grass patches (they will get culled if you're far enough away)
  * Removed Newt Altars from the Simulacrum variant (oops)

# 1.0.7
* Attempted fix for the game maybe sometimes not loading (renamed the mod's asset bundles)

# 1.0.6
* Increased spawn distance for Hermit Crabs (Standard -> Far)

# 1.0.5
* Updated the water gravity script to fix a couple issues:
  * Enemies that spawned underwater would sometimes not be affected by water gravity
  * Enemies launched by Breaching Fin would float upward until they rose above the water
* Fixed Lemurian Eggs not spawning if Artifact of Devotion is enabled
* Fixed a spot where interactables could spawn halfway inside one of the giant sea shells

# 1.0.4
* Music no longer switches depending on whether Alloyed Collective is enabled
  * Previously it was Into the Doldrums when playing without DLC3, or Swamplified Mizzle with DLC3. Now it's always Into the Doldrums
  * This was changed since my implementation of switching music tracks conflicted with soundtrack mods (sorry!!)
* Increased distance required for the clam geysers (cleysers) to get culled. Should hopefully fix them almost always being invisible on low settings
* Slightly reduced selection weight for triple drone shops (5 -> 3)

# 1.0.3
* Unused legendary chests are now removed instead of remaining in the scene (bandaid fix for an issue where Lost and Found would show the contents of invisible chests in this map)

# 1.0.2
* Added LocationsOfPrecipitation as a dependency (oops!!!!!)
* Added a couple crates around a wooden ramp in one of the shell houses to prevent enemies getting stuck on the sides

# 1.0.1
* Changed boss theme (Hydrophobia -> A Tempestuous Noise of Thunder and Lightning Heard)
* Added a config option to enable EnemiesReturns' Archer Bugs on Sunset/Midnight Tropics (disabled by default)
* Fixed manifest.json file linking to Wetland Downpour's GitHub page (oops)

# 1.0.0
* Initial Release