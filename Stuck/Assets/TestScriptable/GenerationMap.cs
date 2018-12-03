using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMap : MonoBehaviour {

    // The GameObject to instantiate.
    private GameObject entityToSpawn;

    // An instance of the ScriptableObject defined above.
    public RoomManagerScriptableObject[] tabRoomManagerValues; // TODO En faire pour room normal, endroom et debutroom
    
    // This will be appended to the name of the created entities and increment when each is created.
    int instanceNumber = 1;




    public int nbRooms = 10; // Nb de rooms a génerer en plus par niveau

    public RoomManagerScriptableObject Level1;
    //private GameObject[] tabRooms; // contient toutes les rooms
    //private GameObject[] tabEndRooms; // contient toutes les rooms de fin 10x10

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








    void Start()
    {
        entityToSpawn = this.gameObject;
        getEntre();
        getEntreeAccepte();


        /************ TEST ************/


        tailleRoom = tailleBlock * 25.6f;
        level = GameObject.Find("GameManager").GetComponent<GameManager>().getLevel();

        Random.state = GameObject.Find("GameManager").GetComponent<GameManager>().getState(); // On donne le state du random

        /********* Ajout de la room de départ *********/
        Vector2 position = new Vector2(0, 0);
        Instantiate(Level1.prefabRoom, position, Quaternion.identity);
        lastRoomAccepter = Level1.entreAccepte;
        lastpositions = position;

        /********* Ajout des rooms *********/
        for (int room = 1; room <= (nbRooms * level); room++)
        {
            lastpositions = generateRooms(room, lastpositions);
        }

        /********* Ajout de la Room de fin *********/
        /* Pour le test on met pas la derniere room
         * 
        bool endRoomAjouter = false;
        while (endRoomAjouter == false)
        {
            int idEndRoom = Random.Range(0, tabEndRooms.Length);
            string entreAccepterAvantEnd = tabRooms[idAvantEndRoom].gameObject.GetComponent<RoomsInfo>().entreAccepter;
            if (entreAccepterAvantEnd == tabEndRooms[idEndRoom].gameObject.GetComponent<RoomsInfo>().entre)
            {
                switch (entreAccepterAvantEnd)
                {
                    case "S":
                        if (lastRoomSize == 10) position = new Vector2(lastpositions.x, lastpositions.y + tailleRoom);
                        else position = new Vector2(lastpositions.x + tailleRoom / 2, lastpositions.y + tailleRoom * 2);
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
        }*/
    }


    public Vector2 generateRooms(int room, Vector2 lastposition)
    {
        Vector2 position = lastposition;
        if (room == 1) // Si la seul room implémenté est le level1
        {
            lastRoomAccepter = Level1.entreAccepte;
            lastRoomSize = Level1.size; // recupère size
            position = new Vector2(lastposition.x + tailleRoom, lastposition.y);
        }
        else
        {
            lastRoomAccepter = tabRoomManagerValues[idLatestRoomImplented].entreAccepte;
        }

        bool ajoutRoom = false;
        while (ajoutRoom == false)
        {

            int randomLimit;
            if (compt10 <= 3)
            {
                randomLimit = tabRoomManagerValues.Length - 10;
            }
            else
            {
                randomLimit = tabRoomManagerValues.Length;
            }
            int idRoomToImplement = Random.Range(0, randomLimit);



            if (lastRoomAccepter == tabRoomManagerValues[idRoomToImplement].entre) // Si les portes correspondent et si ya pas eu 2 block 20 avant 
            {
                Debug.Log("lastRoomAccepter : " + lastRoomAccepter);
                Debug.Log("Test if : " + tabRoomManagerValues[idRoomToImplement].entre);
                //Debug.Log("Est rentre dans if : ");
                //roomAjouter = true;

                if (lastRoomSize == 20 && tabRoomManagerValues[idRoomToImplement].size == 10) // Si avant size = 20 et maintenant size = 10
                {
                    switch (lastRoomAccepter)
                    {
                        case "S":
                            position = new Vector2(lastposition.x + (tailleRoom / 2), lastposition.y + 2 * tailleRoom);// + tailleRoom);
                            break;
                        case "N":
                            position = new Vector2(lastposition.x + (tailleRoom / 2), lastposition.y - tailleRoom);// + tailleRoom);
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
                else if (lastRoomSize == 10 && tabRoomManagerValues[idRoomToImplement].size == 10) // Si avant size = 10 et maintenant size = 10
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
                else if (lastRoomSize == 10 && tabRoomManagerValues[idRoomToImplement].size == 20) // Si avant size = 10 et maintenant size = 20
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
                else if (lastRoomSize == 20 && tabRoomManagerValues[idRoomToImplement].size == 20) // Si avant size = 20 et maintenant size = 20
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

                //Debug.Log("Est rentre dans if if : ");
                // roomAjouter = true;

                GameObject currentEntity = Instantiate(tabRoomManagerValues[idRoomToImplement].prefabRoom, position, Quaternion.identity);
                currentEntity.name = "Room 10x10 : " + instanceNumber;
                instanceNumber++;
                ajoutRoom = true;
                idLatestRoomImplented = idRoomToImplement;
                lastRoomSize = tabRoomManagerValues[idRoomToImplement].size; // recupère size
                if (lastRoomSize == 10)
                {
                    compt10++;
                }
                else
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
            /*else
            {
                Debug.Log("Nest pas rentre dans if : ");
                roomAjouter = true;
            }*/
        }
        setLastRoomComponent(idLatestRoomImplented);
        return position;
    }

    public void setLastRoomComponent(int id)
    {
        idAvantEndRoom = id;
    }

    /*
    public GameObject getPrefab(RoomManagerScriptableObject data)
    {
        return data.prefabRoom;
    }

    public String getEntre(RoomManagerScriptableObject data)
    {
        return data.entre;
    }

    public String getEntre(RoomManagerScriptableObject data)
    {
        return data.entre;
    }*/


    /******** FIN TEST ********/
    void getEntre()
    {
        Debug.Log("Entre : " + tabRoomManagerValues[1].entre);
    }

    void getEntreeAccepte()
    {
        Debug.Log("Entre Acceptées: " + tabRoomManagerValues[1].entreAccepte);
    }
}
