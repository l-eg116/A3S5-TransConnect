using Microsoft.VisualBasic.FileIO;

namespace A3S5_TransConnect
{
	class CityMap : IDisplayEditable<CityMap>, IDisplaySelector<City>
	{
		public HashSet<City> Cities { get; private set; }
		public HashSet<Road> Roads { get; private set; }

		public CityMap() : this(default, default)
		{ }
		public CityMap(HashSet<City>? cities = null, HashSet<Road>? roads = null)
		{
			this.Cities = cities ?? new HashSet<City>();
			this.Roads = roads ?? new HashSet<Road>();
			foreach (Road road in this.Roads)
				if (!this.Cities.Contains(road.Black) || !this.Cities.Contains(road.White))
					throw new ArgumentException("Roads passed should all connect cities in the passed cities.");
		}
		public CityMap(HashSet<Road>? roads = null)
		{
			this.Cities = new HashSet<City>();
			this.Roads = roads ?? new HashSet<Road>();
			foreach (Road road in this.Roads)
			{
				this.Cities.Add(road.Black);
				this.Cities.Add(road.White);
			}
		}
		public void Add(Road road)
		{
			this.Cities.Add(road.Black);
			this.Cities.Add(road.White);
			this.Roads.Add(road);
		}

		public static CityMap? LoadCSV(string path, bool ignoreErrors = false)
		{
			HashSet<Road> roads = new HashSet<Road>();
			List<string[]?> csvTable = new List<string[]?>();
			try
			{
				using (TextFieldParser csvParser = new TextFieldParser(path))
				{
					csvParser.CommentTokens = new string[] { "#" };
					csvParser.SetDelimiters(new string[] { ",", ";" });
					csvParser.HasFieldsEnclosedInQuotes = true;
					while (!csvParser.EndOfData) csvTable.Add(csvParser.ReadFields());
				}
			}
			catch (Exception ex)
			{
				if (ignoreErrors) return null;
				else throw ex;
			}
			foreach (string[]? csvLine in csvTable)
				if (csvLine?.Length >= 3)
				{
					uint d = uint.TryParse(csvLine[2], out uint x) ? x : 0;
					roads.Add(new Road(new City(csvLine[0]), new City(csvLine[1]), d));
				}
			return new CityMap(roads);
		}

		public override string ToString()
			=> $"CityMap | {this.Cities.Count} cities, {this.Roads.Count} roads";

		public List<Road> RoadsTo(City city)
		{
			List<Road> roads = new List<Road>();
			foreach (Road r in this.Roads)
				if (r.Links(city))
					roads.Add(r);
			return roads;
		}

		public Dictionary<City, (uint, City?, bool)> Dijkstra(City root)
		{
			// (city, distance to origin, parent, toTick?)
			Dictionary<City, (uint, City?, bool)> mapState = new Dictionary<City, (uint, City?, bool)>();
			foreach (City c in this.Cities)
				mapState.Add(c, (uint.MaxValue, null, true));
			mapState[root] = (0, null, true);

			while (mapState.Any(x => x.Value.Item3))
			{
				City next = mapState.Where(kvp => kvp.Value.Item3).First().Key;
				foreach (KeyValuePair<City, (uint, City?, bool)> kvp in mapState.Where(kvp => kvp.Value.Item3))
					if (kvp.Value.Item1 < mapState[next].Item1)
						next = kvp.Key;
				mapState[next] = (mapState[next].Item1, mapState[next].Item2, false);

				foreach (Road r in this.RoadsTo(next))
					if (mapState[r.Other(next)].Item1 > mapState[next].Item1 + r.DistanceKm)
						mapState[r.Other(next)] = (mapState[next].Item1 + r.DistanceKm, next, mapState[r.Other(next)].Item3);
			}

			return mapState;
		}
		public uint DistanceBetween(City from, City to)
			=> this.Dijkstra(from).GetValueOrDefault(to, (uint.MaxValue, null, true)).Item1;
		public List<Road>? PathFromTo(City from, City to)
		{
			Dictionary<City, (uint, City?, bool)> map = this.Dijkstra(from);
			if (!(map.ContainsKey(from) && map.ContainsKey(to)) || map.GetValueOrDefault(to, (uint.MaxValue, null, true)).Item1 == uint.MaxValue) return null;

			List<Road> path = new List<Road>();
			for (City? current = to; from != current && path.Count < 128; current = map[current].Item2)
				if (current is null || map[current].Item2 is null) return null;
				else path.Add(new Road(map[current].Item2 ?? new City(), current, this.Roads.Where(r => r.Links(current, map[current].Item2)).Single().DistanceKm));
			path.Reverse();
			return path;
		}

		public List<PropertyCapsule> PropertyCapsules()
		{
			List<PropertyCapsule> propertyCapsules = new List<PropertyCapsule>()
				{ new PropertyCapsule("> Map has ", () => $"{this.Cities.Count} city(ies) and {this.Roads.Count} road(s) <") };
			propertyCapsules.Add(new PropertyCapsule("+ Add new Road", () => "", null,
				l => this.Add(new Road(new City(Display.CleanRead<string>("First city > ", l)),
					new City(Display.CleanRead<string>("Second city > ", l)),
					Display.CleanRead<uint>("Distance > ", l)))));
			foreach (Road road in this.Roads)
				propertyCapsules.Add(new PropertyCapsule($"{road.Black} ←→ {road.White} > ",
					() => $"{road.DistanceKm} km", () => this.Roads.Remove(road),
					l => road.DistanceKm = Display.CleanRead<uint>("New road length > ", l)));
			return propertyCapsules;
		}
		public List<(string, City)> InstanceSelector()
		{
			List<(string, City)> selector = new List<(string, City)>();
			this.Cities.ToList().ForEach(c => selector.Add((c.Name, c)));
			return selector;
		}
	}
}