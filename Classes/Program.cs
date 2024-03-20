using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Simulation_Prototype.Classes;
public class Program
{
	public static void Main()
    {
		CollectionData CollectionData = new CollectionData();

		CollectionData.GenerateCreatures();

		CollectionData.GenerateFood();
		

		for (int i = 0; i < 10000; i++)
		{
			Console.WriteLine(i + "   " + CollectionData.ActiveCreatures.Count() + "   " + CollectionData.Foods1.Count);

			CollectionData.numberOfPredators1 = 0;
			foreach (var creatures in CollectionData.ActiveCreatures)
			{
				if (creatures.Predator == true)
				{
					CollectionData.isPredator = true;
					CollectionData.numberOfPredators1++;
				}
			}
			if (CollectionData.isPredator == true && CollectionData.numberOfPredators1 == 0)
			{
				CollectionData.ChangeId();
				CollectionData.isPredator = false;
			}

			CollectionData.SortList(i);

			CollectionData.Forage();

			CollectionData.Cleaning();

			//if (i % 100 == 1 && i < 4000)
				//CollectionData.ChangeInEnviroment();

			if (CollectionData.ActiveCreatures.Count == 0)
			{
				Console.WriteLine(i);
				break;
			}


			/*foreach (var creature in CollectionData.ActiveCreatures)
			{
				if (creature.Predator == false)
				{
					CollectionData.test++;
				}
				else if (creature.Predator == true) 
				{ 
					CollectionData.test2++;
				}
				if (CollectionData.test == 0)
					Console.WriteLine(i);
				else Console.WriteLine("DET FINNS!!!!" + i);
				if (CollectionData.test2 == 0)
				Console.WriteLine("INGA PREDATORS!!!!!!!!!!!!" + i);
			}
			CollectionData.test = 0;
			CollectionData.test2 = 0;
			*/

			//Console.WriteLine(i + "   " + CollectionData.ActiveCreatures.Count() + "   " + CollectionData.Foods1.Count);
			//CollectionData.Creatures.Clear();
		}


		int totalStats;
		CollectionData.numberOfPredators1 = 0;
		//CollectionData.ActiveCreatures = CollectionData.ActiveCreatures.OrderByDescending(creature => creature.Id).ToList();
		//CollectionData.ActiveCreatures = CollectionData.ActiveCreatures.OrderBy(creature => creature.FoodType).ToList();
		CollectionData.ActiveCreatures = CollectionData.ActiveCreatures.OrderBy(_ => CollectionData.rnd.Next()).ToList();
		Console.WriteLine("antal döda creatures:" + CollectionData.Creatures.Count);

		//Skriver ut alla levande creatures

		foreach (var creature in CollectionData.ActiveCreatures)
		{
			totalStats = 0;
			totalStats = creature.Speed + creature.Energy + creature.Sense + creature.Power + creature.Stealth + creature.PoisonRes;
			Console.WriteLine(creature.Id + " Speed:" + creature.Speed + " Energy:" + creature.Energy + " Sense:" + creature.Sense + " power:" + creature.Power + " P:" + creature.Predator + " Stealth:" + creature.Stealth + " PoisonRes:" + creature.PoisonRes + " PoisonStr:" + creature.PoisonStr + " Poison:" + creature.Poison + " Zone:" + creature.Zone + " Type:" + creature.FoodType + " Total:" + totalStats);
			Console.WriteLine("");
		}

		Console.WriteLine("antal creatures:" + CollectionData.ActiveCreatures.Count);


		CollectionData.ActiveCreatures.ForEach(creature =>
		{
			if (creature.FoodType == 1)
				CollectionData.type1 ++;
			else if (creature.FoodType == 2)
				CollectionData.type2 ++;
			else if(creature.FoodType == 3)
				CollectionData.type3 ++;
			else if (creature.FoodType == 4)
				CollectionData.type4 ++;
			else if (creature.FoodType == 5)
				CollectionData.type5 ++;

		});
		Console.WriteLine("antal typ 1: " + CollectionData.type1 + " antal typ2: " + CollectionData.type2 + " antal typ3: " + CollectionData.type3 + " antal typ4: " + CollectionData.type4 + " antal typ5: " + CollectionData.type5);



		foreach(var creature in CollectionData.ActiveCreatures)
		{
			CollectionData.averageLiving = CollectionData.averageLiving + creature.Children;
			CollectionData.averageAge = CollectionData.averageAge + creature.Age;

			if (creature.Predator == true)
				CollectionData.numberOfPredators1++;
		}
		foreach (var creature in CollectionData.Creatures)
		{
			CollectionData.averageDead = CollectionData.averageDead + creature.Children;
			CollectionData.averageAge = CollectionData.averageAge + creature.Age;
		}
		Console.WriteLine("antalet barn:" + ((CollectionData.averageLiving / CollectionData.ActiveCreatures.Count()) + (CollectionData.averageDead / CollectionData.Creatures.Count())) / 2 + " Medel Livslängd:" + CollectionData.averageAge / (CollectionData.ActiveCreatures.Count() + CollectionData.Creatures.Count()));
		Console.WriteLine("antalet predators:"+ CollectionData.numberOfPredators1);
		Console.WriteLine(CollectionData.Foods1.Count());
		CollectionData.Foods1 = CollectionData.Foods1.OrderByDescending(Food => Food.Id).ToList();
		CollectionData.Foods1 = CollectionData.Foods1.OrderByDescending(Food => Food.FoodType).ToList();
		foreach (var food in CollectionData.Foods1)
			Console.WriteLine(food.Id + "   " + food.FoodType);

	}
}