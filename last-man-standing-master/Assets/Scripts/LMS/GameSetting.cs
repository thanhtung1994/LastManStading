using System.Collections.Generic;

public static class GameSetting
{
    public const float BaseCriticalMultiple = 150;

    public readonly static List<int> ListHeroIndex = new List<int>()
    {
        1,// Berserker
        2,// Frost Ranger
        //3,// Fallen Angel
        //4,// Blade Master
        5,// Samurai
        6,// Light Sorcerer
        7,// Golem
        8,// Priest
        9,// Scout
        10,// Bard
    };

    public const float ScaleStatLevel = 1.1f;
    public const float ScaleMovementSpeed = 2;
    //public const float TimeWarningBeforeImpact = 2.5f;
}
