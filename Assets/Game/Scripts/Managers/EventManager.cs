using System;

namespace Game.Events
{
    public class EventManager
    {
        public static Action GameStarted;
        public static Action SuccessfulMove;
        public static Action MissedInput;
        public static Action CharacterLose;
    }
}
