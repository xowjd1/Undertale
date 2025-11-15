public class DialogueNode
{
    public string Data { get; private set; }
    
    private readonly GameCharacter characterName;
    private readonly CharacterEmotion emotion;
    private DialogueNode nextNode;
    
    public DialogueNode(string data, GameCharacter characterName, CharacterEmotion emotion = CharacterEmotion.Normal)
    {
        Data = data;
        this.characterName = characterName;
        this.emotion = emotion;
    }

    public GameCharacter WhoIsThis()
    {
        return characterName;
    }

    public CharacterEmotion GetEmotion()
    {
        return emotion;
    }

    public void SetNextNode(DialogueNode next)
    {
        nextNode = next;
    }

    public DialogueNode MoveNext()
    {
        return nextNode;
    }
}