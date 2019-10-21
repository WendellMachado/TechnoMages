using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TCP_Gurung_MultiNade : TCP_SkillsObjects
{

	[SerializeField]
	private float throwForce;

	void OnCollisionEnter (Collision col){
		if (true){
			if(col.gameObject.GetComponent<TCP_Character>() || col.gameObject.tag == "Stage" ){
				GameObject miniNade1 = PhotonNetwork.Instantiate("GurungMiniNade", transform.position, Quaternion.Euler(-25, 0, 0), 0);
				GameObject miniNade2 = PhotonNetwork.Instantiate("GurungMiniNade", transform.position, Quaternion.Euler(-25, 120, 0), 0);
				GameObject miniNade3 = PhotonNetwork.Instantiate("GurungMiniNade", transform.position, Quaternion.Euler(-25, 240, 0), 0);
                miniNade1.GetComponent<TCP_SkillsObjects>().PlayerOwner = this.gameObject.GetComponent<TCP_SkillsObjects>().PlayerOwner;
                miniNade2.GetComponent<TCP_SkillsObjects>().PlayerOwner = this.gameObject.GetComponent<TCP_SkillsObjects>().PlayerOwner;
                miniNade3.GetComponent<TCP_SkillsObjects>().PlayerOwner = this.gameObject.GetComponent<TCP_SkillsObjects>().PlayerOwner;
                Rigidbody rb1 = miniNade1.GetComponent<Rigidbody> ();
				Rigidbody rb2 = miniNade2.GetComponent<Rigidbody> ();
				Rigidbody rb3 = miniNade3.GetComponent<Rigidbody> ();
				rb1.AddForce (miniNade1.transform.forward * throwForce, ForceMode.VelocityChange);
				rb2.AddForce (miniNade2.transform.forward * throwForce, ForceMode.VelocityChange);
				rb3.AddForce (miniNade3.transform.forward * throwForce, ForceMode.VelocityChange);
				PhotonNetwork.Destroy(this.gameObject);
			}
		}
	}
}

//Change Tags
//Use Rotation on Spawn
