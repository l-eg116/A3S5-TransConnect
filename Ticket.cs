namespace A3S5_TransConnect
{
	class Ticket
	{
		public Client Client { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }
		public Employee Driver { get; set; }
		public Vehicle Vehicle { get; set; }
		public DateTime Date { get; set; }
		private double _cost;
		public double Cost { get => this._cost; set => this._cost = Math.Max(0, value); }
		public bool Payed { get; set; }

		public Ticket() : this(default, default, default, default, default, default, default, default)
		{ }
		public Ticket(Client? client = null, string origin = "Unknown", string destination = "Unknown",
			Employee? driver = null, Vehicle? vehicle = null, DateTime? date = null, double cost = 0, bool payed = false)
		{
			this.Client = client is null ? new Client() : client;
			this.Origin = origin;
			this.Destination = destination;
			this.Driver = driver is null ? new Employee() : driver;
			this.Vehicle = vehicle is null ? new Vehicle() : vehicle;
			this.Date = date is null ? new DateTime() : (DateTime)date;
			this.Cost = cost;
			this.Payed = payed;
		}

		public override string ToString()
			=> $"Ticket | Client: #{this.Client.SocialSecurityNumber}, {this.Origin} -> {this.Destination}, " +
				$"Driver: #{this.Driver.SocialSecurityNumber}, Vehicle: #{this.Vehicle.NumberPlate}, Date: {this.Date}, " +
				$"Cost: {this.Cost}€, Payed: {this.Payed}";
	}
}