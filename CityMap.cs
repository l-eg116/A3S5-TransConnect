using Microsoft.VisualBasic.FileIO;

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
			foreach (Road road in this.Roads)
				if (!this.Cities.Contains(road.Black) || !this.Cities.Contains(road.White))
					throw new ArgumentException("Roads passed should all connect cities in the passed cities.");
		}
		public CityMap(HashSet<Road>? roads = null)
		{
			this.Cities = new HashSet<City>();
			this.Roads = roads is null ? new HashSet<Road>() : roads;
			foreach (Road road in this.Roads)
			{
				this.Cities.Add(road.Black);
				this.Cities.Add(road.White);
			}
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
				if (csvLine != null && csvLine.Length >= 3)
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
					if (mapState[r.Other(next)].Item1 > mapState[next].Item1 + r.Distance)
						mapState[r.Other(next)] = (mapState[next].Item1 + r.Distance, next, mapState[r.Other(next)].Item3);
			}

			return mapState;
		}
		public uint DistanceBetween(City from, City to)
			=> this.Dijkstra(from)[to].Item1;
		public List<Road>? PathFromTo(City from, City to)
		{
			List<Road> path = new List<Road>();
			Dictionary<City, (uint, City?, bool)> map = this.Dijkstra(from);
			for (City? current = to; current != from; current = map[current].Item2)
				if (current is null || map[current].Item2 is null) return null;
				else path.Add(this.Roads.Where(r => r.Links(current, map[current].Item2)).Single());
			path.Reverse();
			return path;
		}
	}
}