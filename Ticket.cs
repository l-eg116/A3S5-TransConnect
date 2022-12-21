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

		public Ticket(Client? client = null, string origin = "Unknown", string destination = "Unknown",
			Employee? driver = null, Vehicle? vehicle = null, DateTime? date = null, double cost = 0)
		{
			this.Client = client is null ? new Client() : client;
			this.Origin = origin;
			this.Destination = destination;
			this.Driver = driver is null ? new Employee() : driver;
			this.Vehicle = vehicle is null ? new Vehicle() : vehicle;
			this.Date = date is null ? new DateTime() : (DateTime)date;
			this.Cost = cost;
		}

		public override string ToString()
			=> $"Ticket | Client: #{this.Client.SocialSecurityNumber}, {this.Origin} -> {this.Destination}, " +
				$"Driver: #{this.Driver.SocialSecurityNumber}, Vehicle: #{this.Vehicle.NumberPlate}, Date: {this.Date}, " +
				$"Cost: {this.Cost}€";
	}
}