namespace A3S5_TransConnect
{
	internal class Employee : Person
	{
		private DateTime joinDate;
		private string jobTitle;
		private uint salary;
		private List<Employee>? subordinates = new List<Employee>();

		public DateTime JoinDate { get => this.joinDate; set => this.joinDate = value; }
		public string JobTitle { get => this.jobTitle; set => this.jobTitle = value; }
		public uint Salary { get => this.salary; set => this.salary = value; }
		public List<Employee>? Subordinates { get => this.subordinates; set => this.subordinates = value; }

		public Employee(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "",
			DateTime? joinDate = null, string jobTitle = "", uint salary = 0, List<Employee>? subordinates = null)
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{
			this.joinDate = joinDate is null ? new DateTime() : (DateTime)joinDate;
			this.jobTitle = jobTitle;
			this.salary = salary;
			this.subordinates = subordinates;
		}

		public override string ToString()
		{
			return base.ToString() + $"joinDate: {this.joinDate}, jobTitle:{this.jobTitle}, " +
				$"salary: {this.salary}, " + (this.HasSubordinates() ? this.subordinates.Count : 0) + " subordinate(s)";
		}

		public bool HasSubordinates()
			=> !(subordinates is null) && subordinates.Count != 0;
	}
}
