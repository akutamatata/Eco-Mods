// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Objects;
    using Gameplay.Components;
    using System.Collections.Generic;
    [RequireComponent(typeof(ModdedAnimalTrapComponent))]
    public partial class HareTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            //numbers below 1 will increase rate, numbers above 1 will lower rate, 8 is fish trap's rate, 7 digits is max for percision.
            this.GetComponent<ModdedAnimalTrapComponent>().Initialize(new List<string>() { "Hare" }, 16);
        }

    }
}