using UnityEngine;
using Oculus.Interaction;
using UnityEngine.Events;

public class SS_InteractableEventWrapper : MonoBehaviour
{

    [SerializeField, Interface(typeof(IInteractableView))]
    private UnityEngine.Object _interactableView;
    private IInteractableView InteractableView;

    private bool _started;

    public UnityEvent<IInteractorView> WhenInteractorViewAddedParam;



    private void Awake()
    {
        InteractableView = _interactableView as IInteractableView;
    }

    private void Start()
    {
        this.BeginStart(ref _started);
        this.AssertField(InteractableView, nameof(InteractableView));
        this.EndStart(ref _started);
    }

    private void OnEnable()
    {
        if (_started)
        {
            InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
        }
    }

    private void OnDisable()
    {
        if (_started)
        {
            InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
        }
    }

    private void HandleInteractorViewAdded(IInteractorView interactorView)
    {
        WhenInteractorViewAddedParam.Invoke(interactorView);
    }
}
