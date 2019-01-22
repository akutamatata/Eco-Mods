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
    public partial class ElkTrapObject : 
        WorldObject,    
        IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Elk Trap"); } } 

        public virtual Type RepresentedItemType { get { return typeof(ElkTrapItem); } } 



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
    public partial class ElkTrapItem :
        WorldObjectItem<ElkTrapObject> 
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Elk Trap"); } } 
        public override LocString DisplayDescription  { get { return Localizer.DoStr("A trap which catches the Elkes as they hop!"); } }

        static ElkTrapItem()
        {
            
        }

        
    }


    [RequiresSkill(typeof(WoodworkingSkill), 3)]
    public partial class ElkTrapRecipe : Recipe
    {
        public ElkTrapRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<ElkTrapItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CamasBulbItem>(typeof(WoodworkingSkill), 40, WoodworkingSkill.MultiplicativeStrategy),                  //requires Camas bulbs      to make
                new CraftingElement<HuckleberriesItem>(typeof(WoodworkingSkill), 20, WoodworkingSkill.MultiplicativeStrategy),              //requires Huckleberries    to make
                new CraftingElement<LumberItem>(typeof(WoodworkingEfficiencySkill), 25, WoodworkingEfficiencySkill.MultiplicativeStrategy), //requires Lumber           to make
            };
            SkillModifiedValue value = new SkillModifiedValue(40, WoodworkingSkill.MultiplicativeStrategy, typeof(WoodworkingSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(ElkTrapRecipe), Item.Get<ElkTrapItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<ElkTrapItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize(Localizer.DoStr("Elk Trap"), typeof(ElkTrapRecipe));
            CraftingComponent.AddRecipe(typeof(CarpentryTableObject), this);
        }
    }
}