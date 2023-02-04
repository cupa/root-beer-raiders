using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class StateListener<T>
    {
        public static event Action<State<T>> OnStateBeginListeners;
        public static event Action<State<T>> OnStateUpdateListeners;
        public static event Action<State<T>> OnStateEndListeners;

        public void OnStateBegin(State<T> State)
        {
            OnStateBeginListeners?.Invoke(State);
        }

        public void OnStateUpdate(State<T> State)
        {
            OnStateUpdateListeners?.Invoke(State);
        }

        public void OnStateEnd(State<T> State)
        {
            OnStateEndListeners?.Invoke(State);
        }
    }
}
