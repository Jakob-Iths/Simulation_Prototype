using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Simulation_Prototype.Classes;

public class CollectionData
{
	public bool cap = true;
	public bool NoPredators = false;
	public int evolveRate = 100;
	public int maxStat = 20;
	public int maxType = 2;
	public int maxStats;
	int numberOfStats = 6;

	public int creatureIdDecider = 1;
	public double subtract = 1;
	public double add;
	public int maxTilesZone1 = 100;
	public int maxTilesZone2 = 50;
	public int numberOfFoodType1;
	public int numberOfFoodType2;
	public int numberOfFoodType3;
	public int numberOfFoodType4;
	public int numberOfFoodType5;
	public double dividedBy1 = 0.8;
	public double dividedBy2 = 0.3;
	public double dividedBy3 = 0.2;
	public double dividedBy4 = 0.6;
	public double dividedBy5 = 0.1;

	public int type1 = 0;
	public int type2 = 0;
	public int type3 = 0;
	public int type4 = 0;
	public int type5 = 0;
	public float averageLiving;
	public float averageDead;
	public float averageAge;
	public int previousCreature;
	public int previousPower;
	public int previousForage;
	public bool previousType;
	public int previousStealth;
	public int previousSpeed;
	public int previousZone;
	public bool previousPoison;
	public int previousPoisonStr;

	public int standardFoodZone1 = 1;
	public int standardFoodZone2 = 3;

	public int predatorsZone1;
	public int predatorsZone2;
	public bool isPredator = false;
	public bool tried;
	public int test = 1;
	public int test2 = 0;
	public int numberOfPredators1;


	public int idDecider;
	public Random rnd = new Random();

	public List<Food> Foods1 = new List<Food>();
	public List<Food> Foods2 = new List<Food>();
	public List<Food> EatenFood = new List<Food>();
	public List<Food> RemovedFood = new List<Food>();
	public List<Food> FoodHolder = new List<Food>();

	private readonly List<Creature> _creatures = new List<Creature>();
	public List<Creature> Creatures => _creatures;

	public List<Creature> ActiveCreatures = new List<Creature>();
	public List<Creature> ActiveCreatures2 = new List<Creature>();

	public List<Creature> Waitingcreatures = new List<Creature>();

	public List<Creature> DyingCreatures = new List<Creature>();


	int startingCreatures = 5;
	public void GenerateCreatures()
	{
		for (int i = 0; i < startingCreatures; i++)
		{
			idDecider = i + 1;
			Creature creature = new Creature(idDecider.ToString(), 5, 5, 5, 0, 1, 0, 2, 0, 0, 5, false, 5, 0, 1, false, 5, 5, false, 1, creatureIdDecider, false);
			creatureIdDecider++;
			ActiveCreatures.Add(creature);
		}
	}

	public void GenerateFood()
	{
		// Zone 1
		numberOfFoodType1 = Convert.ToInt32(maxTilesZone1 * dividedBy1);
		for (var i = 0; i < numberOfFoodType1; i++)
		{
			Food food = new Food(maxTilesZone1 - i, 1, false, false, 0, 1, 0);
			Foods1.Add(food);
		}
		numberOfFoodType2 = Convert.ToInt32(maxTilesZone1 * dividedBy2);

		for (var i = 0; i < numberOfFoodType2; i++)
		{
			Food food = new Food(maxTilesZone1 - i, 2, false, false, 0, 1, 8);
			Foods1.Add(food);
		}

		// Zone 2
		numberOfFoodType3 = Convert.ToInt32(maxTilesZone2 * dividedBy3);

		for (var i = 0; i < numberOfFoodType3; i++)
		{
			Food food = new Food(maxTilesZone2 - i, 3, false, false, 0, 2, 0);
			Foods1.Add(food);
		}

		numberOfFoodType4 = Convert.ToInt32(maxTilesZone2 * dividedBy4);

		for (var i = 0; i < numberOfFoodType4; i++)
		{
			Food food = new Food(maxTilesZone2 - i, 4, true, false, 0, 2, 5);
			Foods1.Add(food);
		}

		numberOfFoodType5 = Convert.ToInt32(maxTilesZone2 * dividedBy5);

		for (var i = 0; i < numberOfFoodType5; i++)
		{
			Food food = new Food(maxTilesZone2 - i, 5, false, true, 8, 2, 0);
			Foods1.Add(food);
		}



	}

	public void SortList(int i)
	{
		if (i % 50 == 1)
		{
			ActiveCreatures = ActiveCreatures.OrderByDescending(creature => creature.Speed).ToList();
		}
		else
		{
			ActiveCreatures = ActiveCreatures.OrderBy(_ => rnd.Next()).ToList();
		}
	}
	public void Forage()
	{
		Foods1 = Foods1.OrderByDescending(Food => Food.Id).ToList();
		previousForage = 999;
		foreach (var creature in ActiveCreatures)
		{

			predatorsZone1 = 0;
			predatorsZone2 = 0;
			if (creature.Zone == 1)
				ZoneForage(creature, maxTilesZone1, predatorsZone1, 1);

			else if (creature.Zone == 2)
				ZoneForage(creature, maxTilesZone2, predatorsZone2, 2);
		}
	}

	public void ZoneForage(Creature creature, int maxTiles, int predators, int zone)
	{
		bool isPrey = false;
		bool hasEaten = false;
		for (int i = 0; i < creature.Energy / 2; i++)
		{
			creature.Forage = rnd.Next(maxTiles) + creature.Sense;

			if (creature.Forage < previousForage && creature.Predator == true)
			{
				continue;
			}

			if (previousPower < creature.Power && creature.Predator == true && creature.Sense > previousStealth && creature.MeatEaten == 0 && creature.Zone == previousZone)
			{
				if (previousPoison == true && rnd.Next(previousPoisonStr + creature.PoisonRes) >= previousPoisonStr)
				{
					i += 1;
					continue;
				}
				foreach (var preyOrPredator in ActiveCreatures)
				{

					if (preyOrPredator.Predator && preyOrPredator.Zone == zone)
						predators++;

					if (previousCreature == preyOrPredator.SecondaryId)
					{
						creature.MeatEaten++;
						DyingCreatures.Add(preyOrPredator);
						Creatures.Add(preyOrPredator);
						hasEaten = true;
						if (preyOrPredator.Predator == false)
							isPrey = true;
						break;
					}
				}
			}
			else if (creature.MeatEaten == 1 && creature.Predator == true)
			{
				creature.MeatEaten++;
				break;
			}
			else if (creature.MeatEaten == 2 && creature.Predator == true)
			{
				creature.Children += 1;
				creatureIdDecider++;
				NewCreature(creature);
				if (predators < (ActiveCreatures.Count() + 1) / 3)
				{
					creature.Children += 1;
					creatureIdDecider++;
					NewCreature(creature);
				}
				creature.MeatEaten = 0;
				hasEaten = true;
				break;
			}
			foreach (var food in Foods1)
			{
				if (creature.Predator == true)
					break;
				if (creature.FoodType == food.FoodType && creature.Zone == food.Zone)
				{
					if (creature.Forage >= food.Id)
					{
						if (food.Camouflage == true)
							if (food.Hidden >= creature.Sense)
								creature.FoodEaten--;

						if (food.Poison == true && rnd.Next(food.PoisonStr + creature.PoisonRes) >= food.PoisonStr)
						{
							i += 2;
							continue;
						}

						EatenFood.Add(food);
						hasEaten = true;
						creature.FoodEaten++;
						if (creature.FoodEaten == creature.Hunger)
						{
							creature.FoodEaten = 0;
							creature.Children++;
							creatureIdDecider++;
							NewCreature(creature);
						}
						break;
					}

				}
			}
			foreach (var food in EatenFood)
			{
				Foods1.Remove(food);
			}

		}
		if (hasEaten == false)
		{
			DyingCreatures.Add(creature);
			Creatures.Add(creature);
		}

		creature.Age++;
		previousForage = creature.Forage;
		previousType = creature.Predator;
		previousStealth = creature.Stealth;
		previousCreature = creature.SecondaryId;
		previousPower = creature.Power;
		previousSpeed = creature.Speed;
		previousZone = creature.Zone;
		previousPoison = creature.Poison;
		previousPoisonStr = creature.PoisonStr;
	}
	public void NewCreature(Creature oldCreature)
	{
		maxStats = (maxStat / 2) * numberOfStats; 

		int evolve = rnd.Next(evolveRate + 1);
		if (evolve > 10)
		{
			// Vanlig creature
			Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
			AddToList(creature);

		}
		else if (evolve == 1)
		{
			int wichtype = rnd.Next(6);
			if (wichtype == 1)
			{
				int foodtype = rnd.Next(6);
				if (oldCreature.Predator == true)
				{

					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, 0, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, foodtype, creatureIdDecider, false);
					AddToList(creature);
				}
				else
				{
					
					if (foodtype == 1)
					{
						// FoodType 1
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(M1M)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, foodtype, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
					else if (foodtype == 2)
					{
						// FoodType 2
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(M2M)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, foodtype, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
					else if (foodtype == 3)
					{
						// FoodType 3
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(M3M)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, foodtype, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
					else if (foodtype == 4)
					{
						// FoodType 4
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(M4M)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, foodtype, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
					else if (foodtype == 5)
					{
						// FoodType 5
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(M5M)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, foodtype, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
				}

			}

			else if (wichtype == 2 && NoPredators == false)
			{
				// Predator

				if (oldCreature.Predator == false)
				{
					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(P)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, 0, 0, 3, 0, 0, oldCreature.Power, true, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.FoodType, creatureIdDecider, false);
					AddToList(creature);
				}
				else
				{
					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, 3, 0, 0, oldCreature.Power, true, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
					AddToList(creature);
				}
				
			}
			else if (wichtype == 3)
			{
				//Herbivore
				if (oldCreature.Predator == true)
				{
					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(H)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.AncestorFoodType, 0, 2, 0, 0, oldCreature.Power, false, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
					AddToList(creature);
				}
				else
				{
					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, 2, 0, 0, oldCreature.Power, false, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
					AddToList(creature);
				}
				
			}
			else if(wichtype == 4)
			{
				if (rnd.Next(3) == 1)
				{
					// Zone 1
					if (oldCreature.Zone == 1)
					{
						standardFoodZone1 = oldCreature.FoodType;
					}

					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(Z1Z)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, standardFoodZone1, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, 1, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
					AddToList(creature);

					standardFoodZone1 = 1;
				}
				else 
				{
					// Zone 2
					if (oldCreature.Zone == 2)
					{
						standardFoodZone2 = oldCreature.FoodType;
					}

					Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(Z2Z)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, standardFoodZone2, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, 2, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
					AddToList(creature);

					standardFoodZone2 = 3;

				}
			}
			else if (wichtype == 5)
			{
				if (rnd.Next(3) == 1)
				{
					bool created = false;
					foreach (var food in Foods1)
					{
						if (food.Poison == true && oldCreature.FoodType == food.FoodType)
						{
							Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(POS)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, standardFoodZone2, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, 2, true, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
							AddToList(creature);
							created = true;
							break;
						}
					}
					if (created == false)
					{
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, 0, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
				}
				else
				{
					if (oldCreature.Poison == true)
					{
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "(pos)", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, standardFoodZone2, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, 2, false, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
					else
					{
						Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, standardFoodZone2, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, 2, false, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
						AddToList(creature);
					}
				}
			}
		}
		else
		{
			int change = numberOfStats + 1;

			if (oldCreature.Poison == true)
				change += 1;

			int wichStat = rnd.Next(change);
			if (wichStat == 1)
			{
				//Ändrad Stealth
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth + NewStat(oldCreature.Stealth, false), 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				if (creature.Stealth == oldCreature.Stealth + 2 || creature.Stealth == oldCreature.Stealth - 2)
					creature.Id = creature.Id + "(Bingo)";
				AddToList(creature);
			}
			else if (wichStat == 2)
			{
				// Ändrad Energy
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy + NewStat(oldCreature.Energy, false), oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				if (creature.Energy == oldCreature.Energy + 2 || creature.Energy == oldCreature.Energy - 2)
					creature.Id = creature.Id + "(Bingo)";
				AddToList(creature);
			}
			else if (wichStat == 3)
			{
				// Ändrad Sense
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense + NewStat(oldCreature.Sense, false), 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				if (creature.Sense == oldCreature.Sense + 2 || creature.Sense == oldCreature.Sense - 2)
					creature.Id = creature.Id + "(Bingo)";
				AddToList(creature);
			}
			else if (wichStat == 4)
			{
				//Ändrad Power
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power + NewStat(oldCreature.Power, false), oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				if (creature.Power == oldCreature.Power + 2 || creature.Power == oldCreature.Power - 2)
					creature.Id = creature.Id + "(Bingo)";
				AddToList(creature);
			}
			else if (wichStat == 5)
			{
				// Ändrad Speed
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed + NewStat(oldCreature.Speed, false), oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				if (creature.Speed == oldCreature.Speed + 2 || creature.Speed == oldCreature.Speed - 2)
					creature.Id = creature.Id + "(Bingo)";
				AddToList(creature);
			}
			else if (wichStat == 6)
			{
				// Ändrad PoisoRes
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr, oldCreature.PoisonRes + NewStat(oldCreature.PoisonRes, false), false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				if (creature.PoisonRes == oldCreature.PoisonRes + 2 || creature.PoisonRes == oldCreature.PoisonRes - 2)
					creature.Id = creature.Id + "(Bingo)";
				AddToList(creature);
			}
			else if (wichStat == change)
			{
				// Ändrad PoisonStr
				Creature creature = new Creature(oldCreature.Id + "," + oldCreature.Children + "", oldCreature.Speed, oldCreature.Energy, oldCreature.Sense, 0, oldCreature.FoodType, 0, oldCreature.Hunger, 0, 0, oldCreature.Power, oldCreature.Predator, oldCreature.Stealth, 0, oldCreature.Zone, oldCreature.Poison, oldCreature.PoisonStr + NewStat(oldCreature.PoisonStr, oldCreature.Poison), oldCreature.PoisonRes, false, oldCreature.AncestorFoodType, creatureIdDecider, false);
				AddToList(creature);
			}

		}

		idDecider++;

		int NewStat(int oldStat, bool typeStat)
		{
			int totalStats = oldCreature.Speed + oldCreature.Energy + oldCreature.Sense + oldCreature.Power + oldCreature.Stealth + oldCreature.PoisonRes;
			int statchange = rnd.Next(3);

			if (oldStat < 1)
				return +1;
			else if (statchange == 1 && oldStat == 1)
				return 0;

			if (cap == false)
			{
				if (statchange == 1)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else if (typeStat == true)
			{
				if (statchange == 1 && oldStat > 2)
					return -1;
				else if (statchange == 2 && oldStat < maxStat / 2)
					return 1;
				else if (statchange == 2 && oldStat >= maxStat / 2)
					return -1;
				else
					return 0;
				
			}
			else
			{
				if (statchange == 2 && (oldStat >= maxStat || (totalStats >= maxStats)))
				{
					return -1;
				}
				else if (statchange == 1 && oldStat < 2)
				{
					return 0;
				}
				else if (oldStat < 18 && oldStat > 3 && totalStats < maxStats - 2 && rnd.Next(101) == 1)
				{

					if (statchange == 1)
					{
						return -2;
					}
					else
					{
						return 2;
					}
				}
				else if (statchange == 2 && oldStat < maxStat)
				{
					return 1;
				}
				else if (statchange == 1)
				{
					return -1;
				}
				else 
				{ 
					return 0; 
				}
			}
			
		}
	}

	public void Cleaning()
	{
		foreach (var creature in DyingCreatures)
		{
			ActiveCreatures.Remove(creature);
		}
        foreach (var creature in Waitingcreatures)
        {
            ActiveCreatures.Add(creature);
        }
        foreach (var food in EatenFood)
        {
            Foods1.Add(food);
        }
		DyingCreatures.Clear();
		Waitingcreatures.Clear();
		EatenFood.Clear();
	}

	public void ChangeInEnviroment()
	{
		Foods1 = Foods1.OrderBy(Food => Food.Id).ToList();
		for (var i = 0; i < subtract; i++)
		{
			foreach(var food in Foods1) 
			{ 
				if (food.FoodType == 1)
				{
					RemovedFood.Add(food);
					break;
				}
			}
			foreach(var food in RemovedFood)
			{
				Foods1.Remove(food);
			}
		}
	}

	public void AddToList(Creature creature)
	{
		Waitingcreatures.Add(creature);
	}

	public void ChangeId()
	{
        foreach (var creature in ActiveCreatures)
        {
			if (creature.ChangedId == false)
			{
				creature.Id = creature.Id + "[O]";
				creature.ChangedId = true;
			}
        }
    }
}
