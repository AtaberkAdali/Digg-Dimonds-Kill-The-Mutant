using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomurHediyesiScript : MonoBehaviour
{
    private int hiz = 7;
    public GameObject particalSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayeraDogruGit();
    }
    private void PlayeraDogruGit()
    {
        Vector3 hedefPos = FindObjectOfType<PlayerMove>().transform.position;
        hedefPos.y += 2;
        transform.LookAt(hedefPos);
        transform.position = Vector3.MoveTowards(transform.position, hedefPos, hiz * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, hedefPos);
        if(distance < 0.5f)
        {
            FindObjectOfType<PlayerEnvanter>().KomurHediyesiAl();
            Instantiate(particalSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
