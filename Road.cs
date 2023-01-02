namespace A3S5_TransConnect
{
	class Road
	{
		public City Black { get; init; }
		public City White { get; init; }
		public uint Distance { get; set; }

		public Road(City black, City white, uint distance = 0)
		{
			if (black == white) throw new ArgumentException("Black and White cannot be the same City.");
			this.Black = black;
			this.White = white;
			this.Distance = distance;
		}

		public override string ToString()
			=> $"Road | {this.Black} <=> {this.White} ({this.Distance} km)";

		public override int GetHashCode()
			=> this.Black.GetHashCode() ^ this.White.GetHashCode();
		public override bool Equals(object? obj)
			=> obj is Road && this.GetHashCode() == obj.GetHashCode();
		public static bool operator ==(Road left, Road? right)
			=> left.Equals(right);
		public static bool operator !=(Road left, Road? right)
			=> !left.Equals(right);

		public bool Links(City city, City? other = null)
			=> other is null ?
				this.Black == city || this.White == city :
				this == new Road(city, other);
		public City Other(City city)
			=> city == this.Black ? this.White
			: city == this.White ? this.Black
			: throw new ArgumentException("City provided is not connected to the Road");
	}
}