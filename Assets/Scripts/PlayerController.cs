using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject CRTFLASH;
    [SerializeField] GameObject BG, UI, DuckSpawner, duckPrefab, Pause;
    public int ammo = 3;
    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && isPaused == false)
        {
            isPaused = true;
            Time.timeScale = 0f;
            Pause.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Return) && isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1f;
            Pause.SetActive(false);
        }
        if(Input.GetMouseButtonDown(0) && DuckController.instance == true && ammo != 0)
        {
            StartCoroutine(LightGunFlash(0.02f));
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                DuckController.instance.Down();
            }
            ammo--;
        }
        if (ammo == 0 && DuckController.instance == true)
        {
            DuckController.instance.StartCoroutine(DuckController.instance.DisappearAfterTime(1f));
            StartCoroutine(SpawnDuck(1f));
        }
    }

    IEnumerator SpawnDuck(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(duckPrefab, DuckSpawner.transform);
    }

    IEnumerator LightGunFlash(float time)
    {
        CRTFLASH.SetActive(true);
        DuckController.instance.transform.GetChild(0).gameObject.SetActive(true);
        UI.SetActive(false);
        BG.SetActive(false);
        yield return new WaitForSeconds(time);
        DuckController.instance.transform.GetChild(0).gameObject.SetActive(false);
        CRTFLASH.SetActive(false);
        UI.SetActive(true);
        BG.SetActive(true);
    }
}
