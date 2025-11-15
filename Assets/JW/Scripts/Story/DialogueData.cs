using System.Collections.Generic;

public class DialogueData
{
    private readonly Dictionary<int, DialogueNode> dialogueMap = new Dictionary<int, DialogueNode>();

    public DialogueData()
    {
        // Intro Dialogue
        {
            DialogueNode startNode = new DialogueNode("여긴.. 어디지?",
                GameCharacter.Player);
            dialogueMap.Add(DialogueID.GetID("Intro"), startNode);
            
            DialogueNode node1 = new DialogueNode("난 분명 집에 있었는데.. 납치라도 당한건가?",
                GameCharacter.Player);
            startNode.SetNextNode(node1);
            
            DialogueNode node2 = new DialogueNode("여기가 어딘지는 몰라도, 분명 빠져나갈 방법이 있을거야.",
                GameCharacter.Player);
            node1.SetNextNode(node2);
        }
        
        // Kill Count = 1
        {
            DialogueNode startNode = new DialogueNode("..음.",
                GameCharacter.Player);
            dialogueMap.Add(DialogueID.GetID("First"), startNode);
            
            DialogueNode node1 = new DialogueNode("뭔가 얼굴 쪽에 따뜻한 햇살 같은게 느껴지는 것 같은데.",
                GameCharacter.Player);
            startNode.SetNextNode(node1);
            
            DialogueNode node2 = new DialogueNode("기분 탓인가? 아니면 뭔가 내가 놓치고 있는게 있는건가?",
                GameCharacter.Player);
            node1.SetNextNode(node2);
            
            DialogueNode node3 = new DialogueNode("이 괴물들을 계속 잡다보면 뭔가 단서 같은 걸 얻을 수 있을지도 모르겠어.",
                GameCharacter.Player);
            node2.SetNextNode(node3);
            
            DialogueNode node4 = new DialogueNode("Hp가 10 증가하였습니다.\n회복 물약 1개를 얻었습니다.",
                GameCharacter.Player);
            node3.SetNextNode(node4);
        }
        
        // Kill Count = 2
        {
            DialogueNode startNode = new DialogueNode("..이번엔 뭔가 미묘하게 처음과는 기온 자체가 달라졌어.",
                GameCharacter.Player);
            dialogueMap.Add(DialogueID.GetID("Second"), startNode);
            
            DialogueNode node1 = new DialogueNode("아까 전의 미묘한 느낌도 그렇고, 확실히 온도가 높아지고 있는 것 같아.",
                GameCharacter.Player);
            startNode.SetNextNode(node1);
            
            DialogueNode node2 = new DialogueNode("무슨 일이 일어나는 건지.. 아무튼 계속 이대로 가보자.",
                GameCharacter.Player);
            node1.SetNextNode(node2);
            
            DialogueNode node3 = new DialogueNode("Hp가 10 증가하였습니다.\n회복 물약 1개를 얻었습니다.",
                GameCharacter.Player);
            node2.SetNextNode(node3);
        }
        
        // Kill Count = 4
        {
            DialogueNode startNode = new DialogueNode("..이 광경은.. 내 방?",
                GameCharacter.Player);
            dialogueMap.Add(DialogueID.GetID("Third"), startNode);
            
            DialogueNode node1 = new DialogueNode("아침이라고? 내가 잠들었을 때만 해도 한밤중이었는데.",
                GameCharacter.Player);
            startNode.SetNextNode(node1);
            
            DialogueNode node2 = new DialogueNode("뭔가 이상하다 싶었더니, 이제 알겠네... 여긴 꿈 속이야.",
                GameCharacter.Player);
            node1.SetNextNode(node2);
            
            DialogueNode node3 = new DialogueNode("...? 잠깐. 아침..? 아침....",
                GameCharacter.Player);
            node2.SetNextNode(node3);
            
            DialogueNode node4 = new DialogueNode("......헉!",
                GameCharacter.Player);
            node3.SetNextNode(node4);
            
            DialogueNode node5 = new DialogueNode("큰일났다! 나 출근해야되는데!!",
                GameCharacter.Player);
            node4.SetNextNode(node5);
            
            DialogueNode node6 = new DialogueNode("빨리 깨어나야만 해! 계속 앞으로 가다보면 깰 수 있는 방법이 나오겠지..?",
                GameCharacter.Player);
            node5.SetNextNode(node6);
            
            DialogueNode node7 = new DialogueNode("Hp가 10 증가하였습니다.\n회복 물약 1개를 얻었습니다.",
                GameCharacter.Player);
            node6.SetNextNode(node7);
        }
        
        // Boss Stage Dialogue
        {
            DialogueNode startNode = new DialogueNode("넌 못 지나간다.",
                GameCharacter.Boss, CharacterEmotion.Normal);
            dialogueMap.Add(DialogueID.GetID("Boss"), startNode);
            
            DialogueNode node1 = new DialogueNode("..넌 누구야?",
                GameCharacter.Player);
            startNode.SetNextNode(node1);
            
            DialogueNode node2 = new DialogueNode("이곳의 문지기.",
                GameCharacter.Boss, CharacterEmotion.Normal);
            node1.SetNextNode(node2);
            
            DialogueNode node3 = new DialogueNode("문지기라고..? 그럼 저 문 너머에는 뭐가 있는데?",
                GameCharacter.Player);
            node2.SetNextNode(node3);
            
            DialogueNode node4 = new DialogueNode("나야 모르지. 안 가봤으니까.",
                GameCharacter.Boss, CharacterEmotion.Cocky);
            node3.SetNextNode(node4);
            
            DialogueNode node5 = new DialogueNode("되게 불친절하네. 그럼 저 문을 넘으면 이곳은 벗어날 수 있는거네? 난 여기서 빠져나가야겠어!",
                GameCharacter.Player);
            node4.SetNextNode(node5);
            
            DialogueNode node6 = new DialogueNode("할 수 있으면.",
                GameCharacter.Boss, CharacterEmotion.Serious);
            node5.SetNextNode(node6);
        }
    }

    public DialogueNode GetNode(int dialogueId)
    {
        return dialogueMap[dialogueId];
    }

    public void SetBossDialogueForTrueEnding()
    {
        DialogueNode startNode = dialogueMap[DialogueID.GetID("Boss")];
        
        DialogueNode node1 = new DialogueNode("..라고 하기에는 이미 다 알고 있다는 표정이네?",
            GameCharacter.Boss, CharacterEmotion.Normal);
        startNode.SetNextNode(node1);
            
        DialogueNode node2 = new DialogueNode("..뭐야 너, 나 알아?",
            GameCharacter.Player);
        node1.SetNextNode(node2);
            
        DialogueNode node3 = new DialogueNode("알지 그럼. 난 '너'니까. 다른 괴물들은 꿈의 파편같은 존재들이지만, 난 다르거든.",
            GameCharacter.Boss, CharacterEmotion.Cocky);
        node2.SetNextNode(node3);
            
        DialogueNode node4 = new DialogueNode(".....그 말인 즉슨, 지금이 무슨 상황인지도 다 안다는 뜻이겠네?",
            GameCharacter.Player);
        node3.SetNextNode(node4);
            
        DialogueNode node5 = new DialogueNode("당연하지. 곧 출근할 시간이잖아.",
            GameCharacter.Boss, CharacterEmotion.Normal);
        node4.SetNextNode(node5);
            
        DialogueNode node6 = new DialogueNode("알고 있다면 좀 비켜줄래? 나 또 지각하면 큰일나.",
            GameCharacter.Player);
        node5.SetNextNode(node6);
            
        DialogueNode node7 = new DialogueNode("싫어. 난 더 잘거야. 10분만 더 자고 일어날래.",
            GameCharacter.Boss, CharacterEmotion.Cocky);
        node6.SetNextNode(node7);
            
        DialogueNode node8 = new DialogueNode("짤리고 싶은 거야, 뭐야? 이번 달 카드 값 내야 돼. 빨리 비켜.",
            GameCharacter.Player);
        node7.SetNextNode(node8);
            
        DialogueNode node9 = new DialogueNode("안 돼. 안 비켜줘. 비켜줄 생각 없어. 돌아가.",
            GameCharacter.Boss, CharacterEmotion.Serious);
        node8.SetNextNode(node9);
            
        DialogueNode node10 = new DialogueNode("..안되겠다. 두들겨서라도 빨리 일어나야겠어.",
            GameCharacter.Player);
        node9.SetNextNode(node10);
    }
}
