using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class SyncMove : NetworkBehaviour
{
    public GameObject[] qrCodes;
    public GameObject tempObject;

    public TMP_Text metaData;

    public string metaString;
    // Start is called before the first frame update
    void Start()
    {
        if(IsClient)
        Debug.Log("id is " + GetComponent<NetworkObject>().NetworkObjectId);
        Debug.Log("Object Spawned");
        StartCoroutine(GetText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GetText()
    {
        UnityWebRequest www = new UnityWebRequest("https://nnedigitaldesignstorage.blob.core.windows.net/candidatetasks/Metadata.csv?sp=r&st=2021-03-15T09:12:39Z&se=2024-11-05T17:12:39Z&spr=https&sv=2020-02-10&sr=b&sig=oyj3Qyg4W42%2BO0d7YqmjxmKk0k%2BLVmE243ixdLaq3gk%3D");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            metaString = www.downloadHandler.text;

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    [ServerRpc(RequireOwnership =false)]

    public void MoveObjectToServerRpc(Vector3 targetPostion, Quaternion targetRotation, int id)
    {
        if (tempObject != null){
            transform.SetParent(null);
            tempObject.transform.SetParent(transform);
        }
      
        foreach (var qrCode in qrCodes)
        {
            qrCode.gameObject.transform.SetParent(transform);
            
        }
        tempObject = qrCodes[id];
        qrCodes[id].transform.SetParent(null);
        transform.SetParent(qrCodes[id].transform);
        

        qrCodes[id].transform.position = targetPostion;
        qrCodes[id].transform.rotation = targetRotation;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CSVServerRpc(int id)
    {
        var dataValues = metaString.Split(',');
        Debug.Log(dataValues[id+1].ToString());
        metaData.text = dataValues[id].ToString();
        //Debug.Log(dataValues[id][id].ToString());
    }

   
}


