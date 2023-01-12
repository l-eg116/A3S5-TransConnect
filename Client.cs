namespace A3S5_TransConnect
{
	internal class Client : Person
	{
		public Client() : this(default, default, default, default, default, default, default)
		{ }
		public Client(string firstName = "Unknown", string lastName = "UNKNOWN",
			int socialSecurityNumber = 0, DateTime? birthday = null, string address = "",
			string email = "", string phoneNumber = "")
			: base(firstName, lastName, socialSecurityNumber, birthday, address, email, phoneNumber)
		{ }

		public override string ToString()
		{
			return base.ToString() + ", Client";
		}
	}
}
