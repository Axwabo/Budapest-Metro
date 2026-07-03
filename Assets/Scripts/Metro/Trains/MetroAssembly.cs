using System.Collections.Generic;
using UnityEngine;

namespace Metro.Trains
{

    public sealed class MetroAssembly : MonoBehaviour
    {

        private readonly List<AssemblyComponent> _components = new();

        private void Start() => this.InitializeComponents(_components);

    }

}
