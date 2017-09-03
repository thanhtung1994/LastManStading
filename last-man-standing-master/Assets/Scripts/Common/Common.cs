using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack();

public class CONSTANT
{
    #region Layer Mask
    public const string layerUnit = "Character";
    public const string layerBuilding = "Building";
    public const string layerSpawnCreep = "SpawnCreep";
    public const string TagUnit = "Unit";
    public const string TagBuilding = "Building";
    #endregion

    #region Animation name
    public const string AniAttack = "attack";
    public const string AniDead = "dead";
    public const string AniIdle = "idle";
    public const string AniStun = "stun";
    public const string AniMove = "move";
    public const string AniVarMoving = "Moving";
    public const string AniVarStun = "Stun";
    public const string AniVarAttackSpeed = "AS";
    public const string AniVarMovementSpeed = "MS";
    #endregion

    #region PATH
    public const string PathTextDamage = "Prefabs/TextDamage";
    public const string PathUISpellIcon = "UI/SpellIcon/spell_{0}";
    public const string PathUISkillIcon = "UI/SkillIcon/{0}";
    public const string PathUIUnitIconByCode = "UI/UnitIcon/icon_{0}";
    public const string PathUIUnitThumbByCode = "UI/UnitThumb/thumb_{0}";
    public const string PathPowerUp = "Prefabs/PowerUps/{0}";
    public const string PathSpell = "Prefabs/Spells/{0}";
    public const string PathSkillEffect = "Prefabs/SkillEffects/{0}";
    public const string VFXTakePowerUp = "TakePowerUpVFX";
    public const string PathToggleUnit = "Prefabs/UI/ToggleUnit";
    public const string PathBtnSelectUnit = "Prefabs/UI/BtnSelectUnit";
    public const string PathUIUnitSlot = "Prefabs/UI/UIUnitSlot";
    public const string PathFormatProjectilePrefabs = "Prefabs/Projectiles/{0}";
    public const string PathVFXPrefabs = "Prefabs/VFX/{0}";
    public const string PathPrefabUnit = "Prefabs/Units/{0}";
    public const string PathHpBar = "Prefabs/UI/HpBar";


    public const string MatSpriteGray = "Materials/GraySprite";
    public const string MatSpriteNormal = "Materials/NormalSprite";
    #endregion

    #region Name
    public const string VfxStun = "VfxStun";
    public const string VfxImpact = "VfxImpact";
    public const string VfxSpawn = "VfxSpawn";
    public const string VfxMoveTarget = "VfxMoveTarget";
    public const string VfxSkillBossAoe = "VfxSkillBossAoe";
    public const string VfxHealingEffect = "VfxHealingEffect";
    #endregion

    public const float RangeScale = 100;
    public const float MinRangeY = 0.1f;
    public const float TimePerIncome = 1;
    public const float TimePerSpawn = 0.05f;
    public const int SpriteFPS = 12;
    public const float FindTargetRadius = 6f;
    public const float HitTargetRadius = 0.05f;
    public const float BaseAttackSpeed = 1f;
    public const int MinDamage = 1;
    public const float MovementSpeedScale = 150;
    public const float AttackSpeedScale = 1000;
    public const float ProjectileSpeedScale = 2;
    public const float ScaleAxisY = 1f;

    public static Vector3 SpawnBoxSizeNormal = new Vector3(1.5f, 0, 0.5f);
    public static Vector3 SpawnBoxSizeNormalExt = new Vector3(2.5f, 0, 0.5f);
    public static Vector3 SpawnHidePosition = new Vector3(0, 2000, 2000);

    // Game Config
    public const int NumberUnitCanEquip = 6;
    public const float TimePerPowerUp = 30;
    public const float TimePerHoldingSpawn = 0.1f;
    public const int StartGold = 0;
    public const int StartSupply = 50;
    public const float ScalePerLevel = 0.1f;
    public const int MaxLevelUpgrade = 10;
    public const float Time_KnockUp = 1;

    // Game mode
    public static readonly float[] ScaleIncome = { 1f, 1.2f, 1.5f };
    public static readonly Vector3 TouchPositionPadding = new Vector3(0, 0);

    public static List<int> GoldIncome = new List<int>() { 10, 15, 20, 25, 30, 40 };
    public static List<int> GoldCapacity = new List<int>() { 100, 200, 400, 600, 800, 1000 };
    public static Color32 ColorRed = new Color32(255, 0, 0, 255);
    public static Color32 ColorGreen = new Color32(0, 255, 0, 255);
    public static Color32 ColorBlue = new Color32(0, 0, 255, 255);
}

public class GameMath
{
    public static float ReduceSplashDamageByDistance(float distance, float radius)
    {
        return 1 - distance / radius;
    }
}
