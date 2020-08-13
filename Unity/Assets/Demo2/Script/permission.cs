using UnityEngine;
using UnityEngine.Android;

namespace Demo2.Script {
    public class permission : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION ")) {
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE")) {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
