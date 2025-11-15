using System.Collections.Generic;
using UnityEngine;

public class NpcImage
{
    private readonly Dictionary<GameCharacter, Dictionary<CharacterEmotion, Sprite>> characterMap = new Dictionary<GameCharacter, Dictionary<CharacterEmotion, Sprite>>();
    
    public NpcImage()
    {
        characterMap.Add(GameCharacter.Boss, new Dictionary<CharacterEmotion, Sprite>());
    }

    public void SetSprites(GameCharacter character, CharacterEmotion emotion, Sprite sprite)
    {
        characterMap[character].Add(emotion, sprite);
    }

    public Sprite GetSprite(GameCharacter character, CharacterEmotion emotion)
    {
        return characterMap[character][emotion];
    }
}
