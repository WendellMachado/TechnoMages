using UnityEngine;
using UnityEngine.UI;

public class TCP_BasicGun : TCP_Weapon_Core
{   
    [PunRPC]
    protected override void PrimaryFire()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.cam.transform.position, this.cam.transform.forward, out hit, this.range))
        {
            if (hit.collider.gameObject.GetComponent<TCP_Character>()
              && hit.collider.gameObject.GetComponent<PhotonView>().owner.GetTeam() != this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
            {
                crosshairHit.enabled = true;
                Invoke("DisableHitFeedback", fireDelay / 2);

                hit.collider.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.Others, damage, false, PhotonNetwork.player.ID);
            }
        }
    }
}
