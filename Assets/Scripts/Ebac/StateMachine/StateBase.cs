using UnityEngine;


namespace Ebac.StateMachine
{
    public class StateBase
    {
        public virtual void OnStateEnter(params object[] objs)
        {
            //Debug.Log("On " + this + " Enter");
        }

        public virtual void OnStateStay()
        {
            //Debug.Log("On " + this + " Enter");
        }

        public virtual void OnStateExit()
        {
            //Debug.Log("On " + this + " Enter");
        }
    }
}