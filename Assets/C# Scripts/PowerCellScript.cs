using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCellScript : MonoBehaviour {
    public Shader spriteDefault, whiteSprite;
    public GoramScript fortress;
    public Vector2 healthBaseSize;
    public float cellHealth, cellHealthMax, cellHealthPercentage, hitTimer, hitTimerMax;
    public bool isMainCell;
    public MarcelloScript marcello;
    public string bodyPart;
    public List<PowerCellScript> bodyArea;
    public SpriteRenderer[] cellPieces;
    public Color red1, red2, red3, red4, red5;
	// Use this for initialization
	void Start () {
        cellHealth = cellHealthMax;
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
        marcello = GameObject.Find("Marcello Object").GetComponent<MarcelloScript>();
        cellPieces = GetComponentsInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        cellHealthPercentage = cellHealth / cellHealthMax;
        if(cellHealth <= 0 && isMainCell == false){
            fortress.CheckParts();
        }else if(cellHealth <= 0 && isMainCell == true){
            Destroy(gameObject);
        }
        hitTimer -= Time.deltaTime;
        if(hitTimer < hitTimerMax - .1f){
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().material.shader = spriteDefault;
                }
            }
        }
        if(cellHealthPercentage < 1 && cellHealthPercentage > .8f){
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = red1;
                }
            }
        }else if(cellHealthPercentage < .8f && cellHealthPercentage > .6f){
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = red2;
                }
            }
        }else if (cellHealthPercentage < .6f && cellHealthPercentage > .4f)
        {
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = red3;
                }
            }
        }else if (cellHealthPercentage < .4f && cellHealthPercentage > .2f)
        {
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = red4;
                }
            }
        }else if (cellHealthPercentage < .2f)
        {
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = red5;
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player Sword" && hitTimer <= 0){
            cellHealth -= marcello.swordDamage;
            fortress.health -= marcello.swordDamage;
            hitTimer = hitTimerMax;
            gameObject.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if(other.gameObject.tag == "Player Projectile" && hitTimer <= 0){
            cellHealth -= marcello.gunDamage;
            fortress.health -= marcello.gunDamage;
            hitTimer = hitTimerMax;
            foreach (SpriteRenderer piece in cellPieces)
            {
                if (piece.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
                }
            }
        }
    }
    public void DefineList(List<PowerCellScript> cellList)
    {
        cellList.Remove(this.GetComponent<PowerCellScript>());
        Destroy(gameObject);
    }
}
