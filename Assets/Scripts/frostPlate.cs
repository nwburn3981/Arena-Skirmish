using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frostPlate : MonoBehaviour
{
    public GameObject processor;

    GameObject reference = null;

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

        if (reference.GetComponent<PlayerCharacter>().enemyInRange == true)
        {
            reference.GetComponent<PlayerCharacter>().FrostSurroundAttack(matrixX, matrixY);
            reference.GetComponent<PlayerCharacter>().hasSpecial = true;

            reference.GetComponent<PlayerCharacter>().DestroyFrostPlates();
            reference.GetComponent<PlayerCharacter>().DestroyNovaPlates();
            processor.GetComponent<Game>().specialPhase = false;
        }//end if

    }//end onMouseUp()

    public void OnMouseEnter()
    {
        reference.GetComponent<PlayerCharacter>().SurroundNovaCheck(matrixX, matrixY);
    }

    public void OnMouseExit()
    {
        reference.GetComponent<PlayerCharacter>().DestroyNovaPlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }//end SetCoords()

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }//end SetReference()

    public GameObject GetReference()
    {
        return reference;
    }//end GetReference()

}
