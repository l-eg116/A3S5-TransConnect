namespace A3S5_TransConnect
{
	class Program
	{
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
		public static List<Ticket> tickets = new List<Ticket>();
		public static CompanyTree company = new CompanyTree();
		public static CityMap map = new CityMap();
		static void Main()
		{
			Display.ScreenMode(true);
			Display.Title =
			@"  ______                      ______                            __ " + "\n" +
			@" /_  __/________ _____  _____/ ____/___  ____  ____  ___  _____/ /_" + "\n" +
			@"  / / / ___/ __ `/ __ \/ ___/ /   / __ \/ __ \/ __ \/ _ \/ ___/ __/" + "\n" +
			@" / / / /  / /_/ / / / (__  ) /___/ /_/ / / / / / / /  __/ /__/ /_  " + "\n" +
			@"/_/ /_/   \__,_/_/ /_/____/\____/\____/_/ /_/_/ /_/\___/\___/\__/  " + "\n" +
			@"                                                                   ";
			Display.TitleNegative = false;
			Display.BackgroundColor = ConsoleColor.DarkGray;
			Display.TextColor = ConsoleColor.White;

			TestDispEdit();

			// Console.ReadKey();
			Display.ScreenMode(false);
			// Console.WriteLine(returned);
			Console.WriteLine(typeof(Client));
		}
		static void TestDispControl()
		{
			List<string> lines = new List<string>();
			for (int i = 1; i < 100; i++) lines.Add($"Line {i} UwU");
			lines[52] = "ok woaw this one is quite longer that the other ones";
			Display.DisplayController(
				lines,
				new Dictionary<ConsoleKey, Action<string>> {
					{ ConsoleKey.Enter, str => { Display.WriteAligned("Enter", line: 0); Console.ReadKey(true); } },
					{ ConsoleKey.Spacebar, str => { Display.WriteAligned(str, line: 0); Console.ReadKey(true); } },
					{ ConsoleKey.E, str => Display.DisplayText(new string[] {str}) },
				},
				s => $"{s} (l={s.Length})",
				(" Dude this menu is crazy", " = = = = = = ", "made with <3 by EG "),
				default,
				Display.Alignement.Left,
				true
			);
		}
		static void TestDispLblControl()
		{
			List<(int, string)> lines = new List<(int, string)>();
			for (int i = 1; i < 100; i++) lines.Add((i, $"Line {i} UwU"));
			lines[52] = (123456789, "ok woaw this one is quite longer that the other ones");
			Display.DisplayLabeledControler(
				lines,
				new Dictionary<ConsoleKey, Action<int>> {
					{ ConsoleKey.Enter, n => { Display.WriteAligned("Enter", line: 0); Console.ReadKey(true); } },
					{ ConsoleKey.Spacebar, n => { Display.WriteAligned(n, line: 0); Console.ReadKey(true); } },
					{ ConsoleKey.S, n => { Display.WriteAligned(n, line: 0); Console.ReadKey(true); } },
					{ ConsoleKey.E, n => Display.DisplayText(new string[] {(n * 100 + 5).ToString()}) },
				},
				(" Dude this menu is crazy", " = = = = = = ", "made with <3 by EG "),
				default,
				Display.Alignement.Left,
				true
			);
		}
		static void TestDispEdit()
		{
			Vehicle flash = new Vehicle("Flash", "RUSTEEZ", "14MSP33D");
			Display.DisplayEditor(flash, ("Okay you can change your becane here :))", "= = = = =", "vroum vroum"), null, Display.Alignement.Left);
		}
	}
	struct InteractiveList<T> : IDisplayEditable<InteractiveList<T>>, IDisplaySelector<T>
	{
		public List<T> List { get; set; }
		public Func<T, string>? Get { get; set; }
		public Action<T>? Reset { get; set; }
		public Action<T, int>? Editor { get; set; }
		public PropertyCapsule? New { get; set; }
		public List<PropertyCapsule> PropertyCapsules()
		{
			List<PropertyCapsule> capsules = new List<PropertyCapsule>();
			foreach (T t in this.List)
			{
				InteractiveList<T> copy = this;
				capsules.Add(new PropertyCapsule("", () => copy.Get?.Invoke(t) ?? "",
					() => copy.Reset?.Invoke(t), l => copy.Editor?.Invoke(t, l)));
			}
			if(this.New is not null) capsules.Add((PropertyCapsule)this.New);
			return capsules;
		}
		public List<(string, T)> InstanceSelector()
		{
			List<(string, T)> selector = new List<(string, T)>();
			foreach (T t in this.List)
			{
				InteractiveList<T> copy = this;
				selector.Add((copy.Get?.Invoke(t) ?? t + "", t));
			}
			return selector;
		}
	}
}