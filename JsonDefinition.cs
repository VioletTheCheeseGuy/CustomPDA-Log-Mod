using Newtonsoft.Json;
using UnityEngine;

namespace CustomPDALogMod
{
    public class JsonDef
    {
        public string id{ get; set; }
        public string title{ get; set; }
        public string Imagepath { get; set; }
        public string Audiofile { get; set; }
        public string description{ get; set; }
        public string category{ get; set; }
        public Vector3 position{ get; set; }
        public Vector3 rotation{  get; set; }


        public bool HasUploadSignel { get; set; } = false;
        public string SignalLabel { get; set; } = string.Empty;
        public string SignalID { get; set; } = string.Empty;
        public bool ShowinPDA { get; set; } = true;
        public bool Disableonarival { get; set; } = true;
        public Vector3 signalposition { get; set; } = new Vector3(0f,0f,0f);



    }
}
