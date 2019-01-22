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
    public partial class TortoiseTrapObject : 
        WorldObject,    
        IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Tortoise Trap"); } } 

        public virtual Type RepresentedItemType { get { return typeof(TortoiseTrapItem); } } 



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
    public partial class TortoiseTrapItem :
        WorldObjectItem<TortoiseTrapObject> 
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Tortoise Trap"); } } 
        public override LocString DisplayDescription  { get { return Localizer.DoStr("A trap which catches the Tortoisees as they hop!"); } }

        static TortoiseTrapItem()
        {
            
        }

        
    }


    [RequiresSkill(typeof(WoodworkingSkill), 1)]
    public partial class TortoiseTrapRecipe : Recipe
    {
        public TortoiseTrapRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<TortoiseTrapItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LogItem>(typeof(WoodworkingSkill), 20, WoodworkingSkill.MultiplicativeStrategy),   
            };
            SkillModifiedValue value = new SkillModifiedValue(10, WoodworkingSkill.MultiplicativeStrategy, typeof(WoodworkingSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(TortoiseTrapRecipe), Item.Get<TortoiseTrapItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<TortoiseTrapItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize(Localizer.DoStr("Tortoise Trap"), typeof(TortoiseTrapRecipe));
            CraftingComponent.AddRecipe(typeof(CarpentryTableObject), this);
        }
    }
}