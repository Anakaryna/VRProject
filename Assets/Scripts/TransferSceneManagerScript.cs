using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferSceneManagerScript : MonoBehaviour
{

    public static TransferSceneManagerScript Instance;
    
    public static GameObject InstanceGameObject;

    public static GameObject savedPhysicsRig;

    public static Collider savedPlayerCapsule;
    
    public GameObject physicsRig;

    public Collider playerCapsule;

    public Transform startupDestination;

    public GameObject XRSimulator;

    private bool rigNeedSaving = false;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(InstanceGameObject);
        }
        else
        {
            InstanceGameObject = gameObject;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (savedPhysicsRig == null && physicsRig != null)
        {
            rigNeedSaving = true;
            savedPhysicsRig = physicsRig;
        }
        
        if (savedPlayerCapsule == null && playerCapsule != null)
        {
            savedPlayerCapsule = playerCapsule;
        }
        
    }

    private void Start()
    {
        if (rigNeedSaving)
        {
            DontDestroyOnLoad(savedPhysicsRig);
        }
        
        if (XRSimulator != null)
        {
            DontDestroyOnLoad(XRSimulator);
        }
        
        print(startupDestination);
        
        if (startupDestination != null)
        {
            Vector3 feet = new Vector3(savedPlayerCapsule.bounds.center.x, savedPlayerCapsule.bounds.center.y - (savedPlayerCapsule.bounds.size.y/2),
                savedPlayerCapsule.bounds.center.z);
            Vector3 dest = startupDestination.position - feet;
            savedPhysicsRig.transform.position += dest;
        }
        
    }
}
