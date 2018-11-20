using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformGeneration : MonoBehaviour {

    public int nbRooms = 10; // Nb de rooms a génerer

    public GameObject Level1;
    public GameObject[] tabRooms; // contient toutes les rooms 10x10
    public GameObject[] tabEndRooms; // contient toutes les rooms de fin 10x10

    private Vector2 lastpositions; // position de la dernière room

    private int idLatestRoomImplented; // Id de la derniere room implémenté
    private string lastRoomSortie; // Sortie de la derniere room implémenté

    private int idAvantEndRoom; // Id de la derniere room implémenté avant la room de fin

    // Use this for initialization
    void Start ()
    {
        //Random.InitState(12);

        /********* Ajout des rooms *********/
        Vector2 position = new Vector2(0, 0);
        Instantiate(Level1, position, Quaternion.identity);
        lastRoomSortie = Level1.gameObject.GetComponent<RoomsInfo>().entreAccepter;
        lastpositions = position;
        for (int room = 1; room <= nbRooms; room++)
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
                        position = new Vector2(lastpositions.x, lastpositions.y + 25.6f);
                        break;
                    case "N":
                        position = new Vector2(lastpositions.x, lastpositions.y - 25.6f);
                        break;
                    case "NO":
                        position = new Vector2(lastpositions.x + 25.6f, lastpositions.y);
                        break;
                    case "SO":
                        position = new Vector2(lastpositions.x + 25.6f, lastpositions.y);
                        break;
                    case "NE":
                        position = new Vector2(lastpositions.x - 25.6f, lastpositions.y);
                        break;
                    case "SE":
                        position = new Vector2(lastpositions.x - 25.6f, lastpositions.y);
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
            lastRoomSortie = Level1.gameObject.GetComponent<RoomsInfo>().entreAccepter;
        }
        else
        {
            lastRoomSortie = tabRooms[idLatestRoomImplented].gameObject.GetComponent<RoomsInfo>().entreAccepter;
        }
        bool roomAjouter = false;
        while (roomAjouter == false)
        {
            if(lastRoomSortie == "S"){
                position = new Vector2(lastposition.x, lastposition.y + 25.6f);
                int idRoomToImplement = Random.Range(0, tabRooms.Length);
                if (lastRoomSortie == tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre)
                {
                    Instantiate(tabRooms[idRoomToImplement], position, Quaternion.identity);
                    roomAjouter = true;
                    idLatestRoomImplented = idRoomToImplement;
                }
            }
            else if(lastRoomSortie == "N")
            {
                position = new Vector2(lastposition.x, lastposition.y - 25.6f);
                int idRoomToImplement = Random.Range(0, tabRooms.Length);
                if (lastRoomSortie == tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre)
                {
                    Instantiate(tabRooms[idRoomToImplement], position, Quaternion.identity);
                    roomAjouter = true;
                    idLatestRoomImplented = idRoomToImplement;
                }
            }
            else
            {
                position = new Vector2(lastposition.x + 25.6f, lastposition.y);
                int idRoomToImplement = Random.Range(0, tabRooms.Length);
                if (lastRoomSortie == tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre)
                {
                    Instantiate(tabRooms[idRoomToImplement], position, Quaternion.identity);
                    roomAjouter = true;
                    idLatestRoomImplented = idRoomToImplement;
                }
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
