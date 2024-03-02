using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("General Level Spawning")]
    public Transform container;
    public List<GameObject> levels;

    //private
    [SerializeField] private int _index; //serialize field permite aparecer o espectro na engine, similar a se fosse publico, index come�a do 0 sempre
    private GameObject _currentLevel;

    public List<SOLevelRandomSettup> levelRandomSettup;

    private List<LevelPieceBase> _spawnedPieces = new List<LevelPieceBase>();
    private SOLevelRandomSettup _currSettup;
    public float timeBetweenPieces = 1f; //coroutine settup, delay between pieces spawning

    private void Awake()
    {
        //SpawnNextLevel(); //para spawn de um n�vel completo
        CreateLevel();
    }

    #region Controlled Level Spawning

    private void SpawnNextLevel()
    {
        if (_currentLevel != null) 
        {
            Destroy(_currentLevel);
            _index++;
            if(_index >= levels.Count)
            {
                ResetLevelIndex();
            }
        }
        _currentLevel = Instantiate(levels[_index], container); //instancia/cria um level (prefab), com o n� indicado pelo index presente no container (empty de cena com as coodenadas para definir o 0)
        _currentLevel.transform.localPosition = Vector3.zero; //garante que o level vai spawnar nas coordenadas 0,0,0
    }

    private void ResetLevelIndex()
    {
        _index = 0;
    }

    #endregion

    #region Random Level generator

    //vers�o sem ser corrotina

    private void CreateLevel()
    {
        CleanSpawnedPieces();

        if (_currSettup != null)
        {
            _index++;
            if (_index >= levelRandomSettup.Count) ResetLevelIndex();
        }

        _currSettup = levelRandomSettup[_index];

        for (int i = 0; i < _currSettup.nOfPiecesStart; i++) //cria um n� de peda�os de cenario da lista especificada usando a fun��o, repetendo para cada int dentro da variavel
        {
            CreateLevelPieces(_currSettup.levelPiecesStart);
        }

        for (int i = 0; i < _currSettup.nOfPieces; i++) //cria um n� de peda�os de cenario da lista especificada usando a fun��o, repetendo para cada int dentro da variavel
        {
            CreateLevelPieces(_currSettup.levelPieces);
        }

        for (int i = 0; i < _currSettup.nOfPiecesEnd; i++) //cria um n� de peda�os de cenario da lista especificada usando a fun��o, repetendo para cada int dentro da variavel
        {
            CreateLevelPieces(_currSettup.levelPiecesEnd);
        }

        ColorManager.Instance.ChangeColourByType(_currSettup.artType);
    }

    private void CreateLevelPieces(List<LevelPieceBase> list)
    {
        var piece = list[Random.Range(0, list.Count)]; //escolhe uma pe�a aleatoria desde a indicada como 0 na lsita at� o numero total de pe�as listadas
        var spawnedPiece = Instantiate(piece, container);

        if(_spawnedPieces.Count > 0)
        {
            var lastpiece = _spawnedPieces[_spawnedPieces.Count - 1]; //identifica a ultiam pela pela posi��o na lista

            spawnedPiece.transform.position = lastpiece.endPiece.position; //pega a posi��o inicial e insere ela em cima da posi��o final da ultima pe�a
        }
        else
        {
            spawnedPiece.transform.localPosition = Vector3.zero;
        }

        foreach(var p in spawnedPiece.GetComponentsInChildren<ArtPiece>()) //para cada pe�a no ArtPiece ->
        {
            p.ChangePiece(ArtManager.Instance.GetSetupByType(_currSettup.artType).gameObject);
        }

        _spawnedPieces.Add(spawnedPiece); //adiciona a pe�a spawnada na lista
    }


    private void CleanSpawnedPieces()
    {
        for (int i = _spawnedPieces.Count - 1; i >= 0; i--) //deleta do maior para o menor para n�o conflitar com as numera��es de index
        {
            Destroy(_spawnedPieces[i].gameObject);
        }

        _spawnedPieces.Clear(); //limpa o array -> permite que o swapn seja no lugar certo e que a lista nao acabe sendo deletada e trave o jogo
    }

    //vers�o corrotina
    /*private void CreateLevel()
    {
        StartCoroutine(CreateLevelPieceCouroutine());
    }*/

    //n�o est� em uso
    IEnumerator CreateLevelPieceCouroutine()
    {
        _spawnedPieces = new List<LevelPieceBase>();

        for (int i = 0; i <= _currSettup.nOfPieces; i++) //cria um n� de peda�os de cenario usando a fun��o, repetendo para cada int dentro da variavel
        {
            CreateLevelPieces(_currSettup.levelPieces);
            yield return new WaitForSeconds(timeBetweenPieces);
        }
    }

    #endregion


    #region Test triggers

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) SpawnNextLevel();
        if (Input.GetKeyDown(KeyCode.D)) CreateLevel();
    }

    #endregion
}
