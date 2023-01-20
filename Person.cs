namespace A3S5_TransConnect
{
	internal abstract class Person : IComparable, IDisplayEditable<Person>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int SocialSecurityNumber { get; set; }
		public DateTime? Birthday { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public Person() : this(default, default, default, default, default, default, default)
		{ }
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

		public virtual List<PropertyCapsule> PropertyCapsules
		{
			get => new List<PropertyCapsule>
			{
				new PropertyCapsule("First Name : ", () => this.FirstName,
					() => this.FirstName = "Unknown", l => this.FirstName = Display.CleanRead<string>("First Name > ", l)),
				new PropertyCapsule("Last Name : ", () => this.LastName,
					() => this.LastName = "UNKNOWN", l => this.LastName = Display.CleanRead<string>("Last Name > ", l)),
				new PropertyCapsule("Social Security Number : ", () => this.SocialSecurityNumber + "",
					() => this.SocialSecurityNumber = 0, l => this.SocialSecurityNumber = Display.CleanRead<int>("Social Security Number > ", l)),
				new PropertyCapsule("Birthday : ", () => this.Birthday?.ToString() ?? "??",
					() => this.Birthday = null, l => this.Birthday = Display.CleanRead<DateTime>("Birthday > ", l)),
				new PropertyCapsule("Address : ", () => this.Address,
					() => this.Address = "", l => this.Address = Display.CleanRead<string>("Address > ", l)),
				new PropertyCapsule("Email : ", () => this.Email,
					() => this.Email = "", l => this.Email = Display.CleanRead<string>("Email > ", l)),
				new PropertyCapsule("PhoneNumber : ", () => this.PhoneNumber,
					() => this.PhoneNumber = "", l => this.PhoneNumber = Display.CleanRead<string>("PhoneNumber > ", l)),
			};
		}
	}
}
