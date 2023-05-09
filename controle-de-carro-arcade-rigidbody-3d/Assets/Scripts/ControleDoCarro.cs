using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoCarro : MonoBehaviour
{
    [Header("Referências Gerais")]
    private Rigidbody oRigidbody;
    
    [Header("Inputs")]
    private float inputDeMovimento;
    private float inputDeRotacao;

    [Header("Velocidades")]
    [SerializeField] private float velocidadeFrontalNormal;
    [SerializeField] private float velocidadeFrontalComFreio;
    private float velocidadeFrontalAtual;

    [Space(10)]

    [SerializeField] private float velocidadeTraseiraNormal;
    [SerializeField] private float velocidadeTraseiraComFreio;
    private float velocidadeTraseiraAtual;

    [Space(10)]

    [SerializeField] private float velocidadeDeRotacaoDoCarro;

    [Header("Drag do Rigidbody")]
    [SerializeField] private float dragNormal;
    [SerializeField] private float dragComFreio;

    [Header("Rodas")]
    [SerializeField] private float anguloMaximoDeRotacaoDasRodas;

    [Space(10)]

    [SerializeField] private Transform rodaFrontalEsquerda;
    [SerializeField] private Transform rodaFrontalDiretia;

    [Header("Inclinação do Carro na Rampa")]
    [SerializeField] private float velocidadeDeInclinacaoNaRampa;
    [SerializeField] private float tamanhoDosRaiosDeVerificacao;
    [SerializeField] private Transform localDoRaioDeVerificacao01;
    [SerializeField] private Transform localDoRaioDeVerificacao02;
    [SerializeField] private LayerMask layerDoChao;
    private RaycastHit localAtingidoPeloRaio;
    private bool estaNoChao;

    private void Awake()
    {
        oRigidbody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        velocidadeFrontalAtual = velocidadeFrontalNormal;
        velocidadeTraseiraAtual = velocidadeTraseiraNormal;

        oRigidbody.drag = dragNormal;
    }

    private void Update()
    {
        ReceberInputs();
        RotacionarRodas();
        InclinarNaRampa();
        RotacionarCarro();
    }

    private void FixedUpdate()
    {
        MoverCarro();
    }

    private void ReceberInputs()
    {
        inputDeMovimento = Input.GetAxisRaw("Vertical");
        inputDeRotacao = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            FrearCarro();
        }
        else
        {
            PararDeFrearCarro();
        }
    }

    private void RotacionarRodas()
    {
        // Rotaciona a Roda da Esquerda
        rodaFrontalEsquerda.localRotation = Quaternion.Euler(rodaFrontalEsquerda.localRotation.eulerAngles.x, (inputDeRotacao * anguloMaximoDeRotacaoDasRodas) - 180f, rodaFrontalEsquerda.localRotation.eulerAngles.z);

        // Rotaciona a Roda da Direita
        rodaFrontalDiretia.localRotation = Quaternion.Euler(rodaFrontalDiretia.localRotation.eulerAngles.x, (inputDeRotacao * anguloMaximoDeRotacaoDasRodas), rodaFrontalDiretia.localRotation.eulerAngles.z);
    }

    private void InclinarNaRampa()
    {
        Vector3 normalDoPiso = Vector3.zero;

        if (Physics.Raycast(localDoRaioDeVerificacao01.position, -transform.up, out localAtingidoPeloRaio, tamanhoDosRaiosDeVerificacao, layerDoChao))
        {
            estaNoChao = true;
            normalDoPiso = localAtingidoPeloRaio.normal;
        }
        else
        {
            estaNoChao = false;
            normalDoPiso = Vector3.zero;
        }

        if (Physics.Raycast(localDoRaioDeVerificacao02.position, -transform.up, out localAtingidoPeloRaio, tamanhoDosRaiosDeVerificacao, layerDoChao))
        {
            estaNoChao = true;
            normalDoPiso = (normalDoPiso + localAtingidoPeloRaio.normal) / 2f;
        }

        //  Reseta a inclinação no ar
        /*
        if (estaNoChao)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, normalDoPiso) * transform.rotation, velocidadeDeInclinacaoNaRampa * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, normalDoPiso), velocidadeDeInclinacaoNaRampa * Time.deltaTime);
        }
        */

        // Deixa o carro inclinado no ar
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, normalDoPiso) * transform.rotation, velocidadeDeInclinacaoNaRampa * Time.deltaTime);
    }

    private void MoverCarro()
    {
        if (inputDeMovimento > 0f)
        {
            oRigidbody.AddForce(transform.forward * velocidadeFrontalAtual * inputDeMovimento, ForceMode.Acceleration);
        }
        else if (inputDeMovimento < 0f)
        {
            oRigidbody.AddForce(transform.forward * velocidadeTraseiraAtual * inputDeMovimento, ForceMode.Acceleration);
        }
    }

    private void FrearCarro()
    {
        velocidadeFrontalAtual = velocidadeFrontalComFreio;
        velocidadeTraseiraAtual = velocidadeTraseiraComFreio;

        oRigidbody.drag = dragComFreio;
    }

    private void PararDeFrearCarro()
    {
        velocidadeFrontalAtual = velocidadeFrontalNormal;
        velocidadeTraseiraAtual = velocidadeTraseiraNormal;

        oRigidbody.drag = dragNormal;
    }

    private void RotacionarCarro()
    {
        float novaRotacao = inputDeRotacao * velocidadeDeRotacaoDoCarro * inputDeMovimento * Time.deltaTime;
        transform.Rotate(0f, novaRotacao, 0f, Space.World);
    }
}
