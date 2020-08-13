using UnityEngine;

namespace Demo2.Script {
    public class debugBtnClick : MonoBehaviour {
        // Start is called before the first frame update
        [SerializeField] private GameObject c;

        public void DebugBtnClick() {
            c.GetComponent<Canvas>().enabled = !c.GetComponent<Canvas>().enabled;
        }
    }
}