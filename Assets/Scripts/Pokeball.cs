using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public PokemonData pokemonData;

    private void Start()
    {
        ApiRequest.instance.GetNewPokemon(pokemonData);
    }

    public void OnOverlap()
    {
        GetComponentInParent<SpawnPokeball>().OnOverlap(gameObject, pokemonData);
    }
}
