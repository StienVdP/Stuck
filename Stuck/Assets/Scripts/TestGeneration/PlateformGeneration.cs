using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformGeneration : MonoBehaviour {

    public int nbRooms = 10; // Nb de rooms a génerer en plus par niveau

    public GameObject Level1;
    public GameObject[] tabRooms; // contient toutes les rooms
    public GameObject[] tabEndRooms; // contient toutes les rooms de fin 10x10

    private Vector2 lastpositions; // position de la dernière room

    private int idLatestRoomImplented; // Id de la derniere room implémenté
    private string lastRoomAccepter; // RoomAccepter de la derniere room implémenté

    private int lastRoomSize; // Taille de la room précédente

    private int idAvantEndRoom; // Id de la derniere room implémenté avant la room de fin

    private int level;

    private GameObject clone;
    public GameObject[] prefabsEnnemis; // Tableau qui contient les ennemies en prefab

    public float tailleBlock = 1;
    private float tailleRoom;
    
    private int compt10 = 0;


    // Use this for initialization
    void Start ()
    {
        tailleRoom = tailleBlock * 25.6f;
        level = GameObject.Find("GameManager").GetComponent<GameManager>().getLevel();
        
        Random.state = GameObject.Find("GameManager").GetComponent<GameManager>().getState(); // On donne le state du random

        /********* Ajout de la room de départ *********/
        Vector2 position = new Vector2(0, 0);
        Instantiate(Level1, position, Quaternion.identity);
        lastRoomAccepter = Level1.gameObject.GetComponent<RoomsInfo>().entreAccepter;
        lastpositions = position;
        
        /********* Ajout des rooms *********/
        for (int room = 1; room <= (nbRooms * level); room++)
        {
            lastpositions = generateRooms(room, lastpositions);
        }

        /********* Ajout de la Room de fin *********/
        bool endRoomAjouter = false;
        while(endRoomAjouter == false)
        {
            int idEndRoom = Random.Range(0, tabEndRooms.Length);
            string entreAccepterAvantEnd = tabRooms[idAvantEndRoom].gameObject.GetComponent<RoomsInfo>().entreAccepter;
            if (entreAccepterAvantEnd == tabEndRooms[idEndRoom].gameObject.GetComponent<RoomsInfo>().entre)
            {
                switch (entreAccepterAvantEnd)
                {
                    case "S":
                        if (lastRoomSize == 10) position = new Vector2(lastpositions.x, lastpositions.y + tailleRoom);
                        else position = new Vector2(lastpositions.x + tailleRoom/2, lastpositions.y + tailleRoom * 2);
                        break;
                    case "N":
                        if (lastRoomSize == 10) position = new Vector2(lastpositions.x, lastpositions.y - tailleRoom);
                        else position = new Vector2(lastpositions.x + tailleRoom / 2, lastpositions.y - tailleRoom);
                        break;
                    case "NO":
                        if (lastRoomSize == 10) position = new Vector2(lastpositions.x + tailleRoom, lastpositions.y);
                        else position = new Vector2(lastpositions.x + tailleRoom * 2, lastpositions.y + tailleRoom);
                        break;
                    case "SO":
                        if (lastRoomSize == 10) position = new Vector2(lastpositions.x + tailleRoom, lastpositions.y);
                        else position = new Vector2(lastpositions.x + tailleRoom * 2, lastpositions.y);
                        break;
                    default:
                        break;
                }

                Instantiate(tabEndRooms[idEndRoom], position, Quaternion.identity);
                endRoomAjouter = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    public Vector2 generateRooms(int room, Vector2 lastposition)
    {
        Vector2 position = lastposition;
        if (room == 1) // Si la seul room implémenté est le level1
        {
            lastRoomAccepter = Level1.gameObject.GetComponent<RoomsInfo>().entreAccepter;
            lastRoomSize = Level1.gameObject.GetComponent<RoomsInfo>().size; // recupère size
            position = new Vector2(lastposition.x + tailleRoom, lastposition.y);
        }
        else
        {
            lastRoomAccepter = tabRooms[idLatestRoomImplented].gameObject.GetComponent<RoomsInfo>().entreAccepter;
        }

        bool roomAjouter = false;
        while (roomAjouter == false)
        {
            int randomLimit;/*
            if(compt10 <= 2)
            {
                randomLimit = tabRooms.Length - 10;
            }else
            {*/
                randomLimit = tabRooms.Length;
            //}
            int idRoomToImplement = Random.Range(0, randomLimit);
            
            if (lastRoomAccepter == tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre) // Si les portes correspondent et si ya pas eu 2 block 20 avant 
            {
                /*
                if (lastRoomSize == 20 && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().size == 10) // Si avant size = 20 et maintenant size = 10
                {
                    switch (lastRoomAccepter)
                    {
                        case "S":
                            position = new Vector2(lastposition.x + (tailleRoom * 1.2f), lastposition.y + 2 * tailleRoom);// + tailleRoom);
                            break;
                        case "N":
                            position = new Vector2(lastposition.x + (tailleRoom * 1.2f), lastposition.y - tailleRoom);// + tailleRoom);
                            break;
                        case "NO":
                            position = new Vector2(lastposition.x + 2 * tailleRoom, lastposition.y + tailleRoom);
                            break;
                        case "SO":
                            position = new Vector2(lastposition.x + 2 * tailleRoom, lastposition.y);
                            break;
                        default:
                            break;
                    }
                }
                else if (lastRoomSize == 10 && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().size == 10) // Si avant size = 10 et maintenant size = 10
                {
                    if (lastRoomAccepter == "S")
                    {
                        position = new Vector2(lastposition.x, lastposition.y + tailleRoom);
                    }
                    else if (lastRoomAccepter == "N")
                    {
                        position = new Vector2(lastposition.x, lastposition.y - tailleRoom);
                    }
                    else
                    {
                        position = new Vector2(lastposition.x + tailleRoom, lastposition.y);
                    }
                }
                else if (lastRoomSize == 10 && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().size == 20) // Si avant size = 10 et maintenant size = 20
                {
                    switch (lastRoomAccepter)
                    {
                        case "S":
                            position = new Vector2(lastposition.x - tailleRoom / 2, lastposition.y + tailleRoom); // + tailleRoom);
                            break;
                        case "N":
                            position = new Vector2(lastposition.x - tailleRoom / 2, lastposition.y - tailleRoom * 2); // + tailleRoom);
                            break;
                        case "NO":
                            position = new Vector2(lastposition.x + tailleRoom, lastposition.y - tailleRoom);
                            break;
                        case "SO":
                            position = new Vector2(lastposition.x + tailleRoom, lastposition.y);
                            break;
                        default:
                            break;
                    }
                }
                else*/ if (lastRoomSize == 20 && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().size == 20) // Si avant size = 20 et maintenant size = 20
                {
                    if (lastRoomAccepter == "S")
                    {
                        position = new Vector2(lastposition.x, lastposition.y + tailleRoom * 2);
                    }
                    else if (lastRoomAccepter == "N")
                    {
                        position = new Vector2(lastposition.x, lastposition.y - tailleRoom * 2);
                    }
                    else
                    {
                        position = new Vector2(lastposition.x + tailleRoom * 2, lastposition.y);
                    }
                }

                clone = Instantiate(tabRooms[idRoomToImplement], position, Quaternion.identity);
                roomAjouter = true;
                idLatestRoomImplented = idRoomToImplement;
                lastRoomSize = clone.GetComponent<RoomsInfo>().size; // recupère size
                if(lastRoomSize == 10)
                {
                    compt10++;
                }else 
                {

                    compt10 = 0;
                }
                // Test de spawn
               /* Transform spawn = clone.transform.Find("SpawnZone");
                if (spawn != null)
                {
                    Debug.Log("Instancie un ennemi");
                    Instantiate(prefabsEnnemis[Random.Range(0, prefabsEnnemis.Length)], spawn.position, Quaternion.identity);
                }*/
            }
        }
        setLastRoomComponent(idLatestRoomImplented);
        return position;
    }

    public void setLastRoomComponent(int id)
    {
        idAvantEndRoom = id;
    }

}
