using BepInEx;
using CustomPDALogMod;
using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Handlers;
using Nautilus.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomPDALogMod
{
    internal class AudiotoFMODAsset
    {

        internal static string pathid;
        public static string ConverttoFMOD(string AudioPath, JsonDef log)
        {

            try
            {
                var soundSource = new ModFolderSoundSource(Path.Combine(Paths.PluginPath, "CustomPDALogMod", "logs", "Voicelog"), Assembly.GetExecutingAssembly());
                var soundBuilder = new FModSoundBuilder(soundSource);

                if (log.Audiofile != string.Empty)
                {
                    string LogID = $"PdaLog_{log.id}_Audiolog";

                    soundBuilder.CreateNewEvent(LogID, AudioUtils.BusPaths.VoiceOvers)
                                .SetMode2D(false)
                                .SetSounds(true, Path.GetFileNameWithoutExtension(log.Audiofile))
                                .Register();

                    pathid = LogID;

                    Plugin.Log.LogInfo($"Created Custom Log for  PDA:'{log.id}': From Audio {log.Audiofile}");
                    return LogID;
                }

            }
            catch (System.Exception exception)
            {
                Plugin.Log.LogError($"Failed to create custom log for '{log.id}': {exception.Message}");
                return null;
            }
            if (pathid != string.Empty)
            {
                return pathid;
            }
            return null;
        }
    }
}
