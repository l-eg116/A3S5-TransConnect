namespace A3S5_TransConnect
{
	class Truck : Vehicle
	{
		public string PayloadType { get; set; }
		public uint Capacity { get; set; }
		public string CapacityUnit { get; set; }

		public Truck() : this(default, default, default, default, default, default, default, default, default)
		{ }
		public Truck(string model = "Unknown", string brand = "Unknown", string numberPlate = "00AAA000",
			DateTime? lastRevision = null, uint kilometerCount = 0, string fuelType = "Unknown",
			string payloadType = "Generic", uint capacity = 0, string capacityUnit = "kg")
			: base(model, brand, numberPlate, lastRevision, kilometerCount, fuelType)
		{
			this.PayloadType = payloadType;
			this.Capacity = capacity;
			this.CapacityUnit = capacityUnit;
		}

		public override string ToString()
			=> base.ToString() + $", PayloadType: {this.PayloadType}, Capacity: {this.PayloadType}{this.CapacityUnit}";

		public override List<PropertyCapsule> PropertyCapsules
		{
			get => base.PropertyCapsules.Concat(new List<PropertyCapsule>
			{
				new PropertyCapsule("Payload Type : ", () => this.PayloadType,
					() => this.PayloadType = "Generic", l => this.PayloadType = Display.CleanRead<string>("Payload Type : ", l)),
				new PropertyCapsule("Capacity : ", () => $"{this.Capacity} {this.CapacityUnit}",
					() => this.Capacity = 0, l => this.Capacity = Display.CleanRead<uint>("Capacity : ", l)),
				new PropertyCapsule("   -→ Unit : ", () =>this.CapacityUnit,
					() => this.CapacityUnit = "kg", l => this.CapacityUnit = Display.CleanRead<string>("→ New Unit : ", l)),
			}).ToList();
		}
	}
}