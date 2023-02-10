using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokeApiNet;
using RSG;
using System.Text;
using UnityEngine.Networking;
using System.Net;
using System;
using UnityEngine.UI;
public class ApiRequest : MonoBehaviour
{
    PokeApiClient pokeClient = new PokeApiClient();
    [SerializeField] RawImage rawImage;

    async public void GetNewPokemon()
    {
        int rng = UnityEngine.Random.Range(1, 100);
        Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>(rng);
        char[] a = pokemon.Name.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        string s = new string(a);


        var pokeSprite = pokemon.Sprites.FrontDefault;
        /*GetSprite(pokeSprite).Then(texture => rawImage.texture);*/
        StartCoroutine(NewPokemon(pokeSprite,s));

    }

    private IEnumerator NewPokemon(string url,string pokeName)
    {
        var spriteRequest = UnityWebRequestTexture.GetTexture(url);
        yield return spriteRequest.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(spriteRequest);
        texture.filterMode = FilterMode.Point;
        PokemonData pokemonData = new PokemonData();
        pokemonData.name = pokeName;
        pokemonData.spriteUrl = url;
        pokemonData.texture = texture;
        BagUI.instance.AddPokemon(texture, pokeName, pokemonData); //save texture locally
    }

    /*private IPromise<Texture2D> GetSprite(string pokeSprite)
    {
        var promise = new Promise<Texture2D>();
        using (var request = new WebClient())
        {
            request.DownloadDataCompleted +=
            (s, ev) =>
            {
                if (ev.Error != null)
                {
                    promise.Reject(ev.Error);
                    Debug.LogError("Something went wrong with the sprite request!");
                }
                else
                {
                    Texture2D texture = new Texture2D(0,0);
                    print(ev.Result.Length);
                    texture.LoadRawTextureData(ev.Result);
                    promise.Resolve(texture);
                }
            };
            request.DownloadDataAsync(new Uri(pokeSprite), "test");
        }
        return promise;
    }*/
}
