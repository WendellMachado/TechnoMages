using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCP_Skills_MarieHeal : TCP_Skill_Core
{
    [SerializeField]
    Transform shotSpawn;
    [SerializeField]
    LineRenderer shot;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float range, healValue;

    [PunRPC]
    void DisableLaser()
    {
        shot.enabled = false;
    }

    void DisableLaserInvoked()
    {
        pView.RPC("DisableLaser", PhotonTargets.All);
    }

    [PunRPC]
    void EnableLaser(Vector3 destination)
    {
        shot.enabled = true;
        shot.startColor = Color.green;
        shot.endColor = Color.green;
        shot.SetPositions(new Vector3[] { this.shotSpawn.transform.position, destination });
    }

    protected override void UseSkill()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            pView.RPC("EnableLaser", PhotonTargets.All, hit.point);

            if (hit.collider.gameObject.GetComponent<TCP_Character>() 
                && hit.collider.GetComponent<PhotonView>().owner.GetTeam() == this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
            {
                hit.collider.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, healValue, false, PhotonNetwork.player.ID);
            }
        }
        else
        {
            pView.RPC("EnableLaser", PhotonTargets.All, cam.transform.forward * range);
        }

        Invoke("DisableLaserInvoked", 0.05f);
    }
}
