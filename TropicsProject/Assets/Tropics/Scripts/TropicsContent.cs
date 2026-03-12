using HG;
using MapSandbox.Content;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using RoR2.Networking;
using RoR2BepInExPack.GameAssetPaths;
using ShaderSwapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static RoR2.Console;
using static UnityEngine.UI.Image;

namespace Tropics.Content
{
    public static class TropicsContent
    {

        internal const string ScenesAssetBundleFileName = "TropicsScenes";
        internal const string AssetsAssetBundleFileName = "TropicsAssets";

        private static AssetBundle _scenesAssetBundle;
        private static AssetBundle _assetsAssetBundle;

        internal static UnlockableDef[] UnlockableDefs;
        internal static SceneDef[] SceneDefs;

        //Sunset Tropics
        internal static SceneDef TropicsSceneDef;
        internal static Sprite TropicsSceneDefPreviewSprite;
        internal static Material TropicsBazaarSeer;

        //Midnight Tropics
        internal static SceneDef LoopSceneDef;
        internal static Sprite LoopSceneDefPreviewSprite;
        internal static Material LoopBazaarSeer;

        //Simulacrum Sunset Tropics
        internal static SceneDef simuSceneDef;
        internal static Sprite simuSceneDefPreviewSprite;
        internal static Material simuBazaarSeer;

        //Misc. Stuff
        internal static ExplicitPickupDropTable tricornDropTable;

        public static List<Material> SwappedMaterials = new List<Material>();

        internal static IEnumerator LoadAssetBundlesAsync(AssetBundle scenesAssetBundle, AssetBundle assetsAssetBundle, IProgress<float> progress, ContentPack contentPack)
        {
            _scenesAssetBundle = scenesAssetBundle;
            _assetsAssetBundle = assetsAssetBundle;
            
            var upgradeStubbedShaders = _assetsAssetBundle.UpgradeStubbedShadersAsync();
            while (upgradeStubbedShaders.MoveNext())
            {
                yield return upgradeStubbedShaders.Current;
            }

            yield return LoadAllAssetsAsync(assetsAssetBundle, progress, (Action<UnlockableDef[]>)((assets) =>
            {
                contentPack.unlockableDefs.Add(assets);
            }));

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<Sprite[]>)((assets) =>
            {
                TropicsSceneDefPreviewSprite = assets.First(a => a.name == "texTropicsScenePreview");
                LoopSceneDefPreviewSprite = assets.First(a => a.name == "texTropicsNightScenePreview");
                simuSceneDefPreviewSprite = assets.First(a => a.name == "texTropicsScenePreview");
            }));

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<SceneDef[]>)((assets) =>
            {
                SceneDefs = assets;
                TropicsSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == Tropics.RegularSceneName);
                LoopSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == Tropics.LoopSceneName);
                simuSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == Tropics.SimuSceneName);
                Log.Debug(TropicsSceneDef.nameToken + " + " + LoopSceneDef.nameToken + " + " + simuSceneDef.nameToken);
                contentPack.sceneDefs.Add(assets);
            }));

            TropicsSceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)TropicsSceneDef.previewTexture);
            LoopSceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)TropicsSceneDef.previewTexture);
            simuSceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)simuSceneDef.previewTexture);

            var tropicsTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/MusicTrackDefs/muFULLSong02.asset");
            while (!tropicsTrackDefRequest.IsDone)
            {
                yield return null;
            }
            var tropicsBossTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/MusicTrackDefs/muSong16.asset");
            while (!tropicsBossTrackDefRequest.IsDone)
            {
                yield return null;
            }


            TropicsSceneDef.mainTrack = tropicsTrackDefRequest.Result;
            TropicsSceneDef.bossTrack = tropicsBossTrackDefRequest.Result;
            LoopSceneDef.mainTrack = tropicsTrackDefRequest.Result;
            LoopSceneDef.bossTrack = tropicsBossTrackDefRequest.Result;
            simuSceneDef.mainTrack = tropicsTrackDefRequest.Result;
            simuSceneDef.bossTrack = tropicsBossTrackDefRequest.Result;

            // if if if if if if if if if if if if if 
            if (Tropics.enableRegular.Value && Tropics.enableVariant.Value && Tropics.loopExclusiveVariant.Value && !Tropics.swapVariantPlaces.Value)
            {
                //both variants are enabled and the night variant only appears after looping
                TropicsSceneDef.loopedSceneDef = LoopSceneDef;
                LoopSceneDef.loopedSceneDef = null;
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(TropicsSceneDef, StageRegistration.defaultWeight, true, false);
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(LoopSceneDef, StageRegistration.defaultWeight, false, true);
                Log.Debug("Sunset Tropics and Midnight Tropics registered. Sunset pre-loop, Midnight post-loop");
            }
            else if (Tropics.enableRegular.Value && Tropics.enableVariant.Value && Tropics.loopExclusiveVariant.Value && Tropics.swapVariantPlaces.Value)
            {
                //both variants are enabled but their places be swarped
                LoopSceneDef.loopedSceneDef = TropicsSceneDef;
                TropicsSceneDef.loopedSceneDef = null;
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(TropicsSceneDef, StageRegistration.defaultWeight, false, true);
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(LoopSceneDef, StageRegistration.defaultWeight, true, false);
                Log.Debug("Sunset Tropics and Midnight Tropics registered. Midnight pre-loop, Sunset post-loop");
            }
            else if (Tropics.enableRegular.Value && Tropics.enableVariant.Value && !Tropics.loopExclusiveVariant.Value)
            {
                //both variants are enabled but the variant can appear at any time
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(TropicsSceneDef, (StageRegistration.defaultWeight / 2));
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(LoopSceneDef, (StageRegistration.defaultWeight / 2));
                Log.Debug("Sunset Tropics and Midnight Tropics registered. Both stages appear pre & post loop (weight halved)");
            }
            else if (Tropics.enableRegular.Value && !Tropics.enableVariant.Value)
            {
                //only Sunset Tropics is enabled
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(TropicsSceneDef);
                Log.Debug("Sunset Tropics registered only");
            }
            else if (!Tropics.enableRegular.Value && Tropics.enableVariant.Value)
            {
                //only Midnight Tropics is enabled
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(LoopSceneDef);
                Log.Debug("Midnight Tropics registered only");
            }

            if (Tropics.enableSimulacrum.Value && Tropics.simulacrumStage1.Value)
            {
                Simulacrum.RegisterSceneToSimulacrum(simuSceneDef);
            }
            else if (Tropics.enableSimulacrum.Value && !Tropics.simulacrumStage1.Value)
            {
                Simulacrum.RegisterSceneToSimulacrum(simuSceneDef, false);
            }

        }

internal static void Unload()
        {
            _assetsAssetBundle.Unload(true);
            _scenesAssetBundle.Unload(true);
        }

        private static IEnumerator LoadAllAssetsAsync<T>(AssetBundle assetBundle, IProgress<float> progress, Action<T[]> onAssetsLoaded) where T : UnityEngine.Object
        {
            var sceneDefsRequest = assetBundle.LoadAllAssetsAsync<T>();
            while (!sceneDefsRequest.isDone)
            {
                progress.Report(sceneDefsRequest.progress);
                yield return null;
            }

            onAssetsLoaded(sceneDefsRequest.allAssets.Cast<T>().ToArray());

            yield break;
        }
    }
}
