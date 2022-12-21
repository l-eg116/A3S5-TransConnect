namespace A3S5_TransConnect
{
	class Car : Vehicle
	{
		public uint Capacity { get; set; }

		public Car(string model = "Unknown", string brand = "Unknown", string numberPlate = "00AAA000",
			DateTime? lastRevision = null, uint kilometerCount = 0, string fuelType = "Unknown", uint capacity = 4)
			: base(model, brand, numberPlate, lastRevision, kilometerCount, fuelType)
		{
			this.Capacity = capacity;
		}

		public override string ToString()
			=> base.ToString() + $", Capacity: {this.Capacity}";
	}
}