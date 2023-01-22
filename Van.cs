namespace A3S5_TransConnect
{
	class Van : Vehicle
	{
		public string Type { get; set; }

		public Van() : this(default, default, default, default, default, default, default)
		{ }
		public Van(string model = "Unknown", string brand = "Unknown", string numberPlate = "00AAA000",
			DateTime? lastRevision = null, uint kilometerCount = 0, string fuelType = "Unknown", string type = "Utility")
			: base(model, brand, numberPlate, lastRevision, kilometerCount, fuelType)
		{
			this.Type = type;
		}

		public override string ToString()
			=> base.ToString() + $", Type: {this.Type}";

		public override List<PropertyCapsule> PropertyCapsules()
			=> base.PropertyCapsules().Concat(new List<PropertyCapsule>
			{
				new PropertyCapsule("Van Type : ", () => this.Type,
					() => this.Type = "Utility", l => this.Type = Display.CleanRead<string>("Van Type > ", l)),
			}).ToList();
	}
}