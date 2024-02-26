using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EBAC.Core.Singleton;
using TMPro;
using DG.Tweening;


public class PlayerController : Singleton<PlayerController>
{
    #region Variables

    #region Public
    [Header("Lerp")]
    public Transform target;
    public float lerpSpeed = 1f;

    [Header("Movement")]
    public float speed = 1f;

    //public string wallTag = "Wall";


    [Header("Text mesh")]
    public TextMeshPro uiTextPowerUp;

    [Header("End Game Tags")]
    public string enemyTag = "Enemy";
    public string endTag = "EndLine";
    public bool invincible = false;

    [Header("End Game Stuff")]
    public GameObject endScreen;

    [Header("Coin Setup")]
    public GameObject coinCollector;



    #endregion

    #region Private Vars
    private Vector3 _pos;
    private bool _canRun;
    private float _currentSpeed;
    private Vector3 _startPosition;

    #endregion

    #endregion


    private void Start()
    {
        _startPosition = transform.position;
        ResetSpeed();
    }

    void Update()
    {
        if (!_canRun) return;

        _pos = target.position;
        _pos.y = transform.position.y;
        _pos.z = transform.position.z;
        
        //_pos.x = Mathf.Clamp(_pos.x, -5.5f, 5.5f);

        transform.position = Vector3.Lerp(transform.position, _pos, lerpSpeed * Time.deltaTime);
        

        transform.Translate(transform.forward*_currentSpeed*Time.deltaTime); //adiciona movimento para frente no transform do objeto, multiplicado pela velocidade definida, dentro do rel�gio padronizado (apara nao ser afetado por framerate)
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == enemyTag)
        {
            if (!invincible) EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == endTag)
        {
            if (!invincible) EndGame(); //se invincible = false, permite finalizar o jogo
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

    public void SetPowerUpText(string s)
    {
        uiTextPowerUp.text = s;
    }

    #region Power Up: Speed

    public void PowerUpSpeedUp(float f)
    {
        _currentSpeed = f; //vira a nova velocidade
    }

    public void ResetSpeed()
    {
        _currentSpeed = speed;
    }
    #endregion

    #region Power Up: Invincible

    public void SetInvincible(bool b = true)
    {
        invincible = b;
    }

    #endregion

    #region Power Up: Height

    public void ChangeHeight(float amount, float duration, float animationDuration, Ease ease)
    {
        //M�do b�sico, sem anima��o
        /*var p = transform.position; //salva a posi��o atual
        p.y = _startPosition.y + amount; //altera a posi��o Y para a posi��o inicial + a defini��o de altura de voo definido 
        transform.position = p;*/

        transform.DOMoveY(_startPosition.y + amount,animationDuration).SetEase(ease);//.OnComplete(ResetHeight);a
        Invoke(nameof(ResetHeight), duration);
    }

    public void ResetHeight(float animationDuration)
    {
        /*var p = transform.position;
        p.y = _startPosition.y;
        transform.position = p;*/

        transform.DOMoveY(_startPosition.y, animationDuration);
    }

    #endregion

    #region Power Up: Collect coins

    public void ChangeCoinCollectorSize(float amount)
    {
        coinCollector.transform.localScale = Vector3.one * amount;
    }

    #endregion
}
