using UnityEngine;
using UnityEngine.UI;
using Photon;

public abstract class TCP_Weapon_Core : PunBehaviour
{

    #region componentes

    [SerializeField]
    protected Camera cam;
    [SerializeField]
    protected Image crosshair, crosshairHit;
    [SerializeField]
    protected Transform shotSpawn;
    [SerializeField]
    protected PhotonView pView;
    [SerializeField]
    protected Animator animator;


    #endregion

    #region atributos
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected int magazineSize = 0;
    [SerializeField]
    protected float fireDelay = 0.3f;
    [SerializeField]
    protected string primaryFire = "BasicAttack";
    [SerializeField]
    protected bool automaticFire = false;
    [SerializeField]
    protected bool melee = false;
    [SerializeField]
    protected float range = 1000;

	protected bool readyToFire = true;
	protected int currentAmmo;
    #endregion

	#region public atributos
	public int GetMagSize{
		get { return magazineSize; }
		set { magazineSize = value; }
	}

	public float GetFireDelay {
		get { return fireDelay; }
		set { fireDelay = value; }
	}

	public float GetDamage {
		get{ return damage; }
		set{ damage = value; }
	}
	#endregion

    [PunRPC]
    protected abstract void PrimaryFire();

    protected virtual void Start()
	{
        if (!melee) { currentAmmo = magazineSize; }
	}

	protected virtual void Update()
	{
        if (!photonView.isMine) { return; }

        RaycastHit hit;
        if (Physics.Raycast(this.cam.transform.position, this.cam.transform.forward, out hit, this.range))
        {
            if (hit.collider.gameObject.GetComponent<TCP_Character>()
              && hit.collider.gameObject.GetComponent<PhotonView>().owner.GetTeam() != this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
            {
                this.crosshair.color = Color.red;
            }
            else if (hit.collider.gameObject.GetComponent<TCP_Character>()
              && hit.collider.gameObject.GetComponent<PhotonView>().owner.GetTeam() == this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
            {
                this.crosshair.color = Color.blue;
            }
            else
            {
                this.crosshair.color = Color.white;
            }
        }

        CheckInput();
    }

	protected virtual void CheckInput()
	{
		bool primaryFirePressed;

		if(automaticFire)
		{
			primaryFirePressed = Input.GetButton(primaryFire);
		}
		else
		{
			primaryFirePressed = Input.GetButtonDown(primaryFire);
		}

		if(primaryFirePressed && !melee) 
		{	
			if(readyToFire && (currentAmmo > 0 || magazineSize == 0))
			{
                readyToFire = false;
                this.pView.RPC("PrimaryFire", PhotonTargets.All);
				currentAmmo--;
				Invoke("SetReadyToFire", fireDelay);
			}
		}
        else if(primaryFirePressed && melee)
        {
            readyToFire = false;
            this.pView.RPC("PrimaryFire", PhotonTargets.All);
            Invoke("SetReadyToFire", fireDelay);
        }
	}

    protected void DisableHitFeedback()
    {
        crosshairHit.enabled = false;
    }

    protected void SetReadyToFire()
	{
		readyToFire = true;
	}
}
