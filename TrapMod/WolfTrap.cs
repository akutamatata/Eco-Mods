namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    
    [Serialized]    
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]                
    [RequireComponent(typeof(LinkComponent))]                   
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class WolfTrapObject : 
        WorldObject,    
        IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Wolf Trap"); } } 

        public virtual Type RepresentedItemType { get { return typeof(WolfTrapItem); } } 



        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Economy");                                 


        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class WolfTrapItem :
        WorldObjectItem<WolfTrapObject> 
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Wolf Trap"); } } 
        public override LocString DisplayDescription  { get { return Localizer.DoStr("A trap which catches the Wolfes as they hop!"); } }

        static WolfTrapItem()
        {
            
        }

        
    }


    [RequiresSkill(typeof(WoodworkingSkill), 3)]
    public partial class WolfTrapRecipe : Recipe
    {
        public WolfTrapRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<WolfTrapItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<RawMeatItem>(typeof(WoodworkingSkill), 15, WoodworkingSkill.MultiplicativeStrategy),                    //requires Raw Meat         to make
                new CraftingElement<LumberItem>(typeof(WoodworkingEfficiencySkill), 20, WoodworkingEfficiencySkill.MultiplicativeStrategy), //requires Lumber           to make 

            };
            SkillModifiedValue value = new SkillModifiedValue(40, WoodworkingSkill.MultiplicativeStrategy, typeof(WoodworkingSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(WolfTrapRecipe), Item.Get<WolfTrapItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<WolfTrapItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize(Localizer.DoStr("Wolf Trap"), typeof(WolfTrapRecipe));
            CraftingComponent.AddRecipe(typeof(CarpentryTableObject), this);
        }
    }
}