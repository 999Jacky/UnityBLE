using UnityEngine;

namespace Demo2.Script {
    public class hideObj : MonoBehaviour {
        public static GameObject SelectBodyList;
        public static GameObject SelectDeviceList;

        public GameObject SBL;
        public GameObject SDL;

        [SerializeField] public  GameObject DebugList;
        // Start is called before the first frame update
        void Start()
        {
        // SelectBodyList=GameObject.Find("selectBodyList");
        // SelectDeviceList=GameObject.Find("bleDevicesList");
        SelectDeviceList = SDL;
        SelectBodyList = SBL;
        SelectBodyList.SetActive(false);
        }

        public static void HideSelectList() {
            SelectBodyList.SetActive(false);
            SelectDeviceList.SetActive(false);
        }

        public  void HideDebug() {
            DebugList.SetActive(false);
        }
        public  void ShowDebug() {
            DebugList.SetActive(true);
        }
        
        

    }
}
