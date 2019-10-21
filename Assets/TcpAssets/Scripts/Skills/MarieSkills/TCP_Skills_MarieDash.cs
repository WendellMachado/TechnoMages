using UnityEngine;
using Photon;

public class TCP_Skills_MarieDash : TCP_Skill_Core
{
    [SerializeField]
    float dashImpulse;

    protected override void UseSkill()
    {
		if (character.Grounded) 
		{
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			this.GetComponent<Rigidbody> ().velocity = transform.forward * dashImpulse;
		}
        putOnCooldown = true;
    }
}
