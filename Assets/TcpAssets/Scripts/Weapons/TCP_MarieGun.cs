using UnityEngine;
using Photon;
using UnityEngine.UI;

public class TCP_MarieGun : TCP_Weapon_Core
{
    [SerializeField]
    LineRenderer shot;//laser da arma de marie

    void DisableLaser()
    {
        shot.enabled = false;
    }

    [PunRPC]
    protected override void PrimaryFire()
    {
        RaycastHit hit;
        shot.enabled = true;

        if (Physics.Raycast(this.cam.transform.position, this.cam.transform.forward, out hit, this.range))
        {
            shot.startColor = Color.red;
            shot.endColor = Color.red;
            shot.SetPositions(new Vector3[] { this.shotSpawn.transform.position, hit.point });

            if (hit.collider.gameObject.GetComponent<TCP_CharactersHealth>() && 
                hit.collider.gameObject.GetComponent<PhotonView>().owner.GetTeam() != this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
            {
                crosshairHit.enabled = true;//ativando o feedback de acerto
                if(fireDelay != 0)
                {
                    Invoke("DisableHitFeedback", fireDelay / 2);
                }
                else { Invoke("DisableHitFeedback", fireDelay); }

                hit.collider.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, damage, false, PhotonNetwork.player.ID);//dano
            }
        }
        else
        {
            shot.startColor = Color.red;
            shot.endColor = Color.red;
            shot.SetPositions(new Vector3[] { this.shotSpawn.transform.position, this.cam.transform.forward * range });//ativando o laser caso n acerte um jogador
        }
        Invoke("DisableLaser", fireDelay);
    }
}
