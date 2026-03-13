using R2API;
using RoR2;
using SS2;
using Tropics;
using SS2.Survivors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSCStage
{
    public class Starstorm2Compat
    {
        public static string FindEnemyConfig(string monsterName) //Lamp, Lamp Boss, Acid Bug
        {
            var defstring = "00 - Enemy Disabling.Disable Enemy: " + monsterName;
            var monsterConfig = SS2Config.ConfigMonster.GetConfigEntries();
            foreach (var entry in monsterConfig)
            {
                if (entry.Definition.ToString() == defstring)
                {
                    var configValue = entry.GetSerializedValue();
                    return configValue;
                }
            }
            return "false";

        }

        public static void AddEnemies()
        {
            // Beta-exclusive enemies
            if (SS2Config.enableBeta.value)
            {
                // Archer Bug/Wasp
                var acidBugValue = FindEnemyConfig("Acid Bug");

                if (Tropics.Tropics.toggleArcherBug.Value && acidBugValue == "false")
                {
                    var acidBugCard = new RoR2.DirectorCard()
                    {
                        spawnCard = SS2Assets.LoadAsset<RoR2.SpawnCard>("cscAcidBug", (SS2Bundle)17),
                        spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                        selectionWeight = 1
                    };

                    var acidBugHolder = new DirectorAPI.DirectorCardHolder
                    {
                        Card = acidBugCard,
                        MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters
                    };
                    DirectorAPI.Helpers.AddNewMonsterToStage(acidBugHolder, false, DirectorAPI.Stage.Custom, Tropics.Tropics.RegularSceneName);
                    DirectorAPI.Helpers.AddNewMonsterToStage(acidBugHolder, false, DirectorAPI.Stage.Custom, Tropics.Tropics.LoopSceneName);
                    DirectorAPI.Helpers.AddNewMonsterToStage(acidBugHolder, false, DirectorAPI.Stage.Custom, Tropics.Tropics.SimuSceneName);
                    Log.Info("Archer Bug added to Sunset Tropics' spawn pool.");

                }
            }

        }

    }

}
