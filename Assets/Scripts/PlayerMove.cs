using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Joystick joystick;
    public CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 10.0f;
    private PlayerEnvanter playerEnvanterScript;
    public PlayerAnimationControl playerAnim;

    private void Start()
    {
        playerAnim = FindObjectOfType<PlayerAnimationControl>();
        playerEnvanterScript = FindObjectOfType<PlayerEnvanter>();
    } 

    void Update()
    {
        Vector3 move = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        controller.Move(move * Time.deltaTime * playerSpeed);
        //Debug.Log("move: " + move);
        
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            playerAnim.WalkAnim();
        }
        else
        {
            playerAnim.IdleAnim();
        }
        

        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("aktarmaAlani"))
        {
            playerEnvanterScript.komurAktariminiDurdur = false;
            playerEnvanterScript.KomuruMasayaAktar();
        }
        if (other.CompareTag("elmas"))
        {
            playerEnvanterScript.ElmasDurusuAyarla(other.gameObject);
        }
        if (other.CompareTag("kilicAlani"))
        {
            playerEnvanterScript.KilicAl();
        }
        if (other.CompareTag("enemy1") && playerEnvanterScript.kilicSatinAldiMi)
        {
            playerEnvanterScript.AttackEnemy();
            Debug.Log("buraya geldi");
        }
        if (other.CompareTag("kan"))
        {
            Debug.Log("baþardýk!");
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("kan"))
        {
            Debug.Log("baþardýk!");
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("aktarmaAlani"))
        {
            playerEnvanterScript.KomuruMasayaAktar();
        }
        if (other.CompareTag("enemy1") && playerEnvanterScript.kilicSatinAldiMi)
        {
            playerEnvanterScript.AttackEnemy();
            Debug.Log("buraya geldi");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("aktarmaAlani"))
        {
            playerEnvanterScript.komurAktariminiDurdur = true;
        }
    }

    
}
