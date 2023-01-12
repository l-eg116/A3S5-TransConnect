namespace A3S5_TransConnect
{
	class Ticket
	{
		private Client? _client;
		public Client? Client
		{
			get => this._client;
			set
			{
				if (!(this._client is null)) this._client.LinkedTickets.Remove(this);
				if (!(value is null)) value.LinkedTickets.Add(this);
				this._client = value;
			}
		}
		public string Origin { get; set; }
		public string Destination { get; set; }
		private Employee? _driver;
		public Employee? Driver
		{
			get => this._driver;
			set
			{
				if (!(this._driver is null)) this._driver.LinkedTickets.Remove(this);
				if (!(value is null)) value.LinkedTickets.Add(this);
				this._driver = value;
			}
		}
		private Vehicle? _vehicle;
		public Vehicle? Vehicle
		{
			get => this._vehicle;
			set
			{
				if (!(this._vehicle is null)) this._vehicle.LinkedTickets.Remove(this);
				if (!(value is null)) value.LinkedTickets.Add(this);
				this._vehicle = value;
			}
		}
		public DateTime Date { get; set; }
		private double _cost;
		public double Cost { get => this._cost; set => this._cost = Math.Max(0, value); }
		public bool Payed { get; set; }

		public Ticket() : this(default, default, default, default, default, default, default, default)
		{ }
		public Ticket(Client? client = null, string origin = "Unknown", string destination = "Unknown",
			Employee? driver = null, Vehicle? vehicle = null, DateTime? date = null, double cost = 0, bool payed = false)
		{
			this.Client = client;
			this.Origin = origin;
			this.Destination = destination;
			this.Driver = driver;
			this.Vehicle = vehicle;
			this.Date = date is null ? new DateTime() : (DateTime)date;
			this.Cost = cost;
			this.Payed = payed;
		}

		public override string ToString()  // * Make better ToString method taking into account the possiblity of null instances
			=> $"Ticket | Client: #{this.Client}, {this.Origin} -> {this.Destination}, " +
				$"Driver: #{this.Driver}, Vehicle: #{this.Vehicle}, Date: {this.Date}, " +
				$"Cost: {this.Cost}€, Payed: {this.Payed}";
		public override int GetHashCode()
			=> (this.Origin, this.Destination, this.Date, this.Cost).GetHashCode();

		public bool IsPast()
			=> this.Date.CompareTo(DateTime.Now) < 0;
	}
	interface ITicketLinkable
	{
		public HashSet<Ticket> LinkedTickets { get; set; }
		public void AddLinkedTicket(Ticket tkt);
		public void RemoveLinkedTicket(Ticket tkt);
	}
}