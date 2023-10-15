using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastHandler : NetworkBehaviour
{
   

    // Reference to the SyncMove script

    private void Start()
    {
       
    }
   private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;

            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider != null)
                {
                   
                    if (hitInfo.collider.tag == "QRcode") 
                    {
                        int i = hitInfo.collider.GetComponent<ScanQR>().id;
                        Vector3 pos = hitInfo.collider.gameObject.transform.localPosition;
                        Quaternion rot = hitInfo.collider.gameObject.transform.localRotation;

                        if (!IsOwner) return;
                        // Call the public method in the SyncMove script to update the position
                        FindObjectOfType<SyncMove>().MoveObjectToServerRpc(pos,rot,i);
                        FindObjectOfType<SyncMove>().CSVServerRpc(i);
                    } 
                }
            }
        }
    }

 
}
