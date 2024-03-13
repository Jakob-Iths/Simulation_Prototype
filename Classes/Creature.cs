using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation_Prototype.Classes;

public class Creature
{
	public string Id { get; set; }
	public int Speed { get; set; }
	public int Energy { get; set; }
	public int Sense { get; set; }
	public int Children { get; set; }
	public int FoodType { get; set; }
	public int Age { get; set; }
	public int Hunger { get; set; }
	public int FoodEaten { get; set; }
	public int MeatEaten { get; set; }
	public int Forage { get; set; }
	public int Power { get; set; }
	public bool Predator { get; set; }
	public int Stealth { get; set; }
	public int Zone { get; set; }
	public bool Poison { get; set; }
	public int PoisonStr { get; set; }
	public int PoisonRes { get; set; }
	public bool Dying { get; set; }
	public int AncestorFoodType { get; set; }
	public int SecondaryId { get; set; }
	public bool ChangedId { get; set; }

	public Creature(string id, int speed, int energy, int sense, int children, int foodType, int age, int hunger, int foodEaten, int forage, int power, bool predator, int stealth, int meatEaten, int zone, bool poison, int poisonStr, int poisonRes, bool dying, int ancestorFoodType, int secondaryId, bool changedId)
	{
		Id = id;
		Speed = speed;
		Energy = energy;
		Sense = sense;
		Children = children;
		FoodType = foodType;
		Age = age;
		Hunger = hunger;
		FoodEaten = foodEaten;
		Forage = forage;
		Power = power;
		Predator = predator;
		Stealth = stealth;
		MeatEaten = meatEaten;
		Zone = zone;
		Poison = poison;
		PoisonStr = poisonStr;
		PoisonRes = poisonRes;
		Dying = dying;
		AncestorFoodType = ancestorFoodType;
		SecondaryId = secondaryId;
		this.ChangedId = changedId;
	}
}
