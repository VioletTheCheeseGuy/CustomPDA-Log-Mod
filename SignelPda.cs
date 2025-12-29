using Nautilus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UWE;

namespace CustomPDALogMod
{



    internal class SignelPda
    {
        private static readonly Dictionary<string, GameObject> Signals = new Dictionary<string, GameObject>();

        private static GameObject _holder;

        public static IEnumerator GetSignelObject(string classid, JsonDef json)
        {

            GameObject Signel;
            var obj = PrefabDatabase.GetPrefabAsync(classid);

            yield return obj;

            bool loaded = obj.TryGetPrefab(out Signel);
            if (!loaded)
            {
                throw new Exception($"Failed to spawn Signel with class id: {classid} i probs forgor to set it or its null on launch");
            }

            GameObject SignalObject = Signel.transform.gameObject;

            SpawnSignelObject(SignalObject, json);

        }


        private static GameObject SpawnSignelObject(GameObject Signelobject, JsonDef json)
        {
            var Customholder = GetHolder();

            if (Customholder == null)
            {
                throw new Exception($"SignalObject Or The CustomSignalHolder Or The JsonDefinition Is Null Please lmk if you ever see dis");
            }

            if (json.HasUploadSignel == true)
            {

                var SignalName = $"{json.SignalLabel}Signal";



                GameObject Signal = GameObject.Instantiate(Signelobject,position: json.signalposition, rotation: Quaternion.identity);
                PingInstance Signalpingcomp = Signal.EnsureComponent<PingInstance>();
                
                LargeWorldEntity GlobalEnt = Signal.EnsureComponent<LargeWorldEntity>();
                Signal.transform.name = SignalName;


                SignalPing SignalPingComp = Signal.GetComponent<SignalPing>();
                PDANotification SignalPDANotif = Signal.GetComponent<PDANotification>();


                Signalpingcomp.displayPingInManager = json.ShowinPDA;
                SignalPingComp.disableOnEnter = json.Disableonarival;
                GlobalEnt.cellLevel = LargeWorldEntity.CellLevel.Global;
                Signal.SetActive(false);

                Signals[json.id] = Signal;


                return Signal;
            }
            else
            {

                Plugin.Log.LogInfo($"PDA Log {json.id} dosnt use a custom signal Skipping.");
                return null;
            }
        }

        public static void EnableSignal(JsonDef json)
        {
            if (!Signals.TryGetValue(json.id, out var signal))
            {

                Plugin.Log.LogError($"Failed to get signal for json{json.id}");
                return;

            }
            
            

            var SignalName = $"{json.SignalLabel}Signal";
            var pingInstance = signal.EnsureComponent<PingInstance>();
            var signalPing = signal.EnsureComponent<SignalPing>();
            var pda = signal.EnsureComponent<PDANotification>();

            if (signal.name == SignalName)
            {
                pingInstance._id = json.SignalID;
                pingInstance._label = json.SignalLabel;
                CoroutineHost.StartCoroutine(SetLabelOnceInilized(pingInstance,json.SignalLabel,signal, json.signalposition));

                signal.SetActive(true);

                
                pda.Play();
                
            }
        }
        private static GameObject GetHolder()
        {
            if (_holder != null) return _holder;

            _holder = new GameObject("CustomSignalHolder");
            UnityEngine.Object.DontDestroyOnLoad(_holder);
            return _holder;
        }

        private static IEnumerator SetLabelOnceInilized(PingInstance pingInstance, string label,GameObject Ping, Vector3 pingposition)
        {
            pingInstance.Initialize();
            yield return new WaitUntil(() => pingInstance.initialized == true);

            pingInstance.SetLabel(label);

            Ping.transform.position = pingposition;

        }

    }
}
