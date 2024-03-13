using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation_Prototype.Classes;

public class Food
{
	public int Id {  get; set; }
	public int FoodType { get; set; }
	public bool Poison { get; set; }
	public int PoisonStr { get; set; }
	public bool Camouflage { get; set; }
	public int Hidden { get; set; }

	public int Zone { get; set; }
	public Food(int id, int foodType, bool poison, bool camouflage, int hidden, int zone, int poisonStr)
	{
		Id = id;
		FoodType = foodType;
		Poison = poison;
		Camouflage = camouflage;
		Hidden = hidden;
		Zone = zone;
		PoisonStr = poisonStr;
	}
}
