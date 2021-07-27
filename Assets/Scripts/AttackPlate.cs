using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlate : MonoBehaviour
{

    public GameObject processor;

    GameObject reference = null;
    GameObject targetReference = null;

    //Board positions not world positions
    int matrixX;
    int matrixY;

    private void Start()
    {
        processor = GameObject.FindGameObjectWithTag("GameProcessor");
    }

    public void OnMouseUp()
    {
        processor = GameObject.FindGameObjectWithTag("GameProcessor");

       
            reference.GetComponent<PlayerCharacter>().DestroyAttackPlates();

            if (reference.GetComponent<Game>().attackPhase == true)
            {
                processor.GetComponent<Game>().attackPhase = false;
                reference.GetComponent<PlayerCharacter>().SetHasAttacked(true);
                targetReference.GetComponent<NonPlayerCharacter>().health -= 4;
            }

            else if (processor.GetComponent<Game>().specialPhase == true)
            {
                processor.GetComponent<Game>().specialPhase = false;
                reference.GetComponent<PlayerCharacter>().SetHasSpecial(true);
            }

            
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public void SetTargetReference(GameObject obj) {
        targetReference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
   
}
