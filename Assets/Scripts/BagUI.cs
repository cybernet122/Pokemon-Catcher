using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    [SerializeField] GameObject pokemonPrefab, bagObject, smallBagObject;
    public List<GameObject> pokemonInBag = new List<GameObject>();
    public List<PokemonData> pokemonData = new List<PokemonData>();
    [SerializeField] Transform placement, bagPlacement;
    public static BagUI instance;
    public GameObject newPokemon;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        instance = this;
        if (FindObjectsOfType<BagUI>().Length > 1)
            Destroy(gameObject);
        if (pokemonInBag.Count == 0)
        {
            HideSmallBag();
        }
    }

    public void AddPokemon(Texture2D pokeImage,string pokeName, PokemonData newPokemonData)
    {
        HideSmallBag();

        var pokeObject = Instantiate(pokemonPrefab,placement);
        pokeObject.SetActive(false);
        PokemonScript pokeScript = pokeObject.GetComponent<PokemonScript>();

        pokeScript.pokemonData.name = newPokemonData.name;
        pokeScript.pokemonData.spriteUrl = newPokemonData.spriteUrl;
        pokeScript.pokemonData.texture = newPokemonData.texture;
        pokemonData.Add(newPokemonData);

        pokeScript.SetImageAndName(pokeImage, pokeName);
        
        newPokemon = pokeObject;
        if(pokemonInBag.Count < 3)
        {
            pokemonInBag.Add(pokeObject);
            pokemonData.Add(newPokemonData);
            pokeScript.inBag = true;
            pokeObject.transform.parent = bagPlacement;
            pokeObject.SetActive(true);
            newPokemon = null;
            EnableSmallBag();
        }
        else
        {
            pokeObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            HideSmallBag();
        }
    }

    public void AddNewCard(PokemonData newPokemonData)
    {
        pokemonInBag.Add(newPokemon);
        pokemonData.Add(newPokemonData);
        newPokemon.transform.parent = bagPlacement;
        transform.GetChild(0).gameObject.SetActive(false);


        EnableSmallBag();
    }

    public void RemoveCard(GameObject pokeObject, PokemonData pokeData)
    {
        if (pokemonData.Contains(pokeData))
            pokemonData.Remove(pokeData);
        if (pokemonInBag.Contains(pokeObject))
            pokemonInBag.Remove(pokeObject);
        Destroy(pokeObject);
        newPokemon.GetComponent<PokemonScript>().AddCard();
    }

    public void DiscardNewCard()
    {
        Destroy(newPokemon);
        newPokemon = null;
        transform.GetChild(0).gameObject.SetActive(false);
        EnableSmallBag();
    }

    private void EnableSmallBag()
    {
        for (int i = 0; i < pokemonInBag.Count; i++)
        {
            print(i);
            var pokemon = smallBagObject.transform.GetChild(i);
            pokemon.gameObject.SetActive(false);
            var pokemonScript = pokemonInBag[i].GetComponent<PokemonScript>();
            pokemon.GetComponentInChildren<TextMeshProUGUI>().text = pokemonScript.pokemonData.name;
            pokemon.GetComponentInChildren<RawImage>().texture = pokemonScript.pokemonData.texture;
            pokemon.gameObject.SetActive(true);
        }
        smallBagObject.SetActive(true);
    }

    private void HideSmallBag()
    {
        smallBagObject.SetActive(false);
        for (int i = 0; i < smallBagObject.transform.childCount; i++)
        {
            smallBagObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
