using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationUpdater : MonoBehaviour
{

    SharedObject sObj;
    ObjectManipulator objectManipulator;
    public UDPController udpController;
    public GameObject mirrorObj;
    public GameObject flippedObject;

    private bool isInteracted = false;

    private void Awake()
    {
        sObj = this.gameObject.GetComponent<SharedObject>();
        objectManipulator = this.gameObject.GetComponent<ObjectManipulator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        objectManipulator.OnManipulationStarted.AddListener(InteractStarted);
        objectManipulator.OnManipulationEnded.AddListener(InteractEnded);
    }
    private void InteractStarted(ManipulationEventData arg0)
    {
        this.isInteracted = true;
    }
    private void InteractEnded(ManipulationEventData arg0)
    {
        this.isInteracted = false;
    }
}
