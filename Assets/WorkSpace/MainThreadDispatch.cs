using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorkSpace
{
    public class MainThreadDispatch : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private bool visualization;
#pragma warning restore 649
        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();
        private static MainThreadDispatch _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        private void Update()
        {
            if (visualization)
            {
                StartCoroutine(ProgressiveInvoke());
            }
            else
            {
                lock (ExecutionQueue)
                {
                    while (ExecutionQueue.Count > 0)
                    {
                        ExecutionQueue.Dequeue().Invoke();
                    }
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(action.Invoke);
            }
        }

        public void Enqueue(Action<object> action, object parameter)
        {
            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(() => { action.Invoke(parameter); });
            }
        }

        public static bool Exists()
        {
            return _instance != null;
        }

        public static MainThreadDispatch Instance()
        {
            if (!Exists())
            {
                throw new Exception(
                    $"Could not find the {nameof(MainThreadDispatch)} object. Please ensure you have added the {nameof(MainThreadDispatch)} Prefab to your scene."
                );
            }

            return _instance;
        }

        private IEnumerator ProgressiveInvoke()
        {
            lock (ExecutionQueue)
            {
                while (ExecutionQueue.Count > 0)
                {
                    ExecutionQueue.Dequeue().Invoke();
                    yield return null;
                }
            }
        }
    }
}