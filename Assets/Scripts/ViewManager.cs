using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager s_instance;

    [SerializeField] private View _startingView;

    //all views
    [SerializeField] private View[] _views;

    private View _currentView;

    //history of opend views to go back
    private readonly Stack<View> _history = new Stack<View>();

    //get specific view
    public static T GetView<T>() where T : View
    {
        for (int i = 0; i < s_instance._views.Length; i++)
        {
            if (s_instance._views[i] is T tView)
            {
                return tView;
            }
        }

        return null;
    }

    public static void Show<T>(bool remember = true) where T : View
    {
        for (int i = 0; i < s_instance._views.Length; i++)
        {
            if (s_instance._views[i] is T tView)
            {
                RememberAndShow(remember, tView);
            }
        }
    }

    public static void Show(View view, bool remember = true)
    {
        RememberAndShow(remember, view);
    }

    //removes last view from Stack and shows it
    public static void ShowLast()
    {
        if (s_instance._history.Count != 0)
        {
            Show(s_instance._history.Pop(), false);
        }
    }

    private void Awake() => s_instance = this;

    private void Start()
    {
        for (int i = 0; i < s_instance._views.Length; i++)
        {
            _views[i].Initialize();

            _views[i].Hide();
        }

        if(_startingView != null)
        {
            Show(_startingView, true);
        }
    }

    private static void RememberAndShow<T>(bool remember, T view) where T : View
    {
        if (s_instance._currentView != null)
        {
            if (remember)
            {
                s_instance._history.Push(s_instance._currentView);
            }

            s_instance._currentView.Hide();
        }

        view.Show();

        s_instance._currentView = view;
    }
}
