// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Objects;
    using Gameplay.Components;
    using System.Collections.Generic;

    [RequireComponent(typeof(AnimalTrapComponent))]
    public partial class TunaTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() {"Tuna"});
        }
    }
}