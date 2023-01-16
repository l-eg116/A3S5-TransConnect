namespace A3S5_TransConnect
{
	class Ticket
	{
		private static uint nextTicketNumber = 0;
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
		private readonly uint _ticketNumber;
		public uint TicketNumber
		{
			get => this._ticketNumber;
			init
			{
				Ticket.nextTicketNumber = Math.Max(Ticket.nextTicketNumber, value + 1);
				this._ticketNumber = value;
			}
		}

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
			this.TicketNumber = Ticket.nextTicketNumber;
		}

		public override string ToString()
		{
			string strClient = this.Client is null ? "[null]" : $"#{this.Client.SocialSecurityNumber}";
			string strDriver = this.Driver is null ? "[null]" : $"#{this.Driver.SocialSecurityNumber}";
			string strVehicle = this.Vehicle is null ? "[null]" : $"#{this.Vehicle.NumberPlate}";
			return $"Ticket #{this.TicketNumber} | {this.Origin} -> {this.Destination}, Client: {strClient}, " +
				$"Driver: {strDriver}, Vehicle: {strVehicle}, Date: {this.Date}, " +
				$"Cost: {this.Cost}€, Payed: {this.Payed}";
		}
		public override int GetHashCode()
			=> (this.Origin, this.Destination, this.Date, this.Cost).GetHashCode();

		public bool IsPast()
			=> this.Date.CompareTo(DateTime.Now) < 0;

		static public IEnumerable<Ticket> PastTickets(ITicketLinkable thing)
			=> thing.LinkedTickets.Where(ticket => ticket.IsPast());
		static public int PastCount(ITicketLinkable thing)
			=> PastTickets(thing).Count();
		static public double TotalCost(ITicketLinkable thing)
			=> thing.LinkedTickets.Sum(ticket => ticket.Cost);
		static public double MeanCost(ITicketLinkable thing)
			=> thing.LinkedTickets.Average(ticket => ticket.Cost);
		static public double PastTotalCost(ITicketLinkable thing)
			=> PastTickets(thing).Sum(ticket => ticket.Cost);
		static public double PastMeanCost(ITicketLinkable thing)
			=> PastTickets(thing).Average(ticket => ticket.Cost);
	}
	interface ITicketLinkable
	{
		public HashSet<Ticket> LinkedTickets { get; set; }
		public void AddLinkedTicket(Ticket tkt);
		public void RemoveLinkedTicket(Ticket tkt);
	}
}