using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{
    public GameObject kameraNesnesi;
    public Vector3 eskiKameraPozisyonu;
    public float sure;
    // Start is called before the first frame update
    void Start()
    {
        kameraNesnesi = GameObject.FindWithTag("MainCamera");
        eskiKameraPozisyonu = kameraNesnesi.transform.position;
        sure = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator KamerayiSalla(float MaxSure, float MaxSarsinti, float Titresim)
    {
        while (sure < MaxSure)
        {
            float kameraninXekseni = Random.Range(0f, MaxSarsinti) * Titresim;
            float kameraninYekseni = Random.Range(0f, MaxSarsinti) * Titresim;
            kameraNesnesi.transform.position = new Vector3(eskiKameraPozisyonu.x + kameraninXekseni, eskiKameraPozisyonu.y + kameraninYekseni, eskiKameraPozisyonu.z);
            sure += Time.deltaTime;
            yield return null;
        }
        kameraNesnesi.transform.position = eskiKameraPozisyonu;
        sure = 0;
    }
}
