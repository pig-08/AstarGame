using UnityEngine;

namespace GMS.Code.Core.Events
{
    public static class InGameEvents
    {
        public static PlayerDestroyedEvent PlayerDestroyedEvent = new PlayerDestroyedEvent();
    }

    public class PlayerDestroyedEvent : GameEvent
    {

    }
}