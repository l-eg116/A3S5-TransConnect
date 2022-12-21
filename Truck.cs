namespace A3S5_TransConnect
{
	class Truck : Vehicle
	{
		public string PayloadType { get; set; }
		public uint Capacity { get; set; }
		public string CapacityUnit { get; set; }

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
	}
}