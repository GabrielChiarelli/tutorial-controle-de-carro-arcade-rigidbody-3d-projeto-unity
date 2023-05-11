using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraQueSegue : MonoBehaviour
{
    [SerializeField] private float velocidadeParaSeguir;
    [SerializeField] private float velocidadeParaRotacionar;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform objetoParaSeguir;
    private Vector3 posicaoDoAlvo;

    private void FixedUpdate()
    {
        SeguirCarro();
    }

    private void SeguirCarro()
    {
        posicaoDoAlvo = objetoParaSeguir.TransformPoint(offset);

        transform.position = Vector3.Lerp(transform.position, posicaoDoAlvo, velocidadeParaSeguir * Time.deltaTime);

        Vector3 novaPosicao = objetoParaSeguir.position - transform.position;
        Quaternion novaRotacao = Quaternion.LookRotation(novaPosicao, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, novaRotacao, velocidadeParaRotacionar * Time.deltaTime);
    }
}
