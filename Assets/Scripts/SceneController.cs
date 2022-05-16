using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class SceneController : MonoBehaviour
{
    public UDPController udpController;
    public bool studyStarted = false;
    //public GameObject Laser;
    public int pID = 0;
    public GameObject mirrorObj = null;
    public GameObject fittsRing;
    public GameObject fittsRingFake;
    public int[] orderT1;
    public int orderT1Count = 0;
    public int[] orderT2;
    public int orderT2Count = 0;
    public int[] orderT3;
    public int orderT3Count = 0;

    public Vector3 TARGET_LOCATION;
    public float SELECTION_TIME;
    public Vector3 SELECTION_LOCATION;

    // keeps a list of all shared objs between the mirror and hololens
    public Dictionary<int, SharedObject> sharedObjMap;
    
    public int sharedCount = 0;

    float time = 0;

    // Start is called before the first frame update
    void Start(){
        this.sharedObjMap = new Dictionary<int, SharedObject>();
    }

    // allow creation of object only when a mirrorObj has been created here
    // i.e., after we have calibrated the mirror and holoLens
    public void SendCreateMsg(string prefabName, int id, SharedObject obj){
        this.sharedCount++;
        obj.id = this.sharedCount;
        Vector3 pos = this.mirrorObj.transform.InverseTransformPoint(obj.transform.position);
        Vector3 forward = this.mirrorObj.transform.InverseTransformDirection(obj.transform.forward);
        Vector3 upward = this.mirrorObj.transform.InverseTransformDirection(obj.transform.up);
        this.udpController.SendCreateMessage(obj.isBodyAnchored, prefabName, this.sharedCount, pos, forward, upward);
        this.sharedObjMap.Add(obj.id, obj);
    }

    // locks the mirror clone in place
    public void CompleteCalibration(){
        this.mirrorObj.GetComponent<MeshCollider>().enabled = false;
        this.mirrorObj.GetComponent<MeshRenderer>().enabled = false;
        //send Calibration Done message
        this.udpController.SendCalibrationDoneMessage();
    }

    public void startProcedure(){
        startCondition(orderT1Count);
    }

    public void targetSelected (){
        TARGET_LOCATION = fittsRing.GetComponent<FittsRing>().targets[fittsRing.GetComponent<FittsRing>().thisTargetIndex].transform.position;
        SELECTION_LOCATION = TARGET_LOCATION;
        this.udpController.SendDataTransmitMessage();

        if (fittsRing.GetComponent<FittsRing>().targetCount < 11){
            fittsRing.GetComponent<FittsRing>().nextTarget();
            fittsRingFake.GetComponent<FittsRing>().nextTarget();
        }else{
            fittsRing.GetComponent<FittsRing>().targetCount = 0;
            fittsRing.GetComponent<FittsRing>().repCount ++;
        }

        if(fittsRing.GetComponent<FittsRing>().repCount>2){
            fittsRing.GetComponent<FittsRing>().repCount = 0;
            if(orderT1Count < 7){
                startCondition(++orderT1Count); 
            }else{
                fittsRing.gameObject.SetActive(false);
                fittsRingFake.gameObject.SetActive(false);
                // next task
            }
        }
    }
    public void setTaskOrder(){
        switch(pID%8) {
            case 1:
                orderT1 = new int[] { 1,2,8,3,7,4,6,5 };
                break;
            case 2:
                orderT1 = new int[] { 2,3,1,4,8,5,7,6 };
                break;
            case 3:
                orderT1 = new int[] { 3,4,2,5,1,6,8,7 };
                break;
            case 4:
                orderT1 = new int[] { 4,5,3,6,2,7,1,8 };
                break;
            case 5:
                orderT1 = new int[] { 5,6,4,7,3,8,2,1 };
                break;
            case 6:
                orderT1 = new int[] { 6,7,5,8,4,1,3,2 };
                break;
            case 7:
                orderT1 = new int[] { 7,8,6,1,5,2,4,3 };
                break;
            case 0:
                orderT1 = new int[] { 8,1,7,2,6,3,5,4 };
                break;
        }
        switch(pID%2) {
            case 1:
                orderT2 = new int[] { 1,2 };
                break;
            case 0:
                orderT2 = new int[] { 2,1 };
                break;
        }
        switch(pID%4) {
            case 1:
                orderT3 = new int[] { 1,2,4,3 };
                break;
            case 2:
                orderT3 = new int[] { 2,3,1,4 };
                break;
            case 3:
                orderT3 = new int[] { 3,4,2,1 };
                break;
            case 0:
                orderT3 = new int[] { 4,1,3,2 };
                break;
        }
    }
    public void startCondition(int orderT1Count){
        if(orderT1Count<8){
            switch(orderT1[orderT1Count]) {
                case 1:
                    //1pp manual single
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z - 0.5f);
                    fittsRingFake.SetActive(false);
                    //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
                    break;
                case 2:
                    //1pp manual double
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z - 0.5f);
                    fittsRingFake.SetActive(true);
                    fittsRingFake.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z + 0.5f);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
                    break;
                case 3:
                    //1pp remote single
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z - 0.5f);
                    fittsRingFake.SetActive(false);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOn);
                    break;
                case 4:
                    //1pp remote double
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z - 0.5f);
                    fittsRingFake.SetActive(true);
                    fittsRingFake.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z + 0.5f);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOn);
                    break;
                case 5:
                    //2pp manual single
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z + 0.5f);
                    fittsRingFake.SetActive(false);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
                    break;
                case 6:
                    //2pp manual double
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z + 0.5f);
                    fittsRingFake.SetActive(true);
                    fittsRingFake.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z - 0.5f);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
                    break;
                case 7:
                    //2pp remote single
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z + 0.5f);
                    fittsRingFake.SetActive(false);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOn);
                    break;
                case 8:
                    //2pp remote double
                    fittsRing.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z + 0.5f);
                    fittsRingFake.SetActive(true);
                    fittsRingFake.transform.position = new Vector3(
                        mirrorObj.transform.position.x, mirrorObj.transform.position.y, mirrorObj.transform.position.z - 0.5f);
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOn);
                    break;
            }
            //orderT1Count+=1;
        }
        
    }
}
