using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float _timeBetweenWaves = 5f;
    public float _countDown = 5f;
    public int _waveIndex = 0;
    public float _speed = 10f;

   
    public GameObject _virusPrefab;
    private Canvas gameUI;
    public TextMeshProUGUI _virusAlertText;

    private void Start()
    {
        gameUI = GameObject.Find("GameUI").GetComponent<Canvas>();
        _virusAlertText.gameObject.SetActive(false);
    }

    private void Update()
    {     
        _timeBetweenWaves = Random.Range(5, 10);
        CountDown();
        
    }
    private void CountDown()
    {
        if (_countDown <= 0)
        {
            StartCoroutine(SpawnVirusWave());   
            _waveIndex++;
            _countDown = _timeBetweenWaves;
        }
        _countDown -= Time.deltaTime;
    }

    IEnumerator SpawnVirusWave()
    {
        for (int i = 0; i < _waveIndex; i++)
        {
            SpawnVirus();
            yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnVirus()
    {
        _virusAlertText.gameObject.SetActive(true);
        StartCoroutine(VirusAlertTextRoutine());
        Vector2 posToSpawn = new Vector2(Random.Range(20f, -20f), Random.Range(-6f, 6f));
        GameObject newVirus = Instantiate(_virusPrefab, posToSpawn, Quaternion.identity);
    }

    IEnumerator VirusAlertTextRoutine()
    {
        while (true)
        {
            Debug.Log("VirusAlert");
            _virusAlertText.text = "VIRUS ALERT";
            yield return new WaitForSeconds(0.5f);
            _virusAlertText.text = "";
            yield return new WaitForSeconds(0.5f);
            
        }
    }
}
