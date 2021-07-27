using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;

public class NonPlayerCharacter : MonoBehaviour
{
    //References
    public GameObject processor;
    public GameObject movePlate;

    //public GameObject actionMenu;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //Variable to track which team the NPC is on
    public string teamSide;

    //Variables for actions in turn
    public bool hasMoved;
    public bool hasAttacked;
    public bool hasGone;

    //Variable to determine if character has died
    public bool alive;

    //Hp variables
    public int healthMax;
    public int health;

    //references for sprites for NPCs
    public Sprite elf_Sprite;
    public Sprite santa_Sprite;

    private int[] hArray = new int[3];

    


    //-----------------------------------------------------------------Begin Setup
    public void Activate()
    {
        processor = GameObject.FindGameObjectWithTag("GameProcessor");

        SetCoords();

        switch (this.name)
        {
            case "elfChar": this.GetComponent<SpriteRenderer>().sprite = elf_Sprite; 
                teamSide = "ai";
                hasGone = false;
                hasMoved = false;
                hasAttacked = false;
                alive = true;
                health = 8;
                break;
            case "santaChar": this.GetComponent<SpriteRenderer>().sprite = santa_Sprite; 
                teamSide = "ai";
                hasGone = false;
                hasMoved = false;
                hasAttacked = false;
                alive = true;
                health = 18;
                break;
            
        }//end switch
    }//end Activate

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }//end SetCoords

    public int GetXBoard()
    {
        return xBoard;
    }//end GetXBoard

    public int GetYBoard()
    {
        return yBoard;
    }//end GetYBoard

    public void SetXBoard(int x)
    {
        xBoard = x;
    }//end SetXBoard

    public void SetYBoard(int y)
    {
        yBoard = y;
    }//end SetYBoard

    public bool GetHasMoved()
    {
        return hasMoved;
    }//end GetHasMoved

    public bool GetHasAttacked()
    {
        return hasAttacked;
    }//end GetHasAttacked

    public bool GetHasGone()
    {
        return hasGone;
    }// end HasGone

    public void SetHasMoved(bool inBool)
    {
        hasMoved = inBool;
    }//end SetHasMoved

    public void SetHasAttacked(bool inBool)
    {
        hasAttacked = inBool;
    }//end SetHasAttacked

    public void SetHasGone(bool inBool)
    {
        hasGone = inBool;
    }//end SetHasGone

    //-----------------------------------------------------------------End Setup

    public GameObject FindClosestPC()
    {
        Game pc = processor.GetComponent<Game>();

        int aiX = this.xBoard;
        int aiY = this.yBoard;

        int xDiff;
        int yDiff;
        int hValue;

        for (int i = 0; i < pc.playerTeam.Length; i++) {

            if (pc.playerTeam[i].GetComponent<PlayerCharacter>().alive == true)
            {
                xDiff = aiX - pc.playerTeam[i].GetComponent<PlayerCharacter>().GetXBoard();


                yDiff = aiY - pc.playerTeam[i].GetComponent<PlayerCharacter>().GetYBoard();


                hValue = findPyth(xDiff, yDiff);
                hArray[i] = hValue;
            }//end if

            else {
                hArray[i] = 99;  //if player object is out of game set value to keep at farthest position  
            }
            
        }//end for loop

        int closestH = hArray[0];
        int closestIndex = 0;

        for (int i = 1; i < hArray.Length; i++)
        {

            if (hArray[i] != 0) { 

                if (hArray[i] <= closestH)
                {
                    closestH = hArray[i];
                    closestIndex = i;
                }//end if
            }//end if
        }//end for loop

        return pc.playerTeam[closestIndex];

    }//end FindClosestPC

    public int findPyth(int x, int y) {
        int h;
        int h2;
        int x2;
        int y2;

        x2 = x * x;
        y2 = y * y;
        h2 = x2 + y2;

        h = Convert.ToInt32(Math.Sqrt(h2));

        return h;
    }

    public void ApproachClosestPC(GameObject target)
    {
        
        int targetX = target.GetComponent<PlayerCharacter>().GetXBoard();
        int targetY = target.GetComponent<PlayerCharacter>().GetYBoard();

        Game pc = processor.GetComponent<Game>();

        int aiX = this.xBoard;
        int aiY = this.yBoard;

        int xDiff = Math.Abs(targetX - aiX);
        int yDiff = Math.Abs(targetY - aiY);

        int h = findPyth(xDiff, yDiff);
        pc.SetPositionEmpty(this.xBoard, this.yBoard);

        if ((xDiff > 6 && yDiff <= 6) || (xDiff <= 6 && yDiff > 6))
        {//master
            if (xDiff > 6)
            {//1
                if (targetX > aiX)
                {//2a
                    if (pc.PositionOnBoard(this.xBoard + 6, targetY) && pc.GetPosition(this.xBoard + 6, targetY) == null)
                    {//3a
                        this.xBoard += 6;
                        this.yBoard = targetY;
                    }//end if 3a
                    else
                    {//3a
                        BlackListFixXOR(this.xBoard + 6, targetY, aiX, aiY, xDiff, yDiff);
                        //call function for how to find a spot when x,y is blacklist
                    }//end else 3a
                }//end if 2a
                else
                {//2a
                    if (pc.PositionOnBoard(this.xBoard - 6, targetY) && pc.GetPosition(this.xBoard - 6, targetY) == null)
                    {//3b
                        this.xBoard -= 6;
                        this.yBoard = targetY;
                    }//end if 3b
                    else
                    {//3b 
                        BlackListFixXOR(this.xBoard - 6, targetY, aiX, aiY, xDiff, yDiff);
                        //call function for how to find a spot when x,y is blacklist
                    }//end else 3b
                }//end else 2a
            }//end if 1
            else
            {//1
                if (targetY > aiY)
                {//2b
                    if (pc.PositionOnBoard(targetX, this.yBoard + 6) && pc.GetPosition(targetX, this.yBoard + 6) == null)
                    {//3a
                        this.yBoard += 6;
                        this.xBoard = targetX;
                    }//end if 3a
                    else
                    {//3a
                        BlackListFixXOR(targetX, this.yBoard + 6, aiX, aiY, xDiff, yDiff);
                        //call function for how to find a spot when x,y is blacklist
                    }//end else 3a
                }//end if 2b
                else
                {//2b
                    if (pc.PositionOnBoard(targetX, this.yBoard - 6) && pc.GetPosition(targetX, this.yBoard - 6) == null)
                    {//3b
                        this.yBoard -= 6;
                        this.xBoard = targetX;
                    }//end if 3b
                    else
                    {//3b 
                        BlackListFixXOR(targetX, this.yBoard - 6, aiX, aiY, xDiff, yDiff);
                        //call function for how to find a spot when x,y is blacklist
                    }//end else 3b
                }//end else 2b
            }//end else 1
        }//Master if XOR GATE

        else if (xDiff > 6 && yDiff > 6)
        {
            if (targetX > aiX && targetY > aiY)
            {//if up and to right
                if (pc.PositionOnBoard(this.xBoard + 6, this.yBoard + 6) && pc.GetPosition(this.xBoard + 6, this.yBoard + 6) == null)
                {
                    this.xBoard += 6;
                    this.yBoard += 6;
                }
                else
                {
                    BlackListFixAND(this.xBoard + 6, this.yBoard + 6, aiX, aiY);
                }
            }
            else if (targetX < aiX && targetY < aiY)
            {//if down and to left
                if (pc.PositionOnBoard(this.xBoard - 6, this.yBoard - 6) && pc.GetPosition(this.xBoard - 6, this.yBoard - 6) == null)
                {
                    this.xBoard -= 6;
                    this.yBoard -= 6;
                }
                else
                {
                    BlackListFixAND(this.xBoard - 6, this.yBoard - 6, aiX, aiY);
                }
            }
            else if (targetX > aiX && targetY < aiY)
            {//if down and to right
                if (pc.PositionOnBoard(this.xBoard + 6, this.yBoard - 6) && pc.GetPosition(this.xBoard + 6, this.yBoard - 6) == null)
                {
                    this.xBoard += 6;
                    this.yBoard -= 6;
                }
                else
                {
                    BlackListFixAND(this.xBoard + 6, this.yBoard - 6, aiX, aiY);
                }
            }
            else
            {//if up and to left
                if (pc.PositionOnBoard(this.xBoard - 6, this.yBoard + 6) && pc.GetPosition(this.xBoard - 6, this.yBoard + 6) == null)
                {
                    this.xBoard -= 6;
                    this.yBoard += 6;
                }
                else
                {
                    BlackListFixAND(this.xBoard - 6, this.yBoard + 6, aiX, aiY);
                }
            }
        }//end master else if AND GATE
        else
        {//if(xDiff<=6 && yDiff<=6) NOR GATE
            CloseGap(FindClosestPC(), xDiff, yDiff);
        }//end master else
  
        
        pc.SetPositionEmpty(this.xBoard, this.yBoard);
        pc.SetAIPosition(this.gameObject);
        SetCoords();
        this.hasMoved = true;
       
    }//end approach

    public void CloseGap(GameObject target, int xDiff, int yDiff) {

        int targetX = target.GetComponent<PlayerCharacter>().GetXBoard();
        int targetY = target.GetComponent<PlayerCharacter>().GetYBoard();

        Game pc = processor.GetComponent<Game>();

        if (pc.PositionOnBoard(targetX + 1, targetY) == true && pc.GetPosition(targetX + 1, targetY) == null) { //+.0
            this.xBoard = targetX + 1;
            this.yBoard = targetY;
            this.hasMoved = true;
           
        }

        else if (pc.PositionOnBoard(targetX + 1, targetY + 1) == true && pc.GetPosition(targetX + 1, targetY + 1) == null)
        {//+.+
            this.xBoard = targetX + 1;
            this.yBoard = targetY + 1;
            this.hasMoved = true;
            
        }

        else if (pc.PositionOnBoard(targetX, targetY + 1) == true && pc.GetPosition(targetX, targetY + 1) == null)
        { //0.+
            this.xBoard = targetX;
            this.yBoard = targetY + 1;
            this.hasMoved = true;
            
        }

        else if (pc.PositionOnBoard(targetX - 1, targetY - 1) == true && pc.GetPosition(targetX - 1, targetY - 1) == null)
        { //-.-
            this.xBoard = targetX - 1;
            this.yBoard = targetY - 1;
            this.hasMoved = true;
            
        }

        else if (pc.PositionOnBoard(targetX - 1, targetY) == true && pc.GetPosition(targetX - 1, targetY) == null)
        { //-.0
            this.xBoard = targetX - 1;
            this.yBoard = targetY;
            this.hasMoved = true;
            
        }

        else if (pc.PositionOnBoard(targetX, targetY - 1) == true && pc.GetPosition(targetX, targetY - 1) == null)
        { //0.-
            this.xBoard = targetX;
            this.yBoard = targetY - 1;
            this.hasMoved = true;
            
        }

        else if (pc.PositionOnBoard(targetX + 1, targetY - 1) == true && pc.GetPosition(targetX + 1, targetY - 1) == null)
        { //+.-
            this.xBoard = targetX + 1;
            this.yBoard = targetY - 1;
            this.hasMoved = true;
            
        }

        else if (pc.PositionOnBoard(targetX - 1, targetY + 1) == true && pc.GetPosition(targetX - 1, targetY + 1) == null)
        { //-.+
            this.xBoard = targetX - 1;
            this.yBoard = targetY + 1;
            this.hasMoved = true;
            
        }

    }//end CloseGap

    public void BlackListFixXOR(int targetX, int targetY, int aiX, int aiY, int xDiff, int yDiff) {

        Game pc = processor.GetComponent<Game>();

        if (xDiff > 6) {//10
            if (targetX > aiX) {//moving right
                //xBoard + 6, yBoard = targety 
                this.yBoard = targetY;

                for (int i = 0; targetX - i >= this.xBoard; i++) {
                    if (pc.PositionOnBoard(targetX - i, targetY) == true && pc.GetPosition(targetX - i, targetY) == null) {
                        this.xBoard = targetX - i;
                        this.hasMoved = true;
                        break;
                    }
                }//end for

            }


            else { // targetX < aiX , moving left
                   //xBoard - 6, yBoard = targety 
                this.yBoard = targetY;

                for (int i = 0; targetX + i >= this.xBoard; i++)
                {
                    if (pc.PositionOnBoard(targetX + i, targetY) == true && pc.GetPosition(targetX + i, targetY) == null)
                    {
                        this.xBoard = targetX + i;
                        this.hasMoved = true;
                        break;
                    }
                }//end for
            }

        }


        else if (yDiff > 6) { //01
            if (targetY > aiY)//moving up
            {
                //xBoard = targetx, yBoard + 6 

                this.xBoard = targetX;
                this.yBoard = targetY;
                for (int i = 5; i >= 1; i--)
                {
                    if (targetX > aiX)
                    {
                        if (pc.PositionOnBoard(aiX + i, targetY) == true && pc.GetPosition(aiX + i, targetY) == null)
                        {
                            this.xBoard = aiX + i;
                            this.hasMoved = true;
                            break;
                        }
                    }

                    else
                    { //targetX < aiX
                        if (pc.PositionOnBoard(aiX - i, targetY) == true && pc.GetPosition(aiX - i, targetY) == null)
                        {
                            this.xBoard = aiX - i;
                            break;
                        }
                    }

                }//end for
            }


            else//moving down
            { // targetY < aiY
                //xBoard = targetx, yBoard - 6 

                this.xBoard = targetX;
                this.yBoard = targetY;
                for (int i = 5; i >= 1; i--) {
                    if (targetX > aiX)
                    {
                        if (pc.PositionOnBoard(aiX + i, targetY) == true && pc.GetPosition(aiX + i, targetY) == null) {
                            this.xBoard = aiX + i;
                            this.hasMoved = true;
                            break;
                        }
                    }

                    else { //targetX < aiX
                        if (pc.PositionOnBoard(aiX - i, targetY) == true && pc.GetPosition(aiX - i, targetY) == null) {
                            this.xBoard = aiX - i;
                            this.hasMoved = true;
                            break;
                        }
                    }

                }//end for
            }

        }


    }

    public void BlackListFixAND(int targetX, int targetY, int aiX, int aiY) {

        Game pc = processor.GetComponent<Game>();

        if (targetX > aiX && targetY > aiY) { //11

            for (int i = 1; targetX - i >= this.xBoard; i++)
            {
                if (pc.PositionOnBoard(targetX - i, targetY) && pc.GetPosition(targetX - i, targetY) == null)
                {
                    this.xBoard = targetX - i;
                    this.yBoard = targetY;
                }
            }

        }//11

        else if (targetX > aiX && targetY < aiY) { //10
            for (int i = 1; targetX - i >= this.xBoard; i++) {
                if (pc.PositionOnBoard(targetX - i, targetY) && pc.GetPosition(targetX - i, targetY) == null)
                {
                    this.xBoard = targetX - i;
                    this.yBoard = targetY;
                }
            }

        }//10
        else if (targetX < aiX && targetY > aiY) { //01
            for (int i = 5; targetX + i >= this.xBoard; i--)
            {
                if (pc.PositionOnBoard(targetX + i, targetY) && pc.GetPosition(targetX + i, targetY) == null)
                {
                    this.xBoard = targetX + i;
                    this.yBoard = targetY;
                }
            }
        }//01

        else { //00
            for (int i = 5; targetX + i >= this.xBoard; i--)
            {
                if (pc.PositionOnBoard(targetX + i, targetY) && pc.GetPosition(targetX + i, targetY) == null)
                {
                    this.xBoard = targetX + i;
                    this.yBoard = targetY;
                }
            }
        }//00

    }//end BlackListFixAND

    public void InitiateNPC(GameObject obj)
    {

        if (this.hasAttacked == false) { 
        AttackRangeCheck(this.xBoard, this.yBoard);
        }

        if (this.hasMoved == false && this.hasAttacked == false) {
            GameObject target = FindClosestPC();
            ApproachClosestPC(target);
        }//end if

        if (this.hasAttacked == false)
        {
            AttackRangeCheck(this.xBoard, this.yBoard);
        }

    }//end InitiateNPC()

    public void AttackRangeCheck(int x, int y) {

        AttackPointCheck(x, y + 1);
        AttackPointCheck(x, y - 1);
        AttackPointCheck(x + 1, y + 1);
        AttackPointCheck(x - 1, y - 1);
        AttackPointCheck(x + 1, y);
        AttackPointCheck(x - 1, y);
        AttackPointCheck(x - 1, y + 1);
        AttackPointCheck(x + 1, y - 1);

    }//end Attack Range Check

    public void AttackPointCheck(int x, int y) {
        Game checker = processor.GetComponent<Game>();

        if (checker.PositionOnBoard(x, y))
        {
            if (checker.positions[x, y] != null)
            {

                if (checker.positions[x, y].name == "mageChar" || checker.positions[x, y].name == "clericChar" || checker.positions[x, y].name == "fighterChar")
                {

                    
                    GameObject point = processor.GetComponent<Game>().GetPosition(x, y);
                    //Attack here
                    //this.hasAttacked = true;
                    

                    if (this.hasAttacked == false && point.GetComponent<PlayerCharacter>().alive == true) {
                        point.GetComponent<PlayerCharacter>().health -= 3;
                        this.hasAttacked = true;
                    }//end if


                }

            }

        }
    }//end AttackPointCheck


    //-----------------------------------------------------------------Begin Move section

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "elfChar":
                SurroundMovePlate(xBoard, yBoard);
                break;
            case "santaChar":
                SantaMovePlate(xBoard, yBoard);
                break;
        }//end switch
    }//end InititateMovePlates()

    public void SurroundMovePlate(int x, int y)
    {
        PointMovePlate(x, y);
        PointMovePlate(x, y + 1);
        PointMovePlate(x, y - 1);
        PointMovePlate(x + 1, y + 1);
        PointMovePlate(x - 1, y - 1);
        PointMovePlate(x + 1, y);
        PointMovePlate(x - 1, y);
        PointMovePlate(x - 1, y + 1);
        PointMovePlate(x + 1, y - 1);

    }//end SMP

    public void SantaMovePlate(int x, int y)
    {

        LineMovePlate(1, 0);
        LineMovePlate(0, 1);
        LineMovePlate(-1, 0);
        LineMovePlate(0, -1);
        SurroundMovePlate(xBoard + 2, yBoard + 2);
        SurroundMovePlate(xBoard + 5, yBoard + 5);
        SurroundMovePlate(xBoard - 2, yBoard + 2);
        SurroundMovePlate(xBoard - 5, yBoard + 5);
        SurroundMovePlate(xBoard + 2, yBoard - 2);
        SurroundMovePlate(xBoard + 5, yBoard - 5);
        SurroundMovePlate(xBoard - 2, yBoard - 2);
        SurroundMovePlate(xBoard - 5, yBoard - 5);
        SurroundMovePlate(xBoard + 2, yBoard + 5);
        SurroundMovePlate(xBoard + 5, yBoard + 2);
        SurroundMovePlate(xBoard - 2, yBoard + 5);
        SurroundMovePlate(xBoard - 5, yBoard + 2);
        SurroundMovePlate(xBoard + 2, yBoard - 5);
        SurroundMovePlate(xBoard + 5, yBoard - 2);
        SurroundMovePlate(xBoard - 2, yBoard - 5);
        SurroundMovePlate(xBoard - 5, yBoard - 2);


    }//end FightMovePlate

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = processor.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (x <= xBoard + 6 && y <= yBoard + 6 && x >= xBoard - 6 && y >= yBoard - 6)
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
                x += xIncrement;
                y += yIncrement;
            }//end if
            else
            {
                x += xIncrement;
                y += yIncrement;
            }//end else
        }//end while 
    }//end LineMP

    //takes positions from InitiateMovePlates and checks if that position is empty and valid
    public void PointMovePlate(int x, int y)
    {
        Game sc = processor.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject pc = sc.GetPosition(x, y);

            if (pc == null)
            {
                MovePlateSpawn(x, y);
            }//end if
        }//end if
    }//end PointMovePlate()

    //creates the actual move plates
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }//end MovePlateSpawn()

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }

    }//End DestroyMovePlate


    //-----------------------------------------------------------------End Move section


}//end NonPlayerCharacter
