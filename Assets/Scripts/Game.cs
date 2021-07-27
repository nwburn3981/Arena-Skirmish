using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game : MonoBehaviour{

    //NPC
    public GameObject NPChar;
  
    //PC
    public GameObject PChar;

    //Variable to track game status
    private bool gameOver = false;

    //Legal and nonlegal(blacklist) positions in scene
    public GameObject[,] positions = new GameObject[28, 26];
    private bool[,] blackList = new bool[28, 26];

    //Arrays containing all player and nonplayer objects
    public GameObject[] playerTeam = new GameObject[3];
    private GameObject[] aiTeam = new GameObject[5];

    //Current team and player
    private GameObject currentChar;
    private string currentTeam = "player";

    //Trackers for game phases
    public bool attackPhase = false;
    public bool movePhase = false;
    public bool specialPhase = false;


    //--------------------------------------------------------------Start setup

    // Start is called before the first frame update
    void Start()
    {
        playerTeam = new GameObject[] {
            CreatePC("fighterChar", "player", 25, 16),
            CreatePC("clericChar", "player", 25, 18),
            CreatePC("mageChar", "player", 25, 9)
        };//end playerTeam

        aiTeam = new GameObject[] {
            CreateNPC("elfChar", "ai", 8, 7),
            CreateNPC("elfChar", "ai", 9, 8),
            CreateNPC("elfChar", "ai", 2, 6),
            CreateNPC("elfChar", "ai", 4, 5),
            CreateNPC("santaChar", "ai", 4, 20)
        };//end aiTeam

        currentChar = playerTeam[0];

        for (int i = 0; i < playerTeam.Length; i++)
        {
            SetPlayerPosition(playerTeam[i]);   
        }//end forloop

        for (int i = 0; i < aiTeam.Length; i++)
        {
            SetAIPosition(aiTeam[i]);   
        }//end forloop


        //Bottom left pillar
        blackList[8, 9] = true;
        blackList[8, 10] = true;
        blackList[8, 11] = true;

        //Bottom pillar
        blackList[13, 9] = true;
        blackList[13, 8] = true;
        blackList[13, 7] = true;

        //Bottom right pillar
        blackList[18, 9] = true;
        blackList[18, 10] = true;
        blackList[18, 11] = true;

        //right pillar
        blackList[20, 14] = true;

        //Left pillar
        blackList[6, 14] = true;
        blackList[6, 15] = true;
        blackList[6, 16] = true;

        //Top left pillar
        blackList[8, 19] = true;

        //Top pillar
        blackList[13, 21] = true;
        blackList[13, 22] = true;
        blackList[13, 23] = true;

        //Top right
        blackList[18, 19] = true;
        blackList[18, 20] = true;
        blackList[18, 21] = true;

        //Top shrine
        blackList[11, 25] = true;
        blackList[12, 25] = true;
        blackList[13, 25] = true;
        blackList[14, 25] = true;
        blackList[15, 25] = true;

        //Center hole
        for (int x = 10; x <= 16; x++) 
            for (int y = 11; y <= 17; y++)
                blackList[x, y] = true;

        //Poles
        blackList[13, 10] = true;
        blackList[17, 14] = true;
        blackList[13, 18] = true;
        blackList[9, 14] = true;



    }//end start

    public GameObject CreatePC(string name, string teamName, int x, int y)
    {
        GameObject obj = Instantiate(PChar, new Vector3(0, 0, -2), Quaternion.identity);
        PlayerCharacter pc = obj.GetComponent<PlayerCharacter>();
        pc.name = name;
        pc.SetXBoard(x);
        pc.SetYBoard(y);
        pc.Activate();
        return obj;
    }//end Create()

    public GameObject CreateNPC(string name, string teamName,  int x, int y)
    {
        GameObject obj = Instantiate(NPChar, new Vector3(0, 0, -2), Quaternion.identity);
        NonPlayerCharacter npc = obj.GetComponent<NonPlayerCharacter>();
        npc.name = name;
        npc.SetXBoard(x);
        npc.SetYBoard(y);
        npc.Activate();
        return obj;
    }//end Create()

    //--------------------------------------------------------------End setup


    //--------------------------------------------------------------Start position section

    public void SetPlayerPosition(GameObject obj)
    {
        PlayerCharacter pc = obj.GetComponent<PlayerCharacter>();
        positions[pc.GetXBoard(), pc.GetYBoard()] = obj;
    }//end SetPosition()

    public void SetAIPosition(GameObject obj)
    {
        NonPlayerCharacter npc = obj.GetComponent<NonPlayerCharacter>();
        positions[npc.GetXBoard(), npc.GetYBoard()] = obj;
    }//end SetPosition()

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }//end SetPositionEmpty()

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }//end GetPosition()

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1) || blackList[x, y])
            return false;
        return true;
    }//end PositionOnBoard()

    //--------------------------------------------------------------End position section


    //--------------------------------------------------------------Start Game tracking section

    public string GetCurrentTeam()
    {

        return currentTeam;

    }//end GetCurrentTeam

    public string GetCurrentChar(GameObject obj)
    {
        PlayerCharacter pc = obj.GetComponent<PlayerCharacter>();
        return pc.currentChar;
    }//end GetCurrentChar
 
    public void NextTurn()
    {

        StartCoroutine(AITurn());
        
    }//end NextTurn()

    public void PlayerTurn() {
        GameObject pc = PChar;

        pc.GetComponent<PlayerCharacter>().DestroyAllPlates();

        currentTeam = "player";
        NextRound();
    }//end PlayerTurn
    IEnumerator AITurn() {
        GameObject pc = PChar;

        pc.GetComponent<PlayerCharacter>().DestroyAllPlates();

        currentTeam = "ai";

        for (int i = 0; i < aiTeam.Length; i++)
        {
            if (aiTeam[i].GetComponent<NonPlayerCharacter>().alive == true) { 
                aiTeam[i].GetComponent<NonPlayerCharacter>().InitiateNPC(aiTeam[i]);
                yield return new WaitForSeconds(1);
            }//end if
        }//end for

        PlayerTurn();

    }//end AITurn

    public void NextRound()
    {
                 
        for (int i = 0; i < playerTeam.Length; i++)
        {
            playerTeam[i].GetComponent<PlayerCharacter>().SetHasMoved(false);
            playerTeam[i].GetComponent<PlayerCharacter>().SetHasAttacked(false);
            playerTeam[i].GetComponent<PlayerCharacter>().SetHasGone(false);
            playerTeam[i].GetComponent<PlayerCharacter>().SetHasSpecial(false);
        }//end player for

        for (int i = 0; i < aiTeam.Length; i++)
        {
            aiTeam[i].GetComponent<NonPlayerCharacter>().SetHasMoved(false);
            aiTeam[i].GetComponent<NonPlayerCharacter>().SetHasAttacked(false);
            aiTeam[i].GetComponent<NonPlayerCharacter>().SetHasGone(false);
            
        }//end ai for

    }//end NextRound()

    public bool IsGameOver()
    {
        return gameOver;
    }//end IsGameOver()

    //--------------------------------------------------------------End Game tracking section


    public void Update()
    {

        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }//end if

   
        //EndTurn functionality 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
 
            NextTurn();
            NextRound();
        }//end if

        int pcDeathCounter = 0;
        for (int i = 0; i <= playerTeam.Length-1; i++) {
             if (playerTeam[i].GetComponent<PlayerCharacter>().health <= 0) {
               playerTeam[i].GetComponent<PlayerCharacter>().alive = false;
               playerTeam[i].GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                pcDeathCounter++;
            }//end if

            if (pcDeathCounter == playerTeam.Length) {
                gameOver = true;
                GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Text>().enabled = true;
            }//end if
        }

        int npcDeathCounter = 0;
        for (int i = 0; i <= aiTeam.Length-1; i++)
        {
             if (aiTeam[i].GetComponent<NonPlayerCharacter>().health <= 0)
            {
                aiTeam[i].GetComponent<NonPlayerCharacter>().alive = false;
                aiTeam[i].GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                npcDeathCounter++;
            }

            if (npcDeathCounter == aiTeam.Length) {
                gameOver = true;
                GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Text>().enabled = true;
            }//end if
        }
        //Game reset shortcut
        if (Input.GetKeyDown(KeyCode.Escape)) {
            gameOver = true;
            GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Text>().enabled = true;
        }//end if

    }//End Update

}//end game class


//Increased difficulty
//Move values, attack range, heal range etc etc

/*Bughunt
 * Buttons tied to players as whole, not individually
 * NPC movement not behaving correctly
 * 
 * 
 * 
 * 
 * 
 * 
 */
