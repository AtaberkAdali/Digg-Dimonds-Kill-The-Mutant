using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushCoal : MonoBehaviour
{
    public GameObject[] komurParts = new GameObject[8]; //solalt, solüst, sagust, sagalt, soltaban, sagtaban
    public bool[] komurControl = new bool[8];
    public bool suAndaKomurKiriliyorMu = false;
    public GameObject komurHediyesi;
    
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !suAndaKomurKiriliyorMu)
        {
            if (hangiKomureVuruyorBul() != -1 && FindObjectOfType<PlayerEnvanter>().sirtimizdaKacKomurVar < 4)
            {
                //komuru kir.
                StartCoroutine(komuruKir(hangiKomureVuruyorBul()));
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !suAndaKomurKiriliyorMu)
        {
            if(hangiKomureVuruyorBul() != -1 && FindObjectOfType<PlayerEnvanter>().sirtimizdaKacKomurVar < 4)
            {
                //komuru kir.
                StartCoroutine(komuruKir(hangiKomureVuruyorBul()));
            }
        }
    }

    public IEnumerator komuruKir(int komurPartNumber)
    {
        FindObjectOfType<PlayerAnimationControl>().AttackAnim();
        FindObjectOfType<PlayerEnvanter>().playerBalta.SetActive(true);
        suAndaKomurKiriliyorMu = true;
        Debug.Log("Komuru kir.");
        yield return new WaitForSeconds(0.7f);

        StartCoroutine(FindObjectOfType<ShakeCam>().KamerayiSalla(0.3f, 0.2f, 2));

        komurHediyesiniVer(komurParts[komurPartNumber].transform);
        ObjelereRigidbodyEkle(komurParts[komurPartNumber]);
        
        komurControl[komurPartNumber] = false;
        FindObjectOfType<PlayerEnvanter>().kirilanParcaSayisi++;

        yield return new WaitForSeconds(0.3f);
        FindObjectOfType<PlayerAnimationControl>().FinishAttackAnim();
        FindObjectOfType<PlayerEnvanter>().playerBalta.SetActive(false);
        suAndaKomurKiriliyorMu = false;


        if (FindObjectOfType<PlayerEnvanter>().kirilanParcaSayisi == komurParts.Length)
        {
            //Burada yenile artýk kömürleri.
            Debug.Log("1sny sonra komurler yenileneecek.");
            kalanKomurVarsaYokEt();
            yield return new WaitForSeconds(1);
            FindObjectOfType<PlayerEnvanter>().KomurleriYenile();
            Destroy(gameObject);
        }
    }

    private void ObjelereRigidbodyEkle(GameObject GO)
    {
        for (int i = 0; i < GO.transform.childCount; i++)
        {
            //GameObjeye rigidbody ekleyeerek parçalanmasýný saðlýyoruz.
            GameObject child = GO.transform.GetChild(i).gameObject;
            child.AddComponent<Rigidbody>();
        }
        StartCoroutine(kirilanParcayiYokEt(GO));
    }
    IEnumerator kirilanParcayiYokEt(GameObject ParentGO)
    {
        yield return new WaitForSeconds(0.9f);
        Destroy(ParentGO);
    }

    private int hangiKomureVuruyorBul()
    {
        int komurNum = 0;

        Vector3 playerVec = FindObjectOfType<PlayerMove>().transform.position;

        if(playerVec.z  > transform.position.z + 1.5f)
        {
            //Yukarý Alanda
            if(playerVec.x > transform.position.x + 1.5f)
            {
                //sagda
                komurNum = 2;
            }
            else
            {
                //solda
                komurNum = 3;
            }
        }
        else
        {
            //aþagý alanda
            if (playerVec.x > transform.position.x + 1.5f)
            {
                //sagda
                komurNum = 1;
            }
            else
            {
                //solda
                komurNum = 0;
            }
        }

        if (!komurControl[komurNum])
        {
            komurNum += 4;
            if (!komurControl[komurNum])
            {
                //Burada komure yeterince yaklaþmamýþ demektir.
                komurNum = -1;
            }
        }

        return komurNum;
    }

    private void komurHediyesiniVer(Transform GOTransform)
    {
        Instantiate(komurHediyesi, GOTransform.position, Quaternion.identity);
    }

    private void kalanKomurVarsaYokEt()
    {
        for (int i = 0; i < komurParts.Length; i++)
        {
            //kontrol amaçlý yok ediyorum.
            if(komurParts != null)
            {
                Destroy(komurParts[i]);
            }
            Debug.Log("bura çalýþtý");
        }
    }
}
