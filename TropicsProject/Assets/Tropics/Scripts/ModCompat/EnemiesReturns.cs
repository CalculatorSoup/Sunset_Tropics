using EnemiesReturns;
using EnemiesReturns.Configuration;
using R2API;
using Tropics;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemiesReturns.Enemies.LynxTribe.Totem;
using EnemiesReturns.Configuration.LynxTribe;
using EnemiesReturns.Enemies.SandCrab;
using EnemiesReturns.Enemies.Colossus;
using EnemiesReturns.Enemies.ArcherBug;

namespace Tropics
{
    public class EnemiesReturnsCompat
    {
        public static void AddEnemies()
        {
            // Archer Bug
            if (Tropics.toggleArcherBugReturns.Value && General.EnableArcherBug.Value)
            {
                var newCard = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)ArcherBugBody.SpawnCards.cscArcherBugJungle,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = 1, //Can't seem to access archer bug config, so just using integers here
                    minimumStageCompletions = 0
                };

                var newCardHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = newCard,
                    MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters
                };

                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.RegularSceneName)) //Checking whether the stage list has this enemy to avoid adding a duplicate spawn card
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(newCardHolder, false, DirectorAPI.Stage.Custom, Tropics.RegularSceneName);
                    Log.Info("Archer Bug added to Sunset Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.LoopSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(newCardHolder, false, DirectorAPI.Stage.Custom, Tropics.LoopSceneName);
                    //Log.Info("Archer Bug added to Midnight Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.SimuSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(newCardHolder, false, DirectorAPI.Stage.Custom, Tropics.SimuSceneName);
                    //Log.Info("Archer Bug added to Sunset Tropics' simulacrum spawn pool.");
                }
            }


                // Sand Crab
                if (Tropics.toggleSandCrabs.Value && General.EnableSandCrab.Value)
            {
                var crabCard = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)SandCrabBody.SpawnCards.cscSandCrabSandy,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = SandCrab.SelectionWeight.Value,
                    minimumStageCompletions = SandCrab.MinimumStageCompletion.Value
                };

                var crabHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = crabCard,
                    MonsterCategory = DirectorAPI.MonsterCategory.Minibosses
                };

                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.RegularSceneName)) //Checking whether the stage list has this enemy to avoid adding a duplicate spawn card
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(crabHolder, false, DirectorAPI.Stage.Custom, Tropics.RegularSceneName);
                    Log.Info("Sand Crab added to Sunset Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.LoopSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(crabHolder, false, DirectorAPI.Stage.Custom, Tropics.LoopSceneName);
                    //Log.Info("Sand Crab added to Midnight Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.SimuSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(crabHolder, false, DirectorAPI.Stage.Custom, Tropics.SimuSceneName);
                    //Log.Info("Sand Crab added to Sunset Tropics' simulacrum spawn pool.");
                }

            }

            // Lynx Totem
            if (Tropics.toggleLynxTotems.Value && General.EnableLynxTotem.Value)
            {
                var totemCard = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)TotemBody.SpawnCards.cscLynxTotemDefault,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = LynxTotem.SelectionWeight.Value,
                    minimumStageCompletions = LynxTotem.MinimumStageCompletion.Value
                };

                var totemHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = totemCard,
                    MonsterCategory = DirectorAPI.MonsterCategory.Champions
                };

                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.RegularSceneName)) //Checking whether the stage list has this enemy to avoid adding a duplicate spawn card
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(totemHolder, false, DirectorAPI.Stage.Custom, Tropics.RegularSceneName);
                    Log.Info("Lynx Totem added to Sunset Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.LoopSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(totemHolder, false, DirectorAPI.Stage.Custom, Tropics.LoopSceneName);
                    //Log.Info("Lynx Totem added to Midnight Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.SimuSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(totemHolder, false, DirectorAPI.Stage.Custom, Tropics.SimuSceneName);
                    //Log.Info("Lynx Totem added to Sunset Tropics' simulacrum spawn pool.");
                }

            }

            // Colossus
            if (Tropics.toggleColossus.Value && General.EnableColossus.Value)
            {
                var colossusCard = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)ColossusBody.SpawnCards.cscColossusSandy,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = Colossus.SelectionWeight.Value,
                    minimumStageCompletions = Colossus.MinimumStageCompletion.Value
                };

                var colossusHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = colossusCard,
                    MonsterCategory = DirectorAPI.MonsterCategory.Champions
                };

                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.RegularSceneName)) //Checking whether the stage list has this enemy to avoid adding a duplicate spawn card
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(colossusHolder, false, DirectorAPI.Stage.Custom, Tropics.RegularSceneName);
                    Log.Info("Colossus added to Sunset Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.LoopSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(colossusHolder, false, DirectorAPI.Stage.Custom, Tropics.LoopSceneName);
                    //Log.Info("Lynx Totem added to Midnight Tropics' spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Tropics.SimuSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(colossusHolder, false, DirectorAPI.Stage.Custom, Tropics.SimuSceneName);
                    //Log.Info("Lynx Totem added to Sunset Tropics' simulacrum spawn pool.");
                }

            }
        }
    }
}