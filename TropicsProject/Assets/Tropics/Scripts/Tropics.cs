using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using FSCStage;
using HG;
using IL.RoR2;
using On.RoR2;
using R2API;
using R2API.AddressReferencedAssets;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Tropics.Content;
using Tropics.ModChecks;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
//Copied from Hollow Summit copied from a private Unity project I use for testing maps copied from Ancient Observatory copied from Wetland Downpour copied from Fogbound Lagoon copied from Nuketown


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace Tropics
{
    [BepInPlugin(GUID, Name, Version)]
    public class Tropics : BaseUnityPlugin
    {
        public const string Author = "wormsworms";

        public const string Name = "Sunset_Tropics";

        public const string Version = "1.0.1";

        public const string GUID = Author + "." + Name;

        public const string RegularSceneName = "tropics_wormsworms";

        public const string LoopSceneName = "tropicsnight_wormsworms";

        public const string SimuSceneName = "ittropics_wormsworms";

        private static GameObject scalingChestPrefab;

        public static Tropics instance;

        public static ConfigEntry<bool> enableRegular;
        public static ConfigEntry<bool> waterMuffle;
        public static ConfigEntry<bool> enableVariant;
        public static ConfigEntry<bool> loopExclusiveVariant;
        public static ConfigEntry<bool> swapVariantPlaces;
        public static ConfigEntry<bool> enableSimulacrum;
        public static ConfigEntry<bool> simulacrumStage1;
        public static ConfigEntry<bool> toggleSandCrabs;
        public static ConfigEntry<bool> toggleLynxTotems;
        public static ConfigEntry<bool> toggleColossus;
        public static ConfigEntry<bool> toggleArcherBugReturns;
        public static ConfigEntry<bool> toggleArcherBug;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);

            ConfigSetup();

            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;

            RoR2.Language.collectLanguageRootFolders += CollectLanguageRootFolders;

            RoR2.RoR2Application.onLoadFinished += AddModdedEnemies;

            SceneManager.sceneLoaded += SceneSetup;

            RoR2.Run.onRunStartGlobal += InitializeBazaarSeerValues;

            On.RoR2.Run.PickNextStageScene += SwapBazaarFilters;

        }

        public void InitializeBazaarSeerValues(RoR2.Run run)
        {
            
            //filtering variants out of bazaar manually to prevent them appearing when they should not. vanilla does not do this
            
            if (Tropics.enableRegular.Value && Tropics.enableVariant.Value && Tropics.loopExclusiveVariant.Value && !Tropics.swapVariantPlaces.Value)
            {
                RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = false;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).filterOutOfBazaar = true;
            }
            else if (Tropics.enableRegular.Value && Tropics.enableVariant.Value && Tropics.loopExclusiveVariant.Value && Tropics.swapVariantPlaces.Value)
            {
                RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = true;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).filterOutOfBazaar = false;
            }
            

            // change music depending on whether DLC3 is enabled
            var dlc3 = Addressables.LoadAssetAsync<RoR2.ExpansionManagement.ExpansionDef>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.DLC3_asset).WaitForCompletion();
            if (RoR2.Run.instance.IsExpansionEnabled(dlc3))
            {
                var newmusic = Addressables.LoadAssetAsync<RoR2.MusicTrackDef>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.muGameplayDLC3_02_P_Map_asset).WaitForCompletion();
                RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).mainTrack = newmusic;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).mainTrack = newmusic;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(SimuSceneName).mainTrack = newmusic;
            } else
            {
                var regularmusic = Addressables.LoadAssetAsync<RoR2.MusicTrackDef>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_MusicTrackDefs.muFULLSong02_asset).WaitForCompletion();
                RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).mainTrack = regularmusic;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).mainTrack = regularmusic;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(SimuSceneName).mainTrack = regularmusic;
            }

        }

        public void SwapBazaarFilters(On.RoR2.Run.orig_PickNextStageScene orig, RoR2.Run self, WeightedSelection<RoR2.SceneDef> choices)
        {
            orig.Invoke(self, choices);

            if (Tropics.enableRegular.Value && Tropics.enableVariant.Value && Tropics.loopExclusiveVariant.Value && !Tropics.swapVariantPlaces.Value)
            {
                //swap bazaar values after clearing 5 stages
                if (RoR2.Run.instance.stageClearCount >= 5 && RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar != true)
                {
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = true;
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).filterOutOfBazaar = false;
                    Log.Debug("Swapped bazaar filter-out values (Regular TRUE, Loop FALSE)");
                }
            }
            else if ((Tropics.enableRegular.Value && Tropics.enableVariant.Value && Tropics.loopExclusiveVariant.Value && Tropics.swapVariantPlaces.Value))
            {
                if (RoR2.Run.instance.stageClearCount >= 5 && RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).filterOutOfBazaar != true)
                {
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = false;
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(LoopSceneName).filterOutOfBazaar = true;
                    Log.Debug("Swapped bazaar filter-out values (Regular FALSE, Loop TRUE)");
                }
            }
        }



        public static void AddModdedEnemies()
        {
            if (IsEnemiesReturns.enabled)
            {
                EnemiesReturnsCompat.AddEnemies(); //Sand Crab, Lynx Totem, Colossus
            }
            if (IsStarstorm2.enabled)
            {
                Starstorm2Compat.AddEnemies(); //Archer Bug
            }
        }

        private void Destroy()
        {
            RoR2.Language.collectLanguageRootFolders -= CollectLanguageRootFolders;
        }

        private static void GiveToRoR2OurContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(new ContentProvider());
        }

        public void CollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Language"));
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Plugins/Language"));
        }

        private void SceneSetup(Scene newScene, LoadSceneMode loadSceneMode)
        {
            if (newScene.name == RegularSceneName || newScene.name == LoopSceneName || newScene.name == SimuSceneName)
            {
                //swap legendary chests to a custom prefab that scales price with time
                GameObject[] chestObjects = {
                    GameObject.Find("Gold Chest 1/GoldChest(Clone)"),
                    GameObject.Find("Gold Chest 2/GoldChest(Clone)"),
                    GameObject.Find("Gold Chest 3/GoldChest(Clone)"),
                    GameObject.Find("Gold Chest 4/GoldChest(Clone)"),
                    GameObject.Find("Gold Chest 5/GoldChest(Clone)")
                };
                GameObject[] chestHolders = {
                    GameObject.Find("Gold Chests/Under Planks"),
                    GameObject.Find("Gold Chests/Beach Nautilus"),
                    GameObject.Find("Gold Chests/Ruins Staircase"),
                    GameObject.Find("Gold Chests/Cliff Hole"),
                    GameObject.Find("Gold Chests/Trench")
                };
                for (int i = 0; i < chestObjects.Length; i++)
                {
                    GameObject chest = chestObjects[i];
                    GameObject chestHolder = chestHolders[i];
                    if (chest != null)
                    {
                        GameObject newChest = UnityEngine.GameObject.Instantiate(scalingChestPrefab, chest.transform.position, chest.transform.rotation, chestHolder.transform);
                        NetworkServer.Spawn(newChest);
                        GameObject.Destroy(chest);
                    }
                }

            }
        }

        public static void CreateScalingChestPrefab(ContentPack contentPack)
        {
            var goldChest = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_GoldChest.GoldChest_prefab).WaitForCompletion();

            scalingChestPrefab = goldChest.InstantiateClone("TropicsScalingGoldChest", true);

            scalingChestPrefab.TryGetComponent(out RoR2.PurchaseInteraction pi);
            pi.automaticallyScaleCostWithDifficulty = true;

            contentPack.networkedObjectPrefabs.Add(new GameObject[] { scalingChestPrefab });
        }


        private void ConfigSetup()
        {
            enableRegular =
                base.Config.Bind<bool>("00 - Sunset Tropics",
                                       "Enable Sunset Tropics",
                                       true,
                                       "If true, Sunset Tropics can appear in runs.");
            waterMuffle =
                base.Config.Bind<bool>("00 - Sunset Tropics",
                                       "Underwater Music Muffling",
                                       true,
                                       "If true, music gets muffled while the camera is underwater in Sunset Tropics and its variants.");
            enableVariant =
                base.Config.Bind<bool>("01 - Midnight Tropics",
                                       "Enable Midnight Tropics",
                                       true,
                                       "If true, Midnight Tropics can appear in runs. If Sunset Tropics is disabled, the Loop Variant and Swap Places values are ignored and Midnight Tropics can appear at any time, effectively replacing it.");
            loopExclusiveVariant =
                base.Config.Bind<bool>("01 - Midnight Tropics",
                                       "Loop Variant",
                                       true,
                                       "If true, Midnight Tropics replaces Sunset Tropics after looping. If false, it can appear at any time (both variants will have their weight halved to prevent making Tropics more common than other stages).");
            swapVariantPlaces =
                base.Config.Bind<bool>("01 - Midnight Tropics",
                                       "Swap Places with Sunset Tropics",
                                       false,
                                       "If true, Midnight Tropics will appear before looping and Sunset Tropics will replace it after looping.");
            enableSimulacrum =
                base.Config.Bind<bool>("02 - Simulacrum Variant",
                                       "Enable Simulacrum Variant",
                                       true,
                                       "If true, Sunset Tropics can appear in the Simulacrum.");
            simulacrumStage1 =
                base.Config.Bind<bool>("02 - Simulacrum Variant",
                                       "Enable Simulacrum Variant on Stage 1",
                                       true,
                                       "If true, Sunset Tropics can appear as the first stage in the Simulacrum. If false, it can only appear on Stage 2 or higher, like Commencement.");
            toggleSandCrabs =
                base.Config.Bind<bool>("03 - Modded Enemies - EnemiesReturns",
                                       "Enable Sand Crabs",
                                       true,
                                       "If true, Sand Crabs can appear in Sunset Tropics and Midnight Tropics.");
            toggleLynxTotems =
                base.Config.Bind<bool>("03 - Modded Enemies - EnemiesReturns",
                                       "Enable Lynx Totems",
                                       true,
                                       "If true, Lynx Totems can appear in Sunset Tropics and Midnight Tropics.");
            toggleColossus =
                base.Config.Bind<bool>("03 - Modded Enemies - EnemiesReturns",
                                       "Enable Colossus",
                                       true,
                                       "If true, Colossi can appear in Sunset Tropics and Midnight Tropics.");
            toggleArcherBugReturns =
                base.Config.Bind<bool>("03 - Modded Enemies - EnemiesReturns",
                                       "Enable Archer Bugs",
                                       false,
                                       "If true, Archer Bugs can appear in Sunset Tropics and Midnight Tropics.");
            toggleArcherBug =
                base.Config.Bind<bool>("04 - Modded Enemies - Starstorm 2",
                                        "Enable Archer Bugs",
                                        true,
                                        "If true, Archer Bugs can appear in Sunset Tropics and Midnight Tropics.");
        }




    }
}
