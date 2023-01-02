namespace A3S5_TransConnect
{
	class City : IComparable
	{
		public string Name { get; init; }

		public City(string name = "")
		{
			this.Name = name;
		}

		public override string ToString()
			=> $"{this.Name} City";

		public override int GetHashCode()
			=> this.Name.ToUpper().GetHashCode();
		public override bool Equals(object? obj)
			=> obj is City && this.GetHashCode() == obj.GetHashCode();
		public static bool operator ==(City? left, City? right)
			=> !(left is null) && left.Equals(right);
		public static bool operator !=(City? left, City? right)
			=> left is null || !left.Equals(right);
		public int CompareTo(object? obj)
			=> obj is City ? this.Name.CompareTo(((City)obj).Name) : 1;
	}
}