using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BarrierInfo : MonoBehaviour
{
    [FormerlySerializedAs("sprites")]
    [SerializeField]
    Sprite[] _sprites;

    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void ChangeDirection(int num)
    {
        switch (num)
        {
            case 0:
                _spriteRenderer.sprite = _sprites[0];
                _boxCollider.offset = new Vector2(0f, 0.125f);
                _boxCollider.size = new Vector2(1.1f,0.25f);
                transform.localPosition = new Vector3(0f, -0.5f,0f);
                transform.localScale = new Vector3(1.5f,1f,1f);
                _spriteRenderer.sortingOrder = 3;
                break;
            case 1:
                _spriteRenderer.sprite = _sprites[1];
                _boxCollider.offset = new Vector2(0f, 0.5f);
                _boxCollider.size = new Vector2(0.25f,1.1f);
                transform.localPosition = new Vector3(-0.7f, 0f,0f);
                transform.localScale = new Vector3(1f, 1.5f, 1f);
                _spriteRenderer.sortingOrder = 2;
                break;
            case 2:
                _spriteRenderer.sprite = _sprites[0];
                _boxCollider.offset = new Vector2(0f, 0.875f);
                _boxCollider.size = new Vector2(1.1f,0.25f);
                transform.localScale = new Vector3(1.5f, 1f, 1f);
                transform.localPosition = new Vector2(0f, 0.5f);
                _spriteRenderer.sortingOrder = 2;
                break;
            case 3:
                _spriteRenderer.sprite = _sprites[2];
                _boxCollider.offset = new Vector2(0f, 0.5f);
                _boxCollider.size = new Vector2(0.25f, 1.1f);
                transform.localPosition = new Vector3(0.7f, 0f,0f);
                transform.localScale = new Vector3(1f, 1.5f, 1f);
                _spriteRenderer.sortingOrder = 2;
                break;

        }
    }
}
