namespace A3S5_TransConnect
{
	class Vehicle  // ? Make abstract but this causes issues for the default value in Ticket constructor
	{
		public string Model { get; set; }
		public string Brand { get; set; }
		public string NumberPlate { get; init; }
		public DateTime LastRevision { get; set; }
		public uint KilometerCount { get; set; }
		public string FuelType { get; set; }
		private HashSet<Ticket> _linkedTickets;
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
			this.LastRevision = lastRevision is null ? new DateTime() : (DateTime)lastRevision;
			this.KilometerCount = kilometerCount;
			this.FuelType = fuelType;
			this.LinkedTickets = linkedTickets == null ? new HashSet<Ticket>() : linkedTickets;
		}

		public override string ToString()
			=> $"{this.GetType().Name} {this.Model} ({this.Brand}) #{this.NumberPlate} | LastRevision: {this.LastRevision}, " +
			$"KilometerCount: {this.KilometerCount}km, FuelType: {this.FuelType}";

		public void AddLinkedTicket(Ticket tkt)
			=> tkt.Vehicle = this;
		public void RemoveLinkedTicket(Ticket tkt)
			=> tkt.Vehicle = null;
	}
}