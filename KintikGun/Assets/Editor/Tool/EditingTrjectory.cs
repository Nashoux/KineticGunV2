using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockAlreadyMovingV2))]
class EditingTrjectory  : Editor
{

    BlockAlreadyMovingV2 myBlock;
   

   void OnEnable() {
       myBlock = (BlockAlreadyMovingV2)target;
   }
    
    void OnSceneGUI()
    {
        
		Handles.Label(myBlock.transform.position + new Vector3(0,50,0), "start = "+ myBlock.transform.position +  "\n end = " + myBlock.direction + "\n speed = " + myBlock.energie); //to know the speed/position start and end
		//myBlock.transform.position = Handles.FreeMoveHandle( myBlock.transform.position, Quaternion.identity, 5, new Vector3(5,5,5),Handles.RectangleHandleCap ); //to move the start pos
		myBlock.direction =  Handles.FreeMoveHandle( myBlock.direction+myBlock.transform.position, Quaternion.identity, 1, new Vector3(5,5,5),Handles.CubeHandleCap ) - myBlock.transform.position ; //to move the second pos
		myBlock.direction = Vector3.Normalize(myBlock.direction)*4;
		//myBlock.speed = Handles.ScaleValueHandle(myBlock.speed, myBlock.transform.position, Quaternion.identity,myBlock.speed*10, Handles.ArrowCap,1); //to change the speed
              
           
        }
}