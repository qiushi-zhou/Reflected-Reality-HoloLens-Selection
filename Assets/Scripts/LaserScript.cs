using UnityEngine;
using System.Collections;
using Microsoft.MixedReality.Toolkit.Utilities;
//using MRTKExtensions.Gesture;
public class LaserScript : MonoBehaviour
{
   public LineRenderer laserLineRenderer;
   public float laserWidth = 0.1f;
   public float laserMaxLength = 5f;
   //public SceneController sceneController;
 
   void Start() {
       Vector3[] initLaserPositions = new Vector3[ 2 ] { Vector3.zero, Vector3.zero };
       laserLineRenderer.SetPositions( initLaserPositions );
       //laserLineRenderer.SetWidth( laserWidth, laserWidth );
   }
 
   void Update() 
   {
        laserLineRenderer.enabled = true;
        ShootLaserFromTargetPosition( transform.position, transform.forward, laserMaxLength );
   }
 
   void ShootLaserFromTargetPosition( Vector3 targetPosition, Vector3 direction, float length )
   {
       Ray ray = new Ray( targetPosition, direction );
       RaycastHit raycastHit;
       Vector3 endPosition = targetPosition + ( length * direction );
 
       if( Physics.Raycast( ray, out raycastHit, length ) ) {
           endPosition = raycastHit.point;
           //raycastHit.transform.SendMessage ("HitByRay");
       }
 
       laserLineRenderer.SetPosition( 0, targetPosition );
       laserLineRenderer.SetPosition( 1, endPosition );
   }
 }