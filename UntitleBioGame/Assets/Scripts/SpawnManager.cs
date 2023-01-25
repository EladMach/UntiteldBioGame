using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  [SerializeField]
  float timeBetweenWaves = 45f;

  [SerializeField]
  GameObject virusPrefab;
  [SerializeField]
  GameObject virusAlert;

  private int waveIndex = 0;

  private float lastWaveTime;

  private void Start()
  {
    lastWaveTime = Time.time;

    virusAlert.SetActive(false);
  }

  private void Update()
  {
    CountDown();
  }
  private void CountDown()
  {
    if (lastWaveTime + timeBetweenWaves < Time.time/*  && waveIndex == 0 */)
    {
      lastWaveTime = Time.time;
      waveIndex++;
      StartCoroutine(VirusAlertTextRoutine());
      StartCoroutine(SpawnVirusWave());
      //   timeBetweenWaves = Random.Range(5, 10);
    }
  }

  IEnumerator SpawnVirusWave()
  {
    for (int i = 0; i < waveIndex * 4; i++)
    {
      SpawnVirus();
      yield return new WaitForSeconds(3f);
    }
  }

  private void SpawnVirus()
  {
    // Vector2 posToSpawn = new Vector2(Random.Range(20f, -20f), Random.Range(-6f, 6f));
    Vector2 posToSpawn = transform.position;
    GameObject newVirus = Instantiate(virusPrefab, posToSpawn, Quaternion.identity);
  }

  IEnumerator VirusAlertTextRoutine()
  {
    while (lastWaveTime + 4 > Time.time)
    {
      virusAlert.SetActive(true);
      //   virusAlertText.text = "VIRUS ALERT";
      yield return new WaitForSeconds(0.5f);
      virusAlert.SetActive(false);
      //   virusAlertText.text = "";
      yield return new WaitForSeconds(0.5f);
    }
  }
}
