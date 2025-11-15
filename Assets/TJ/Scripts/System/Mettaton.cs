using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = System.Drawing.Color;

// 진입state
public class MettatonIntroState : EnemyState
{
    private BattlePlayerController _player;
    private TextMeshProUGUI _mettatonText;
    private UISystemManager _uiSystemManager;
    private GameObject _buttonUI;
    private GameObject _playerTextUI;
    private GameObject _uiBox;
    private GameObject _playerTurnUI;
    public MettatonIntroState(EnemyStateMachine stateMachine, BattlePlayerController player,
        TextMeshProUGUI mettatonText, UISystemManager uiSystemManager, GameObject buttonUI,
        GameObject playerTextUI, GameObject uiBox,  GameObject playerTurnUI)
        : base(stateMachine)
    {
        _player = player;
        _mettatonText =  mettatonText;
        _uiSystemManager = uiSystemManager;
        _buttonUI = buttonUI;
        _playerTextUI = playerTextUI;
        _uiBox = uiBox;
        _playerTurnUI = playerTurnUI;
    }
    public override void Enter()
    {
        Debug.Log("메타톤 등장! 화려한 등장 연출 시작");
        _uiSystemManager.PhaseOpenEffect();
        //플레이어 끄기
        _player.gameObject.SetActive(false);
        //네모창에 대사 입력
        _mettatonText.gameObject.SetActive(true);
        _buttonUI.SetActive(true);
        _playerTextUI.SetActive(true);
        _uiBox.SetActive(true);
        _playerTurnUI.SetActive(true);
        
    }

    public override void Update()
    {
        //엔터키로 state 넘기기
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _enemyStateMachine.mettatonAdvanceState();
        }
    }

    public override void Exit()
    {
        Debug.Log("메타톤 Intro 종료");
        _mettatonText.gameObject.SetActive(false);
    }
}

// 블럭과 폭탄이 무작위로 내려오는 패턴
public class MettatonRainAttackState : EnemyState
{
    private GameObject _blockPrefab;
    private GameObject _bombPrefab;
    private BattlePlayerController _player;
    private GameState _gameState;

    private Coroutine _rainAttackRoutine;
    private Coroutine _timeoutRoutine;
    
    private List<GameObject> _spawnedObjects = new List<GameObject>();

    public MettatonRainAttackState(EnemyStateMachine stateMachine, GameObject blockPrefab,
        GameObject bombPrefab, BattlePlayerController player, GameState gameState) : base(stateMachine)
    {
        _blockPrefab = blockPrefab;
        _bombPrefab = bombPrefab;
        _player = player;
        _gameState = gameState;
    }

    public override void Enter()
    {
        _player.gameObject.transform.position = new Vector3(1,-1,0);
        _player.normalMinX = -1.62f;
        _player.normalMaxX = 1.62f;
        _player.normalMinY = -2.87f;
        _player.normalMaxY = -0.43f;
        
        Debug.Log("메타톤 블럭·폭탄 비 공격 시작");
        _enemyStateMachine.EnemyStateUI();
        
        _spawnedObjects.Clear();

        _rainAttackRoutine = _enemyStateMachine
            .StartCoroutine(SpawnBlockAndBomb());

        _timeoutRoutine = _enemyStateMachine
            .StartCoroutine(Timeout(15f));
    }

    public override void Update()
    {
        if (_gameState.playerHp <= 0)
        {
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseState());
        }
    }

    public override void Exit()
    {
        Debug.Log("메타톤 블럭·폭탄 비 공격 종료");
        if (_rainAttackRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_rainAttackRoutine);
            _rainAttackRoutine = null;
        }

        if (_timeoutRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
        foreach (var go in _spawnedObjects)
        {
            if (go != null)
                GameObject.Destroy(go);
        }
        _spawnedObjects.Clear();
    }

    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.mettatonAdvanceState();
    }

    private IEnumerator SpawnBlockAndBomb()
    {
        yield return new WaitForSeconds(1f);
        int spawnCount = 0;
        float interval = 0.2f;
        float yPos = 4.5f;
        float minSep = 0.5f;

        while (spawnCount < 19)
        {
            float blockX = Random.Range(-2f, 2f);
            var block = GameObject.Instantiate(_blockPrefab, new Vector3(blockX, yPos, 0), Quaternion.identity);

            _spawnedObjects.Add(block);
            float bombX;
            do
            {
                bombX = Random.Range(-2f, 2f);
            } while (Mathf.Abs(bombX - blockX) < minSep);

            var bomb = GameObject.Instantiate(_bombPrefab, new Vector3(bombX, yPos, 0), Quaternion.identity);
            _spawnedObjects.Add(bomb);
            spawnCount++;
            yield return new WaitForSeconds(interval);
        }

        _enemyStateMachine.mettatonAdvanceState();
    }
}


// 우산 든 에너미 나오는 패턴
public class MettatonUmbAttackState : EnemyState
{
    private GameObject _mettabomb;
    private GameObject _umbrellabomb;
    private BattlePlayerController _player;
    private GameState _gameState;

    private Coroutine _umbrellaRoutine;
    private Coroutine _mettaRoutine;
    private Coroutine _timeoutRoutine;

    public MettatonUmbAttackState(EnemyStateMachine stateMachine, GameObject mettabomb,
        GameObject umbrellabomb, BattlePlayerController player, GameState gameState) : base(stateMachine)
    {
        _mettabomb = mettabomb;
        _umbrellabomb = umbrellabomb;
        _player = player;
        _gameState = gameState;
    }

    public override void Enter()
    {
        Debug.Log("메타톤 우산폭탄 공격 시작");
        _enemyStateMachine.EnemyStateUI();

        _umbrellaRoutine = _enemyStateMachine.StartCoroutine(SpawnUmbrellaBombs());
        _mettaRoutine = _enemyStateMachine.StartCoroutine(SpawnMettabombs());
        
        _timeoutRoutine = _enemyStateMachine.StartCoroutine(Timeout(7f));
    }

    public override void Update()
    {
        if (_gameState.playerHp <= 0)
        {
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseState());
        }
    }

    public override void Exit()
    {
        Debug.Log("메타톤 우산폭탄 공격 종료");
        
        if (_umbrellaRoutine != null)
            _enemyStateMachine.StopCoroutine(_umbrellaRoutine);

        if (_mettaRoutine != null)
            _enemyStateMachine.StopCoroutine(_mettaRoutine);

        if (_timeoutRoutine != null)
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
    }

    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.mettatonAdvanceState();
    }

    private IEnumerator SpawnUmbrellaBombs()
    {
        yield return new WaitForSeconds(0.2f);
        int cnt = 0;
        while (cnt < 6)
        {
            float yPos = 4.5f;
            GameObject.Instantiate(_umbrellabomb, new Vector3( 1.8f, yPos,0), Quaternion.identity);
            GameObject.Instantiate(_umbrellabomb, new Vector3(-1.8f, yPos,0), Quaternion.identity);
            cnt++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SpawnMettabombs()
    {
        yield return new WaitForSeconds(0.2f);
        int cnt = 0;
        while (cnt < 6)
        {
            float yPos = 4.5f;
            GameObject.Instantiate(_mettabomb, new Vector3(Random.Range(-0.7f,0.7f), yPos,0), Quaternion.identity);
            cnt++;
            yield return new WaitForSeconds(0.4f);
        }
    }
}


// 하트에서 번개모양 탄막 쏘는 패턴
public class MettatonLightingAttackState : EnemyState
{
    private GameObject _mettaHeart;
    private Coroutine _timeoutRoutine;
    private Coroutine  _delayedActivateRoutine;
    private BattlePlayerController _player;
    private GameState _gameState;
    public MettatonLightingAttackState(EnemyStateMachine stateMachine, GameObject mettaHeart,
        BattlePlayerController player, GameState gameState) : base(stateMachine)
    {
        _mettaHeart = mettaHeart;
        _player = player;
        _gameState =  gameState;
    }
    
    public override void Enter()
    {
        _enemyStateMachine.EnemyStateUI();
        //_mettaHeart.SetActive(false);
        _delayedActivateRoutine = _enemyStateMachine.StartCoroutine(ActivateAfterDelay(1f));
        _timeoutRoutine = _enemyStateMachine.StartCoroutine(EndAfterDelay(15f));
    }

    public override void Update()
    {
        if (_gameState.playerHp <= 0)
        {
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseState());
        }
    }

    public override void Exit()
    {
        if (_timeoutRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
        if (_delayedActivateRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_delayedActivateRoutine);
            _delayedActivateRoutine = null;
        }
        _mettaHeart.SetActive(false);

    }
    private IEnumerator ActivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _mettaHeart.SetActive(true);
        _delayedActivateRoutine = null;
    }
    private IEnumerator EndAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.mettatonAdvanceState();
    }
}

public class MettatonRECAttackState : EnemyState
{
    private GameObject _blockArray;
    private BattlePlayerController _player;
    private GameObject _rewUI;
    private GameObject _recUI;
    private Coroutine _spawnRoutine;
    private Coroutine recBlinkCoroutine;
    private Coroutine rewBlinkCoroutine;
    private List<BlockArray> spawnedArrays = new List<BlockArray>();
    private GameState _gameState;
    private Coroutine _timeoutRoutine;

    public MettatonRECAttackState(EnemyStateMachine stateMachine, GameObject blockArray,
        BattlePlayerController player, GameObject RECUI, GameObject REWUI
        , GameState gameState) : base(stateMachine)
    {
        _blockArray = blockArray;
        _player = player;
        _recUI = RECUI;
        _rewUI = REWUI;
        _gameState = gameState;
    }

    public override void Enter()
    {
        _player.normalMinX = -1.26f;
        _player.normalMaxX = 1.26f;
        _player.normalMinY = -2.88f;
        _player.normalMaxY = -0.45f;
        
        _enemyStateMachine.EnemyStateUI();
        recBlinkCoroutine = _enemyStateMachine.StartCoroutine(BlinkUI(_recUI));
        _spawnRoutine = _enemyStateMachine.StartCoroutine(SpawnBlockArray());
        _recUI.SetActive(false);
        _rewUI.SetActive(false);
        _timeoutRoutine = _enemyStateMachine.StartCoroutine(Timeout(13f));
    }

    public override void Update()
    {
        if (_gameState.playerHp <= 0)
        {
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseState());
        }
    }

    public override void Exit()
    {
        if (_spawnRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }

        _enemyStateMachine.StopCoroutine(BlinkUI(_rewUI));

        if (_timeoutRoutine != null)
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
    }

    private IEnumerator SpawnBlockArray()
    {
        int spawned = 0;
        yield return new WaitForSeconds(0.2f);

        while (spawned < 5)
        {
            Vector3 spawnPos = new Vector3(-1.2f, 4.5f, 0);
            GameObject obj = GameObject.Instantiate(_blockArray, spawnPos, Quaternion.identity);
            
            BlockArray array = obj.GetComponent<BlockArray>();
            if (array != null)
                spawnedArrays.Add(array);

            spawned++;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.1f);
        
        _enemyStateMachine.StopCoroutine(recBlinkCoroutine);
        _recUI.SetActive(false);
        foreach (var array in spawnedArrays)
        {
            if (array != null)
                array.BeginSlowDownAndRise();
        }
        _enemyStateMachine.StartCoroutine(BlinkUI(_rewUI));


    }
    
    private IEnumerator BlinkUI(GameObject targetUI)
    {
        bool isOn = false;
        while (true)
        {
            isOn = !isOn;
            targetUI.SetActive(isOn);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.mettatonAdvanceState();
    }
}

// 플레이어 턴
public class PlayerTurnState : EnemyState
{
    private BattlePlayerController _player;
    private Button _firstButton;
    private Button  _playButton;
    private GameState _gameState;
    public PlayerTurnState(EnemyStateMachine stateMachine, BattlePlayerController player,
        Button firstButton, Button playButton, GameState gameState) : base(stateMachine)
    {
        _player = player;
        _firstButton = firstButton;
        _playButton = playButton;
        _gameState = gameState;
    }

    public override void Enter()
    {
        Debug.Log("플레이어 턴 시작");
        _player.gameObject.SetActive(false);
        _enemyStateMachine.PlayerStateUI();
        EventSystem.current.SetSelectedGameObject(null);
        
        _playButton.gameObject.SetActive(false);
        _playButton .onClick.RemoveListener(OnPlayButtonClicked);


        _enemyStateMachine.StartCoroutine(SelectFirstButtonToNextFrame());
        _firstButton.onClick.RemoveListener(OnFirstButtonClicked);
        _playButton .onClick.RemoveListener(OnPlayButtonClicked);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("플레이어 턴 종료");
        _player.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _firstButton.onClick.RemoveListener(OnFirstButtonClicked);
        _playButton.gameObject.SetActive(false);
    }
    private IEnumerator SelectFirstButtonToNextFrame()
    {
        yield return null;
        _firstButton.Select();
        EventSystem.current.SetSelectedGameObject(_firstButton.gameObject);
        
        _firstButton.onClick.AddListener(OnFirstButtonClicked);
    }
    private void OnPlayButtonClicked()
    {
        _enemyStateMachine.mettatonAdvanceState();
    }
    private void OnFirstButtonClicked()
    {
        _playButton.gameObject.SetActive(true);
        _playButton.Select();
        EventSystem.current.SetSelectedGameObject(_playButton.gameObject);
        
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        //_firstButton.onClick.RemoveListener(OnFirstButtonClicked);
    }
}

public class PlayerWinState : EnemyState
{
    private UISystemManager _uiSystemManager;
    private GameState _gameState;
    private BattlePlayerController _player;
    private GameObject _mettaton;
    private GameObject _mettatonUI;
    private GameObject _mainCamera;
    private GameObject _battleCamera;
    private GameObject _env;
    private GameObject _fieldPlayer;
    
    public PlayerWinState(EnemyStateMachine stateMachine, UISystemManager uiSystemManager,
        GameState gameState, BattlePlayerController player, GameObject mettaton,
        GameObject mettatonUI, GameObject mainCamera, GameObject battleCamera, GameObject env,
        GameObject fieldPlayer) : base(stateMachine)
    {  
        _uiSystemManager = uiSystemManager;
        _gameState = gameState;
        _player = player;
        _mettaton = mettaton;
        _mettatonUI = mettatonUI;
        _mainCamera = mainCamera;
        _battleCamera = battleCamera;
        _env = env;
        _fieldPlayer = fieldPlayer;
    }

    public override void Enter()
    {
        //Time.timeScale = 0f;
        _gameState.killCount++;
        _player.gameObject.SetActive(false);
        _mettaton.SetActive(false);
        _mettatonUI.SetActive(false);
        _mainCamera.SetActive(true);
        _battleCamera.SetActive(false);
        _env.SetActive(true);
        _fieldPlayer.gameObject.SetActive(true);
        _enemyStateMachine.gameObject.SetActive(false);
        _uiSystemManager.IsFighting = false;
        _uiSystemManager.IsFacing = false;
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}

public class PlayerLoseState : EnemyState
{
    private GameObject _loseUI;
    private BattlePlayerController _player;
    
    public PlayerLoseState(EnemyStateMachine stateMachine, GameObject loseUI,
        BattlePlayerController player) : base(stateMachine)
    {
        _player = player;
        _loseUI = loseUI;
    }

    public override void Enter()
    {
        //Time.timeScale = 0f;
        _loseUI.SetActive(true);
        //_player.gameObject.SetActive(false);
    }

    public override void Update()
    {
        // 공격 패턴 로직 처리 (예: 총알 발사, 타이밍 조절 등)
    }

    public override void Exit()
    {
        
    }
}
