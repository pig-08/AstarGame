namespace GMS.Code.Core.Events
{
    public static class MapEvents
    {
        public static HealEvent HealEvent = new HealEvent();
        public static HitEvent HitEvent = new HitEvent();
        public static DeadEvent DeadEvent = new DeadEvent();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
    }

    public class ClearEvent : GameEvent
    {

    }

    public class HealEvent : GameEvent
    {

    }

    public class HitEvent : GameEvent
    {
        public bool isShield;
    }

    public class DeadEvent : GameEvent
    {

    }

    public class GameOverEvent : GameEvent
    {

    }
}
