using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferSceneManagerScript : MonoBehaviour
{

    public static TransferSceneManagerScript Instance;
    
    public static GameObject InstanceGameObject;

    public static GameObject savedPhysicsRig;

    public static Collider savedPlayerCapsule;
    
    public GameObject physicsRig;

    public Collider playerCapsule;

    public Transform startupDestination;

    public float playerScale = 1;

    private bool rigNeedSaving = false;
    
    
    private void Awake()
    {
        
        /*DontDestroyOnLoad(gameObject);
        Instance = this;
        Destroy(InstanceGameObject);
        InstanceGameObject = gameObject;*/
        
        var res = FindObjectsByType<imPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        if (res.Length == 0)
        {
            var o = Instantiate(physicsRig, null, startupDestination);
            savedPhysicsRig = o.gameObject;
            savedPlayerCapsule = o.transform.GetChild(0).transform.GetChild(1).GetComponent<Collider>();
            DontDestroyOnLoad(o);
        }
        else
        {
            savedPhysicsRig.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        
            if (startupDestination != null)
            {
                Vector3 feet = new Vector3(savedPlayerCapsule.bounds.center.x, savedPlayerCapsule.bounds.center.y - (savedPlayerCapsule.bounds.size.y/2),
                    savedPlayerCapsule.bounds.center.z);
                Vector3 dest = startupDestination.position - feet;
                savedPhysicsRig.transform.position += dest;
            }
        }
        
    }

    void Start()
    {
        
    }
}
