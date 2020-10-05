namespace Units.Controllers.Enums
{
    public enum AiBehaviorType: byte
    {
        NONE = 0,
        MOVE_TO_TARGET = 1,
        MOVE_TO_TARGET_AND_ATTACK_ENEMIES = 2,
        ROAMING = 3,
        ROAMING_AND_ATTACK_ENEMIES = 4,
        STAND = 5,
        STAND_AND_ATTACK_ENEMIES = 6
    }
}