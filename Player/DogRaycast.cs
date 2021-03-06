﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogRaycast : MonoBehaviour
{
    //condição de distância para verificar se ele está perto o suficiente para interagir
    public static int distDogObj; 
    //variaveis para avisar os scripts de quando pode morder ou cheirar
    public static bool bocaDog, fucinhoDog, atingindoNada; 
    public static GameObject objSendoObservado;
    //layer do player pro raycast ignorar
    public LayerMask ignoraRaycast;
    //pega o dog como referencia para distancia do raycast
    public GameObject dog;

    float distanceObjRay;

    //modificar a gosto
    [SerializeField]
    string ModificarAGosto = "//";
    public float raycastDistance; //tamanho do raycast
    public Camera mainCamera;
    public float diamRay, distLonge;


    void Update()
    {
        //pega as coordenadas do mouse na tela e converte para o world space,
        //saindo da câmera e chegando ao primeiro ponto de impacto
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); 

        RaycastHit whatIHit;

        if (Physics.Raycast(ray, out whatIHit, raycastDistance, ~ignoraRaycast)) {


            //eu troquei pra ser o dog o ponto de comparação, pra poder fazer o raycast da camera fixa depois
            //atualizei pra não pegar a distância em y, era isso que tava quebrando o elevador
            distanceObjRay = Vector3.Distance(
            new Vector3 (dog.transform.position.x, 0, dog.transform.position.z),
            new Vector3 (whatIHit.point.x, 0, whatIHit.point.z));

            //ifs para calcular a distãncia e 
            //fazer verificação do que o jogador pode fazer ou n
            if (distanceObjRay >= distLonge ) {
                distDogObj = 1;
            }

            //o farejar e morder só vão acontecer qnd
            //estiver abaixo da distância longe
            else if (distanceObjRay < distLonge){ 

                distDogObj = 0;

                if (whatIHit.transform.CompareTag("Farejar"))
                {
                    fucinhoDog = true;
                    bocaDog = false;

                    if(whatIHit.transform.gameObject.name == "Flor" 
                    || whatIHit.transform.gameObject.name == "terra")
                    {
                        Farejar.efeitoPositivo = true;
                        Farejar.efeitoNegativo = false;
                    }
                    else if(whatIHit.transform.gameObject.name == "cesta de frutas" 
                    || whatIHit.transform.gameObject.name == "ProdutoLimpeza"
                    || whatIHit.transform.gameObject.name == "perfume")
                    {
                        Farejar.efeitoNegativo = true;
                        Farejar.efeitoPositivo = false;
                    }
                    else
                    {
                        Farejar.efeitoPositivo = false;
                    }

                    //armazena o gameobject q está com o mouse em cima para passar essa informação
                    //pros outros scripts de morder e cheirar
                    objSendoObservado = whatIHit.transform.gameObject;
                }
                else if (whatIHit.transform.CompareTag("Morder"))
                {
                    bocaDog = true;
                    fucinhoDog = false;

                    //armazena o gameobject q está com o mouse em cima para passar essa informação
                    //pros outros scripts de morder e cheirar
                    objSendoObservado = whatIHit.transform.gameObject;
                }
                else if (whatIHit.transform.CompareTag("FarejarEMorder"))
                {
                    bocaDog = true;
                    fucinhoDog = true;
                    Farejar.efeitoPositivo = false;

                    //armazena o gameobject q está com o mouse em cima para passar essa informação
                    //pros outros scripts de morder e cheirar
                    objSendoObservado = whatIHit.transform.gameObject;
                }
                else if (whatIHit.transform.CompareTag("EventosJogador") && Input.GetMouseButtonDown(0))
                {
                    whatIHit.transform.GetComponent<PegaEventoParaExecutar>().eventoSolicitado = true;
                }
                /*to comentando esse pedaço porque não tem diágolo com clique no npc atualmente
                else if(whatIHit.transform.CompareTag("NPC") && Input.GetMouseButtonDown(0))
                {
                    whatIHit.transform.GetComponent<ChamaFalaNPC>().ligarTexto = true;
                    if (MudarCameras.camNoPlayer)
                        whatIHit.transform.GetComponent<ChamaFalaNPC>().reinteragir += 1;
                }*/
                else if (whatIHit.transform.CompareTag("Cenário") || whatIHit.transform.CompareTag ("Untagged"))
                {
                    bocaDog = false;
                    fucinhoDog = false;
                }

            }

        }
        else
        {
            //cria uma variavel que indica se o raycast está atingindo alguma coisa
            atingindoNada = true;
            bocaDog = false;
            fucinhoDog = false;

            //armazena o gameobject q está com o mouse em cima para passar essa informação
            //pros outros scripts de morder e cheirar
            objSendoObservado = null;
        }
    }
}
