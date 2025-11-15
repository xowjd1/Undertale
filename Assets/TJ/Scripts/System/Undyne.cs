using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UndyneIntroState : EnemyState
{
    private BattlePlayerController _player;
    private GameObject _buttonUI;
    private GameObject _playerTextUI;
    private GameObject _playerHPSlider;
    private GameObject _uiBox;
    private GameObject _playerTurnUI;

    public UndyneIntroState(EnemyStateMachine stateMachine, BattlePlayerController player,
        GameObject buttonUI, GameObject playerTextUI, GameObject playerHPSlider, GameObject uiBox,
        GameObject playerTurnUI ): base(stateMachine)
    {
        _player = player;
        _buttonUI = buttonUI;
        _playerTextUI = playerTextUI;
        _playerHPSlider = playerHPSlider;
        _uiBox = uiBox;
        _playerTurnUI = playerTurnUI;
        
    }

    public override void Enter()
    {
        Debug.Log("UndyneIntroState Enter");
        _player.gameObject.SetActive(false);
        _buttonUI.SetActive(true);
        _playerTextUI.SetActive(true); 
        _playerHPSlider.SetActive(true);
        _uiBox.SetActive(true);
        _playerTurnUI.SetActive(true);
        
    }

    public override void Update()
    {
        if (Input.anyKeyDown)
            Debug.Log("[UndyneIntroState] Input.anyKeyDown: " + Time.frameCount);

        // 메인 Enter 또는 키패드 Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("[UndyneIntroState] Enter detected! advancing…");
            _enemyStateMachine.undyneAdvanceState();
        }
    }

    public override void Exit()
    {

    }
}

// 닷지 패턴
public class DodgeArrowState : EnemyState
{
    private BattlePlayerController _player;
    private GameObject _dodgeArrow;
    private GameState _gameState;
    private List<GameObject> _spawnedArrows;
    private Coroutine spawnCoroutine;
    private Coroutine _timeoutRoutine;
    public DodgeArrowState(EnemyStateMachine stateMachine, BattlePlayerController player,
        GameObject dodgeArrow, GameState gameState) : base(stateMachine)
    {
        _player = player;
        _dodgeArrow = dodgeArrow;
        _gameState = gameState;
        _spawnedArrows = new List<GameObject>();
    }
    public override void Enter()
    {
        _enemyStateMachine.UnDyneEnemyStateUI();
        
        _player.gameObject.SetActive(true);
        _player.normalMinX = -1.8f;
        _player.normalMaxX = 1.8f;
        _player.normalMinY = -3.06f;
        _player.normalMaxY = 0.7f;
        
        spawnCoroutine = _enemyStateMachine.StartCoroutine(SpawnArrows());
        _timeoutRoutine = _enemyStateMachine
            .StartCoroutine(Timeout(10f));
    }

    public override void Update()
    {
        if(_gameState.playerHp <= 0)
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseUndyneState());
        
    }

    public override void Exit()
    {
        if (_timeoutRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
        if (spawnCoroutine != null)
        {
            _enemyStateMachine.StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        _player.gameObject.SetActive(false);
        
        foreach (var arrow in _spawnedArrows)
        {
            if (arrow != null)
                GameObject.Destroy(arrow);
        }
        _spawnedArrows.Clear();
    }

    private IEnumerator SpawnArrows()
    {
        float spawnDuration = 7f;
        float spawnInterval = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < spawnDuration)
        {
            float randomX = Random.Range(-4f, 3.5f);
            float randomY = Random.Range(-3.7f, 2f);
            
            Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
            
            var arrowInstance = GameObject.Instantiate(_dodgeArrow, spawnPos,
                Quaternion.identity);
            _spawnedArrows.Add(arrowInstance);

            elapsedTime += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.undyneAdvanceState();
    }
}

// 위로 올라오는 화살표 피하는 패턴
public class ArrowRisingState : EnemyState
{
    private BattlePlayerController _player;
    private GameObject _risingArrowSpawner;
    private GameState _gameState;
    private Coroutine _timeoutRoutine;
    private List<GameObject> _spawnedArrows = new List<GameObject>();
    
    public ArrowRisingState(EnemyStateMachine stateMachine, BattlePlayerController player,
        GameObject risingArrowSpawner, GameState gameState) : base(stateMachine)
    {
        _player = player;
        _risingArrowSpawner = risingArrowSpawner;
        _gameState = gameState;
    }

    public override void Enter()
    {
        _player.gameObject.SetActive(true);
        _enemyStateMachine.UnDyneEnemyStateUI();
        _player.normalMinX = -0.6f;
        _player.normalMaxX = 0.5f;
        _player.normalMinY = -3.0f;
        _player.normalMaxY = -1.6f;
        Vector3 spawnPos = new Vector3(-0.55f, -3.9f, 0);
        var temp = GameObject.Instantiate(_risingArrowSpawner, spawnPos, Quaternion.identity);
        _spawnedArrows.Add(temp);
        _timeoutRoutine = _enemyStateMachine.StartCoroutine(Timeout(15f));
    }

    public override void Update()
    {
      // 플레이어 체력 0 이하 되면 lose로 c
      if(_gameState.playerHp <= 0)
      _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseUndyneState());
    }

    public override void Exit()
    {
        if (_spawnedArrows.Count != 0)
        {
            GameObject.Destroy(_spawnedArrows[0]);
            _spawnedArrows.RemoveAt(0);
        }
        if (_timeoutRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
    }
    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.undyneAdvanceState();
    }
}

// 화살표 방향 맞추는 패턴
public class RhythmArrowState : EnemyState
{
    private BattlePlayerController _player;
    private GameObject _crossArrowSpawner;
    private GameObject _arrowPlayer;
    private GameState _gameState;
    private GameObject _spawnerInstance;
    private Coroutine _timeoutRoutine;

    public RhythmArrowState(EnemyStateMachine stateMachine, BattlePlayerController player,
        GameObject crossArrowSpawner, GameObject arrowPlayer, GameState gameState) : base(stateMachine)
    {
        _player = player;
        _crossArrowSpawner = crossArrowSpawner;
        _arrowPlayer = arrowPlayer;
        _gameState = gameState;
    }
    public override void Enter()
    {
        _enemyStateMachine.UnDyneEnemyStateUI();
        Vector3 spawnPos = new Vector3(0, 0, 0);
        _spawnerInstance = GameObject.Instantiate(_crossArrowSpawner, Vector3.zero,
                Quaternion.identity);
        _player.gameObject.SetActive(false);
        _arrowPlayer.SetActive(true);
        _timeoutRoutine = _enemyStateMachine.StartCoroutine(Timeout(20f));

    }

    public override void Update()
    {
        if(_gameState.playerHp <= 0)
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseUndyneState());
    }

    public override void Exit()
    {
        _arrowPlayer.SetActive(false);
        
        if (_spawnerInstance != null)
        {
            GameObject.Destroy(_spawnerInstance);
            _spawnerInstance = null;
        }
        
        if (_timeoutRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
    }
    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.undyneAdvanceState();
    }
}

// 화살표가 원형으로 플레이어를 추적하는 패턴
public class CircleArrowState : EnemyState
{
    private BattlePlayerController _player;
    private GameObject _heptagonSpawner;
    private GameState _gameState;
    private const int VertexCount = 7;
    private Coroutine _timeoutRoutine;
    
    public CircleArrowState(
        EnemyStateMachine stateMachine, BattlePlayerController player,
        GameObject heptagonSpawner, GameState gameState) : base(stateMachine)
    {
        _player = player;
        _heptagonSpawner = heptagonSpawner;
        _gameState = gameState;
    }

    public override void Enter()
    {
        _player.gameObject.SetActive(true);
        _enemyStateMachine.UnDyneEnemyStateUI();
        _player.normalMinX = -6.16f;
        _player.normalMaxX = 6.16f;
        _player.normalMinY = -3f;
        _player.normalMaxY = 2.85f;
        _enemyStateMachine.StartCoroutine(SpawnArrows());
        _timeoutRoutine = _enemyStateMachine.StartCoroutine(Timeout(9f));

    }

    public override void Update()
    {
        if(_gameState.playerHp <= 0)
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerLoseUndyneState());
    }

    public override void Exit()
    {
        _enemyStateMachine.StopCoroutine(SpawnArrows());
        if (_timeoutRoutine != null)
        {
            _enemyStateMachine.StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
    }
    private IEnumerator Timeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        _enemyStateMachine.undyneAdvanceState();
    }
    private IEnumerator SpawnArrows()
    {
        for (int wave = 0; wave < 7; wave++)
        {
            // ① 스포너를 플레이어 현재 위치에 Instantiate
            Vector3 spawnCenter = _player.transform.position;
            GameObject spawner = GameObject.Instantiate(
                _heptagonSpawner,
                spawnCenter,
                Quaternion.identity
            );
            
            yield return new WaitForSeconds(1f);
        }
    }
}


public class PlayerTurnUndyneState : EnemyState
{
    private BattlePlayerController _player;
    private GameObject _undyne;
    private UndyneHP _undyneHP;
    private GameObject _playerAttackBar;
    private Button _firstButton;
    private Button _playButton;
    private GameState  _gameState;

    private Coroutine _selectFirstRoutine;
    private Coroutine _disableBarRoutine;

    public PlayerTurnUndyneState(EnemyStateMachine stateMachine, GameObject undyne, UndyneHP undyneHP,
            BattlePlayerController player, GameObject playerAttackBar, Button firstButton,
            Button playButton, GameState gameState) : base(stateMachine)
    {
        _player  = player;
        _undyne = undyne;
        _playerAttackBar = playerAttackBar;
        _firstButton = firstButton;
        _playButton = playButton;
        _gameState = gameState;
        _undyneHP =  undyneHP;
        
        _undyneHP = _undyne.GetComponent<UndyneHP>();
        if (_undyneHP == null)
            Debug.LogError("PlayerTurnUndyneState: UndyneHP 컴포넌트를 찾을 수 없습니다!");
    }

    public override void Enter()
    {
        Debug.Log("플레이어 턴");
        _player.gameObject.SetActive(false);
        _enemyStateMachine.UndynePlayerStateUI();
        
        EventSystem.current.SetSelectedGameObject(null);
        _playButton.gameObject.SetActive(false);
        _firstButton.onClick.RemoveListener(OnFirstButtonClicked);
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _selectFirstRoutine = _enemyStateMachine.StartCoroutine(SelectFirstButtonToNextFrame());
    }

    private IEnumerator SelectFirstButtonToNextFrame()
    {
        yield return null;
        _firstButton.Select();
        EventSystem.current.SetSelectedGameObject(_firstButton.gameObject);
        _firstButton.onClick.AddListener(OnFirstButtonClicked);
        _selectFirstRoutine = null;
    }

    private void OnFirstButtonClicked()
    {
        // 플레이 버튼 활성 & 선택
        _playButton.gameObject.SetActive(true);
        _playButton.Select();
        EventSystem.current.SetSelectedGameObject(_playButton.gameObject);

        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        _playButton.gameObject.SetActive(false);
        _playerAttackBar.SetActive(true);

        _playButton.gameObject.SetActive(false);
        // 타이밍 바 코루틴 시작
        _disableBarRoutine = _enemyStateMachine.StartCoroutine(DoTimingAndDamage());
    }
    private IEnumerator DoTimingAndDamage()
    {
        // 1) 타이밍 어택 UI 켜기
        _playerAttackBar.SetActive(true);

        // 2) PlayerTimingAttack 시작
        var timing = _playerAttackBar.GetComponent<PlayerTimingAttack>();
        timing.StartTiming();                     // 실제로 타이밍을 시작하는 메서드
        yield return new WaitUntil(() => timing.IsCompleted);  // 유저가 입력하고 끝날 때까지 대기

        // 3) 타이밍 완료 후 데미지 계산
        int dmg = Mathf.RoundToInt(timing.TotalDamage);
        Debug.Log($"[TimingComplete] dmg = {dmg}");

        // 4) HP 차감
        _undyneHP.TakeDamage(dmg);
        Debug.Log($"[OnPlay] 남은 undyneHP: {_undyneHP.undyneHP}");

        // 5) 잠깐 보여주고 비활성화 & 다음 스테이트로 전환
        yield return new WaitForSeconds(6f);
        _playerAttackBar.SetActive(false);
        _disableBarRoutine = null;
        _enemyStateMachine.undyneAdvanceState();
    }

    private IEnumerator DisableBarAndAdvance(float delay)
    {
        yield return new WaitForSeconds(delay);

        _playerAttackBar.SetActive(false);
        _disableBarRoutine = null;

        // 다음 언다인 상태로
        _enemyStateMachine.undyneAdvanceState();
    }

    public override void Update()
    {
        // 언다인 사망 체크
        if (_undyneHP.undyneHP <= 0)
        {
            _enemyStateMachine.ChangeState(_enemyStateMachine.CreatePlayerWinUndyneState());
        }
    }

    public override void Exit()
    {
        // 코루틴 정리
        if (_selectFirstRoutine != null)
            _enemyStateMachine.StopCoroutine(_selectFirstRoutine);
        if (_disableBarRoutine != null)
            _enemyStateMachine.StopCoroutine(_disableBarRoutine);

        // 리스너 제거
        _firstButton.onClick.RemoveListener(OnFirstButtonClicked);
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);

        // UI / 오브젝트 복구
        _playerAttackBar.SetActive(false);
        _player.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }
}

public class PlayerWinUndyneState : EnemyState
{
    private  BattlePlayerController _player;
    private UISystemManager _uiSystemManager;
    private GameState  _gameState;
    private GameObject _undyne;
    private GameObject _undyneUI;
    private GameObject _mainCamera;
    private GameObject _battleCamera;
    private GameObject _env;
    private GameObject _fieldPlayer;
    private SpriteRenderer _undyneImage;

    public PlayerWinUndyneState(EnemyStateMachine stateMachine, UISystemManager uiSystemManager,
        GameState gameState, BattlePlayerController player,GameObject undyne, GameObject undyneUI, GameObject mainCamera,
        GameObject battleCamera, GameObject env, GameObject fieldPlayer) : base(stateMachine)
    {
        _player = player;
        _uiSystemManager = uiSystemManager;
        _gameState = gameState;
        _undyne = undyne;
        _undyneUI = undyneUI;
        _mainCamera = mainCamera;
        _battleCamera = battleCamera;
        _env = env;
        _fieldPlayer = fieldPlayer;
        _undyneImage = _undyne.GetComponent<SpriteRenderer>();
    }

    public override void Enter()
    {
        _gameState.killCount++;
        _player.gameObject.SetActive(false);
        _undyne.SetActive(false);
        _undyneUI.SetActive(false);
        _mainCamera.SetActive(true);
        _battleCamera.SetActive(false);
        _env.SetActive(true);
        _fieldPlayer.gameObject.SetActive(true);
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

public class PlayerLoseUndyneState : EnemyState
{
    private GameObject _loseUI;
    public PlayerLoseUndyneState(EnemyStateMachine stateMachine, GameObject loseUI) : base(stateMachine)
    {
        _loseUI = loseUI;
    }
    public override void Enter()
    {
        _loseUI.SetActive(true);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
