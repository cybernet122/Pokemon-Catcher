using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPokeball : MonoBehaviour
{
    PolygonCollider2D polygonCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Pokeball>())
            collision.GetComponent<Pokeball>().OnOverlap();
    }

    private void Update()
    {
        Vector2 newVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        transform.position = newVec;
    }
}
