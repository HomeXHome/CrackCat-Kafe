using UnityEngine.Events;
public static class EventManager
{
    public static event UnityAction StartEvent;

    public static event UnityAction LoseEvent;
    public static event UnityAction WinEvent;

    public static void OnStartEvent() => StartEvent?.Invoke();

    public static void OnLoseEvent() => LoseEvent?.Invoke();

    public static void OnWinEvent() => WinEvent?.Invoke();

}
