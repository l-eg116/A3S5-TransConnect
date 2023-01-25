namespace A3S5_TransConnect
{
	[System.Text.Json.Serialization.JsonDerivedType(typeof(Vehicle), "Vehicle")]
	[System.Text.Json.Serialization.JsonDerivedType(typeof(Car), "Car")]
	[System.Text.Json.Serialization.JsonDerivedType(typeof(Truck), "Truck")]
	[System.Text.Json.Serialization.JsonDerivedType(typeof(Van), "Van")]
	class Vehicle : ITicketLinkable, IDisplayEditable<Vehicle>  // ? Make abstract but this causes issues for the default value in Ticket constructor
	{
		public string Model { get; set; }
		public string Brand { get; set; }
		public string NumberPlate { get; set; }
		public DateTime LastRevision { get; set; }
		public uint KilometerCount { get; set; }
		public string FuelType { get; set; }
		private HashSet<Ticket> _linkedTickets;
		[System.Text.Json.Serialization.JsonIgnore]
		public HashSet<Ticket> LinkedTickets
		{
			get => this._linkedTickets;
			set
			{
				this._linkedTickets = new HashSet<Ticket>();
				value.ToList().ForEach(tkt => tkt.Vehicle = this);
			}
		}

		public Vehicle() : this(default, default, default, default, default, default, default)
		{ }
		public Vehicle(string model = "Unknown", string brand = "Unknown", string numberPlate = "00AAA000",
			DateTime? lastRevision = null, uint kilometerCount = 0, string fuelType = "Unknown", HashSet<Ticket>? linkedTickets = null)
		{
			this.Model = model;
			this.Brand = brand;
			this.NumberPlate = numberPlate;
			this.LastRevision = lastRevision ?? new DateTime();
			this.KilometerCount = kilometerCount;
			this.FuelType = fuelType;
			this.LinkedTickets = linkedTickets ?? new HashSet<Ticket>();
		}

		public override string ToString()
			=> $"[{this.GetType().Name}] {this.Model} ({this.Brand}) #{this.NumberPlate} | LastRevision: {this.LastRevision}, " +
			$"KilometerCount: {this.KilometerCount}km, FuelType: {this.FuelType}";
		public virtual string PrettyString()
			=> $"[{this.GetType().Name}] {this.Model} ({this.Brand}) #{this.NumberPlate}";
		public override int GetHashCode()
			=> this.ToString().GetHashCode();
		public override bool Equals(object? obj)
			=> obj is Vehicle && this.GetHashCode() == obj.GetHashCode();
		public static bool operator == (Vehicle v, object? obj)
			=> v.Equals(obj);
		public static bool operator != (Vehicle v, object? obj)
			=> !v.Equals(obj);

		public void AddLinkedTicket(Ticket tkt)
			=> tkt.Vehicle = this;
		public void RemoveLinkedTicket(Ticket tkt)
			=> tkt.Vehicle = null;

		public virtual List<PropertyCapsule> PropertyCapsules()
			=> new List<PropertyCapsule>
			{
				new PropertyCapsule($"Vehicule type : {this.GetType().Name}"),
				new PropertyCapsule("Model : ", () => this.Model,
					() => this.Model = "Unknown", l => this.Model = Display.CleanRead<string>("Model > ", l)),
				new PropertyCapsule("Brand : ", () => this.Brand,
					() => this.Brand = "Unknown", l => this.Brand = Display.CleanRead<string>("Brand > ", l)),
				new PropertyCapsule("Number Plate : #", () => this.NumberPlate,
					() => this.NumberPlate = "00AAA000", l => this.NumberPlate = Display.CleanRead<string>("Number Plate > #", l)),
				new PropertyCapsule("Last Revision : ", () => this.LastRevision.ToString(),
					() => this.LastRevision = new DateTime(), l => this.LastRevision = Display.CleanRead<DateTime>("Last Revision > ", l)),
				new PropertyCapsule("Kilometer Count : ", () => $"{this.KilometerCount} km",
					() => this.KilometerCount = 0, l => this.KilometerCount = Display.CleanRead<uint>("Kilometer Count > ", l)),
				new PropertyCapsule("Fuel Type : ", () => this.FuelType,
					() => this.FuelType = "Unknown", l => this.FuelType = Display.CleanRead<string>("Fuel Type > ", l)),
			};
	}
}