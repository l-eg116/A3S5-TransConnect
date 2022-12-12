namespace A3S5_TransConnect
{
	internal abstract class Person : IComparable
	{
		private string firstName;
		private string lastName;
		private int socialSecurityNumber;
		private DateTime birthday;
		private string address;
		private string email;
		private string phoneNumber;

		public string FirstName { get => this.firstName; set => this.firstName = value; }
		public string LastName { get => this.lastName; set => this.lastName = value.ToUpper(); }
		public int SocialSecurityNumber { get => this.socialSecurityNumber; set => this.socialSecurityNumber = value; }
		public DateTime Birthday { get => this.birthday; set => this.birthday = value; }
		public string Address { get => this.address; set => this.address = value; }
		public string Email { get => this.email; set => this.email = value; }
		public string PhoneNumber { get => this.phoneNumber; set => this.phoneNumber = value; }

		public Person(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "")
		{
			this.firstName = firstName;
			this.lastName = lastName.ToUpper();
			this.socialSecurityNumber = socialSecurityNumber;
			this.birthday = birthday is null ? new DateTime() : (DateTime)birthday;
			this.address = address;
			this.email = email;
			this.phoneNumber = phoneNumber;
		}

		public override string ToString()
		{
			return $"{this.firstName} {this.lastName} #{this.socialSecurityNumber} | " +
				$"birthday: {this.birthday}, address: {this.address}, email: {this.email}, phoneNumber: {this.phoneNumber}";
		}

		public int CompareTo(object? obj)
		{
			return obj is Person ? this.ToString().CompareTo(obj.ToString()) : 1;
		}
	}
}
