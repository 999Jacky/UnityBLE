using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Demo2.Script.AngleOffsetScript;

namespace Demo2.Script {
    public class BleEvent : MonoBehaviour {
        [SerializeField] private GameObject itemList;
        [SerializeField] private GameObject bleListBtnPrefab;
        private int count;

        public static AndroidJavaObject plugin;
        public static Dictionary<String, GameObject> ConnectionTextMap;
        public static Dictionary<String, GameObject> ConnectionBodyMap;
        public static List<String> BleMac;
        public static GameObject ConnObj;

        void Start() {
            ConnectionTextMap = new Dictionary<string, GameObject>();
            ConnectionBodyMap = new Dictionary<string, GameObject>();
            BleMac = new List<string>();
            count = -1;
            plugin = new AndroidJavaObject("com.nnn.bleplugin.unityBLE");
            bool isOk = plugin.Call<bool>("BLEInit");
            Debug.Log("Android ble Init:" + isOk);
            if (isOk) {
                Toast.Instance.Show("init done", 1f);
                ScanDev();
            }

            // int r = plugin.Call<int>("test");
            // Debug.Log("log:" + r);
        }

        public static void ScanDev() {
            plugin.Call("ScanBleDev");
        }

        public void ScanDevCall() {
            plugin.Call("ScanBleDev");
        }

        public void NewDevicesFunc(String msg) {
            // name,mac
            AddItem(msg);
            String[] t = msg.Split(',');
            BleMac.Add(t[1]);
            Debug.Log("newDev:" + msg);
        }

        public void SensorXYZUpdate(String msg) {
            // Debug.Log("UnityLogXYZ:" + msg.ToString());
            // mac@z,y,x
            String[] t = msg.Split('@');
            TextMeshProUGUI txt = ConnectionTextMap[t[0]].GetComponent<TextMeshProUGUI>();
            txt.SetText(t[1]);
            String[] angle = t[1].Split(',');
            Point p = new Point();
            p.x = float.Parse(angle[0]);
            p.y = float.Parse(angle[1]);
            p.z = float.Parse(angle[2]);
            LastAngleStore[t[0]] = new Point(p.x, p.y, p.z);
            if (AngleOffset.ContainsKey(t[0])) {
                Point offsetP = AngleOffset[t[0]];
                p += offsetP;
            }

            // y,x,z
            ConnectionBodyMap[t[0]].transform.localEulerAngles = new Vector3(p.y, p.x, p.z);
        }

        public static bool ConnectBleDev(int index) {
            bool isOk = plugin.Call<bool>("ConnectBLE", index);
            return isOk;
        }

        void AddItem(String devName) {
            GameObject btn = Instantiate(bleListBtnPrefab, itemList.transform);
            TextMeshProUGUI btnTxt = btn.GetComponentInChildren<TextMeshProUGUI>();
            count++;
            btnTxt.SetText(devName);
            btn.GetComponent<conn2BLE>().index = count;
        }
    }
}
