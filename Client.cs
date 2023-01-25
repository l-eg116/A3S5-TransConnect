namespace A3S5_TransConnect
{
	class Client : Person, ITicketLinkable, IDisplayEditable<Client>
	{
		public string Company { get; set; }
		public City? City { get; set; }
		private HashSet<Ticket> _linkedTickets;
		[System.Text.Json.Serialization.JsonIgnore]
		public HashSet<Ticket> LinkedTickets
		{
			get => this._linkedTickets;
			set
			{
				this._linkedTickets = new HashSet<Ticket>();
				value.ToList().ForEach(tkt => tkt.Client = this);
			}
		}
		public Client() : this(default, default, default, default, default, default, default, default, default)
		{ }
		public Client(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "", string company = "", City? city = null, HashSet<Ticket>? linkedTickets = null)
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{
			this.Company = company;
			this.City = city;
			this.LinkedTickets = linkedTickets ?? new HashSet<Ticket>();
		}

		public override string ToString()
		{
			return base.ToString() + $", Company : {this.Company}, City : {this.City}";
		}
		public override string PrettyString()
			=> base.PrettyString() + $" - {this.Company} in {this.City}";

		public void AddLinkedTicket(Ticket tkt)
			=> tkt.Client = this;
		public void RemoveLinkedTicket(Ticket tkt)
			=> tkt.Client = null;

		public override List<PropertyCapsule> PropertyCapsules()
			=> base.PropertyCapsules().Concat(new List<PropertyCapsule>() {
				new PropertyCapsule("Company : ", () => this.Company, () => this.Company = "",
					l => this.Company = Display.CleanRead<string>("Company : ", l)),
				new PropertyCapsule("City : ", () => this.City + "", () => this.City = null,
					l => this.City = Display.DisplayInstanceSelector(Program.map, ("  Select a city", "", "")) ?? this.City),
			}).ToList();
	}
}
