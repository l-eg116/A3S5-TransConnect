namespace A3S5_TransConnect
{
	class Client : Person, ITicketLinkable
	{
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
		public Client() : this(default, default, default, default, default, default, default, default)
		{ }
		public Client(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "", HashSet<Ticket>? linkedTickets = null)
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{
			this.LinkedTickets = linkedTickets ?? new HashSet<Ticket>();
		}

		public override string ToString()
		{
			return base.ToString() + $", {this.LinkedTickets.Count} LinkedTicket(s)";
		}

		public void AddLinkedTicket(Ticket tkt)
			=> tkt.Client = this;
		public void RemoveLinkedTicket(Ticket tkt)
			=> tkt.Client = null;
	}
}
