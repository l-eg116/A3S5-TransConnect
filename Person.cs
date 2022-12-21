namespace A3S5_TransConnect
{
	internal abstract class Person : IComparable
	{
		public string FirstName { get; set; }
		private string _lastName;
		public string LastName { get => this._lastName; set => this._lastName = value.ToUpper(); }
		public int SocialSecurityNumber { get; init; }
		public DateTime? Birthday { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public Person(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "")
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.SocialSecurityNumber = socialSecurityNumber;
			this.Birthday = birthday;
			this.Address = address;
			this.Email = email;
			this.PhoneNumber = phoneNumber;
		}

		public override string ToString()
		{
			return $"{this.FirstName} {this.LastName} #{this.SocialSecurityNumber} | " +
				$"Birthday: {this.Birthday}, Address: {this.Address}, Email: {this.Email}, PhoneNumber: {this.PhoneNumber}";
		}

		public int CompareTo(object? obj)
		{
			return obj is Person ? this.ToString().CompareTo(obj.ToString()) : 1;
		}
	}
}
