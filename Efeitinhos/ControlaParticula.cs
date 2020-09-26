﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaParticula : MonoBehaviour
{
    /*
    comentar esse script direito

    public Sprite [] mixSpriteArray; //seleção do array da partícula
    em outro script 
    pegar o número de um e outro e escolher a sheet pelo nome PartSheet_1oApto_i_j

    void MixParticles()
    {
        

        quando criar a partícula, mudar a cor dela
        cor é a mistura de a e b, ver aquele bagulho da soma
        a v1 pode não ter cor, tudo bem
    
    void ativaParticle()
    {
        criar uma pode ligar
        pegar do raycast qual o objeto que eu cliquei
        mandar mensagem do próprio raycast talvez seja mais fácil
        chamar esse método lá, objQueEuCliquei.ControlaParticula.ativaParticle()
        if obj interagido = this/chegou aqui, pode ligar = true
        esse true NÃO FICA FALSE NUNCAAAA

        if(Estadosplayer bla cheirando && pode ligar)
            liga o filho meo parça
    }

    fazer o morder e cheirar as well pra ficar melhor, mas depois
    */

    private GameObject particle; //objeto com o emitter
    private ParticleSystem parSys; //referência pro particle system
    
    public GameObject partPool; //partícula na cena que vai modificar
    private ParticleSystem partPoolSys; //referência pro particle system do pool
    
    private int rowNumber; //onde vai pegar imagem na sprite sheet
    private Vector3 collidePosition; //guarda a posição de onde colidiu

    private Sprite currentSprite; //pega a sprite da partícula original
    public Sprite mixSprite; //sprite pra partícula misturada
    private bool addedSprite = false; //checa se já trocou a partícula

    void Start()
    {
        particle = transform.GetChild(0).gameObject; //pega a partícula dentro do obj
        parSys = particle.GetComponent<ParticleSystem>(); //referência do particlesystem

        partPool = GameObject.Find("PartMistura"); //mesmas referências mas pro pool
        partPoolSys = partPool.GetComponent<ParticleSystem>();        
    }
    void Update()
    {
        MixParticles();
    }

    
    //cria partículas bonitas no lugar da colisão
    void MixParticles()
    {
        var texAnim = parSys.textureSheetAnimation; //referência da animação por sheet
        var mixTexAnim = partPoolSys.textureSheetAnimation; //referência da animação por sheet da mistura

        currentSprite = texAnim.GetSprite(0); //sprite atual da partícula


        if(EstadosPlayer.gerandoParticula)
        {
            partPool.SetActive(true); //ativa a partícula mistura

            //move a partícula pro lugar da col
            if(collidePosition.x != 0 || collidePosition.z != 0) 
                partPool.transform.position = new Vector3 (collidePosition.x, collidePosition.y * 2, collidePosition.z);

            //muda a sprite
            if(!addedSprite)
            {
                mixTexAnim.SetSprite(0, mixSprite);
                addedSprite = true;
            }
        }
        else
        {
            partPool.SetActive(false);
            addedSprite = false;
        }
    }


    //liga e desliga o gerador de partícula com a colisão
    void OnTriggerEnter(Collider other) 
    {
        if(!EstadosPlayer.gerandoParticula)
        {
            EstadosPlayer.gerandoParticula = true;
            collidePosition = particle.transform.position;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(EstadosPlayer.gerandoParticula)
        {
            EstadosPlayer.gerandoParticula = false;
        }
    }
}
