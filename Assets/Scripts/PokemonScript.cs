using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonScript : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] TextMeshProUGUI pokeName;
    [SerializeField] Button discardButton;
    public PokemonData pokemonData;
    public bool inBag;

    private void Start()
    {
        if (!inBag)
            discardButton.gameObject.SetActive(false);
        else
            discardButton.gameObject.SetActive(true);
    }

    public void SetImageAndName(Texture2D pokeSprite, string name)
    {
        rawImage.texture = pokeSprite;
        pokeName.text = name; 
    }
    public void SetImage(Texture2D texture) => rawImage.texture = texture;
    public void DiscardCard()
    {
        if (inBag)
            BagUI.instance.RemoveCard(gameObject, pokemonData);
    }

    public void AddPokemon()
    {
        inBag = true;
        discardButton.gameObject.SetActive(true);
        BagUI.instance.AddNewPokemon(pokemonData);
    }
}
