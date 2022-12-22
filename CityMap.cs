namespace A3S5_TransConnect
{
	class CityMap
	{
		public HashSet<City> Cities { get; private set; }
		public HashSet<Road> Roads { get; private set; }

		public CityMap(HashSet<City>? cities = null, HashSet<Road>? roads = null)
		{
			this.Cities = cities is null ? new HashSet<City>() : cities;
			this.Roads = roads is null ? new HashSet<Road>() : roads;
			foreach(Road road in this.Roads)
				if(!this.Cities.Contains(road.Black) || !this.Cities.Contains(road.White))
					throw new ArgumentException("Roads passed should all connect cities in the passed cities.");
		}

		public override string ToString()
			=> $"CityMap | {this.Cities.Count} cities, {this.Roads.Count} roads";
	}
}