using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerEnvanter : MonoBehaviour
{
    public TextMeshProUGUI mytext;
    public GameObject parentKomurGameObje;
    public int kirilanParcaSayisi = 0;//þu ana kadar mevcut olan kömürlükten kaç tane kýrdýk(yenilemek için)

    public GameObject initPosElmasPlayer;//elmasin kafadaki ilk konumu.

    public GameObject[] sirttakiKomurler = new GameObject[4];
    public GameObject[] masadakiKomurler = new GameObject[4];
    public GameObject[] elmaslar = new GameObject[3];
    public GameObject elmasParticleEffect;

    public GameObject playerBalta;
    public GameObject playerKilic;//bunu da saldýrýrken kullanacagiz.
    public bool kilicSatinAldiMi = false;

    public GameObject gostermelikKilic;

    public GameObject kanPS;
    public Transform initPosKanPS;

    private Vector3[] masadakiKomurlerDefaultPos = new Vector3[4];

    public int sirtimizdaKacKomurVar = 0;//sýrtýmýzda kaç tane hediye var
    public int masadaKacKomurVar = 0;//masada kaç tane hediye var
    public int yandaKacElmasVar = 0;//yanda kaç tane hediye var
    public int kafamizdaKacElmasVar = 0;//Player'ýn elmas sayisi

    public int dusmanaKacKereVurdu = 0;
    public int dusmanKacKeredeOlecek = 4;


    public bool komurAktariminiDurdur = false;
    private bool elmasUretimiVar = false;

    private PlayerMove playerMoveScript;

    private bool suAndaSaldiriyorMu = false;
    private bool dusmanOlduMu = false;

    public GameObject winPanel;


    // Start is called before the first frame update
    void Start()
    {
        playerMoveScript = FindObjectOfType<PlayerMove>();
        for (int i = 0; i < 4; i++)
        {
            masadakiKomurlerDefaultPos[i] = masadakiKomurler[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (elmasUretimiVar)
        {
            for (int i = 0; i < 4; i++)
            {
                HareketEt(masadakiKomurler[i], elmaslar[yandaKacElmasVar],3);
            }
        }
    }

    public void KomurHediyesiAl()
    {
        //burda da arka tarafýnda biriktirmeyi yapacaksýn.
        sirttakiKomurler[sirtimizdaKacKomurVar].SetActive(true);
        sirtimizdaKacKomurVar++;
    }

    public void KomuruMasayaAktar()
    {
        if (!komurAktariminiDurdur && sirtimizdaKacKomurVar > 0 && !elmasUretimiVar)
        {
            sirttakiKomurler[sirtimizdaKacKomurVar - 1].SetActive(false);
            sirtimizdaKacKomurVar--;

            masadakiKomurler[masadaKacKomurVar].SetActive(true);
            masadaKacKomurVar++;
            if(masadaKacKomurVar >= 4)
            {
                StartCoroutine(ElmasUret());
            }
        }
    }

    IEnumerator ElmasUret()
    {
        elmasUretimiVar = true; 
        yield return new WaitForSeconds(1.5f);
        Instantiate(elmasParticleEffect, elmaslar[yandaKacElmasVar].transform.position, Quaternion.identity);
        elmasUretimiVar = false;
        for (int i = 0; i < 4; i++)
        {
            masadakiKomurler[i].SetActive(false);
            masadakiKomurler[i].transform.position = masadakiKomurlerDefaultPos[i];
            masadaKacKomurVar = 0;
        }
        yield return new WaitForSeconds(0.5f);
        elmaslar[yandaKacElmasVar].SetActive(true);
        yandaKacElmasVar++;
    }

    public void ElmasDurusuAyarla(GameObject elmas)
    {
        elmas.transform.position = new Vector3(initPosElmasPlayer.transform.position.x, initPosElmasPlayer.transform.position.y + (FindObjectOfType<PlayerEnvanter>().kafamizdaKacElmasVar * 1),
            initPosElmasPlayer.transform.position.z);
        elmas.transform.parent = initPosElmasPlayer.transform;
        kafamizdaKacElmasVar++;
    }

    public void KilicAl()
    {
        if(kafamizdaKacElmasVar >= 3)
        {
            kilicSatinAldiMi = true;
            gostermelikKilic.SetActive(false);
            for (int i = 0; i < initPosElmasPlayer.transform.childCount; i++)
            {
                GameObject child = initPosElmasPlayer.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
        else
        {
            //3 elmasa ihtiyacýn var messageE'yi çaðýr.
            string uyari = "3 elmasa ihtiyacýn var";
            StartCoroutine(MessageE(uyari, 1));
        }
    }

    public void AttackEnemy()
    {
        Debug.Log("saldirmasi lazimdi");
        if (!suAndaSaldiriyorMu && !dusmanOlduMu)
        {
            Debug.Log("saldirmasi lazimdi.0");
            StartCoroutine(AttackEnemyE());
        }
    }

    IEnumerator AttackEnemyE()
    {
        suAndaSaldiriyorMu = true;
        playerKilic.SetActive(true);
        GameObject geciciObje = Instantiate(kanPS, initPosKanPS.position, Quaternion.identity);
        if (dusmanKacKeredeOlecek > dusmanaKacKereVurdu)
        {
            playerMoveScript.playerAnim.AttackAnim();
            yield return new WaitForSeconds(0.7f);
            playerMoveScript.playerAnim.EnemyDarbeAnim();
            StartCoroutine(FindObjectOfType<ShakeCam>().KamerayiSalla(0.4f, 0.3f, 3));
            yield return new WaitForSeconds(0.3f);
            playerMoveScript.playerAnim.FinishAttackAnim();
            yield return new WaitForSeconds(0.1f);
            playerMoveScript.playerAnim.EnemyIdleAnim();
            dusmanaKacKereVurdu++;
        }
        else
        {
            //die
            playerMoveScript.playerAnim.AttackAnim();
            yield return new WaitForSeconds(0.7f);
            playerMoveScript.playerAnim.EnemydieAnim();
            StartCoroutine(FindObjectOfType<ShakeCam>().KamerayiSalla(0.4f, 0.7f, 5));
            yield return new WaitForSeconds(0.3f);
            playerMoveScript.playerAnim.FinishAttackAnim();

            playerMoveScript.playerAnim.DanceAnim();
            dusmanOlduMu = true;
            yield return new WaitForSeconds(7);
            winPanel.SetActive(true);

        }
        playerKilic.SetActive(false);
        suAndaSaldiriyorMu = false;
        yield return new WaitForSeconds(3f);
        Destroy(geciciObje);
    }


    public void KomurleriYenile()
    {
        kirilanParcaSayisi = 0;
        Instantiate(parentKomurGameObje, parentKomurGameObje.transform.position, Quaternion.identity);
    }

    public void HareketEt(GameObject hareketEdecekGO, GameObject TargetGO, float hiz)
    {
        hareketEdecekGO.transform.LookAt(TargetGO.transform.position);
        hareketEdecekGO.transform.position = Vector3.MoveTowards(hareketEdecekGO.transform.position, TargetGO.transform.position, hiz * Time.deltaTime);
    }

    public IEnumerator MessageE(string yazi,float sure)
    {
        mytext.text = yazi.ToString();
        yield return new WaitForSeconds(sure);
        mytext.text = " ";
    }
}
