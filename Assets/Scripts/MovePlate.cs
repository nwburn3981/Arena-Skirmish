using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
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

        if (reference.CompareTag("PlayerChar")) { 

            processor.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<PlayerCharacter>().GetXBoard(),
            reference.GetComponent<PlayerCharacter>().GetYBoard());

            reference.GetComponent<PlayerCharacter>().SetXBoard(matrixX);
            reference.GetComponent<PlayerCharacter>().SetYBoard(matrixY);
            reference.GetComponent<PlayerCharacter>().SetCoords();

            processor.GetComponent<Game>().SetPlayerPosition(reference);

            reference.GetComponent<PlayerCharacter>().DestroyMovePlates();
            reference.GetComponent<PlayerCharacter>().SetHasMoved(true);
            reference.GetComponent<PlayerCharacter>().DestroyCurrentPlate();
            reference.GetComponent<PlayerCharacter>().CurrentPlateSpawn(matrixX, matrixY);

        }//end if

        if (reference.CompareTag("AI"))
        {

            processor.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<NonPlayerCharacter>().GetXBoard(),
                reference.GetComponent<NonPlayerCharacter>().GetYBoard());

            reference.GetComponent<NonPlayerCharacter>().SetXBoard(matrixX);
            reference.GetComponent<NonPlayerCharacter>().SetYBoard(matrixY);
            reference.GetComponent<NonPlayerCharacter>().SetCoords();

            processor.GetComponent<Game>().SetAIPosition(reference);

            reference.GetComponent<NonPlayerCharacter>().DestroyMovePlates();
            reference.GetComponent<NonPlayerCharacter>().SetHasMoved(true);

        }//end if

    }//end onMouseUp()

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
