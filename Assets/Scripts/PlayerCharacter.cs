using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    //References
    public GameObject processor;
    public GameObject textUI;
    public GameObject buttonUI;
    public GameObject movePlate;
    public GameObject healPlate;
    public GameObject currentPlate;
    public GameObject attackPlate;
    public PlayerCharacter playerReference;
    public GameObject fireballPlate;
    public GameObject endTurnButton;
    public GameObject attackButton;
    public GameObject moveButton;
    public GameObject specialButton;
    public GameObject FrostPlate;
    public GameObject NovaPlate;
    
    //public GameObject actionMenu;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //Variables to track whether a character belongs to the player or AI and current character selected
    public string teamSide;
    public string currentChar;

    //Variables for actions in turn
    public bool hasMoved;
    public bool hasAttacked;
    public bool hasGone;
    public bool hasSpecial;

    //Variable to determine if character has died
    public bool alive;

    //Hp Variables
    public int health;
    public int healthMax;

    //Range Check Variables
    public bool enemyInRange;
    public bool allyInRange;


    //references for sprites for PCs
    public Sprite fighter_Sprite;
    public Sprite cleric_Sprite;
    public Sprite mage_Sprite;


    //------------------------------------------------------------------------------------Begin setup

    public void Activate() {

        processor = GameObject.FindGameObjectWithTag("GameProcessor");

        SetCoords();

        switch (this.name) {
            case "fighterChar": 
                this.GetComponent<SpriteRenderer>().sprite = fighter_Sprite; 
                teamSide = "player"; 
                hasGone = false;
                hasMoved = false;
                hasAttacked = false;
                hasSpecial = false;
                enemyInRange = false;
                alive = true;
                healthMax = 12;
                health = 12;
                break;
            case "clericChar": 
                this.GetComponent<SpriteRenderer>().sprite = cleric_Sprite; 
                teamSide = "player"; 
                hasGone = false;
                hasMoved = false;
                hasAttacked = false;
                hasSpecial = false;
                alive = true;
                healthMax = 10;
                health = 10;
                break;
            case "mageChar": 
                this.GetComponent<SpriteRenderer>().sprite = mage_Sprite; 
                teamSide = "player"; 
                hasGone = false;
                hasMoved = false;
                hasAttacked = false;
                hasSpecial = false;
                alive = true;
                healthMax = 6;
                health = 6;
                break;
        }//end switch
    }//end Activate()

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        this.transform.position = new Vector3(x, y, -2.0f);
    }//end SetCoords()

    public int GetXBoard()
    {
        return xBoard;
    }//end GetXBoard()

    public int GetYBoard()
    {
        return yBoard;
    }//end GetYBoard()

    public void SetXBoard(int x)
    {
        xBoard = x;
    }//end SetXBoard()

    public void SetYBoard(int y)
    {
        yBoard = y;
    }//end SetYBoard()

    public bool GetHasMoved()
    {
        return hasMoved;
    }//end GetHasMoved()

    public bool GetHasAttacked()
    {
        return hasAttacked;
    }//end GetHasAttacked()

    public bool GetHasGone()
    {
        return hasGone;
    }// end GetHasGone()

    public bool GetHasSpecial()
    {
        return hasSpecial;
    }// end GetHasSpecial()

    public void SetHasMoved(bool inBool)
    {
         hasMoved = inBool;
    }//end SetHasMoved()

    public void SetHasAttacked(bool inBool)
    {
        hasAttacked = inBool;
    }//end SetHasAttacked()

    public void SetHasGone(bool inBool)
    {
        hasGone = inBool;
    }//end SetHasGone()

    public void SetHasSpecial(bool inBool)
    {
        hasSpecial = inBool;
    }//end SetHasSpecial()

    public int GetHealth()
    {
        return health;
    }//end GetYBoard()

    //--------------------------------------------------------------------------end setup


    //Character selected ------Move button not spawning correctly bug
    public void OnMouseUp()
    {
        if (alive) {

            DestroyMovePlates();
            DestroyCurrentPlate();
            DestroyAttackPlates();
            DestroyFireballPlates();
            DestroyHealPlates();
            currentChar = this.name;
            processor.GetComponent<Game>().movePhase = false;
            processor.GetComponent<Game>().attackPhase = false;
            processor.GetComponent<Game>().specialPhase = false;


            if (!processor.GetComponent<Game>().IsGameOver() 
                && processor.GetComponent<Game>().GetCurrentTeam() == teamSide) {

                CurrentPlateSpawn(this.xBoard, this.yBoard);
                DestroyButtons();
                endTurnButtonSpawn();
                hpBarSpawn();

                if (this.hasMoved == false) 
                {
                    moveButtonSpawn();                 
                }//end if

                if (this.hasAttacked == false) 
                {
                    attackButtonSpawn();
                }//end if

                if (this.hasSpecial == false)
                {                 
                    specialButtonSpawn();
                }//end if

            }//end if
           
        }//end if

    }//end OnMouseUp()

    //Spawns plate under current character
    public void CurrentPlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject cp = Instantiate(currentPlate, new Vector3(x, y, -1.0f), Quaternion.identity);

        CurrentPlate cpScript = cp.GetComponent<CurrentPlate>();
        cpScript.SetReference(gameObject);
        cpScript.SetCoords(matrixX, matrixY);
    }//end CurrentPlateSpawn()

    public void DestroyCurrentPlate()
    {
        GameObject currentPlate = GameObject.FindGameObjectWithTag("CurrentPlate");
        Destroy(currentPlate);

    }//End DestroyCurrentPlate


    //---------------------------------------------------------------------------Begin Move section

    //takes in moveRange to generate move plates positions
    public void InitiateMovePlates()
    {
    

        switch (this.name)
        {
            case "fighterChar":
                FighterMovePlate(xBoard, yBoard);
                break;
            case "mageChar":
                SurroundMovePlate(xBoard, yBoard);
                break;
            case "clericChar":
                FighterMovePlate(xBoard, yBoard);
                break;
        }//end switch


    }//end InitiateMovePlates()

    //Generates Fighter move grid
    public void FighterMovePlate(int x, int y)
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

    //Generates move plates in a line each direction from passed in position
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

    //Spawns move plates in a square around the passed in position
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

    //Checks if position is legal and spawns moveplate
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
        }//end for

    }//End DestroyMovePlate

    //---------------------------------------------------------------------------End Move section


    //---------------------------------------------------------------------------Begin Attack/Fireball section

    public void InitiateAttackPhase()
    {


        switch (this.name)
        {
            case "fighterChar":
                SurroundAttackCheck(xBoard, yBoard);
                break;
            case "mageChar":
                FireballRangePlate(xBoard, yBoard);
                break;
            case "clericChar":
                SurroundAttackCheck(xBoard, yBoard);
                break;
        }//end switch


    }//end InitiateMovePlates()

    public void FireballRangePlate(int x, int y)
    {
        LineFirePlate(1, 0);
        LineFirePlate(0, 1);
        LineFirePlate(-1, 0);
        LineFirePlate(0, -1);
        SurroundFirePlate(xBoard + 2, yBoard + 2);
        SurroundFirePlate(xBoard + 5, yBoard + 5);
        SurroundFirePlate(xBoard - 2, yBoard + 2);
        SurroundFirePlate(xBoard - 5, yBoard + 5);
        SurroundFirePlate(xBoard + 2, yBoard - 2);
        SurroundFirePlate(xBoard + 5, yBoard - 5);
        SurroundFirePlate(xBoard - 2, yBoard - 2);
        SurroundFirePlate(xBoard - 5, yBoard - 5);
        SurroundFirePlate(xBoard + 2, yBoard + 5);
        SurroundFirePlate(xBoard + 5, yBoard + 2);
        SurroundFirePlate(xBoard - 2, yBoard + 5);
        SurroundFirePlate(xBoard - 5, yBoard + 2);
        SurroundFirePlate(xBoard + 2, yBoard - 5);
        SurroundFirePlate(xBoard + 5, yBoard - 2);
        SurroundFirePlate(xBoard - 2, yBoard - 5);
        SurroundFirePlate(xBoard - 5, yBoard - 2);
    }

    public void LineFirePlate(int xIncrement, int yIncrement)
    {

        Game sc = processor.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (x <= xBoard + 6 && y <= yBoard + 6 && x >= xBoard - 6 && y >= yBoard - 6)
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
            {
                FirePlateSpawn(x, y);
                x += xIncrement;
                y += yIncrement;
            }//end if
            else
            {
                x += xIncrement;
                y += yIncrement;
            }//end else
        }//end while 

    }

    public void SurroundFirePlate(int x, int y)
    {
        PointFirePlate(x, y);
        PointFirePlate(x, y + 1);
        PointFirePlate(x, y - 1);
        PointFirePlate(x + 1, y + 1);
        PointFirePlate(x - 1, y - 1);
        PointFirePlate(x + 1, y);
        PointFirePlate(x - 1, y);
        PointFirePlate(x - 1, y + 1);
        PointFirePlate(x + 1, y - 1);

    }//end SMP

    public void PointFirePlate(int x, int y)
    {
        Game sc = processor.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject pc = sc.GetPosition(x, y);

            if (pc == null)
            {
                FirePlateSpawn(x, y);
            }//end if
        }//end if
    }

    public void FirePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject fp = Instantiate(fireballPlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        var fpScript = fp.GetComponent<FireballPlate>();
        fpScript.SetReference(gameObject);
        fpScript.SetCoords(matrixX, matrixY);
    }

    public void DestroyFireballPlates()
    {
        GameObject[] fireballPlates = GameObject.FindGameObjectsWithTag("fireballPlate");
        for (int i = 0; i < fireballPlates.Length; i++)
        {
            Destroy(fireballPlates[i]);
        }//end for

    }//End DestroyAttackPlate

    public void SurroundFireAttack(int x, int y)
    {
        FireballDamage(x, y);
        FireballDamage(x, y + 1);
        FireballDamage(x, y - 1);
        FireballDamage(x + 1, y + 1);
        FireballDamage(x - 1, y - 1);
        FireballDamage(x + 1, y);
        FireballDamage(x - 1, y);
        FireballDamage(x - 1, y + 1);
        FireballDamage(x + 1, y - 1);

    }//end SMP

    public void FireballDamage(int x, int y)
    {

        Game checker = processor.GetComponent<Game>();



        if (checker.PositionOnBoard(x, y))
        {
            if (checker.positions[x, y] != null)
            {

                if (checker.positions[x, y].name == "elfChar"
                    || checker.positions[x, y].name == "santaChar")
                {
                    //Create an attack plate under the NPChar
                    this.enemyInRange = true;
                    GameObject point = processor.GetComponent<Game>().GetPosition(x, y);

                    if (point.GetComponent<NonPlayerCharacter>().alive == true) { 
                    point.GetComponent<NonPlayerCharacter>().health -= 3;
                    }
                }

            }

        }
    


    }//end CheckAttackRange

    public void SurroundAttackCheck(int x, int y)
    {
        CheckAttackRange(x, y);
        CheckAttackRange(x, y + 1);
        CheckAttackRange(x, y - 1);
        CheckAttackRange(x + 1, y + 1);
        CheckAttackRange(x - 1, y - 1);
        CheckAttackRange(x + 1, y);
        CheckAttackRange(x - 1, y);
        CheckAttackRange(x - 1, y + 1);
        CheckAttackRange(x + 1, y - 1);

    }//end SurroundAttackCheck

    public void CheckAttackRange(int x, int y)
    {

        Game checker = processor.GetComponent<Game>();

        if (checker.PositionOnBoard(x, y))
        {
            if (checker.positions[x, y] != null)
            {

                if (checker.positions[x, y].name == "elfChar" && checker.positions[x, y].GetComponent<NonPlayerCharacter>().alive == true
                    || checker.positions[x, y].name == "santaChar" && checker.positions[x, y].GetComponent<NonPlayerCharacter>().alive == true)
                {
                    //Create an attack plate under the NPChar
                    this.enemyInRange = true;
                    GameObject point = processor.GetComponent<Game>().GetPosition(x, y);

                    if (point.GetComponent<NonPlayerCharacter>().alive == true) { 
                    AttackPlateSpawn(x, y, point);
                    }//end if
                }

            }

        }

    }//end CheckAttackRange

    public void AttackPlateSpawn(int matrixX, int matrixY, GameObject target)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject ap = Instantiate(attackPlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        AttackPlate apScript = ap.GetComponent<AttackPlate>();
        apScript.SetReference(gameObject);
        apScript.SetCoords(matrixX, matrixY);
        apScript.SetTargetReference(target);
    }//end AttackPlateSpawn()

    public void DestroyAttackPlates()
    {
        GameObject[] attackPlates = GameObject.FindGameObjectsWithTag("AttackPlate");
        for (int i = 0; i < attackPlates.Length; i++)
        {
            Destroy(attackPlates[i]);
        }//end for

    }//End DestroyAttackPlate

    //---------------------------------------------------------------------------End Attack/Fireball section

    //---------------------------------------------------------------------------Begin Frost Nova section

    //begin frostplate
    public void FrostRangePlate(int x, int y)
    {
        LineFrostPlate(1, 0);
        LineFrostPlate(0, 1);
        LineFrostPlate(-1, 0);
        LineFrostPlate(0, -1);
        //Quadrant 1
        SurroundFrostPlate(xBoard + 2, yBoard + 2);
        SurroundFrostPlate(xBoard + 2, yBoard + 5);
        SurroundFrostPlate(xBoard + 5, yBoard + 2);
        SurroundFrostPlate(xBoard + 5, yBoard + 5);
        //Quadrant2
        SurroundFrostPlate(xBoard - 2, yBoard + 2);
        SurroundFrostPlate(xBoard - 2, yBoard + 5);
        SurroundFrostPlate(xBoard - 5, yBoard + 2);
        SurroundFrostPlate(xBoard - 5, yBoard + 5);
        //Quadrant3
        SurroundFrostPlate(xBoard - 2, yBoard - 2);
        SurroundFrostPlate(xBoard - 2, yBoard - 5);
        SurroundFrostPlate(xBoard - 5, yBoard - 2);
        SurroundFrostPlate(xBoard - 5, yBoard - 5);
        //Quadrant4
        SurroundFrostPlate(xBoard + 2, yBoard - 2);
        SurroundFrostPlate(xBoard + 2, yBoard - 5);
        SurroundFrostPlate(xBoard + 5, yBoard - 2);
        SurroundFrostPlate(xBoard + 5, yBoard - 5);
        
    }//end charMovePlate
    public void LineFrostPlate(int xIncrement, int yIncrement)
    {
        Game sc = processor.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (((x < xBoard + 7) && (y < yBoard + 7)) && ((x > xBoard - 7) && (y > yBoard - 7)))
        {
            if ((sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null))
            {
                
                FrostPlateSpawn(x, y);
                x += xIncrement;
                y += yIncrement;
            }//end if
            else
            {
                x += xIncrement;
                y += yIncrement;
            }//end else
            
        }//end while 

    }//end LineFP
    public void SurroundFrostPlate(int x, int y)
    {
        PointFrostPlate(x, y);
        PointFrostPlate(x, y + 1);
        PointFrostPlate(x, y - 1);
        PointFrostPlate(x + 1, y + 1);
        PointFrostPlate(x - 1, y - 1);
        PointFrostPlate(x + 1, y);
        PointFrostPlate(x - 1, y);
        PointFrostPlate(x - 1, y + 1);
        PointFrostPlate(x + 1, y - 1);
        
    }//end SFP
    public void PointFrostPlate(int x, int y)
    {
        Game sc = processor.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            
            GameObject cp = sc.GetPosition(x, y);
            if (cp == null)
            {
                
                FrostPlateSpawn(x, y);
            }//end nested if

        }//end if
    }//end PFP
    public void FrostSurroundAttack(int x, int y)
    {
        
        FrostNovaHold(x, y);
        FrostNovaHold(x, y + 1);
        FrostNovaHold(x, y - 1);
        FrostNovaHold(x + 1, y + 1);
        FrostNovaHold(x - 1, y - 1);
        FrostNovaHold(x + 1, y);
        FrostNovaHold(x - 1, y);
        FrostNovaHold(x - 1, y + 1);
        FrostNovaHold(x + 1, y - 1);
    }//end FSA
    public void FrostNovaHold(int x, int y)
    {
        
        Game checker = processor.GetComponent<Game>();

        if (checker.PositionOnBoard(x, y))
        {

            if (checker.positions[x, y] != null)
            {

                if (checker.positions[x, y].name == "elfChar" ||
                 checker.positions[x, y].name == "santaChar")
                {
                    //CREATE An ATTACK PLATE UNDER THE NPCHAR
                    GameObject point = checker.GetPosition(x, y);
                    //AttackPlateSpawn(x, y, point);
                    
                    point.GetComponent<NonPlayerCharacter>().hasMoved = true;
                }//end nest 2 char check
            }//end if nest 1 positions[]
        }//end master if positiononboard

    }//end FAD
    public void FrostPlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject fp = Instantiate(FrostPlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        frostPlate fpScript = fp.GetComponent<frostPlate>();

        fpScript.SetReference(gameObject);
        fpScript.SetCoords(matrixX, matrixY);
        
    }//end FPSpawn
    public void DestroyFrostPlates()
    {
        GameObject[] frostPlates = GameObject.FindGameObjectsWithTag("FrostPlate");
        for (int i = 0; i < frostPlates.Length; i++)
        {
            Destroy(frostPlates[i]);
        }//end for loop        
    }//end DestroyFirePlates
     //end frostplate
     //
     //begin novaplate
    public void NovaPlateSpawn(int matrixX, int matrixY, GameObject target)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject ap = Instantiate(NovaPlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        novaPlate apScript = ap.GetComponent<novaPlate>();

        apScript.SetReference(gameObject);
        apScript.SetCoords(matrixX, matrixY);
        apScript.SetTargetReference(target);
    }//end MPSpawn

    public void SurroundNovaCheck(int x, int y)
    {
        CheckNovaRange(x, y);
        CheckNovaRange(x, y + 1);
        CheckNovaRange(x, y - 1);
        CheckNovaRange(x + 1, y + 1);
        CheckNovaRange(x - 1, y - 1);
        CheckNovaRange(x + 1, y);
        CheckNovaRange(x - 1, y);
        CheckNovaRange(x - 1, y + 1);
        CheckNovaRange(x + 1, y - 1);

    }//end SurroundAttackCheck

    public void CheckNovaRange(int x, int y)
    {

        Game checker = processor.GetComponent<Game>();

        if (checker.PositionOnBoard(x, y))
        {
            if (checker.positions[x, y] != null)
            {

                if (checker.positions[x, y].name == "elfChar" && checker.positions[x, y].GetComponent<NonPlayerCharacter>().alive == true
                    || checker.positions[x, y].name == "santaChar" && checker.positions[x, y].GetComponent<NonPlayerCharacter>().alive == true)
                {
                    //Create an attack plate under the NPChar
                    this.enemyInRange = true;
                    GameObject point = processor.GetComponent<Game>().GetPosition(x, y);

                    if (point.GetComponent<NonPlayerCharacter>().alive == true)
                    {
                        NovaPlateSpawn(x, y, point);
                    }//end if
                }

            }

        }

    }//end CheckAttackRange
    public void DestroyNovaPlates()
    {
        GameObject[] novaPlates = GameObject.FindGameObjectsWithTag("NovaPlate");
        for (int i = 0; i < novaPlates.Length; i++)
        {
            Destroy(novaPlates[i]);
        }//end for loop        
    }//end DestroyMovePlates
    //end novaplate

    //---------------------------------------------------------------------------End Frost Nova section


    //---------------------------------------------------------------------------Begin Special/Heal section

    public void InitiateSpecialPhase()
    {

        switch (this.name)
        {
            
            case "fighterChar":
                if (this.hasSpecial == false)
                {

                    if (this.hasMoved == true)
                    {
                        this.hasMoved = false;
                    }//end if
                    SurroundAttackCheck(xBoard, yBoard);

                    break;
                }
                else
                {
                    break;
                }
            case "mageChar":
                if (this.hasSpecial == false)
                {
                    FrostRangePlate(xBoard, yBoard);
                    break;
                }
                else
                {
                    break;
                }

            case "clericChar":
                if (this.hasSpecial == false)
                {
                    SurroundHealPlate(xBoard, yBoard);
                    break;
                }
                else
                {
                    break;
                }
        }//end switch

    }//end InitiateMovePlates()

    public void SurroundHealPlate(int x, int y)
    {
        CheckHealRange(x, y);
        CheckHealRange(x, y + 1);
        CheckHealRange(x, y - 1);
        CheckHealRange(x + 1, y + 1);
        CheckHealRange(x - 1, y - 1);
        CheckHealRange(x + 1, y);
        CheckHealRange(x - 1, y);
        CheckHealRange(x - 1, y + 1);
        CheckHealRange(x + 1, y - 1);

    }//end SHP

    public void CheckHealRange(int x, int y)
    {
        Game checker = processor.GetComponent<Game>();

        if (checker.PositionOnBoard(x, y))
        {
            if (checker.positions[x, y] != null)
            {
                if (checker.positions[x, y].name == "fighterChar" || checker.positions[x, y].name == "clericChar" || checker.positions[x, y].name == "mageChar")
                {
                    //Create a heal plate under the NPChar
                    this.allyInRange = true;
                    GameObject point = processor.GetComponent<Game>().GetPosition(x, y);
                    HealPlateSpawn(x, y, point);
                }

            }

        }

    }//end CheckAttackRange

    public void HealPlateSpawn(int matrixX, int matrixY, GameObject target)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.336f;
        y *= 0.336f;

        x += -4.535f;
        y += -4.535f;

        GameObject hp = Instantiate(healPlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        HealPlate hpScript = hp.GetComponent<HealPlate>();
        hpScript.SetReference(gameObject);
        hpScript.SetCoords(matrixX, matrixY);
        hpScript.SetTargetReference(target);
    }//end HealPlateSpawn()

    public void DestroyHealPlates()
    {
        GameObject[] healPlates = GameObject.FindGameObjectsWithTag("HealPlate");
        for (int i = 0; i < healPlates.Length; i++)
        {
            Destroy(healPlates[i]);
        }//end for

    }//End DestroyHealPlate

    //---------------------------------------------------------------------------End Special/Heal section


    //-------------------------------------------------------------Begin Button Spawns

    public void moveButtonSpawn() {

        GameObject mb = Instantiate(moveButton, new Vector3(7, 6 - 3.0f), Quaternion.identity);
        ButtonManager mbScript = mb.GetComponent<ButtonManager>();

        mbScript.SetReference(gameObject);
    }

    public void attackButtonSpawn()
    {

        GameObject ab = Instantiate(attackButton, new Vector3(7, 4 - 3.0f), Quaternion.identity);
        ButtonManager abScript = ab.GetComponent<ButtonManager>();

        abScript.SetReference(gameObject);
    }

    public void specialButtonSpawn()
    {

        GameObject sb = Instantiate(specialButton, new Vector3(7, 2 - 3.0f), Quaternion.identity);
        ButtonManager sbScript = sb.GetComponent<ButtonManager>();

        sbScript.SetReference(gameObject);
    }

    public void endTurnButtonSpawn()
    {

        GameObject eb = Instantiate(endTurnButton, new Vector3(7, 0 - 3.0f), Quaternion.identity);
        ButtonManager ebScript = eb.GetComponent<ButtonManager>();

        ebScript.SetReference(gameObject);
    }

    public void hpBarSpawn() 
    {

        GameObject.FindGameObjectWithTag("Hp").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("Hp").GetComponent<Text>().text = this.health + " / " + this.healthMax;

    }

    public void DestroyButtons()
    {
        GameObject moveButton = GameObject.FindGameObjectWithTag("moveTurn");
        Destroy(moveButton);

        GameObject attackButton = GameObject.FindGameObjectWithTag("attackButton");
        Destroy(attackButton);

        GameObject specialButton = GameObject.FindGameObjectWithTag("specialButton");
        Destroy(specialButton);

        GameObject endButton = GameObject.FindGameObjectWithTag("EndTurn");
        Destroy(endButton);


    }//End DestroyButtons

    public void DestroyAllPlates() {

        DestroyCurrentPlate();
        DestroyMovePlates();
        DestroyAttackPlates();
        DestroyHealPlates();
        DestroyFireballPlates();
    }

    //----------------------------------------------------------------End Button Spawns

    public void Update()
    {
        if (this.hasMoved == true)
        {
            GameObject moveButton = GameObject.FindGameObjectWithTag("moveTurn");
            Destroy(moveButton);
        }//end if

        if (this.hasAttacked == true)
        {
            GameObject attackButton = GameObject.FindGameObjectWithTag("attackButton");
            Destroy(attackButton);
        }//end if

        if (this.hasSpecial == true)
        {
            GameObject specialButton = GameObject.FindGameObjectWithTag("specialButton");
            Destroy(specialButton);
        }//end if

        if (this.health > this.healthMax)
        {
            this.health = healthMax;
        }
    }//end Update
}//end PlayerCharacter


