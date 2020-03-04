using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

using Unity.Collections;


namespace RLTKTutorial.Part1_5A
{
    [System.Serializable]
    public struct WelcomeText : IComponentData
    {
        public FixedString32 value;
        public static implicit operator FixedString32(WelcomeText c) => c.value;
        public static implicit operator WelcomeText(FixedString32 v) => new WelcomeText { value = v };

        public static implicit operator string(WelcomeText c) => c.value.ToString();
        public static implicit operator WelcomeText(string v) => new WelcomeText { value = v };

        public override string ToString() => value.ToString();
    }
}