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
			New = new PropertyCapsule(" + Add new vehicle", null, null, _ => NewVehicle())
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

			MainMenu();

			DisableDisplay();
			SaveVariables();

			// Console.WriteLine("\n\nProgram ended, press any key to exit...");
			// Console.ReadKey(true);
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
		static void MainMenu()
		{
			bool loop = true;
			while (loop)
			{
				Display.DisplayActionSelector(new List<(string, Action)>()
				{
					(" > Employees       ", Employees),
					(" > Clients         ", Clients),
					(" > Vehicles        ", Vehicles),
					(" > Tickets         ", PlaceHolder),
					(" > Map             ", PlaceHolder),
					(" ", () => { }),
					(" ) Settings        ", Settings),
					(" ) Credits & Infos ", CreditsInfos),
					(" ", () => { }),
					(" → Save            ", ManualSave),
					(" → Save & Exit     ", () => loop = false),
				}, null,
				("", " = =   Main Menu   = = ", ""),
				("", "[Space|Enter] Select   [W|Z|↑/S|↓] Selection up/down", ""),
				Display.Alignement.Center, true);
			}
		}
		static void DisableDisplay()
		{
			Display.ScreenMode(true);
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
			System.Threading.Thread.Sleep(500);
			for (int i = 0; i < 3; i++) { Console.Write("."); System.Threading.Thread.Sleep(500); }
			Console.WriteLine();
		}
		static void SaveVariables()
		{
			void Saverer(object? obj, string path)
			{
				if (Saver.Save(obj, path, true))
					Console.WriteLine($"Successfully saved '{path}'");
				else Console.WriteLine($"! Failled to save '{path}'");
			}

			List<Thread> savers = new List<Thread>()
			{
				new Thread(() => Saverer(fleet.List, Options.FleetSavePath)),
				new Thread(() => Saverer(clients.List, Options.ClientsSavePath)),
				new Thread(() => Saverer(tickets.List, Options.TicketsSavePath)),
				new Thread(() => Saverer(company, Options.CompanySavePath)),
				// Program.map is not saved
			};

			savers.ForEach(saverer => saverer.Start());
			savers.ForEach(saverer => saverer.Join());

			Console.Write("Saving done");
			System.Threading.Thread.Sleep(500);
			for (int i = 0; i < 3; i++) { Console.Write("."); System.Threading.Thread.Sleep(500); }
			Console.WriteLine();
		}
		static void NewVehicle()
		{
			Vehicle? vehicle = null;
			Display.DisplayActionSelector(new List<(string, Action)>()
				{
					(" Create a Car   ", () => vehicle = Display.DisplayConstructor<Car>()),
					(" Create a Truck ", () => vehicle = Display.DisplayConstructor<Truck>()),
					(" Create a Van   ", () => vehicle = Display.DisplayConstructor<Van>()),
				}, null,
				("", ">  Client sorting mode  <", ""),
				("", "[Space|Enter] Select   [W|Z|↑/S|↓] Selection up/down", ""),
				Display.Alignement.Center, true);
			if (vehicle is not null) fleet.List.Add(vehicle);
		}

		static void Employees()
		{
			Display.DisplayEditor(company,
				("  Managing company", "", ""),
				("", "[Space|Enter] Edit   [Suppr] Remove employee   [W|Z|↑/S|↓] Selection up/down   [Esc] Leave", ""),
				Display.Alignement.Left
			);
		}
		static void Clients()
		{
			void SortAlphaName() => clients.List.Sort((c1, c2) => c1.PrettyString().CompareTo(c2.PrettyString()));
			void SortAlphaCity() => clients.List.Sort((c1, c2) => c1.City?.CompareTo(c2.City) ?? -1);
			void SortTotalHist() => clients.List.Sort((c1, c2) => (int)(Ticket.TotalCost(c1) - Ticket.TotalCost(c2)));
			void SortTotalLost() => clients.List.Sort((c1, c2) => (int)(Ticket.TotalCost(c2) - Ticket.TotalCost(c1)));
			void SortCountHist() => clients.List.Sort((c1, c2) => (int)(Ticket.PastCount(c1) - Ticket.PastCount(c2)));
			void SortCountLost() => clients.List.Sort((c1, c2) => (int)(Ticket.PastCount(c2) - Ticket.PastCount(c1)));
			Display.DisplayActionSelector(new List<(string, Action)>()
				{
					(" Alphabetical (Name | A → Z)       ", SortAlphaName),
					(" Alphabetical (City | A → Z)       ", SortAlphaCity),
					(" Total spent (Highest first)       ", SortTotalHist),
					(" Total spent (Lowest first)        ", SortTotalLost),
					(" Number of tickets (Highest first) ", SortCountHist),
					(" Number of tickets (Lowest first)  ", SortCountLost),
				}, null,
				("", ">  Client sorting mode  <", ""),
				("", "[Space|Enter] Select   [W|Z|↑/S|↓] Selection up/down", ""),
				Display.Alignement.Center, true);

			Display.DisplayEditor(clients,
				("  Managing clients", "", ""),
				("", "[Space|Enter] Edit   [Suppr] Remove client   [W|Z|↑/S|↓] Selection up/down   [Esc] Leave", ""),
				Display.Alignement.Left
			);
		}
		static void Vehicles()
		{
			fleet.List.Sort((v1, v2) => v1.ToString().CompareTo(v2.ToString()));
			Display.DisplayEditor(fleet,
				("  Managing fleet", "", ""),
				("", "[Space|Enter] Edit   [Suppr] Remove vehicle   [W|Z|↑/S|↓] Selection up/down   [Esc] Leave", ""),
				Display.Alignement.Left
			);
		}
		static void Settings()
			=> Display.DisplayText(new string[]
				{
					"", "", "",
					"Your settings are in",
					"another castle      ",
					"", "", "",
					"[Press any key to go back]"
				},
				("", " >  Settings  < ", ""),
				("", "", ""),
				Display.Alignement.Center);
		static void CreditsInfos()
		{
			Display.DisplayScrollableText(new string[]
			{
				"<=> INFOS <=>",
				"App made as an academic project : ESILV-A3-S5",
				"This app is build to simplify the management of employees, clients and tickets",
				"of the company 'TransConnect', and follows certain specifications.",
				"There are 4 functionalities of this app that where creative liberties :",
				"---1---, ---2---, ---3---, ---4---", // ! TODO
				"", "",
				"<=> CREDITS <=>",
				"~ Programming ~", "Emile GATIGNON", "",
				"~ User Interface ~", "Emile GATIGNON", "",
				"~ User Experience ~", "Emile GATIGNON", "",
				"~ App Concept ~", "Aline ELLUL", "",
				"~ Music & Sound Design ~", "Emile GATIGNON", "",
				"~ Testers ~", "None as of yet", "",  // TODO
			},
			("", " >  Credits & Infos  < ", ""),
			("", "[Press ESC to go back]", ""),
			Display.Alignement.Center);
		}
		static void ManualSave()
		{
			Display.ClearContentArea();
			Display.PrintFooter("", "", "");
			Display.PrintTitle();
			Display.PrintHeader("  Saving...", "", "");
			SaveVariables();
		}

		static void PlaceHolder()
			=> Display.DisplayText(new string[]
				{
					"", "", "",
					"This functionality is in",
					"another castle          ",
					"", "", "",
					"[Press any key to go back]"
				},
				("", " [ Not implemented ] ", ""),
				("", " [ Not implemented ] ", ""),
				Display.Alignement.Center);
	}
}