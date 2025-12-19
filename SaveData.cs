using System;
using Nautilus.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDALogs;

namespace CustomPDALogMod
{
    public class SaveData : SaveDataCache
    {
        public List<string> CollectedPDAs { get; set; } = new List<string>();

        public bool IsColleted(string id) => CollectedPDAs.Contains(id);

        public void MarkAsCollected(string id)
        {
            if (!CollectedPDAs.Contains(id))
                CollectedPDAs.Add(id);
        }
    }
}
