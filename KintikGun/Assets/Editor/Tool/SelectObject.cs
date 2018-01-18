using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SelectObject : ScriptableWizard 
{

    public string searchTag = "MyTag";
    public myComponent searchByComponent = myComponent.AlreadyMove;

    public enum myComponent { AlreadyMove, none  };

    [MenuItem ("My Tools/Select All objects...")]
    static void SelectAllOfTagWizard() {
        ScriptableWizard.DisplayWizard<SelectObject> ("Select All objects...", "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject[] gameObjectsTag = GameObject.FindGameObjectsWithTag (searchTag);

      //  List<GameObject> objectName = new List<GameObject>(); //not functinnal
        switch (searchByComponent){
            case (myComponent.AlreadyMove) :        
        break;
        }


        Selection.objects = gameObjectsTag;
    }
}