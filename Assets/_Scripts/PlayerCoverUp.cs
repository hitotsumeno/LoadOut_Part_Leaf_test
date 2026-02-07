using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoverUp : MonoBehaviour
{
    //Sprite Renders
    [SerializeField] private SpriteRenderer brudSR;
    [SerializeField] private SpriteRenderer lystyaSR;

    [SerializeField] private Color brudAlfa;
    [SerializeField] private Color lystyaAlfa;


    //bools for CoverUps
    private bool isDirtCovered;
    private bool isLeafCovered;

    //CoverUp Metrics
    [Range(0, 3)]public float _dirtCoverUpLevel = 0f;
    [Range(0, 3)] public float _leafCoverUpLevel = 0f;

    //SpriteRenders
    [SerializeField] private Sprite[] dirtSprite;
    [SerializeField] private Sprite[] leafSprite;

    private void Start()
    {
        initSpriteRenderers();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    addDirt(.5f);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    addLeafs(.5f);
        //}
    }
    private void initSpriteRenderers()
    {
        brudSR.sprite = dirtSprite[0];
        brudAlfa = brudSR.color;

        lystyaSR.sprite = leafSprite[0];
        lystyaAlfa = lystyaSR.color;
    }

    #region Add substances
    private void addDirt(float dirtLevel)
    {
        _dirtCoverUpLevel += dirtLevel;

        brudAlfa.a = _dirtCoverUpLevel;
        brudSR.color = brudAlfa;

        if (_dirtCoverUpLevel > 1 && _dirtCoverUpLevel < 2)
        {
            brudSR.sprite = dirtSprite[1];
        }

        if (_dirtCoverUpLevel > 2 && _dirtCoverUpLevel < 3)
        {
            brudSR.sprite = dirtSprite[2];
        }
        if (_dirtCoverUpLevel >= 3)
        {
            isDirtCovered = true;
        } 
    }

    private void addLeafs(float leafLevel)
    {
        _leafCoverUpLevel += leafLevel;

        lystyaAlfa.a = _leafCoverUpLevel;
        lystyaSR.color = lystyaAlfa;

        if (_leafCoverUpLevel > 1 && _leafCoverUpLevel < 2)
        {
            lystyaSR.sprite = leafSprite[1];
        }
        if (_leafCoverUpLevel >= 2)
        {
            isLeafCovered = true;
        }
    }
    #endregion

    #region Future update for the substances
    //private void AddSubstance(float substenceLevel, float addSubstenceLevel, SpriteRenderer affectedRenderer, Color rendererAlfa,float maxSubstanceLevel)
    //{
    //    substenceLevel += addSubstenceLevel;

    //    rendererAlfa.a = substenceLevel;
    //    affectedRenderer.color = rendererAlfa;

    //    if (substenceLevel > 1 && substenceLevel < maxSubstanceLevel)
    //    {

    //    }
    //}
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dirt") && !isDirtCovered)
        {
            if (_dirtCoverUpLevel < 3)
            {
                addDirt(.5f);
                //ChangeState(isDirtCovered, dirtSprite);
            }

            Debug.Log("dirt - Cover up level " + _dirtCoverUpLevel);
        }
        if (collision.CompareTag("leafPile") && isDirtCovered)
        {
            if (_leafCoverUpLevel < 2)
            {
                addLeafs(.5f);
            }
            Debug.Log("leaf - Cover up level " + _leafCoverUpLevel);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Kushch"))
        {
            if (isLeafCovered)
            {
                Debug.Log("Level complete");
            }
        }
    }
    #endregion
}
