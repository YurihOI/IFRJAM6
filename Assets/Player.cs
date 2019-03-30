﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager manager;
    Rigidbody2D rb;
    [SerializeField]
    float speed;
    public PlayerInfo info;
    GameObject currentCol;
    public GameObject carried, npcBehind;

    void Start()
    {
        info = GetComponent<PlayerInfo>();
        rb = GetComponent<Rigidbody2D>();

        info.SetStatus(1, 1, 1, 1, 1);

    }

    void FixedUpdate()
    {
        switch (info.playerState)
        {
            case PlayerInfo.PlayerState.Idle:
                rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;
                if (currentCol != null)
                {
                    if (Input.GetButtonDown("Action"))
                    {
                        Debug.Log("Entrou?");
                        if (currentCol.GetComponent<Funcao>().charUsing == null && npcBehind == null)
                        {
                            rb.velocity = Vector3.zero;
                            info.workingNow = currentCol.GetComponent<Funcao>();
                            transform.position = currentCol.transform.position;
                            currentCol.GetComponent<Funcao>().ChangeCharUsing(info);
                            info.playerState = PlayerInfo.PlayerState.Working;
                        }
                    }

                }
                if (npcBehind != null)
                {
                    if (Input.GetButton("Action"))
                    {
                        Debug.Log("GetNPC");
                        carried = npcBehind;
                        info.playerState = PlayerInfo.PlayerState.Carrying;
                    }
                    if(currentCol != null && Input.GetButton("Action"))
                    {
                        Debug.Log("RemoveNPC");
                        currentCol.GetComponent<Funcao>().ChangeCharUsing(null);
                    }
                }

                break;
            case PlayerInfo.PlayerState.Working:
                if (Input.GetButtonDown("Action"))
                {
                    Debug.Log("Leave Work");
                    info.workingNow.ChangeCharUsing(null);
                    info.playerState = PlayerInfo.PlayerState.Idle;
                }
                break;
            case PlayerInfo.PlayerState.Carrying:
                carried.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
                rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;
                if (Input.GetButtonDown("Action"))
                {
                    if (currentCol != null && currentCol.GetComponent<Funcao>().charUsing == null)
                    {
                        npcBehind = null;
                        carried.transform.position = currentCol.transform.position;
                        currentCol.GetComponent<Funcao>().ChangeCharUsing(carried.GetComponent<PlayerInfo>());
                        carried = null;
                        info.playerState = PlayerInfo.PlayerState.Idle;
                    }
                    else
                    {
                        npcBehind = null;
                        carried.transform.position = transform.position;
                        carried = null;
                        info.playerState = PlayerInfo.PlayerState.Idle;
                    }
                }
                break;

        }
        if (Input.GetButtonDown("Bar"))
        {
            manager.panel.GetComponent<RectTransform>().position = new Vector3(manager.panel.GetComponent<RectTransform>().position.x * -1, manager.panel.GetComponent<RectTransform>().position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Machine"))
        {
            currentCol = col.gameObject;
        }
        if (col.CompareTag("NPC"))
        {
            npcBehind = col.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Machine"))
        {
            currentCol = null;
        }
        if (col.CompareTag("NPC"))
        {
            npcBehind = null;
        }

    }
}
