using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
public class FittsRing : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    public int targetCount = 0;
    public int repCount = 0;
    private float armLength = 0.2f;
    public int thisTargetIndex = 0;
    void Start(){
        for (int i = 0; i < targets.Count; i++){
            targets[i].Translate(armLength * Mathf.Sin(32.72727272727273f * i * Mathf.Deg2Rad) * -1, armLength * Mathf.Cos(32.72727272727273f * i * Mathf.Deg2Rad), 0);
        }
        if(targets[0].gameObject.GetComponent<Interactable>() != null){
            targets[0].gameObject.GetComponent<Interactable>().enabled = true;
        targets[0].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
    }

    public void nextTarget(){
        targetCount ++;
        targets[thisTargetIndex].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        if (targets[thisTargetIndex].gameObject.GetComponent<Interactable>() != null){
            targets[thisTargetIndex].gameObject.GetComponent<Interactable>().enabled = false;
        }
        
        thisTargetIndex = (thisTargetIndex + 6) % 11;
        targets[thisTargetIndex].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        if (targets[thisTargetIndex].gameObject.GetComponent<Interactable>() != null){
            targets[thisTargetIndex].gameObject.GetComponent<Interactable>().enabled = true;
        }
        if(targetCount == 11){
            ++repCount;
            targetCount = 0;
        }
    }
}
