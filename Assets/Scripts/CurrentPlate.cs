using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlate : MonoBehaviour {

    private GameObject currentPlate;

    GameObject reference = null;

    //Board positions not world positions
    int matrixX;
    int matrixY;
    // Start is called before the first frame update
    void Start()
    {
        
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
