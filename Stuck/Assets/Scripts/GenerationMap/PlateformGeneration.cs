﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformGeneration : MonoBehaviour {

    public int nbRooms = 10; // Nb de rooms a génerer en plus par niveau

    public GameObject[] tabRoomDebut; // Room de départ
    public GameObject[] tabRooms; // contient toutes les rooms
    public GameObject[] tabEndRooms; // contient toutes les rooms de fin

    private int idRoomDebut; 
    private Vector2 lastpositions; // position de la dernière room
    private int idLatestRoomImplented; // Id de la derniere room implémenté
    private string lastRoomAccepter; // RoomAccepter de la derniere room implémenté
    private int lastRoomSize; // Taille de la room précédente
    private int idAvantEndRoom; // Id de la derniere room implémenté avant la room de fin

    private GameManager gameManagerScript;
    private int level; // Numero du level (cf GameManager)

    private GameObject clone; // Va contenir le prefab instantié

    public float tailleBlock = 1;
    private float tailleRoom;

    // Use this for initialization
    void Start ()
    {
        tailleRoom = tailleBlock * 25.6f;

        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        level = gameManagerScript.getLevel();
        Random.state = gameManagerScript.getState(); // On donne le state du random

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
        while(endRoomAjouter == false)
        {
            int idEndRoom = Random.Range(0, tabEndRooms.Length);
            string entreAccepterAvantEnd = tabRooms[idAvantEndRoom].gameObject.GetComponent<RoomsInfo>().entreAccepter;
            if (entreAccepterAvantEnd == tabEndRooms[idEndRoom].gameObject.GetComponent<RoomsInfo>().entre)
            {
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
	
	// Update is called once per frame
	void Update () {
    }

    public Vector2 generateRooms(int room, Vector2 lastposition)
    {
        Vector2 position = lastposition;
        if (room == 1) // Si la seul room implémenté est le level1
        {
            lastRoomAccepter = tabRoomDebut[idRoomDebut].gameObject.GetComponent<RoomsInfo>().entreAccepter;
            lastRoomSize = tabRoomDebut[idRoomDebut].gameObject.GetComponent<RoomsInfo>().size; // recupère size
            position = new Vector2(lastposition.x + tailleRoom, lastposition.y);
        }
        else
        {
            lastRoomAccepter = tabRooms[idLatestRoomImplented].gameObject.GetComponent<RoomsInfo>().entreAccepter;
        }

        bool roomAjouter = false;
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
                if (idRoomToImplement != idLatestRoomImplented && ajout == true) // Si on a pas mis la même room juste avant et qu'on a le droit d'instancier
                {
                    clone = Instantiate(tabRooms[idRoomToImplement], position, Quaternion.identity);
                    roomAjouter = true;
                    idLatestRoomImplented = idRoomToImplement;
                    lastRoomSize = clone.GetComponent<RoomsInfo>().size; // recupère size
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