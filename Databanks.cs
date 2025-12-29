using BepInEx;
using CustomPDALogMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UWE;

namespace CustomPDALogMod
{
    internal class Databanks
    {
        public static bool LogsLoaded = false;

        public static readonly List<JsonDef> LoadedJsons = new List<JsonDef>();
        private static readonly HashSet<string> RegisteredLogs = new HashSet<string>();

        protected static void RegisterDataBanks(JsonDef logsettings, Texture2D image,FMODAsset Audiolog)
        {
            var encypath = $"EncyPath_{logsettings.category}"; 

            LanguageHandler.SetLanguageLine(encypath, logsettings.category);

            if (string.IsNullOrEmpty(logsettings.id))
                throw new Exception("Log id is null or empty");
            else
                PDAHandler.AddEncyclopediaEntry
                    (
                    logsettings.id,
                    logsettings.category,
                    logsettings.title,
                    logsettings.description,
                    image,
                    null,
                    unlockSound: PDAHandler.UnlockImportant,
                    Audiolog
                    
                    );
            Plugin.Log.LogInfo($"Log {logsettings.title} has registered.");

            StoryGoalHandler.RegisterCustomEvent(logsettings.id, () =>
            {
                PDAEncyclopedia.Add(logsettings.id, true);

                // Save Data Stuff || Verileri Kaydetme Şeyleri
                Plugin.Pdacache.CollectedPDAs.Add(logsettings.id);


                if (logsettings.HasUploadSignel == true)
                {
                    SignelPda.EnableSignal(logsettings);
                    
                }
                

            });
            
        }

        internal static IEnumerator registerdatabankswithaudio(string audio, JsonDef log)
        {
            Plugin.Log.LogInfo($"starting to convert audiofile to fmod path: {log.Audiofile}");
            string VoiceLogFMODAsset = AudiotoFMODAsset.ConverttoFMOD(log.Audiofile, log);

            RegisterDataBanks(log, null, AudioUtils.GetFmodAsset(VoiceLogFMODAsset));

            yield return VoiceLogFMODAsset;

        }

        


        internal static IEnumerator RegisterDatabankwithimage(string imagepath, JsonDef log)
        {
                string pathofmodfolder = Path.Combine(Paths.PluginPath, "CustomPDALogMod");
                string pathoflogfolder = Path.Combine(pathofmodfolder, "logs");
                string pathofimagefolder = Path.Combine(pathoflogfolder, "Image");
                string pathofimage = Path.Combine(pathofimagefolder, imagepath);
                Plugin.Log.LogInfo($"attempting to set up pda with image {pathofimage}");

                Texture2D image = ImagetoTexture2d.convert(pathofimage);

                RegisterDataBanks(log, image, null);

                yield return image;
        }
        

        internal static void LoadLogs(string directory)
        {
            foreach (string dir in Directory.GetFiles(directory, "*.json"))
            {
                try
                {
                    string json = File.ReadAllText(dir);
                    JsonDef log = JsonConvert.DeserializeObject<JsonDef>(json);

                    if (log == null)
                    {
                    }

                    if (log.Imagepath == string.Empty)
                    {
                            RegisterDataBanks(log, null, null);
                            LoadedJsons.Add(log);
                        
                    }
                    else
                    {

                        if (log.Audiofile != string.Empty)
                        {
                            CoroutineHost.StartCoroutine(registerdatabankswithaudio(log.Audiofile, log));
                        }
                        else
                        {

                            CoroutineHost.StartCoroutine(RegisterDatabankwithimage(log.Imagepath, log));
                        }


                        LoadedJsons.Add(log);
                    }

                    
                }
                catch (System.Exception exception)
                {

                }

                LogsLoaded = true;
            }
        }

    }
}
