﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearEnemyMovement : MonoBehaviour {

    private GameObject _Player;
    private GameObject _LeftShooter;
    private GameObject _RightShooter;
    
    private Rigidbody2D rb;
    private Rigidbody2D _PlayerRb;

    private bool _FacingRight;

    private int _LayerMask;
    private int _JumpCoolDown;

    //private Vector2 _JumpForce = new Vector2(Random.Range(0f, 10f), Random.Range(200f, 350f));
    private Vector2 _JumpForce = new Vector2(0, 350);

    private RaycastHit2D _LineOfFire;

    private SpearShooter _SpearThrowR;
    private SpearShooter _SpearThrowL;

    void Start()
    {
        _Player = GameObject.FindWithTag(GameTags.player);
        _PlayerRb = _Player.GetComponent<Rigidbody2D>();

        rb = GetComponent<Rigidbody2D>();

        _LayerMask = LayerMask.GetMask("Player");

        _LeftShooter = GameObject.Find("LeftShooter");
        _RightShooter = GameObject.Find("RightShooter");

        _SpearThrowL = _LeftShooter.GetComponent<SpearShooter>();
        _SpearThrowR = _RightShooter.GetComponent<SpearShooter>();
    }

    void Update()
    {
        //_JumpForce = new Vector2(Random.Range(-100f, 100f), Random.Range(250f, 350f));

        if(_JumpCoolDown < 0)
        {
            _JumpCoolDown = 0;
        }
    }

    public void Attack()
    {
        Jump();
        SearchForTarget();
    }

    void SearchForTarget()
    {
        //determine what side enemy must look at
        if (transform.position.x < _PlayerRb.position.x)
            _FacingRight = true;
        else
            _FacingRight = false;

        //cast raycast in facing direction
        if (_FacingRight)
        {
            _LineOfFire = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, _LayerMask);
            Throw();
        }
        else
        {
            _LineOfFire = Physics2D.Raycast(transform.position, -transform.right, Mathf.Infinity, _LayerMask);
            Throw();
        }  
    }

    void Jump()
    {
        if(_JumpCoolDown == 0)
        {
            //jump
            rb.AddForce(_JumpForce);
            _JumpCoolDown = 25;
        }        
    }

    void Throw()
    {
        //when player is in line of fire
        if (_LineOfFire.collider.tag == GameTags.player)
        {
            if (_FacingRight) // throw spear to right
                _SpearThrowR.ThrowSpear();
            else // throw spear to left
                _SpearThrowL.ThrowSpear();
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.tag == GameTags.ground)
        {
            Debug.Log("collision");
            _JumpCoolDown--;
        }
    }
}