using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

using Unity.Collections;


namespace RLTKTutorial.Part1_5A
{
    [System.Serializable]
    public struct ActionText : IComponentData
    {
        public FixedString32 value;
        public static implicit operator FixedString32(ActionText c) => c.value;
        public static implicit operator ActionText(FixedString32 v) => new ActionText { value = v };

        public static implicit operator string(ActionText c) => c.value.ToString();
        public static implicit operator ActionText(string v) => new ActionText { value = v };

        public override string ToString() => value.ToString();
    }
}