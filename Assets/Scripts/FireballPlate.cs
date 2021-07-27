using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPlate : MonoBehaviour
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
            reference.GetComponent<PlayerCharacter>().SurroundFireAttack(matrixX, matrixY);
            reference.GetComponent<PlayerCharacter>().hasAttacked = true;

            reference.GetComponent<PlayerCharacter>().DestroyAttackPlates();
            reference.GetComponent<PlayerCharacter>().DestroyFireballPlates();
            processor.GetComponent<Game>().attackPhase = false;
        }//end if

    }//end onMouseUp()

    public void OnMouseEnter()
    {
        reference.GetComponent<PlayerCharacter>().SurroundAttackCheck(matrixX, matrixY);
    }

    public void OnMouseExit()
    {
        reference.GetComponent<PlayerCharacter>().DestroyAttackPlates();
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
