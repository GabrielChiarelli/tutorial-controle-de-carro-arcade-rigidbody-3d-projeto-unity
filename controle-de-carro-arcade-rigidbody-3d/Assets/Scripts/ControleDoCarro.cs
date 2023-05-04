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
    [SerializeField] private float velocidadeFrontalAtual;

    [Space(10)]

    [SerializeField] private float velocidadeTraseiraNormal;
    [SerializeField] private float velocidadeTraseiraComFreio;
    [SerializeField] private float velocidadeTraseiraAtual;

    [Space(10)]

    [SerializeField] private float velocidadeDeRotacaoDoCarro;

    private void Awake()
    {
        oRigidbody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        velocidadeFrontalAtual = velocidadeFrontalNormal;
        velocidadeTraseiraAtual = velocidadeTraseiraNormal;
    }

    private void Update()
    {
        ReceberInputs();
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

    private void RotacionarCarro()
    {
        float novaRotacao = inputDeRotacao * velocidadeDeRotacaoDoCarro * inputDeMovimento * Time.deltaTime;
        transform.Rotate(0f, novaRotacao, 0f, Space.World);
    }
}
