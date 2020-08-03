﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Variables

    #region References
    public Vector3 Spawn;
    public GameObject Clone;
    #endregion

    #region Inputs
    [Header("INPUT")]
    public KeyCode MoveUp;
    public KeyCode MoveUp_Alt;
    [Space(5)]
    public KeyCode MoveLeft;
    public KeyCode MoveLeft_Alt;
    [Space(5)]
    public KeyCode MoveRight;
    public KeyCode MoveRight_Alt;
    [Space(5)]
    public KeyCode MoveDown;
    public KeyCode MoveDown_Alt;
    

    #endregion

    #region Tuning
    [Header("TUNING")]
    public float _Step = 1;
    public float _RewindTimer = 30;
    
    #endregion

    #region Timers
    float _Timer = 0;
    #endregion

    [HideInInspector]
    public List<Vector3> ListOf_Movements;
    public List<float> ListOf_Timing;
    public List<bool> ListOf_Interaction;

    #endregion

    private void Start()
    {
        Spawn = transform.position; 
    }

    private void Update()
    {
        
        _Timer += Time.deltaTime;

        if (_Timer >= _RewindTimer)
        {
            Rewind();
        }


        #region Input Handling
        if (Input.GetKeyDown(MoveUp)|| Input.GetKeyDown(MoveUp_Alt))
        {
            Move(0, _Step);
        }
        else if (Input.GetKeyDown(MoveLeft) || Input.GetKeyDown(MoveLeft_Alt))
        {
            Move(-_Step, 0);
        }
        else if (Input.GetKeyDown(MoveRight) || Input.GetKeyDown(MoveRight_Alt))
        {
            Move(_Step, 0);
        }
        else if (Input.GetKeyDown(MoveDown) || Input.GetKeyDown(MoveDown_Alt))
        {
            Move(0, -_Step);
        }
        else if (Input.GetKeyDown("space"))
        {
            
            ListOf_Movements.Add(transform.position);
            float currentTime = _Timer;
            ListOf_Timing.Add(currentTime);

            ListOf_Interaction.Add(true);

        }
        #endregion
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("clone"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else if (collision.gameObject.layer == 8)
        {
            Rewind();
        }
    }

    public void Move(float horizontal, float vertical)
    {

        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + new Vector3(horizontal, vertical));
        GetComponent<BoxCollider2D>().enabled = true;

        if (hit.transform == null || hit.collider.gameObject.layer != 8 )
        {
            transform.position = transform.position + new Vector3(horizontal, vertical);
            ListOf_Movements.Add(transform.position);

            float currentTime = _Timer;
            ListOf_Timing.Add(currentTime);

            ListOf_Interaction.Add(false);
        }
        
    }

    public void Rewind()
    {
        

        if (ListOf_Movements.Count > 0) //Se mi sono mosso almeno una volta
        {
            #region Crea Clone
            GameObject newClone = Instantiate(Clone, transform.position, Quaternion.identity); //creo clone

            newClone.GetComponent<CloneBehaviour>().ListOf_Movements = new List<Vector3>(ListOf_Movements); //gli do la lista dei movimenti
            newClone.GetComponent<CloneBehaviour>().ListOf_Timing = new List<float>(ListOf_Timing); //e la lista dei tempi
            newClone.GetComponent<CloneBehaviour>()._Timer = _Timer; //e il tempo totale
            newClone.GetComponent<CloneBehaviour>().ListOf_Interactions = new List<bool>(ListOf_Interaction);
            #endregion

            //Riporta giocatore alla partenza
            transform.position = Spawn;

            //Pulisco liste
            for (int i = ListOf_Movements.Count - 1; i > -1; i--)
            {
                ListOf_Movements.RemoveAt(i);
                ListOf_Timing.RemoveAt(i);
                ListOf_Interaction.RemoveAt(i);
            }
        }

        _Timer = 0;
    }

    
}
