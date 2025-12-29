using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using CustomPDALogMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UWE;
using XGamingRuntime;

namespace CustomPDALogMod
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "TDCDev.CustomPDALogMod";
        public const string NAME = "Custom PDA Log Mod";
        public const string VERSION = "1.0.4";

        public static ManualLogSource Log { get; private set; }

        private static Harmony Harmony;

        public readonly static string classidPDA = "233f0235-50b5-4dfe-b5db-a7cdcbeb064e";
        public readonly static string classidSignel = "155f315f-bc11-40f5-b4d3-87587621fa37";
        public static string PDAchildpath;
        public static SaveData Pdacache;



        private void Awake()
        {
            Log = Logger;

            Pdacache = SaveDataHandler.RegisterSaveDataCache<SaveData>();


            Logger.LogInfo($"Loading {NAME} Version Is {VERSION}");

                    foreach (string folder in FindJsonDirs())
                    {

                        Databanks.LoadLogs(folder);
                    }
            WaitScreenHandler.RegisterLateAsyncLoadTask(NAME, WaitForPlayerToSpawn, "Loading/Spawn PDA's");
        }

        public static IEnumerator WaitForPlayerToSpawn(WaitScreenHandler.WaitScreenTask task)
        {
            
            yield return new WaitUntil(() => Databanks.LogsLoaded == true);
            foreach (JsonDef json in Databanks.LoadedJsons)
            {
                try
                {
                    if (Pdacache.IsColleted(json.id) == false)
                    {
                        Debug.Log($"{NAME} SpawningPda's");
                        Debug.Log($"{NAME} Checking PDA {json.id}, collected? {Pdacache.IsColleted(json.id)}");
                        CoroutineHost.StartCoroutine(GetPDAObject(json));
                        if (json.HasUploadSignel == true)

                        CoroutineHost.StartCoroutine(SignelPda.GetSignelObject(classidSignel, json));
                    }
                    else
                    {
                        Log.LogInfo($"{NAME} Skipping Pda {json.title} Reason: Collected: {Pdacache.IsColleted(json.id)}");
                    }
                }
                catch(System.Exception expection)
                {
                    Log.LogError($"{NAME} Failed To Spawn a PDA Log Error message: {expection.Message}");
                }
            }
        }

        private static IEnumerator GetPDAObject(JsonDef Logsettings)
        {

            GameObject PDA;
            var task = PrefabDatabase.GetPrefabAsync(Plugin.classidPDA);
            yield return task;
            bool loaded = task.TryGetPrefab(out PDA);
            if (!loaded)
            {
                throw new Exception($"Failed to spawn pda with class id: {Plugin.classidPDA} try a dif one if this happends");
            }

            GameObject ClonedPDA = string.IsNullOrEmpty(PDAchildpath) ? PDA : PDA.transform.gameObject;

            ClonePDAObject(ClonedPDA, Logsettings.position, Logsettings.rotation, Logsettings.id);
            

        }
        // Clone the prefab of the pda at the island base || Ada üssündeki PDA'nın prefabrik modelini klonlayın.
        
        private static void ClonePDAObject(GameObject PDA, Vector3 Position, Vector3 Rotation, string key)
        {

            GameObject PDAHolderObject = new GameObject();
            PDAHolderObject.name = "CustomPDALogs";
            Transform PDAHolderObjectTransform = PDAHolderObject.transform;
            quaternion PDARotation = quaternion.Euler(Rotation);

            GameObject PDAObject = GameObject.Instantiate(PDA, position: Position, rotation: PDARotation);
            PDAObject.transform.parent = PDAHolderObjectTransform;
            StoryHandTarget PDAstoryHandTarget =  PDAObject.GetComponent<StoryHandTarget>();
            PDAstoryHandTarget.goal.key = key;

        }

        private IEnumerable<string> FindJsonDirs()
        {
            // Pass the plugin paths || Eklenti yollarını iletin 
            string pluginfolder = Paths.PluginPath;
            string CustomPDALogsfolder = Path.Combine(pluginfolder, "CustomPDALogMod");
            string Logsfolder = Path.Combine(CustomPDALogsfolder, "logs");

            Directory.CreateDirectory(Logsfolder);
            yield return Logsfolder;

            // Check every folder in the plugins for a CustomPDAlogs folder || Eklentiler klasöründeki her bir klasörü kontrol edin ve CustomPDAlogs klasörünü arayın.
            foreach (string Pluginmods in Directory.GetDirectories(pluginfolder))
            {
                if (Path.GetFullPath(pluginfolder).Equals(Path.GetFullPath(CustomPDALogsfolder), System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string CustomPDAlogsfolder = Path.Combine(Pluginmods, "CustomPDALogMod");
                // If found return the mod folder || Bulunması durumunda mod klasörünü iade edin.
                if (Directory.Exists(CustomPDAlogsfolder))
                {
                    Logger.LogInfo($"[{NAME}] Found Logs In Folders {Path.GetFileName(CustomPDAlogsfolder)}");
                    yield return CustomPDAlogsfolder;
                }
            }
        }

    }

}
