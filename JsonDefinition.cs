using Newtonsoft.Json;
using UnityEngine;

namespace PDALogs
{
    public class JsonDef
    {
        public string id{ get; set; }
        public string title{ get; set; }
        public string description{ get; set; }
        public string category{ get; set; }
        public string subcategory{ get; set; }
        public Vector3 position{ get; set; }
        public Vector3 rotation{  get; set; }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

    }
}
