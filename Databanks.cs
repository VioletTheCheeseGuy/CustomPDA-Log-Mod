using CustomPDALogMod;
using Nautilus.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PDALogs
{
    internal class Databanks
    {

        private static readonly List<JsonDef> LoadedJsons = new List<JsonDef>();
        private static readonly HashSet<string> RegisteredLogs = new HashSet<string>();

        public void RegisterDataBanks(JsonDef logsettings)
        {
            PDAHandler.AddEncyclopediaEntry
                (
                logsettings.id,
                logsettings.category,
                logsettings.title,
                logsettings.description,
                unlockSound: PDAHandler.UnlockImportant
                );

            StoryGoalHandler.RegisterCustomEvent(logsettings.id, () =>
            {

                PDAEncyclopedia.Add(logsettings.id, true);

            });
            

            


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
                        throw new Exception($"PDA Log file {Path.GetFileName(dir)} is invalid make sure the def is correct.");
                    }

                    //null; <== will replace once function is set up || Fonksiyon kurulduktan sonra değiştirilecektir.
                    //null; <== will replace once function is set up || Fonksiyon kurulduktan sonra değiştirilecektir.


                    LoadedJsons.Add(log);

                    Plugin.Log.LogInfo($"PDA Log ");
                }
                catch (System.Exception exception)
                {
                    Plugin.Log.LogError($"{Plugin.NAME} failed to load log {Path.GetFileName(dir)}: {exception.Message}");
                }
            }
        }

    }
}
