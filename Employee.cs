namespace A3S5_TransConnect
{
	internal class Employee : Person
	{
		public DateTime JoinDate { get; set; }
		public string JobTitle { get; set; }
		public uint Salary { get; set; }
		public List<Employee>? Subordinates { get; set; }

		public Employee() : this(default, default, default, default, default, default, default, default, default, default, default)
		{ }
		public Employee(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "",
			DateTime? joinDate = null, string jobTitle = "", uint salary = 0, List<Employee>? subordinates = null)
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{
			this.JoinDate = joinDate is null ? new DateTime() : (DateTime)joinDate;
			this.JobTitle = jobTitle;
			this.Salary = salary;
			this.Subordinates = subordinates;
		}

		public override string ToString()
		{
			return base.ToString() + $", JoinDate: {this.JoinDate}, JobTitle: {this.JobTitle}, " +
				$"Salary: {this.Salary}, " + (this.HasSubordinates() ? this.Subordinates.Count : 0) + " subordinate(s)";
		}

		public bool HasSubordinates()
			=> !(this.Subordinates is null) && this.Subordinates.Count != 0;
	}
}
