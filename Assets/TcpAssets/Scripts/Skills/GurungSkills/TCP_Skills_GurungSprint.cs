using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TCP_Skills_GurungSprint : TCP_Skill_Core
{

	[SerializeField]
	float sprintBoost;

	protected override void UseSkill ()
    {
        if(this.gameObject.GetComponent<TCP_Character>().Grounded)
        {
            //this.GetComponent<Rigidbody>().AddForce(this.transform.position + transform.forward * sprintBoost, ForceMode.Impulse);
			this.GetComponent<Rigidbody> ().velocity = transform.forward * sprintBoost;
        }
	}
}
