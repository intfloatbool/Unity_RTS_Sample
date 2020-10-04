namespace Units.Enums
{
    public enum UnitActionType: byte
    {
        NONE = 0,
        STOP = 1,
        MOVE_START = 2,
        MOVE_STOP = 3,
        ATTACK_START = 4,
        ATTACK_STOP = 5,
        SPELL_START = 6,
        SPELL_STOP = 7,
        DIE = 8
    }
}