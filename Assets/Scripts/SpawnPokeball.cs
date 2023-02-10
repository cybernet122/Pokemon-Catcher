using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPokeball : MonoBehaviour
{
    [SerializeField] GameObject pokeball;
    List<GameObject> pokeballs = new List<GameObject>();

    private void Start()
    {
        Invoke("NewPokeballs",0.2f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewPokeballs();
        }
    }

    public void NewPokeballs()
    {
        Collider2D collider = GetComponent<PolygonCollider2D>();
        Bounds bounds = collider.bounds;

        float rangeOfX = bounds.size.x * 0.5f;
        float rangeOfY = bounds.size.y * 0.5f;
        int amount = Random.Range(3, 6);
        for (int i = 0; i < amount; i++)
        {
            Vector3 newVec = transform.TransformPoint(new Vector3(
                   Random.Range(-rangeOfX, rangeOfX),
                   Random.Range(-rangeOfY, rangeOfY),
                   2));
            while (!collider.OverlapPoint(newVec))
            {
                newVec = transform.TransformPoint(new Vector3(
                   Random.Range(-rangeOfX, rangeOfX),
                   Random.Range(-rangeOfY, rangeOfY),
                   2));
            }
            GameObject pokeball = Instantiate(this.pokeball);
            pokeball.transform.SetParent(transform);
            pokeball.transform.position = newVec;
            pokeballs.Add(pokeball);
        }
    }

    public void OnOverlap(GameObject pokeball, PokemonData pokemonData)
    {
        if (pokeballs.Contains(pokeball))
        {
            BagUI.instance.AddPokemon(pokemonData);
            pokeballs.Remove(pokeball);
            Destroy(pokeball);
        }
        if (pokeballs.Count == 0)
            NewPokeballs();
    }
}
