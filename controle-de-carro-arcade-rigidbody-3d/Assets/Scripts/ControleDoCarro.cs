using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoCarro : MonoBehaviour
{
    [Header("Referências Gerais")]
    private Rigidbody oRigidbody;
    
    [Header("Inputs")]
    private float inputDeMovimento;

    [Header("Velocidades")]
    [SerializeField] private float velocidadeFrontalNormal;
    [SerializeField] private float velocidadeFrontalComFreio;
    [SerializeField] private float velocidadeFrontalAtual;

    [Space(10)]

    [SerializeField] private float velocidadeTraseiraNormal;
    [SerializeField] private float velocidadeTraseiraComFreio;
    [SerializeField] private float velocidadeTraseiraAtual;

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
    }

    private void FixedUpdate()
    {
        MoverCarro();
    }

    private void ReceberInputs()
    {
        inputDeMovimento = Input.GetAxisRaw("Vertical");
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
}
