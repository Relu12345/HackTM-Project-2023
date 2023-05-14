using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateColor : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Sprite darkSprite;
    public Text text;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(sprite.sprite == darkSprite)
        {
            text.color = new Color(255, 255, 255);
        }
        else 
        {
            text.color = new Color(0, 0, 0);
        }
    }
}
