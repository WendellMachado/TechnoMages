using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TCP_Skills_GurungMultiNades : TCP_Skill_Core {

	[SerializeField]
	private Transform camera;
	[SerializeField]
	private float throwingForce;

	protected override void UseSkill (){
		GameObject nade = PhotonNetwork.Instantiate("GurungMultiNade", camera.transform.position+(camera.transform.forward*1f), camera.transform.rotation, 0);
        nade.GetComponent<TCP_SkillsObjects>().PlayerOwner = this.gameObject.GetComponent<PhotonView>().owner;
		Rigidbody rb = nade.GetComponent<Rigidbody> ();
		rb.AddForce (camera.transform.forward * throwingForce, ForceMode.VelocityChange);
        putOnCooldown = true;
	}
}
