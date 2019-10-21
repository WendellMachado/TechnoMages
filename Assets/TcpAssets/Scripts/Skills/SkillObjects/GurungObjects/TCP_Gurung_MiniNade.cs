using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TCP_Gurung_MiniNade : TCP_SkillsObjects
{

	[SerializeField]
	GameObject explosionEffect;
	[SerializeField]
	private float blastRadius = 5f, blastForce = 300f, damage = -30f;

    GameObject explFX;


    private bool check = false;

    protected override void DestroyMe()
    {
        PhotonNetwork.Destroy(explFX);
        PhotonNetwork.Destroy(this.gameObject);
    }


    void OnTriggerEnter(Collider collision){
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Stage") { //Change Tag to whatever is used on walls and floor
			if (check == false) {
				check = true;
				Explode ();
			}
		} else {
			Physics.IgnoreCollision (collision.GetComponent<Collider>(), GetComponent<Collider> ());
		}
	}

	void Explode () {
		explFX = PhotonNetwork.Instantiate("GurungMiniNadeExplosionFX", transform.position, transform.rotation, 0);

		Collider [] colliders = Physics.OverlapSphere (transform.position, blastRadius);

		foreach (Collider nearbyObject in colliders) {
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody> ();
			if (rb != null) {
				rb.AddExplosionForce (blastForce, explFX.transform.position, blastRadius);
			}
			if (nearbyObject.gameObject.GetComponent<TCP_CharactersHealth> () != null 
                && nearbyObject.gameObject.GetComponent<PhotonView>().owner.GetTeam() != PlayerOwner.GetTeam())
            {
				nearbyObject.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, damage, false, PlayerOwner.ID);
			}
		}
        Invoke("DestroyMe", 1f);
    }
}
