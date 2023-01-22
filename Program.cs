﻿namespace A3S5_TransConnect
{
	class Program
	{
		public static class Options
		{
			public static readonly string FleetSavePath = "data/fleet.json";
			public static readonly string ClientsSavePath = "data/clients.json";
			public static readonly string TicketsSavePath = "data/tickets.json";
			public static readonly string CompanySavePath = "data/company.json";
			public static readonly string DistancesSavePath = "data/distances.csv";
			public static readonly ConsoleColor BackgroundColor = ConsoleColor.Black;
			public static readonly ConsoleColor TextColor = ConsoleColor.White;
		}
		public static InteractiveList<Vehicle> fleet = new InteractiveList<Vehicle>()
		{
			List = new List<Vehicle>(),
			Get = v => v.PrettyString(),
			Reset = v => fleet.List.Remove(v),
			Editor = (v, l) => Display.DisplayEditor(v, (" Editing vehicle", "", "")),
			New = new PropertyCapsule(" + Add new vehicle", null, null,
				_ => fleet.List.Add(Display.DisplayConstructor<Vehicle>((" Adding new vehicle", "", ""))))
		};
		public static InteractiveList<Client> clients = new InteractiveList<Client>()
		{
			List = new List<Client>(),
			Get = c => c.PrettyString(),
			Reset = c => clients.List.Remove(c),
			Editor = (c, l) => Display.DisplayEditor<Client>(c, (" Editing client", "", "")),
			New = new PropertyCapsule(" + Add new client", null, null,
				_ => clients.List.Add(Display.DisplayConstructor<Client>((" Adding new client", "", ""))))
		};
		public static InteractiveList<Ticket> tickets = new InteractiveList<Ticket>()
		{
			List = new List<Ticket>(),
			Get = t => t.PrettyString(),
			Reset = t => tickets.List.Remove(t),
			Editor = (t, l) => Display.DisplayEditor(t, (" Editing ticket", "", "")),
			New = new PropertyCapsule(" + Add new ticket", null, null,
				_ => tickets.List.Add(Display.DisplayConstructor<Ticket>((" Adding new ticket", "", ""))))
		};
		public static CompanyTree company = new CompanyTree();
		public static CityMap map = new CityMap();
		static void Main()
		{
			LoadVariables();
			InitializeDisplay();
		}
		static void InitializeDisplay()
		{
			Display.ScreenMode(true);
			Display.Title =
			@"  ______                      ______                            __ " + "\n" +
			@" /_  __/________ _____  _____/ ____/___  ____  ____  ___  _____/ /_" + "\n" +
			@"  / / / ___/ __ `/ __ \/ ___/ /   / __ \/ __ \/ __ \/ _ \/ ___/ __/" + "\n" +
			@" / / / /  / /_/ / / / (__  ) /___/ /_/ / / / / / / /  __/ /__/ /_  " + "\n" +
			@"/_/ /_/   \__,_/_/ /_/____/\____/\____/_/ /_/_/ /_/\___/\___/\__/  " + "\n" +
			@"                                                           by Emile";
			Display.TitleNegative = false;
			Display.BackgroundColor = Options.BackgroundColor;
			Display.TextColor = Options.TextColor;
		}
		static void LoadVariables()
		{
			void Loader<T>(string path, Action<T> load)
			{
				T? loaded = Saver.Load<T>(path, true);
				if (loaded is null) Console.WriteLine($"! Failed to load '{path}'");
				else
				{
					load((T)loaded);
					Console.WriteLine($"Succesfully loaded '{path}'");
				}
			}
			void MapLoader()
			{
				CityMap? loaded = CityMap.LoadCSV(Options.DistancesSavePath, true);
				if (loaded is null) Console.WriteLine($"! Failed to load '{Options.DistancesSavePath}'");
				else
				{
					map = (CityMap)loaded;
					Console.WriteLine($"Succesfully loaded '{Options.DistancesSavePath}'");
				}
			}

			List<Thread> loaders = new List<Thread>()
			{
				new Thread(() => Loader<List<Vehicle>>(Options.FleetSavePath, loaded => fleet.List = loaded)),
				new Thread(() => Loader<List<Client>>(Options.ClientsSavePath, loaded => clients.List = loaded)),
				new Thread(() => Loader<List<Ticket>>(Options.TicketsSavePath, loaded => tickets.List = loaded)),
				new Thread(() => Loader<CompanyTree>(Options.CompanySavePath, loaded => company = loaded)),
				new Thread(MapLoader),
			};
			loaders.ForEach(loader => loader.Start());
			loaders.ForEach(loader => loader.Join());

			Console.Write("Loading done");
			System.Threading.Thread.Sleep(1000);
			for(int i = 0; i < 3; i++) { Console.Write("."); System.Threading.Thread.Sleep(1000); }
			Console.WriteLine();
		}
	}
}