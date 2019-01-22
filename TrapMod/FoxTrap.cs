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
    public partial class FoxTrapObject : 
        WorldObject,    
        IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Fox Trap"); } } 

        public virtual Type RepresentedItemType { get { return typeof(FoxTrapItem); } } 



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
    public partial class FoxTrapItem :
        WorldObjectItem<FoxTrapObject> 
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Fox Trap"); } } 
        public override LocString DisplayDescription  { get { return Localizer.DoStr("A trap which catches the foxes as they hop!"); } }

        static FoxTrapItem()
        {
            
        }

        
    }


    [RequiresSkill(typeof(WoodworkingSkill), 2)]
    public partial class FoxTrapRecipe : Recipe
    {
        public FoxTrapRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<FoxTrapItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<ScrapMeatItem>(typeof(WoodworkingSkill), 40, WoodworkingSkill.MultiplicativeStrategy),                  //requires Scrap meat       to make
                new CraftingElement<BoardItem>(typeof(WoodworkingEfficiencySkill), 25, WoodworkingEfficiencySkill.MultiplicativeStrategy),  //requires Lumber           to make 
            };
            SkillModifiedValue value = new SkillModifiedValue(25, WoodworkingSkill.MultiplicativeStrategy, typeof(WoodworkingSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(FoxTrapRecipe), Item.Get<FoxTrapItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<FoxTrapItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize(Localizer.DoStr("Fox Trap"), typeof(FoxTrapRecipe));
            CraftingComponent.AddRecipe(typeof(CarpentryTableObject), this);
        }
    }
}