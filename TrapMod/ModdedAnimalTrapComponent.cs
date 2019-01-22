// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Gameplay.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Utils.AtomicAction;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems;
    using Eco.Simulation.WorldLayers;
    using Items;
    using Shared.Serialization;
    using Shared.Utils;
    //modified following to 'Eco.' ... and added Types
    using Eco.Simulation;
    using Eco.Simulation.Agents;
    using Eco.Simulation.WorldLayers.Layers;
    using Eco.Simulation.Types;

    [Serialized]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Hunting")]
    public class ModdedAnimalTrapComponent : WorldObjectComponent
    {
        private const string AnimalCountState = "AnimalsInTrap";
        private const string HasAnimalsState = "HasAnimalsInTrap";
        PublicStorageComponent storage;
        private double rate = -1;
        // target layers?
        private List<AnimalLayer> targetLayers;

        public ModdedAnimalTrapComponent()
        { }

        public override void Initialize()
        {
            base.Initialize();
            WorldLayerSync.Obj.PreTickActions.Add(this.LayerTick);
        }

        internal IEnumerable<IAtomicAction> OnPickUp(Player player, InventoryChangeSet playerInvChanges)
        {
            yield return new SimpleAtomicAction(() => WorldLayerSync.Obj.PreTickActions.Remove(this.LayerTick));
        }

        public void Initialize(List<string> layers, double passed)
        {
            this.rate = passed;
            base.Initialize();
            var animalLayers = new List<AnimalLayer>();
            foreach (var layerName in layers)
                animalLayers.Add(WorldLayerManager.SpeciesToLayers[EcoSim.AllSpecies.FirstOrDefault(x => x.Name == layerName)] as AnimalLayer);
            this.targetLayers = animalLayers;
            this.storage = this.Parent.GetComponent<PublicStorageComponent>();
            this.storage.Initialize(4);

            this.storage.Inventory.OnChanged.Add(this.UpdateAnimalCount);
            this.UpdateAnimalCount();
        }
        public void LayerTick()
        {
            if (this.rate != -1)
            {
                // TODO this should be implemented via pull/interact/push pattern
                foreach (var layer in this.targetLayers)
                {
                    var layerPos = layer.WorldPosToLayerPos(this.Parent.Position.XZi);
                    var layerValue = layer.SafeEntry(layerPos);

                    // 8 times faster than fish traps
                    if (RandomUtil.Chance(layerValue/ (layer.MaxValue * (float)this.rate)))
                    {
                        var organisms = layer.SafePopMapEntry(layerPos);
                        if (organisms == null)
                            return;
                        var animal = organisms.Random() as Animal;

                        // give items
                        var newItems = Item.Create(animal.Species.ResourceItem, (int)animal.Species.ResourceRange.Max);
                        if (this.storage.Inventory.TryAddItems(animal.Species.ResourceItem, (int)animal.Species.ResourceRange.Max))
                            animal.Kill(DeathType.Harvesting);
                        return;
                    }
                }
            }
            else
            {
                return;
            }
            
        }

        private void UpdateAnimalCount(User user = null)
        {
            // Set # animals in trap to # of items in inventory
            // TODO: Make sure nothing else can get into the inventory besides trapped animals
            int count = this.storage.Inventory.Stacks.Sum(stack => stack.Quantity);
            this.Parent.SetAnimatedState(AnimalCountState, count);
            this.Parent.SetAnimatedState(HasAnimalsState, count > 0);
        }
    }
}
