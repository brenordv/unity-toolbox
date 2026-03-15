using RaccoonNinjaToolbox.Scripts.Abstractions.Controllers;
using UnityEngine;

namespace RaccoonNinjaToolbox._Demo.Scripts
{
    public class SingletonGameObject: BaseSingletonController<SingletonGameObject>
    {
        [field: SerializeField] public int Count { get; private set; }
        
        public void IncrementCount()
        {
            Count++;
        }
    }
}