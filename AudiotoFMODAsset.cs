using BepInEx;
using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Utility;
using PDALogs;
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


        public static FMODAsset ConverttoFMOD(string Audio, JsonDef log)
        {
            Plugin.Log.LogInfo($"starting to convert audiofile to fmod path: {log.Audiofile}");

            string pathofmodfolder = Path.Combine(Paths.PluginPath, "CustomPDALogMod");
            string pathoflogfolder = Path.Combine(pathofmodfolder, "logs");
            string pathofaudiofolder = Path.Combine(pathoflogfolder, "Voicelog");
            string audiofile = Path.Combine(pathofaudiofolder, Audio);

            var AudioSources = new ModFolderSoundSource(pathofaudiofolder, Assembly.GetExecutingAssembly());
            FModSoundBuilder builder = new FModSoundBuilder(AudioSources);

            string Voicelogeventid = $"voicelog_{log.id}_log";

            IFModSoundBuilder eventbuilder = builder.CreateNewEvent(Voicelogeventid, AudioUtils.BusPaths.PDAVoice)
                .SetMode2D(false)
                .SetSounds(true, Path.GetFileNameWithoutExtension(log.Audiofile));
            eventbuilder.SetSound(Audio).SetFadeDuration(0.5f).SetMode2D(false);

            var FModAsset = Nautilus.Utility.AudioUtils.GetFmodAsset(Voicelogeventid);

            if (FModAsset == null)
            {
                Plugin.Log.LogError($"Fmod Asset for {log.title} is null make sure the path is correct path for audio:{log.Audiofile}");
            }
            else
            {
                Plugin.Log.LogInfo($"FMODAsset {FModAsset} has created correctly.");
                return FModAsset;
            }
            return FModAsset;


        }
    }
}
