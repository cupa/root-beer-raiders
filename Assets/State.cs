using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{

    public abstract class State<T>
    {
        protected T StatePlayer { get; }
        protected StateMachine<T> Machine { get; }

        public State(T StatePlayer, StateMachine<T> Machine)
        {
            this.StatePlayer = StatePlayer;
            this.Machine = Machine;
        }
        public abstract void OnBeginState();

        public abstract void OnUpdate();

        public abstract void OnExitState();
    }

    public class StateMachine<T>
    {
        protected State<T> CurrentState;
        private StateListener<T> stateListener;

        public StateMachine(StateListener<T> StateListener)
        {
            this.stateListener = StateListener;
        }

        public void SetState(State<T> NewState)
        {
            if (HasState())
            {
                CurrentState.OnExitState();
                stateListener.OnStateEnd(CurrentState);
            }
            NewState.OnBeginState();
            stateListener.OnStateBegin(NewState);
            NewState.OnUpdate();
            stateListener.OnStateUpdate(NewState);
            CurrentState = NewState;
        }

        public void OnUpdate()
        {
            CurrentState.OnUpdate();
            stateListener.OnStateUpdate(CurrentState);
        }

        public State<T> GetCurrent()
        {
            return CurrentState;
        }

        public bool HasState()
        {
            return CurrentState != null;
        }
    }
    class State
    {
    }
}
