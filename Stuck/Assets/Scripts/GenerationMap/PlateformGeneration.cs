using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformGeneration : MonoBehaviour {

    public int nbRooms = 10; // Nb de rooms a génerer en plus par niveau

    public GameObject[] tabRoomDebut; // contient toutes les rooms de départ
    public GameObject[] tabRooms; // contient toutes les rooms
    public GameObject[] tabEndRooms; // contient toutes les rooms de fin

    private int idRoomDebut; 
    private Vector2 lastpositions; // position de la dernière room
    private int idLatestRoomImplented; // Id de la derniere room implémenté
    private string lastRoomAccepter; // RoomAccepter de la derniere room implémenté
    private int idAvantEndRoom; // Id de la derniere room implémenté avant la room de fin

    private GameManager gameManagerScript;
    private int level; // Numero du level du GameManager

    public float tailleBlock = 1;
    private float tailleRoom;

    // Use this for initialization
    void Start ()
    {
        tailleRoom = tailleBlock * 25.6f;

        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        level = gameManagerScript.getLevel();
        Debug.Log("Ramdom dans generation : " + gameManagerScript.getState());
        Random.InitState(gameManagerScript.getState()); // Initialisation du state de Random

        /********* Ajout de la room de départ *********/
        idRoomDebut = Random.Range(0, tabRoomDebut.Length);
        Vector2 position = new Vector2(0, 0);
        Instantiate(tabRoomDebut[idRoomDebut], position, Quaternion.identity);
        lastRoomAccepter = tabRoomDebut[idRoomDebut].gameObject.GetComponent<RoomsInfo>().entreAccepter;
        lastpositions = position;
        
        /********* Ajout des rooms *********/
        for (int room = 1; room <= (nbRooms * level); room++)
        {
            lastpositions = generateRooms(room, lastpositions);
        }

        /********* Ajout de la Room de fin *********/
        bool endRoomAjouter = false;
        // Tant que la room de fin n'a pas été ajouté
        while(endRoomAjouter == false)
        {
            int idEndRoom = Random.Range(0, tabEndRooms.Length);
            string entreAccepterAvantEnd = tabRooms[idAvantEndRoom].gameObject.GetComponent<RoomsInfo>().entreAccepter;
            if (entreAccepterAvantEnd == tabEndRooms[idEndRoom].gameObject.GetComponent<RoomsInfo>().entre)
            {
                // Positionne correctement la room
                switch (entreAccepterAvantEnd)
                {
                    case "S":
                        position = new Vector2(lastpositions.x, lastpositions.y + tailleRoom * 2);
                        break;
                    case "N":
                        position = new Vector2(lastpositions.x, lastpositions.y - tailleRoom * 2);
                        break;
                    case "NO":
                        position = new Vector2(lastpositions.x + tailleRoom * 2, lastpositions.y);
                        break;
                    case "SO":
                        position = new Vector2(lastpositions.x + tailleRoom * 2, lastpositions.y);
                        break;
                    default:
                        break;
                }

                Instantiate(tabEndRooms[idEndRoom], position, Quaternion.identity);
                endRoomAjouter = true;
            }
        }

        /********* Si on est au niveau1 on desactive tous les ennemis *********/
        if(level == 1)
        {
            GameObject[] ennemies = GameObject.FindGameObjectsWithTag("Ennemy");
            foreach (GameObject ennemy in ennemies)
            {
                ennemy.SetActive(false);
            }
        }
    }

    /* Fonction pour générer et instancier une nouvelle room qui correspond à la sortie de la room précédente */
    public Vector2 generateRooms(int room, Vector2 lastposition)
    {
        Vector2 position = lastposition;
        if (room == 1) // Si la seul room implémenté est le level1
        {
            lastRoomAccepter = tabRoomDebut[idRoomDebut].gameObject.GetComponent<RoomsInfo>().entreAccepter;
            position = new Vector2(lastposition.x + tailleRoom, lastposition.y);
        }
        else
        {
            lastRoomAccepter = tabRooms[idLatestRoomImplented].gameObject.GetComponent<RoomsInfo>().entreAccepter;
        }

        bool roomAjouter = false;
        // Tant que la room n'a pas été ajouté
        while (roomAjouter == false)
        {
            int randomLimit;
            randomLimit = tabRooms.Length;
            int idRoomToImplement = Random.Range(0, randomLimit);
            
            /* On verifie si les portes correspondent */
            if (lastRoomAccepter == tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre) 
            {
                /********** Gère la position de la room **********/
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
                
                bool ajout = true;

                // Si la room a besoin de compétance speciale
                if (tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isDash == true
                   || tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isTP == true
                   || tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isWallJump == true
                   || tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isShoot == true)
                {
                    // On verifie que les compétances correspondent
                    ajout = false;
                    if (gameManagerScript.isDashOn() == true && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isDash == true)
                    {
                        ajout = true;
                    }
                    if (gameManagerScript.isTpOn() == true && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isTP == true)
                    {
                        ajout = true;
                    }
                    if (gameManagerScript.isWallJumpOn() == true && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isWallJump == true)
                    {
                        ajout = true;
                    }
                    if (gameManagerScript.isShootOn() == true && tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().isShoot == true)
                    {
                        ajout = true;
                    }

                }

                /********** Instancie la nouvelle Room **********/
                // Si on a pas mis la même room juste avant et qu'on a le droit d'instancier
                if (idRoomToImplement != idLatestRoomImplented && ajout == true) 
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

    /* Fonction pour sauvegarder l'id de la dernière room instancier avant la room de fin */
    public void setLastRoomComponent(int id)
    {
        idAvantEndRoom = id;
    }

}
