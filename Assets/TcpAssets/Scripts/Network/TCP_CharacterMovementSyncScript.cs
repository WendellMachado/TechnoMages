using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class TCP_CharacterMovementSyncScript : PunBehaviour
{
    #region Atributos

    bool initialLoad = true;

    Vector3 position, rbVelocity, weaponPosition;
    Quaternion rotation, weaponRotation;
    [SerializeField]
    float smoothing = 15;

    #endregion

    #region Componentes

    [SerializeField]
    GameObject weapon, otherPlayersCanvas, healthFillImage;

    Rigidbody rb;

    #endregion

    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        if (photonView.isMine)
        {
            //ativo os scripts importantes que pertencem ao jogador local
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponentInChildren<AudioListener>().enabled = true;
            this.otherPlayersCanvas.SetActive(false);

            foreach(Camera cam in this.GetComponentsInChildren<Camera>()) { cam.enabled = true; }

            weapon.layer = 8;
        }
        else
        {
            Text text = this.otherPlayersCanvas.GetComponentInChildren<Text>();
            text.text = this.gameObject.GetComponent<PhotonView>().owner.NickName;
            if (PhotonNetwork.player.GetTeam() == GetComponent<PhotonView>().owner.GetTeam())
            {
                text.color = Color.blue;
                this.healthFillImage.GetComponent<Image>().color = Color.blue;
            }
            else
            {
                text.color = Color.red;
                this.healthFillImage.GetComponent<Image>().color = Color.red;
            }

            StartCoroutine(UpdateData()); //se n for o meu jogador, chamo a rotina de atualização
        }
    }

    protected virtual IEnumerator UpdateData()
    {
        if (initialLoad)
        {
            initialLoad = false;
            transform.position = position;
            transform.rotation = rotation;
            weapon.transform.position = weaponPosition;
            weapon.transform.rotation = weaponRotation;
            rb.velocity = rbVelocity;
        }

        while (true)
        {   //rotina de atualização, igualo a minha posição a um lerp.
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, weaponPosition, Time.deltaTime * smoothing);
            weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, weaponRotation, Time.deltaTime * smoothing);
            rb.velocity = Vector3.Lerp(rb.velocity, rbVelocity, Time.deltaTime * smoothing);
            otherPlayersCanvas.transform.LookAt(Camera.main.transform.position);

            yield return null;
        }
    }

    /// <summary>
    /// on serialize view é a rotina que distribui pacotes de informação pela rede, uso ela para pegar as informações dos outros jogadores e fazer a suavização do movimento.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(weapon.transform.position);
            stream.SendNext(weapon.transform.rotation);
            stream.SendNext(rb.velocity);
        }
        else if (stream.isReading)
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            weaponPosition = (Vector3)stream.ReceiveNext();
            weaponRotation = (Quaternion)stream.ReceiveNext();
            rbVelocity = (Vector3)stream.ReceiveNext();
        }
    }
}
