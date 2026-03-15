using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Abstractions.Controllers
{

    public abstract class BaseSingletonController<T> : MonoBehaviour where T: BaseSingletonController<T>
    {
        private static T _instance;

        public static T Instance => _instance;

        static BaseSingletonController()
        {
            SingletonResetter.Register(() => _instance = null);
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = (T)this;
            PostAwake();
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        protected virtual void PostAwake() {}
    }
}