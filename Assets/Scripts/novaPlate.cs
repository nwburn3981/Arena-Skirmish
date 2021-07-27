using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class novaPlate : MonoBehaviour
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

        if (reference.CompareTag("PlayerChar"))
        {

            reference.GetComponent<PlayerCharacter>().DestroyNovaPlates();

            reference.GetComponent<PlayerCharacter>().SetHasSpecial(true);
        

            targetReference.GetComponent<NonPlayerCharacter>().health -= 4;

        }//end if

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

    public void SetTargetReference(GameObject obj)
    {
        targetReference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

}
