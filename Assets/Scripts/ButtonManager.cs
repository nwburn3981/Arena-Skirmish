using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
    //x = -308
    //y = 0
{

    //References
    public GameObject processor;
    public GameObject pc;
    public GameObject reference = null;

    //
    private int xPos = 0;
    private int yPos = 0;

    public void OnMouseUp()
    {
        switch (gameObject.tag) { 

            case "moveTurn":
                InitiateMove();
                break;
            case "attackButton":
                InitiateAttack();
                break;
            case "specialButton":
                InitiateSpecial();
                break;
            case "EndTurn":
                EndTurn();
                break;
            }
        }


    //------------------------------------------------------------------------Start Button actions
    public void InitiateAttack() {

        processor = GameObject.FindGameObjectWithTag("GameProcessor");
        pc = reference;
        if (processor.GetComponent<Game>().attackPhase == false)
        {

            processor.GetComponent<Game>().attackPhase = true;

            if (pc.GetComponent<PlayerCharacter>().hasAttacked == false)
            {
                pc.GetComponent<PlayerCharacter>().InitiateAttackPhase();
            }
            
        }//end if

        else if (processor.GetComponent<Game>().attackPhase == true)
        {
            processor.GetComponent<Game>().attackPhase = false;
            reference.GetComponent<PlayerCharacter>().DestroyAttackPlates();
            reference.GetComponent<PlayerCharacter>().DestroyFireballPlates();
        }//end if

    }//end InitiateAttack()

    public void InitiateMove()
    {

        processor = GameObject.FindGameObjectWithTag("GameProcessor");
        pc = reference;
        if (processor.GetComponent<Game>().movePhase == false)
        {
            processor.GetComponent<Game>().movePhase = true;

            if (pc.GetComponent<PlayerCharacter>().hasMoved == false)
            {
                pc.GetComponent<PlayerCharacter>().InitiateMovePlates();
            }

        }//end if

        else if (processor.GetComponent<Game>().movePhase == true)
        {
            processor.GetComponent<Game>().movePhase = false;
            reference.GetComponent<PlayerCharacter>().DestroyMovePlates();
        }//end if
    
    }//end InitiateAttack()

    public void InitiateSpecial()
    {

        processor = GameObject.FindGameObjectWithTag("GameProcessor");
        pc = reference;
        if (processor.GetComponent<Game>().specialPhase == false)
        {

            processor.GetComponent<Game>().specialPhase = true;
            if (pc.GetComponent<PlayerCharacter>().hasSpecial == false)
            {
                pc.GetComponent<PlayerCharacter>().InitiateSpecialPhase();
            }

        }//end if

        else if (processor.GetComponent<Game>().specialPhase == true)
        {
            processor.GetComponent<Game>().specialPhase = false;
            reference.GetComponent<PlayerCharacter>().DestroyHealPlates();
        }//end if

    }//end InitiateSpecial()

    //------------------------------------------------------------------------End Button actions


    //calls NextTurn method on button click
    public void EndTurn() {

        processor = GameObject.FindGameObjectWithTag("GameProcessor");

        DestroyMovePlates();
        processor.GetComponent<Game>().NextTurn();
        reference.GetComponent<PlayerCharacter>().DestroyAllPlates();
    }//end EndTurn()
    
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }

    }//End DestroyMovePlate()


    //----------------------------------------------------Begin setup

    public void SetXPos(int x) {
        xPos = x;
    }

    public void SetYPos(int y) {
        yPos = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }//end SetReference()

    public GameObject GetReference()
    {
        return reference;
    }//end GetReference()

    //----------------------------------------------------End setup

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
           
            InitiateMove();
        }//end if

        if (Input.GetKeyDown(KeyCode.A))
        {
            
            InitiateAttack();
        }//end if

        if (Input.GetKeyDown(KeyCode.S))
        {
            
            InitiateSpecial();
        }//end if
    }
}

