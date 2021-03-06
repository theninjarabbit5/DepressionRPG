﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject textbox;
    public GameObject player;
    public GameObject[] baddies;
	public bool forTesting;
	public GameObject HelpfulKnight;
	public GameObject Wizard;
    public string RoomName;
	public GameObject can;
    public string[] negthoughts;
    public string[] negthoughts2;
    public GameObject dog;
    public GameObject overlay;

    public GameObject journal;
    private bool journalCreate;
    private bool journalActive;
    private GameObject jrn;
    public string[] journalEntries;
    private int currentPage;
    public GameObject myAlert;
    private GameObject alert;
    private bool createAlert;

    private GameObject hk;
	private GameObject wiz;
	private bool spawn = false;

	public GameObject magic;

	private GameObject tb;
	private int numToSpawn;
	private int numOfSpawn;
	private GameObject[] boxes;
	private float time;
	private bool trigger;
	private GameObject mg;
	private bool iconCreate;
    private int spawnNum;
    private GameObject doggo;

    public int Tester;

	// Use this for initialization
	void Start () {
        if(RoomName =="Area 1")
        {
            if (GlobalStuff.WasInBattle)
            {
                Respawn();
                GlobalStuff.WasInBattle = false;
            }
            if (GlobalStuff.HaveQuestItem)
            {
                doggo = Instantiate(dog, new Vector3(player.transform.position.x, player.transform.position.y - 2.5f, 0), new Quaternion(0, 0, 0, 0));
                GlobalStuff.HaveQuestItem = true;
                doggo.GetComponent<Dog>().Follow = true;
            }            
        }
        if(RoomName == "Area 2")
        {
            if (GlobalStuff.HaveQuestItem)
            {
                doggo = Instantiate(dog, new Vector3(player.transform.position.x + 1.5f, player.transform.position.y, 0), new Quaternion(0, 0, 0, 0));
                GlobalStuff.HaveQuestItem = true;
                doggo.GetComponent<Dog>().Follow = true;
            }
        }
		if (RoomName == "Area 3") {
			boxes = new GameObject[40];	
			numToSpawn = 5;
			numOfSpawn = 0;
			time = 1.5f;
			trigger = false;
            spawnNum = 1;
		}
        if(RoomName == "Crossroads")
        {
			player.GetComponent<Animator> ().enabled = true;
			player.GetComponent<Animator> ().SetInteger ("Direction", 2);
            if (GlobalStuff.HaveQuestItem)
            {
                doggo = Instantiate(dog, new Vector3(player.transform.position.x, player.transform.position.y-2.5f, 0), new Quaternion(0, 0, 0, 0));
                GlobalStuff.HaveQuestItem = true;
                doggo.GetComponent<Dog>().Follow = true;
            }
        }
        if(RoomName =="Wizard house")
        {
            if (GlobalStuff.HaveQuestItem)
            {
                doggo = Instantiate(dog, new Vector3(player.transform.position.x + 1, player.transform.position.y, 0), new Quaternion(0, 0, 0, 0));
                doggo.GetComponent<Dog>().Follow = true;
            }
        }
		iconCreate = false;
        journalCreate = false;
        journalActive = false;
        currentPage = 0;

        createAlert = false;
    }
	
	// Update is called once per frame
	void Update () {

        //GlobalStuff.EntriesUnlocked = Tester;

        if (RoomName == "Area 1") {
			if (GlobalStuff.Aniexty == true) {
				if (GlobalStuff.AnxTalk >= 3 && !player.GetComponent<Player> ().InInteraction) {
					player.GetComponent<Player> ().CanMove = false;
					//summon other knight
					spawnKnight ();
					//set anxtalk to weird number
					GlobalStuff.AnxTalk = -1;
				}
			}
			if (GlobalStuff.KindFinished) {
				GameObject temp = GameObject.FindGameObjectWithTag ("Kind Knight");
				if (temp != null) {
					float distance = Mathf.Pow (player.transform.localPosition.x - hk.transform.localPosition.x, 2) + Mathf.Pow (player.transform.localPosition.y - hk.transform.localPosition.y, 2);
					distance = Mathf.Sqrt (distance);

					if (distance > 9) {
						Destroy (hk, 0);
					}
				}
			}
            
		} else if (RoomName == "Area 2") {
			if (GlobalStuff.KindFinished) {
				if (spawn == false) {
					hk = Instantiate (HelpfulKnight, new Vector3 (-17, 16, 0), new Quaternion (0, 0, 0, 0));
					spawn = true;
				}
			}

		} else if (RoomName == "Area 3") {
			if (trigger || GlobalStuff.UseSpell) {
				numToSpawn = 30;
				spawnTextBoxes ();
				time -= Time.deltaTime;
			}
			if (numOfSpawn >= numToSpawn && !GlobalStuff.UseSpell) {
				DestroyBoxes (); // add delay later
				player.transform.position = new Vector2 (-12.9f, -.48f);
				player.GetComponent<Player> ().SpriteDir = 2;
				//spawn in wizard to talk
				spawnWizard ();
			}
			if(numOfSpawn >= NumToSpawn && GlobalStuff.UseSpell){
				DestroyBoxes ();
				player.transform.position = new Vector2 (-12.9f, -.48f);
				player.GetComponent<Player> ().SpriteDir = 2;
				time = 3f;
                GlobalStuff.HaveQuestItem = false;
			}
		} else if (RoomName == "Wizard house") {
			if (GlobalStuff.TalkToWiz) {
				if (spawn == false) {
					wiz = Instantiate (Wizard, new Vector3 (player.transform.position.x - 4, player.transform.position.y, 0), new Quaternion (0, 0, 0, 0));
					spawn = true;
				}
			}
		}

		if (GlobalStuff.UseSpell) {
			if (!iconCreate) {
				mg = Instantiate (magic, new Vector2 (300, Screen.height - 50), new Quaternion (0, 0, 0, 0), can.transform);
				mg.transform.SetAsLastSibling ();
				iconCreate = true;
			}
		}

        OpenCloseJournal();
        if (GlobalStuff.GetAlert)
        {
            NewJournalEntry();
            GlobalStuff.GetAlert = false;  
        }
    }
    //respawn into proper place 
    private void Respawn() {
        player.transform.position = GlobalStuff.PrevPos;
    }

    private void spawnKnight() {
		if (spawn == false) {
			hk = Instantiate (HelpfulKnight, new Vector3 (player.transform.position.x, player.transform.position.y + 6, 0), new Quaternion (0, 0, 0, 0));
			spawn = true;
			//player.GetComponent<Player> ().CanMove = false;
		}
    }

	private void spawnTextBoxes(){
        //if (numOfSpawn < numToSpawn && time <= 0) {
        if (time <= 0 && !GlobalStuff.IsPaused)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                tb = Instantiate(textbox, new Vector2(Random.Range((20), (Screen.width - 20)), Random.Range((20), (Screen.height - 20))), new Quaternion(0, 0, 0, 0), can.transform);
                if (GlobalStuff.HaveQuestItem)
                {
                    tb.transform.GetChild(0).GetComponent<Text>().text = negthoughts2[Random.Range(0, negthoughts.Length - 1)];
                }
                else
                {
                    tb.transform.GetChild(0).GetComponent<Text>().text = negthoughts[Random.Range(0, negthoughts.Length - 1)];
                }
                boxes[numOfSpawn] = tb;
                numOfSpawn++;
                tb.transform.SetAsFirstSibling();
            }
            spawnNum++;
            time = 1.5f;
        }
	}

    private void OpenCloseJournal()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObject.Find("Alert(Clone)"))
            {
                Destroy(alert, 0);
                createAlert = false;
            }
            journalActive = !journalActive;
            if (journalActive)
            {
                if (!journalCreate)
                {
                    //create it
                    jrn = Instantiate(journal, new Vector2(Screen.width/2,Screen.height/2), new Quaternion(0, 0, 0, 0), can.transform);
                    journalCreate = true;
                }
                GlobalStuff.IsPaused = true;
            }
            else
            {
                if (journalCreate)
                {
                    //destroy it
                    Destroy(jrn, 0);
                    journalCreate = false;
                    currentPage = 0;
                }
                GlobalStuff.IsPaused = false;
            }
        }
        if (journalActive)
        {
            DisplayJournalEntries(); 
        }
    }

    public void DisplayJournalEntries()
    {
        //only draw the arrows if you can move pages

        if (Input.GetKeyDown(KeyCode.A)) {
            //go back a page
            currentPage -= 2;
            if (currentPage < 0)
            {
                currentPage = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //go forward a page
            currentPage += 2;
            if (currentPage > GlobalStuff.EntriesUnlocked)
            {
                currentPage = GlobalStuff.EntriesUnlocked;
            }
        }
        //arrow stuff
        //show prev arrow if cur > 0
        if(currentPage >= 2)
        {
            Color color = jrn.transform.GetChild(3).GetComponent<Image>().color;
            color.a = 255f;
            jrn.transform.GetChild(3).GetComponent<Image>().color = color;
        }
        else
        {
            Color color = jrn.transform.GetChild(3).GetComponent<Image>().color;
            color.a = 0f;
            jrn.transform.GetChild(3).GetComponent<Image>().color = color;
        }
        //show next arrpw if curr < total
        if(currentPage < GlobalStuff.EntriesUnlocked-1)
        {
            Color color = jrn.transform.GetChild(2).GetComponent<Image>().color;
            color.a = 255f;
            jrn.transform.GetChild(2).GetComponent<Image>().color = color;
        }
        else
        {
            Color color = jrn.transform.GetChild(2).GetComponent<Image>().color;
            color.a = 0f;
            jrn.transform.GetChild(2).GetComponent<Image>().color = color;
        }

        //show all entries on both pages
        if (currentPage == 0 || currentPage == 1)
        {
            jrn.transform.GetChild(0).GetComponent<Text>().text = journalEntries[0];
            if (GlobalStuff.EntriesUnlocked >= 1)
            {
                jrn.transform.GetChild(1).GetComponent<Text>().text = journalEntries[1];
            }
            else
            {
                jrn.transform.GetChild(1).GetComponent<Text>().text = " ";
            }
        }
        else if (currentPage == 2 || currentPage == 3)
        {
            if (GlobalStuff.EntriesUnlocked >= 2)
            {
                jrn.transform.GetChild(0).GetComponent<Text>().text = journalEntries[2];
            }
            if (GlobalStuff.EntriesUnlocked >= 3)
            {
                jrn.transform.GetChild(1).GetComponent<Text>().text = journalEntries[3];
            }
            else
            {
                jrn.transform.GetChild(1).GetComponent<Text>().text = " ";
            }
        }
        else if (currentPage == 4 || currentPage == 5)
        {
            if (GlobalStuff.EntriesUnlocked >= 4)
            {
                jrn.transform.GetChild(0).GetComponent<Text>().text = journalEntries[4];
            }
            if (GlobalStuff.EntriesUnlocked >= 5)
            {
                jrn.transform.GetChild(1).GetComponent<Text>().text = journalEntries[5];
            }
            else
            {
                jrn.transform.GetChild(1).GetComponent<Text>().text = " ";
            }
        }
        else if (currentPage == 6)
        {
            jrn.transform.GetChild(0).GetComponent<Text>().text = journalEntries[6];
            jrn.transform.GetChild(1).GetComponent<Text>().text = " ";
        }
    }

    public void DestroyBoxes(){
		for (int i = 0; i < numOfSpawn; i++) {
			Destroy (boxes [i], 0);
		}
		numOfSpawn = 0;
        spawnNum = 1;
		trigger = false;
	}

	private void spawnWizard(){
		if (spawn == false) {
			wiz = Instantiate (Wizard, new Vector3 (player.transform.position.x - 15, player.transform.position.y, 0), new Quaternion (0, 0, 0, 0));
			spawn = true;
			numOfSpawn = 0;
			player.GetComponent<Player>().CanMove = false;
		}
	}

    public void NewJournalEntry()
    {
        GlobalStuff.EntriesUnlocked++;
        currentPage = GlobalStuff.EntriesUnlocked;
        //Debug.Log(GlobalStuff.EntriesUnlocked);
        //GlobalStuff.GetAlert = true;
        if (!createAlert)
        {
            alert = Instantiate(myAlert, new Vector2(Screen.width - 150, Screen.height - 100), new Quaternion(0, 0, 0, 0), can.transform);
            createAlert = true;
        }
    }

	public int NumToSpawn{
		get{ return numToSpawn; }
		set{ numToSpawn = value; }
	}

	public bool Trigger{
		set{ trigger = value; }
	}
}
