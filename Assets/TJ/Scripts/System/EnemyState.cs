public abstract class EnemyState
{

    protected EnemyStateMachine _enemyStateMachine;

    public EnemyState(EnemyStateMachine stateMachine)
    {
        _enemyStateMachine = stateMachine;
    }
    
    // 상태 진입 시 필요한 초기화 동작
    public virtual void Enter() { }

    // 상태가 활성화되었을 때 매 프레임 호출
    public virtual void Update() { }

    // 상태 종료 시 정리 작업
    public virtual void Exit() { }
}