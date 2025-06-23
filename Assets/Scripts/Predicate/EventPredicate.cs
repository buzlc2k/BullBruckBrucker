using System;
using UnityEngine;

namespace BullBrukBruker{
    public class EventPredicate : Predicate
    {
        private readonly EventID listenedEvent;
        private bool isEventTriggered = false;
        private bool predicateRunning = false;
        private readonly Action<object> onEventTriggered;

        public EventPredicate(EventID listenedEvent)
        {
            this.listenedEvent = listenedEvent;

            onEventTriggered = (param) =>
            {
                if (predicateRunning) isEventTriggered = true;
            };

            Observer.AddListener(this.listenedEvent, onEventTriggered);
        }

        public EventPredicate(EventID listenedEvent, object eventValueCondition, bool equalCondition = true)
        {
            this.listenedEvent = listenedEvent;

            onEventTriggered = (param) =>
            {
                if (param.Equals(eventValueCondition) == equalCondition)
                    if (predicateRunning) isEventTriggered = true;
            };

            Observer.AddListener(this.listenedEvent, onEventTriggered);
        }

        ~EventPredicate()
        {
            Observer.RemoveListener(listenedEvent, onEventTriggered);
        }

        public override bool Evaluate()
        {
            predicateRunning = true;
            
            return isEventTriggered;
        }

        public override void StopPredicate()
        {
            isEventTriggered = false;
            predicateRunning = false;
        }
    }
}