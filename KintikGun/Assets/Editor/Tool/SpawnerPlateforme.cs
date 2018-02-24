using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpawnerPlateforme : ScriptableWizard 
{

    public Mesh plateformeMesh;
    public Material myMat;
    public string objectName = "Plateform";

	public Vector3 direction;

	public float energie = 50;

	public Vector3 size = new Vector3 (50,50,50);

	public string parentObject = "Islands";

	//public Vector3 baseRotation;
	public Vector3 basePosition;

    [MenuItem ("My Tools/CreatePlateform")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SpawnerPlateforme> ("Create Plateform", "Create new");
    }

    void OnWizardCreate()  {
        
        //spawn my plateform and change the transform
        GameObject myPlateform = new GameObject (); 
		myPlateform.name = objectName;		
		//myPlateform.transform.localRotation = Quaternion.EulerAngles(baseRotation);
		myPlateform.transform.position = basePosition;
		myPlateform.transform.localScale = size;
		if(GameObject.Find(parentObject)){
			myPlateform.transform.parent = GameObject.Find(parentObject).transform; 
		}
		

        //change the component to move the object
        BlockAlreadyMovingV2 plateformComponent = myPlateform.AddComponent<BlockAlreadyMovingV2> (); 
		plateformComponent.direction = direction;
		plateformComponent.energie = energie;


        //add some more graphic 
		myPlateform.AddComponent<MeshRenderer>().material = myMat;
		myPlateform.AddComponent<MeshFilter>().mesh = plateformeMesh;

		myPlateform.AddComponent<MeshCollider>().convex = true;		
       
    }

    void OnWizardOtherButton(){ //to update a target 
        if (Selection.activeTransform != null){
            BlockAlreadyMovingV2 plateformComponent = Selection.activeTransform.GetComponent<BlockAlreadyMovingV2>();

            if (plateformComponent != null)
            {
                
            }
        }
    }

   // void OnWizardUpdate()
    //{
      //  helpString = "Enter character details";
   // }

}