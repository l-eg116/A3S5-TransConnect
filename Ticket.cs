namespace A3S5_TransConnect
{
	class Ticket : IDisplayEditable<Ticket>
	{
		private static uint nextTicketNumber = 0;
		public string Name { get; set; }
		private Client? _client;
		public Client? Client
		{
			get => this._client;
			set
			{
				this._client?.LinkedTickets.Remove(this);
				value?.LinkedTickets.Add(this);
				this._client = value;
			}
		}
		public City? Origin { get; set; }
		public City? Destination { get; set; }
		private Employee? _driver;
		public Employee? Driver
		{
			get => this._driver;
			set
			{
				this._driver?.LinkedTickets.Remove(this);
				value?.LinkedTickets.Add(this);
				this._driver = value;
			}
		}
		private Vehicle? _vehicle;
		public Vehicle? Vehicle
		{
			get => this._vehicle;
			set
			{
				this._vehicle?.LinkedTickets.Remove(this);
				value?.LinkedTickets.Add(this);
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

		public Ticket() : this(default, default, default, default, default, default, default, default, default)
		{ }
		public Ticket(string name = "", Client? client = null, City? origin = null, City? destination = null,
			Employee? driver = null, Vehicle? vehicle = null, DateTime? date = null, double cost = 0, bool payed = false)
		{
			this.Name = name;
			this.Client = client;
			this.Origin = origin;
			this.Destination = destination;
			this.Driver = driver;
			this.Vehicle = vehicle;
			this.Date = date ?? new DateTime();
			this.Cost = cost;
			this.Payed = payed;
			this.TicketNumber = Ticket.nextTicketNumber;
		}

		public override string ToString()
		{
			return $"Ticket #{this.TicketNumber} - {this.Name} | {this.Origin} → {this.Destination}, Client: #{this.Client?.SocialSecurityNumber}, " +
				$"Driver: #{this.Driver?.SocialSecurityNumber}, Vehicle: #{this.Vehicle?.NumberPlate}, Date: {this.Date}, " +
				$"Cost: {this.Cost}€, Payed: {this.Payed}";
		}
		public string PrettyString()
			=> $"{this.Name} #{this.TicketNumber} on {this.Date} | {this.Origin} → {this.Destination}";
		public override int GetHashCode()
			=> (this.Name, this.Origin, this.Destination, this.Date, this.Cost).GetHashCode();

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

		public List<PropertyCapsule> PropertyCapsules()
			=> new List<PropertyCapsule>()
			{
				new PropertyCapsule("Ticket ", () => $"n°{this.TicketNumber} - {this.Name}", () => this.Name = "",
					l => this.Name = Display.CleanRead<string>("Ticket name > ", l) ),
				new PropertyCapsule("Client : ", () => this.Client?.PrettyString() + "", () => this.Client = null,
					_ => this.Client = Display.DisplayInstanceSelector(Program.clients, (" Select a client", "", "")) ?? this.Client),
				new PropertyCapsule("Origin : ", () => this.Origin + "", () => this.Origin = null,
					_ => this.Origin = Display.DisplayInstanceSelector(Program.map, (" Select an origin", "", "")) ?? this.Origin),
				new PropertyCapsule("Destination : ", () => this.Destination + "", () => this.Destination = null,
					_ => this.Destination = Display.DisplayInstanceSelector(Program.map, (" Select a destination", "", "")) ?? this.Destination),
				new PropertyCapsule("Driver : ", () => this.Driver?.PrettyString() + "", () => this.Driver = null,
					_ => this.Driver = Display.DisplayInstanceSelector(Program.company, (" Select a driver", "", "")) ?? this.Driver),
				new PropertyCapsule("Vehicle : ", () => this.Vehicle?.PrettyString() + "", () => this.Vehicle = null,
					_ => this.Vehicle = Display.DisplayInstanceSelector(Program.fleet, (" Select a vehicle", "", "")) ?? this.Vehicle),
				new PropertyCapsule("Date : ", () => this.Date + "", () => this.Date = new DateTime(),
					l => this.Date = Display.CleanRead<DateTime>("Date > ", l)),
				new PropertyCapsule("Cost : ", () => $"{this.Cost:.2f} €", () => this.Cost = 0,
					l => this.Cost = Display.CleanRead<double>("Cost > ", l)),
				new PropertyCapsule("Payed : ", () => this.Payed ? "Yes" : "No", null,
					l => this.Payed ^= this.Payed),
			};
	}
	interface ITicketLinkable
	{
		public HashSet<Ticket> LinkedTickets { get; set; }
		public void AddLinkedTicket(Ticket tkt);
		public void RemoveLinkedTicket(Ticket tkt);
	}
}