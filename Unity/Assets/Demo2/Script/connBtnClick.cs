using System;
using TMPro;
using UnityEngine;

namespace Demo2.Script {
    public class connBtnClick : MonoBehaviour {
        [SerializeField]
        private GameObject bleDevicesList;
        private static GameObject thisObj;

        void Start() {
            // bleDevicesList = this.transform.parent.Find("bleDevicesList").gameObject;
            
            // thisObj = this.gameObject;
            // Debug.Log("UnityObjName:"+thisObj.name);
        }

        public void BtnClick() {
            GameObject toConnObj = this.transform.GetChild(2).gameObject;
            if (toConnObj == null) {
                Toast.Instance.Show("物件連結失敗", 1f);
                return;
            }

            thisObj = this.gameObject;
            BleEvent.ConnObj = toConnObj;
            // bleDevicesList.SetActive(true);
        }

        public static void ChangeBtnName(String name) {
            thisObj.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(name);
        }
    }
}