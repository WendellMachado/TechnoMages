using UnityEngine;
using System.Collections.Generic;
using Photon;

public class TCP_Skills_MarieMine : TCP_Skill_Core
{
    [SerializeField]
    float maximumMinesAllowed;

    Queue<GameObject> mines; //estrutura de dados fila(onde o primeiro elemento a entrar é o primeiro a sair), usada para garantir que ao exceder o limite de minas plantadas, a mina que foi plantada a mais tempo será destruída.

    int minesPlaced; //variavel que irá monitorar o número de minas plantadas

    public int setMinesPlaced
    {
        get { return minesPlaced; }
        set { minesPlaced = value; }
    }

    void Start()
    {
        mines = new Queue<GameObject>();
    }

    protected override void UseSkill()
    {
        GameObject mine = PhotonNetwork.Instantiate("MarieMine", this.transform.position + (this.transform.forward * 2), Quaternion.identity, 0);
        mine.GetComponent<TCP_Marie_Mine>().SetOwner = this;//passo a referência desse objeto para a mina plantada, para que quando a mesma seja destruida, possa diminuir a variável minesPlaced
        mine.GetComponent<TCP_Marie_Mine>().PlayerOwner = PhotonNetwork.player;
        mines.Enqueue(mine);//a função enqueue adiciona um elemento ao final da fila.
        minesPlaced++;

        if (mines.Count > maximumMinesAllowed)
        {
            PhotonNetwork.Destroy(mines.Dequeue()); //a função dequeue remove e retorna o elemento mais antigo da fila
            minesPlaced--;
        }
        putOnCooldown = true;
    }
}
