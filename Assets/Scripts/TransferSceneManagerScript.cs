using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferSceneManagerScript : MonoBehaviour
{

    public Transform startupDestination;
    
    
    private void Awake()
    {
        
        var res = FindObjectsByType<imPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        if (res.Length > 0 && startupDestination != null)
        {
            Collider cap = res[0].transform.GetChild(0).GetChild(1).GetComponent<Collider>();
            
            Vector3 feet = new Vector3(cap.bounds.center.x, cap.bounds.center.y - (cap.bounds.size.y/2),
                cap.bounds.center.z);
            Vector3 dest = startupDestination.position - feet;
            res[0].transform.position += dest;
        }
        
    }
}
