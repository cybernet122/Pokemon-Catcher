using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
public class BagUI : MonoBehaviour
{
    [SerializeField] GameObject pokemonPrefab, bagObject, smallBagObject, selector, loadingPanel;
    public List<GameObject> pokemonInBag = new List<GameObject>();
    public List<PokemonData> pokemonData = new List<PokemonData>();
    [SerializeField] Transform bagUIPlacement, smallBagPlacement, newPokemonPlacement;
    public static BagUI instance;
    GameObject newPokemon;
    [SerializeField] Image loadingScreen;
    [SerializeField] Slider loadingSlider;

    private void Start()
    {
        StartCoroutine(LoadingScreen());
        HideSmallBag();
        transform.GetChild(0).gameObject.SetActive(false);
        instance = this;
        if (FindObjectsOfType<BagUI>().Length > 1)
            Destroy(gameObject);
        selector.SetActive(true);
        LoadParty();
        
    }

    private IEnumerator LoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
        float count = 0f;
        float loadTime = 4f;
        loadingSlider.maxValue = loadTime;
        while (count < loadTime)
        {
            count += Time.deltaTime;
            loadingSlider.value = count;
            yield return null;
        }
        loadingPanel.SetActive(false);
        float alpha = loadingScreen.color.a;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            loadingScreen.color = new Color(171,171,171,alpha);
            yield return null;
        }
        loadingScreen.gameObject.SetActive(false);
    }

    public void AddPokemon(PokemonData newPokemonData)
    {
        GameObject pokeObject;
        PokemonScript pokeScript;
        CreatePokemon(newPokemonData, out pokeObject, out pokeScript);
        newPokemon = pokeObject;
        if (pokemonInBag.Count < 3)
        {
            pokemonInBag.Add(pokeObject);
            pokeScript.inBag = true;
            pokemonData.Add(newPokemonData);
            pokeObject.transform.SetParent(bagUIPlacement);
            EnableSmallBag();
            SaveParty();
        }
        else
        {
            pokeObject.transform.SetParent(newPokemonPlacement);
            pokeObject.transform.localPosition = Vector2.zero;
            transform.GetChild(0).gameObject.SetActive(true);
            HideSmallBag();
        }
    }

    private void CreatePokemon(PokemonData newPokemonData, out GameObject pokeObject, out PokemonScript pokeScript)
    {
        pokeObject = Instantiate(pokemonPrefab, bagUIPlacement);
        pokeScript = pokeObject.GetComponent<PokemonScript>();
        pokeScript.pokemonData.texture = newPokemonData.texture;
        pokeScript.SetImageAndName(newPokemonData.texture, newPokemonData.name);
        pokeScript.pokemonData.name = newPokemonData.name;
        pokeScript.pokemonData.spriteUrl = newPokemonData.spriteUrl;
    }

    public void AddNewPokemon(PokemonData newPokemonData)
    {
        pokemonInBag.Add(newPokemon);
        pokemonData.Add(newPokemonData);
        newPokemon.transform.SetParent(bagUIPlacement);
        transform.GetChild(0).gameObject.SetActive(false);
        EnableSmallBag();
    }

    public void RemoveCard(GameObject pokeObject, PokemonData pokeData)
    {
        for (int i = 0; i < pokemonData.Count; i++)
        {
            if (pokemonData[i].name == pokeData.name)
                pokemonData.RemoveAt(i);
        }
        if (pokemonInBag.Contains(pokeObject))
            pokemonInBag.Remove(pokeObject);
        Destroy(pokeObject);
        newPokemon.GetComponent<PokemonScript>().AddPokemon();
        SaveParty();
    }

    public void DiscardNewCard()
    {
        newPokemon.GetComponentInChildren<DoTweenAnimation>().StopAnimation();
        Destroy(newPokemon);
        newPokemon = null;
        transform.GetChild(0).gameObject.SetActive(false);
        EnableSmallBag();
        SaveParty();
    }

    private void EnableSmallBag()
    {
        selector.SetActive(true);

        for (int i = 0; i < pokemonInBag.Count; i++)
        {
            var smallBag = smallBagObject.transform.GetChild(i);
            smallBag.gameObject.SetActive(false);
            var pokemonScript = pokemonInBag[i].GetComponent<PokemonScript>();
            smallBag.GetComponentInChildren<TextMeshProUGUI>().text = pokemonScript.pokemonData.name;
            smallBag.GetComponentInChildren<RawImage>().texture = pokemonScript.pokemonData.texture;
            smallBag.gameObject.SetActive(true);
            
        }
        smallBagObject.transform.parent.gameObject.SetActive(true);
    }

    private void HideSmallBag()
    {
        selector.SetActive(false);
        smallBagObject.transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < smallBagObject.transform.childCount; i++)
        {
            smallBagObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SaveParty()
    {
        string json = JsonHelper.ToJson(pokemonData.ToArray(), true);
        File.WriteAllText(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "PokemonParty.json", json);
    }

    private void LoadParty()
    {
        if (File.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "PokemonParty.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "PokemonParty.json");
            List<PokemonData> pokemonDatas = JsonHelper.FromJson<PokemonData>(json).ToList();
            StartCoroutine(CreatePokemonParty(pokemonDatas));
        }
    }

    private IEnumerator CreatePokemonParty(List<PokemonData> pokemonDatas)
    {
        foreach (PokemonData pokemonData in pokemonDatas)
        {
            var spriteRequest = UnityWebRequestTexture.GetTexture(pokemonData.spriteUrl);
            yield return spriteRequest.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(spriteRequest);
            texture.filterMode = FilterMode.Point;
            pokemonData.texture = texture;
            AddPokemon(pokemonData);
        }
    }  
    
    public void ExitGame() =>  Application.Quit();
}
