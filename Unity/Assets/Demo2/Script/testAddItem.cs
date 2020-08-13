using TMPro;
using UnityEngine;

namespace Demo2.Script {
    public class testAddItem : MonoBehaviour {
        private GameObject itemList;
        private GameObject bleListBtnPrefab;
        private int count;

        void Start() {
            itemList = GameObject.Find("Content");
            bleListBtnPrefab = GameObject.Find("bleListBtn");
            count = 0;
        }

        public void AddItem() {
            GameObject btn = Instantiate(bleListBtnPrefab, itemList.transform);
            TextMeshProUGUI btnTxt = btn.GetComponentInChildren<TextMeshProUGUI>();
            count++;
            btnTxt.SetText(count.ToString());
        }
    }
}