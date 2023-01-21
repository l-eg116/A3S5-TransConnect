namespace A3S5_TransConnect
{
	class Client : Person, ITicketLinkable
	{
		public string Company { get; set; }
		private HashSet<Ticket> _linkedTickets;
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
			string email = "", string phoneNumber = "", string company = "", HashSet<Ticket>? linkedTickets = null)
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{
			this.Company = company;
			this.LinkedTickets = linkedTickets ?? new HashSet<Ticket>();
		}

		public override string ToString()
		{
			return base.ToString() + $", Company : {this.Company}, {this.LinkedTickets.Count} LinkedTicket(s)";
		}
		public override string PrettyString()
			=> base.PrettyString() + $" - {this.Company}";

		public void AddLinkedTicket(Ticket tkt)
			=> tkt.Client = this;
		public void RemoveLinkedTicket(Ticket tkt)
			=> tkt.Client = null;

		public override List<PropertyCapsule> PropertyCapsules
			=> base.PropertyCapsules.Concat(new List<PropertyCapsule>() {
				new PropertyCapsule("Company : ", () => this.Company, () => this.Company = "",
					l => this.Company = Display.CleanRead<string>("Company : ", l))
			}).ToList();
	}
}
