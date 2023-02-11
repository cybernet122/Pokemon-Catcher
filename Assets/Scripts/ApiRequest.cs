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
    private void Update()
    { 
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            PokemonData pokemonData = new PokemonData();
            GetNewPokemon(pokemonData);
        }*/
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
        //GetSprite(pokemonData.spriteUrl).Then(texture => pokemonData.texture);
        
        StartCoroutine(PokemonSprite(pokemonData));
    }
    
    private IEnumerator PokemonSprite(PokemonData pokemonData)
    {
        var spriteRequest = UnityWebRequestTexture.GetTexture(pokemonData.spriteUrl);
        yield return spriteRequest.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(spriteRequest);
        texture.filterMode = FilterMode.Point;
        pokemonData.texture = texture;        
    }

    private IPromise<Texture2D> GetSprite(string pokeSprite)
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
                    var bytes = ev.Result;
                    Texture2D texture = new Texture2D(96,96, TextureFormat.RGBA32, false);
                    texture.LoadRawTextureData(ev.Result);
                    texture.Apply();
                    promise.Resolve(texture);
                }
            };
            request.DownloadDataAsync(new Uri(pokeSprite), null);
        }
        return promise;
    }
}
