using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    public Player player;
    public Collider collider;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player otherPlayer = other.transform.parent.GetComponent<Player>();
            if (player.isMove == true && otherPlayer.isMove == true)
            {
                if (player.collectedAntList.Count >= otherPlayer.collectedAntList.Count)
                {
                    StartCoroutine(player.ColliderToWinIEnum());
                    StartCoroutine(otherPlayer.ThrowAntOutSideIEnum());
                }
                else
                {
                    StartCoroutine(otherPlayer.ColliderToWinIEnum());
                    StartCoroutine(player.ThrowAntOutSideIEnum());
                }
            }
        }
        else if (other.gameObject.name == "Finish")
        {
            player.WinFunc();
        }
    }
}
