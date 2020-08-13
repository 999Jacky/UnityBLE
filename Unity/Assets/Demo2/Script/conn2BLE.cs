using UnityEngine;

namespace Demo2.Script {
    public class conn2BLE : MonoBehaviour {
        public int index;
        static int ToBindIndex;
        static GameObject thisBtnOnj;
        [SerializeField] public GameObject selectBodyList;

        public void connectClick() {
            hideObj.SelectBodyList.transform.gameObject.SetActive(true);
            thisBtnOnj = this.gameObject;
            ToBindIndex = index;
        }

        public void selectBodyDone(string body) {
            GameObject selectedBody = GameObject.FindWithTag(body);
            connect2BleFunc(selectedBody);
        }

        public void connect2BleFunc(GameObject body) {
            bool isOk = BleEvent.ConnectBleDev(ToBindIndex);
            if (isOk) {
                Toast.Instance.Show("連線成功", 1f);
                BleEvent.ConnectionTextMap[BleEvent.BleMac[ToBindIndex]] = BleEvent.ConnObj;
                BleEvent.ConnectionBodyMap[BleEvent.BleMac[ToBindIndex]] = body;
                connBtnClick.ChangeBtnName(BleEvent.BleMac[ToBindIndex]);
                Destroy(thisBtnOnj);
                hideObj.HideSelectList();
            }
        }
    }
}