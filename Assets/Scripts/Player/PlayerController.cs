using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    #region Public
    [Header("Lerp")]
    public Transform target;
    public float lerpSpeed = 1f;

    [Header("Movement")]
    public float speed = 1f;


    [Header("End Game Tags")]
    public string enemyTag = "Enemy";
    public string endTag = "EndLine";

    public GameObject endScreen;


    #endregion

    #region Private
    private Vector3 _pos;
    private bool _canRun;

    #endregion

    #endregion

    void Update()
    {
        if (!_canRun) return;

        _pos = target.position;
        _pos.y = transform.position.y;
        _pos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, _pos, lerpSpeed * Time.deltaTime);

        transform.Translate(transform.forward*speed*Time.deltaTime); //adiciona movimento para frente no transform do objeto, multiplicado pela velocidade definida, dentro do relógio padronizado (apara nao ser afetado por framerate)
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == enemyTag)
        {
            EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == endTag)
        {
            EndGame();
        }
    }



    private void EndGame()
    {
        _canRun = false;
        endScreen.SetActive(true);
    }

    public void StartRunning()
    {
        _canRun = true;
    }
}
