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
    public partial class BisonTrapObject : 
        WorldObject,    
        IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Bison Trap"); } } 

        public virtual Type RepresentedItemType { get { return typeof(BisonTrapItem); } } 



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
    public partial class BisonTrapItem :
        WorldObjectItem<BisonTrapObject> 
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Bison Trap"); } } 
        public override LocString DisplayDescription  { get { return Localizer.DoStr("A trap which catches the Bisones as they Run."); } }

        static BisonTrapItem()
        {
            
        }

        
    }


    [RequiresSkill(typeof(WoodworkingSkill), 4)]
    public partial class BisonTrapRecipe : Recipe
    {
        public BisonTrapRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<BisonTrapItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<WheatItem>(typeof(WoodworkingSkill), 15, WoodworkingSkill.MultiplicativeStrategy),                      //requires wheat            to make
                new CraftingElement<HuckleberriesItem>(typeof(WoodworkingSkill), 40, WoodworkingSkill.MultiplicativeStrategy),              //requires Huckleberries    to make
                new CraftingElement<BoardItem>(typeof(WoodworkingEfficiencySkill), 25, WoodworkingEfficiencySkill.MultiplicativeStrategy),  //requires boards           to make
                new CraftingElement<ScrewsItem>(typeof(WoodworkingEfficiencySkill), 5, WoodworkingEfficiencySkill.MultiplicativeStrategy),  //requires screws(iron)     to make
            };
            SkillModifiedValue value = new SkillModifiedValue(45, WoodworkingSkill.MultiplicativeStrategy, typeof(WoodworkingSkill), Localizer.DoStr("craft time"));  //45 minute base time
            SkillModifiedValueManager.AddBenefitForObject(typeof(BisonTrapRecipe), Item.Get<BisonTrapItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<BisonTrapItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize(Localizer.DoStr("Bison Trap"), typeof(BisonTrapRecipe));
            CraftingComponent.AddRecipe(typeof(CarpentryTableObject), this);
        }
    }
}