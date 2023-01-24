namespace A3S5_TransConnect
{
	class Car : Vehicle, IDisplayEditable<Car>
	{
		public uint CapacityPassengers { get; set; }

		public Car() : this(default, default, default, default, default, default, default)
		{ }
		public Car(string model = "Unknown", string brand = "Unknown", string numberPlate = "00AAA000",
			DateTime? lastRevision = null, uint kilometerCount = 0, string fuelType = "Unknown", uint capacityPassengers = 4)
			: base(model, brand, numberPlate, lastRevision, kilometerCount, fuelType)
		{
			this.CapacityPassengers = capacityPassengers;
		}

		public override string ToString()
			=> base.ToString() + $", Capacity: {this.CapacityPassengers}";

		public override List<PropertyCapsule> PropertyCapsules()
			=> base.PropertyCapsules().Concat(new List<PropertyCapsule>
			{
				new PropertyCapsule("Capacity : ", () => $"{this.CapacityPassengers} passenger(s)",
					() => this.CapacityPassengers = 0, l => this.CapacityPassengers = Display.CleanRead<uint>("Capacity > ", l)),
			}).ToList();
	}
}