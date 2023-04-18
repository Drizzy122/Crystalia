using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDataPersistence
{
    private AudioSource audioS;
    public AudioClip coin;

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private MeshRenderer visual;
    private ParticleSystem collectParticle;
    private bool collected = false;

    private void Awake()
    {
        visual = this.GetComponentInChildren<MeshRenderer>();
        audioS = GetComponent<AudioSource>(); ;

        collectParticle = this.GetComponentInChildren<ParticleSystem>();
        collectParticle.Stop();
    }

    public void LoadData(GameData data)
    {
        data.coinsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            visual.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }
        data.coinsCollected.Add(id, collected);
    }

    private void OnTriggerEnter()
    {
        if (!collected)
        {
            collectParticle.Play();
            CollectCoin();
            // Play sound effect
            
        }
    }

    private void CollectCoin()
    {
        collected = true;
        visual.gameObject.SetActive(false);
        GameEventsManager.instance.CoinCollected();
        audioS.clip = coin;
        audioS.Play();

    }

}
