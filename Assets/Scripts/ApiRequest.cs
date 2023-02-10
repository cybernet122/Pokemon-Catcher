using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokeApiNet;
using RSG;
using UnityEngine.Networking;
using System.Net;
using System;
public class ApiRequest : MonoBehaviour
{
    public static ApiRequest instance;
    PokeApiClient pokeClient;
    //[SerializeField] RawImage rawImage;

    private void Start()
    {
        instance = this;
        if (FindObjectsOfType<ApiRequest>().Length > 1)
            Destroy(gameObject);
        pokeClient = new PokeApiClient();
    }

    async public void GetNewPokemon(PokemonData pokemonData)
    {
        int rng = UnityEngine.Random.Range(1, 100);
        Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>(rng);
        char[] a = pokemon.Name.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        string s = new string(a);

        pokemonData.name = s;
        pokemonData.spriteUrl = pokemon.Sprites.FrontDefault;
        /*GetSprite(pokeSprite).Then(texture => rawImage.texture);*/
        StartCoroutine(NewPokemon(pokemonData));
    }

    private IEnumerator NewPokemon(PokemonData pokemonData)
    {
        var spriteRequest = UnityWebRequestTexture.GetTexture(pokemonData.spriteUrl);
        yield return spriteRequest.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(spriteRequest);
        texture.filterMode = FilterMode.Point;
        pokemonData.texture = texture;
        //BagUI.instance.AddPokemon(pokemonData); //save texture locally
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
