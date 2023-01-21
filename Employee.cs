namespace A3S5_TransConnect
{
	class Employee : Person, ITicketLinkable
	{
		public DateTime JoinDate { get; set; }
		public string JobTitle { get; set; }
		public uint Salary { get; set; }
		public List<Employee>? Subordinates { get; set; }
		private HashSet<Ticket> _linkedTickets;
		public HashSet<Ticket> LinkedTickets
		{
			get => this._linkedTickets;
			set
			{
				this._linkedTickets = new HashSet<Ticket>();
				value.ToList().ForEach(tkt => tkt.Driver = this);
			}
		}

		public Employee() : this(default, default, default, default, default, default, default, default, default, default, default, default)
		{ }
		public Employee(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "",
			DateTime? joinDate = null, string jobTitle = "", uint salary = 0, List<Employee>? subordinates = null,
			HashSet<Ticket>? linkedTickets = null)
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{
			this.JoinDate = joinDate ?? new DateTime();
			this.JobTitle = jobTitle;
			this.Salary = salary;
			this.Subordinates = subordinates;
			this.LinkedTickets = linkedTickets ?? new HashSet<Ticket>();
		}

		public override string ToString()
		{
			return base.ToString() + $", JoinDate: {this.JoinDate}, JobTitle: {this.JobTitle}, " +
				$"Salary: {this.Salary}, {this.Subordinates?.Count ?? 0} subordinate(s)";
		}
		public override string PrettyString()
			=> base.PrettyString() + $" - {this.JobTitle}";

		public bool HasSubordinates()
			=> this.Subordinates?.Count > 0;

		public void AddLinkedTicket(Ticket tkt)
			=> tkt.Driver = this;
		public void RemoveLinkedTicket(Ticket tkt)
			=> tkt.Driver = null;

		public override List<PropertyCapsule> PropertyCapsules
		{
			get => base.PropertyCapsules.Concat(new List<PropertyCapsule>
			{
				new PropertyCapsule(),  // For a nice empty line :)
				new PropertyCapsule("Join Date : ", () => this.JoinDate + "",
					() => this.JoinDate = new DateTime(), l => this.JoinDate = Display.CleanRead<DateTime>("JoinDate > ", l)),
				new PropertyCapsule("Job Title : ", () => this.JobTitle,
					() => this.JobTitle = "", l => this.JobTitle = Display.CleanRead<string>("JobTitle > ", l)),
				new PropertyCapsule("Salary : ", () => $"{this.Salary} €/mth",
					() => this.Salary = 0, l => this.Salary = Display.CleanRead<uint>("Salary > ", l)),
				new PropertyCapsule("Suborninates : ", () => this.Subordinates?.Count.ToString() ?? "0",
					null, l => Display.DisplayScrollableText(this.Subordinates?.ConvertAll(sub => sub.ToString()) ?? new List<string>(),
					($" Viewing subordinates of {this.FirstName} {this.LastName}", "", ""))),
			}).ToList();
		}
	}
}
