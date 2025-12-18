using BepInEx;
using Nautilus.Handlers;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using PDALogs;
using BepInEx.Logging;

namespace CustomPDALogMod
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "TDCDev.CustomPDALogMod";
        public const string NAME = "Custom PDA Log Mod";
        public const string VERSION = "1.0.2";

        public static ManualLogSource Log;

        private string classidPDA = "233f0235-50b5-4dfe-b5db-a7cdcbeb064e";
        

        private void Awake()
        {
            Logger.LogInfo($"Loading {NAME} Version Is {VERSION}");

            foreach (string folder in FindJsonDirs()) 
            {
                Databanks.LoadLogs(folder);
            }
           

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

                string CustomPDAlogsfolder = Path.Combine(Pluginmods, "CustomPDAlogs");
                // If found return the mod folder || Bulunması durumunda mod klasörünü iade edin.
                if (Directory.Exists(CustomPDAlogsfolder))
                {
                    Logger.LogInfo($"[{NAME}] Found Logs In Folders {Path.GetFileName(Pluginmods)}");
                    yield return CustomPDAlogsfolder;
                }
            }
        }

    }

}
