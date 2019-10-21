using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCP_Skills_GurungShield : TCP_Skill_Core
{

	[SerializeField]
	Camera cam;
	[SerializeField]
	float shieldRange, shieldAmount;

	protected override void UseSkill()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shieldRange))
		{
			if (hit.collider.gameObject.GetComponent<TCP_Character>() 
                && hit.collider.GetComponent<PhotonView>().owner.GetTeam() == this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
			{
				hit.collider.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, shieldAmount, true, PhotonNetwork.player.ID);
                putOnCooldown = true;
			}
		}
	}

}
