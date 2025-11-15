using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateMachine))]
public class EnemyStateMachineEditor : Editor
{
    private SerializedProperty enemyType;

        // 메타톤
        private  SerializedProperty mettablock;
        private SerializedProperty mettabomb;
        private SerializedProperty umbrellabomb;
        private SerializedProperty mettaHeart;
        private SerializedProperty blockArray;
        private SerializedProperty mettaton;
        private SerializedProperty mettatonText;
        private SerializedProperty RECUI;
        private SerializedProperty REWUI;
        private SerializedProperty mettatonClip;
        
        // 언다인
        private SerializedProperty undyne;
        private SerializedProperty dodgeArrow;
        private SerializedProperty trackingArrow;
        private SerializedProperty crossArrowSpawner;
        private SerializedProperty heptagonSpawner;
        private SerializedProperty risingArrowSpawner;
        private SerializedProperty arrowPlayer;
        private SerializedProperty undyneClip;
        private SerializedProperty undyneHP;
        private SerializedProperty playerAttackBar;

        private void OnEnable()
        {
            enemyType = serializedObject.FindProperty("enemyType");

            mettablock = serializedObject.FindProperty("mettablock");
            mettabomb = serializedObject.FindProperty("mettabomb");
            umbrellabomb = serializedObject.FindProperty("umbrellabomb");
            mettaHeart = serializedObject.FindProperty("mettaHeart");
            blockArray = serializedObject.FindProperty("blockArray");
            mettatonText = serializedObject.FindProperty("mettatonText");
            mettaton = serializedObject.FindProperty("mettaton");
            RECUI = serializedObject.FindProperty("RECUI");
            REWUI = serializedObject.FindProperty("REWUI");
            mettatonClip = serializedObject.FindProperty("mettatonClip");

            undyne = serializedObject.FindProperty("undyne");
            dodgeArrow = serializedObject.FindProperty("dodgeArrow");
            trackingArrow = serializedObject.FindProperty("trackingArrow");
            crossArrowSpawner = serializedObject.FindProperty("crossArrowSpawner");
            heptagonSpawner = serializedObject.FindProperty("heptagonSpawner");
            risingArrowSpawner = serializedObject.FindProperty("risingArrowSpawner");
            arrowPlayer = serializedObject.FindProperty("arrowPlayer");
            undyneClip = serializedObject.FindProperty("undyneClip");
            undyneHP = serializedObject.FindProperty("undyneHP");
            playerAttackBar = serializedObject.FindProperty("playerAttackBar");
            
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(enemyType);
            
            EditorGUILayout.Space();

            if ((EnemyType)enemyType.enumValueIndex == EnemyType.Mettaton)
            {
                EditorGUILayout.PropertyField(mettablock);
                EditorGUILayout.PropertyField(mettabomb);
                EditorGUILayout.PropertyField(umbrellabomb);
                EditorGUILayout.PropertyField(mettaHeart);
                EditorGUILayout.PropertyField(blockArray);
                EditorGUILayout.PropertyField(mettaton);
                EditorGUILayout.PropertyField(mettatonText);
                EditorGUILayout.PropertyField(RECUI);
                EditorGUILayout.PropertyField(REWUI);
                EditorGUILayout.PropertyField(mettatonClip);
            }
            else if ((EnemyType)enemyType.enumValueIndex == EnemyType.Undyne)
            {
                EditorGUILayout.PropertyField(undyne);
                EditorGUILayout.PropertyField(dodgeArrow);
                EditorGUILayout.PropertyField(trackingArrow);
                EditorGUILayout.PropertyField(crossArrowSpawner);
                EditorGUILayout.PropertyField(heptagonSpawner);
                EditorGUILayout.PropertyField(risingArrowSpawner);
                EditorGUILayout.PropertyField(arrowPlayer);
                EditorGUILayout.PropertyField(undyneClip);
                EditorGUILayout.PropertyField(undyneHP);
                EditorGUILayout.PropertyField(playerAttackBar);

            }

            // 나머지 기본 인스펙터 표시
            DrawPropertiesExcluding(serializedObject,
                "enemyType",
                "mettablock", "mettabomb", "umbrellabomb", "mettaHeart", "blockArray",
                "mettaton", "mettatonText", "RECUI", "REWUI","mettatonClip",
                "undyne", "dodgeArrow", "trackingArrow", "crossArrowSpawner",
                "heptagonSpawner", "risingArrowSpawner", "arrowPlayer","undyneClip",
                "undyneHP","playerAttackBar"
            );

            serializedObject.ApplyModifiedProperties();
        }
    
}