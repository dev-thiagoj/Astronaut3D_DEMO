using UnityEngine;


public class PlayerAbillityBase : MonoBehaviour
{
    protected Player player;

    protected Inputs inputs;

    private void OnValidate()
    {
        if (player == null) player = GetComponent<Player>();
    }

    private void Start()
    {
        Init();
        OnValidate();
        RegisterListeners();

        inputs = new Inputs();
        inputs.Enable();
    }

    private void OnEnable()
    {
        if (inputs != null) inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    protected virtual void Init()
    {

    }

    protected virtual void RegisterListeners()
    {

    }

    protected virtual void RemoveListeners()
    {

    }
}
