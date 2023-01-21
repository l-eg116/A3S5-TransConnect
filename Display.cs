namespace A3S5_TransConnect
{
	static class Display
	{
		public static string Title { get; set; } = "";
		public static int TitleHeight { get => Title.Length == 0 ? 0 : Title.Count(c => c == '\n') + 1; }
		public static int TitleWidth
		{
			get
			{
				int max = 0;
				foreach (string line in Title.Split('\n'))
					max = Math.Max(max, line.Length);
				return max;
			}
		}
		public static bool TitleNegative { get; set; } = false;
		public static ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
		public static ConsoleColor TextColor { get; set; } = ConsoleColor.White;
		public static ConsoleColor DefaultBackgroundColor { get; } = Console.BackgroundColor;
		public static ConsoleColor DefaultForegroundColor { get; } = Console.ForegroundColor;
		public enum Alignement
		{
			Left,
			Center,
			Right
		}
		public static Dictionary<string, IEnumerable<ConsoleKey>> KeyBundles { get; } =
		new Dictionary<string, IEnumerable<ConsoleKey>>
		{
			{"Back", new ConsoleKey[] { ConsoleKey.Escape, ConsoleKey.BrowserBack } },
			{"Up", new ConsoleKey[] { ConsoleKey.W, ConsoleKey.Z, ConsoleKey.UpArrow } },
			{"Left", new ConsoleKey[] { ConsoleKey.A, ConsoleKey.Q, ConsoleKey.LeftArrow } },
			{"Down", new ConsoleKey[] { ConsoleKey.S, ConsoleKey.DownArrow } },
			{"Right", new ConsoleKey[] { ConsoleKey.D, ConsoleKey.RightArrow } },
			{"Select", new ConsoleKey[] { ConsoleKey.Enter, ConsoleKey.Spacebar, ConsoleKey.E } },
			{"Delete", new ConsoleKey[] { ConsoleKey.Delete, ConsoleKey.Backspace, ConsoleKey.R } },
		};

		public static void ApplyColor(bool negative = false)
		{
			Console.BackgroundColor = negative ? TextColor : BackgroundColor;
			Console.ForegroundColor = negative ? BackgroundColor : TextColor;
		}
		public static void RestoreColor()
		{
			Console.BackgroundColor = DefaultBackgroundColor;
			Console.ForegroundColor = DefaultForegroundColor;
		}
		public static void ScreenMode(bool active = false)
		{
			Console.CursorVisible = !active;
			if (!active) RestoreColor();
			Console.Clear();
		}
		public static void WriteAligned(object? obj, Alignement aligned = Alignement.Left, int line = -1)
		{
			string text = obj?.ToString() ?? "";
			if (line < 0) line = Console.CursorTop;
			if (text.Length < Console.WindowWidth) switch (aligned)
				{
					case (Alignement.Left): Console.SetCursorPosition(0, line); break;
					case (Alignement.Center): Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, line); break;
					case (Alignement.Right): Console.SetCursorPosition(Console.WindowWidth - text.Length, line); break;
				}
			else Console.SetCursorPosition(0, line);
			Console.Write(text);
		}
		public static T CleanRead<T>(string? label = null, int line = -1) where T : IConvertible
		{
			bool prevCursorVisible = Console.CursorVisible;
			label ??= $"{typeof(T)} > ";
			if (line < 0) line = Console.CursorTop;
			T result;

			while (true)
			{
				ClearLine(line, true, true);
				Console.SetCursorPosition(0, line);
				ApplyColor(true);
				Console.Write(label);
				Console.CursorVisible = true;
				string read = Console.ReadLine() ?? "";
				Console.CursorVisible = false;
				try { result = (T)Convert.ChangeType(read, typeof(T)); }
				catch { Console.Write("\a"); continue; }
				break;
			}
			RestoreColor();
			Console.CursorVisible = prevCursorVisible;
			return result;
		}
		public static string AlignString(string str, int size, Alignement aligned = Alignement.Left, bool truncate = false)
		{
			int padding = size - str.Length;
			if (truncate && padding < 0) switch (aligned)
				{
					case (Alignement.Left): return str.Substring(0, size);
					case (Alignement.Center): return str.Substring((-padding) / 2, size);
					case (Alignement.Right): return str.Substring(-padding, size);
				}
			switch (aligned)
			{
				case (Alignement.Left):
					return str.PadRight(size);
				case (Alignement.Center):
					return str.PadLeft(padding / 2 + padding % 2 + str.Length).PadRight(padding + str.Length);
				case (Alignement.Right):
					return str.PadLeft(size);
			}
			return str;
		}

		public static void PrintTitle()
		{
			Console.SetCursorPosition(0, 0);
			if (Title.Length == 0) return;
			ApplyColor(TitleNegative);
			foreach (string line in Title.Split('\n'))
				Console.WriteLine(AlignString(line, Console.WindowWidth, Alignement.Center, true));
			RestoreColor();
		}
		public static void PrintHeader(string headerLeft = "", string headerCenter = "", string headerRight = "", bool negative = true, bool atCursor = false)
		{
			if (!atCursor) Console.SetCursorPosition(0, TitleHeight);
			ApplyColor(negative);
			string header = AlignString(headerCenter, Console.WindowWidth, Alignement.Center, true);
			header = header.Substring(0, header.Length - headerRight.Length) + headerRight;
			header = headerLeft + header.Substring(headerLeft.Length);
			Console.WriteLine(header);
			RestoreColor();
		}
		public static void PrintFooter(string footerLeft = "", string footerCenter = "", string footerRight = "", bool negative = true)
		{
			Console.SetCursorPosition(0, Console.WindowHeight - 1);
			ApplyColor(negative);
			string footer = AlignString(footerCenter, Console.WindowWidth, Alignement.Center, true);
			footer = footer.Substring(0, footer.Length - footerRight.Length) + footerRight;
			footer = footerLeft + footer.Substring(footerLeft.Length);
			Console.Write(footer);
			RestoreColor();
		}
		public static void ClearLine(int line, bool applyBackground = false, bool bgNegative = false)
		{
			if (applyBackground) ApplyColor(bgNegative);
			else RestoreColor();
			WriteAligned("".PadRight(Console.WindowWidth), default, line);
			RestoreColor();
		}
		public static void ClearContentArea(bool applyBackground = false)
		{
			if (applyBackground) ApplyColor();
			else RestoreColor();
			for (int i = TitleHeight + 1; i < Console.WindowHeight - 1; i++)
				WriteAligned("".PadRight(Console.WindowWidth), default, i);
			RestoreColor();
		}

		public static void DisplayText(IEnumerable<string> lines,
			(string, string, string)? header_ = null, (string, string, string)? footer_ = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			(string, string, string) header = header_ ?? ("", "", "");
			(string, string, string) footer = footer_ ?? ("", "Press any key to go back", "");
			int maxLength = lines.Count() > 0 ? lines.Max(s => s.Length) : 0;
			if (truncate) maxLength = Math.Min(maxLength, Console.WindowWidth);

			PrintTitle();
			PrintHeader(header.Item1, header.Item2, header.Item3);
			PrintFooter(footer.Item1, footer.Item2, footer.Item3);
			ClearContentArea();

			ApplyColor();
			int i = TitleHeight + 1;
			foreach (string line in lines)
			{
				WriteAligned(AlignString(line, maxLength, aligned, truncate), aligned, i++);
				if (truncate && i >= Console.WindowHeight - 1) break;
			}
			Console.ReadKey(true);
			RestoreColor();
		}
		public static void DisplayScrollableText(IEnumerable<string> lines,
			(string, string, string)? header_ = null, (string, string, string)? footer_ = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			(string, string, string) header = header_ ?? ("", "", "");
			(string, string, string) footer = footer_ ?? ("", "[W|Z|↑] Scroll up   [S|↓] Scroll down   [Esc] Go back", "");

			int maxLength = lines.Count() > 0 ? lines.Max(s => s.Length) : 0;
			if (truncate) maxLength = Math.Min(maxLength, Console.WindowWidth);

			List<string> linesList = lines.ToList();
			int top = 0;
			Func<int> bottom = () => Console.WindowHeight - TitleHeight - 5 + top;

			PrintTitle();
			PrintHeader(header.Item1, header.Item2, header.Item3);
			PrintFooter(footer.Item1, footer.Item2, footer.Item3);
			ClearContentArea();

			ApplyColor();
			while (true)
			{
				for (int i = top, line = TitleHeight + 2; i <= bottom() && i < linesList.Count; i++, line++)
					WriteAligned(AlignString(linesList[i], maxLength, aligned, truncate), aligned, line);
				ClearLine(TitleHeight + 1); ClearLine(Console.WindowHeight - 2); ApplyColor();
				if (top != 0) WriteAligned($" ↑  ↑  ↑  +{top}", aligned, TitleHeight + 1);
				if (bottom() < linesList.Count - 1) WriteAligned($" ↓  ↓  ↓  +{linesList.Count - bottom() - 1}", aligned, Console.WindowHeight - 2);

				ConsoleKey action = Console.ReadKey(true).Key;
				if (KeyBundles["Up"].Contains(action) && top > 0) top--;
				else if (KeyBundles["Down"].Contains(action) && bottom() < linesList.Count - 1) top++;
				else if (KeyBundles["Back"].Contains(action)) break;
			}
			RestoreColor();
		}
		public static int DisplaySimpleSelector<T>(List<T> objects,
			(string, string, string)? header_ = null, (string, string, string)? footer_ = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			(string, string, string) header = header_ ?? ("", "", "");
			(string, string, string) footer = footer_ ?? ("", "[Space|Enter] Select   [W|Z|↑] Selection up   [S|↓] Selection down   [Esc] Go back", "");

			int maxLength = objects.Count > 0 ? objects.Max(obj => (obj + "").Length) : 0;
			if (truncate) maxLength = Math.Min(maxLength, Console.WindowWidth);

			int selected = -1;
			int top = 0;
			Func<int> bottom = () => Console.WindowHeight - TitleHeight - 5 + top;
			int cursor = 0;

			PrintTitle();
			PrintHeader(header.Item1, header.Item2, header.Item3);
			PrintFooter(footer.Item1, footer.Item2, footer.Item3);
			ClearContentArea();

			ApplyColor();
			while (true)
			{
				cursor = Math.Max(0, Math.Min(cursor, objects.Count - 1));
				if (cursor < top) top--;
				if (cursor > bottom()) top++;

				for (int i = top, line = TitleHeight + 2; i <= bottom() && i < objects.Count; i++, line++)
					if (i == cursor)
					{
						ApplyColor(true);
						WriteAligned(AlignString(objects[i] + "", maxLength, aligned, truncate), aligned, line);
						ApplyColor(false);
					}
					else
						WriteAligned(AlignString(objects[i] + "", maxLength, aligned, truncate), aligned, line);
				ClearLine(TitleHeight + 1); ClearLine(Console.WindowHeight - 2); ApplyColor();
				if (top != 0) WriteAligned($" ↑  ↑  ↑  +{top}", aligned, TitleHeight + 1);
				if (bottom() < objects.Count - 1) WriteAligned($" ↓  ↓  ↓  +{objects.Count - bottom() - 1}", aligned, Console.WindowHeight - 2);

				ConsoleKey action = Console.ReadKey(true).Key;
				if (KeyBundles["Up"].Contains(action)) cursor--;
				else if (KeyBundles["Down"].Contains(action)) cursor++;
				else if (KeyBundles["Select"].Contains(action)) { selected = cursor; break; }
				else if (KeyBundles["Back"].Contains(action)) break;
			}
			RestoreColor();
			return selected;
		}
		public static int DisplayTransformedSelector<T>(List<T> objects, Func<T, string> transformer,
			(string, string, string)? header = null, (string, string, string)? footer = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			List<string> objectStrings = new List<string>();
			foreach (T obj in objects) objectStrings.Add(transformer(obj));
			return DisplaySimpleSelector(objectStrings, header, footer, aligned, truncate);
		}
		public static void DisplayActionSelector<T>(List<(T, Action)> labeledActions, Func<T, string>? transformer = null,
			(string, string, string)? header = null, (string, string, string)? footer = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			transformer ??= obj => obj + "";
			int selected = DisplayTransformedSelector(labeledActions, tpl => transformer(tpl.Item1), header, footer, aligned, truncate);
			if (selected >= 0) labeledActions[selected].Item2();
		}
		public static void DisplayController<T>(List<T> objects,
			Dictionary<ConsoleKey, Action<T>> controls, Func<T, string>? transformer = null,
			(string, string, string)? header_ = null, (string, string, string)? footer_ = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			transformer ??= obj => obj + "";
			(string, string, string) header = header_ ?? ("", "", "");
			(string, string, string) footer = footer_ ?? ("", "[W|Z|↑/S|↓] Selection up/down   [Esc] Go back", "");

			int maxLength = objects.Count > 0 ? objects.Max(obj => transformer(obj).Length) : 0;
			if (truncate) maxLength = Math.Min(maxLength, Console.WindowWidth);

			int top = 0;
			Func<int> bottom = () => Console.WindowHeight - TitleHeight - 5 + top;
			int cursor = 0;
			bool fullRefresh = true;


			while (true)
			{
				if (fullRefresh)
				{
					PrintTitle();
					PrintHeader(header.Item1, header.Item2, header.Item3);
					PrintFooter(footer.Item1, footer.Item2, footer.Item3);
					ClearContentArea();
					fullRefresh = false;
				}
				ApplyColor();
				for (int i = top, line = TitleHeight + 2; i <= bottom() && i < objects.Count; i++, line++)
					if (i == cursor)
					{
						ApplyColor(true);
						WriteAligned(AlignString(transformer(objects[i]) + "", maxLength, aligned, truncate), aligned, line);
						ApplyColor(false);
					}
					else
						WriteAligned(AlignString(transformer(objects[i]) + "", maxLength, aligned, truncate), aligned, line);
				ClearLine(TitleHeight + 1); ClearLine(Console.WindowHeight - 2); ApplyColor();
				if (top != 0) WriteAligned($" ↑  ↑  ↑  +{top}", aligned, TitleHeight + 1);
				if (bottom() < objects.Count - 1) WriteAligned($" ↓  ↓  ↓  +{objects.Count - bottom() - 1}", aligned, Console.WindowHeight - 2);

				ConsoleKey action = Console.ReadKey(true).Key;
				if (KeyBundles["Up"].Contains(action)) cursor--;
				else if (KeyBundles["Down"].Contains(action)) cursor++;
				else if (KeyBundles["Back"].Contains(action)) break;

				cursor = Math.Max(0, Math.Min(cursor, objects.Count - 1));
				if (cursor < top) top--;
				if (cursor > bottom()) top++;

				if (controls.ContainsKey(action))
				{
					controls[action](objects[cursor]);
					fullRefresh = true;
				}
			}
			RestoreColor();
		}
		public static void DisplayLabeledControler<T>(List<(T, string)> labeledObjects, Dictionary<ConsoleKey, Action<T>> controls,
			(string, string, string)? header = null, (string, string, string)? footer = null,
			Alignement aligned = Alignement.Left, bool truncate = true)
		{
			Dictionary<ConsoleKey, Action<(T, string)>> tweakedControls = new Dictionary<ConsoleKey, Action<(T, string)>>();
			foreach (KeyValuePair<ConsoleKey, Action<T>> control in controls)
				tweakedControls.Add(control.Key, lblObj => control.Value(lblObj.Item1));
			Func<(T, string), string> tranformer = lblObj => lblObj.Item2;
			DisplayController(labeledObjects, tweakedControls, tranformer, header, footer, aligned, truncate);
		}
		public static void DisplayEditor<T>(IDisplayEditable<T> editedInstance,
			(string, string, string)? header_ = null, (string, string, string)? footer_ = null,
			Alignement aligned = Alignement.Left, bool truncate = true) where T : new()
		{
			(string, string, string) header = header_ ?? ("", "", "");
			(string, string, string) footer = footer_ ?? ("", "[Space|Enter] Edit   [Suppr] Reset   [W|Z|↑/S|↓] Selection up/down   [Esc] Go back", "");

			int maxLength;
			void updateMaxLength()
				=> maxLength = Math.Min(editedInstance.PropertyCapsules.Max(pc => pc.ToString().Length),
										truncate ? Console.WindowWidth : int.MaxValue);
			updateMaxLength();

			int top = 0;
			Func<int> bottom = () => Console.WindowHeight - TitleHeight - 5 + top;
			int cursor = 0;
			bool fullRefresh = true;


			while (true)
			{
				if (fullRefresh)
				{
					updateMaxLength();
					PrintTitle();
					PrintHeader(header.Item1, header.Item2, header.Item3);
					PrintFooter(footer.Item1, footer.Item2, footer.Item3);
					ClearContentArea();
					fullRefresh = false;
				}
				ApplyColor();
				for (int i = top, line = TitleHeight + 2; i <= bottom() && i < editedInstance.PropertyCapsules.Count; i++, line++)
					if (i == cursor)
					{
						ApplyColor(true);
						WriteAligned(AlignString(editedInstance.PropertyCapsules[i].ToString(), maxLength, aligned, truncate), aligned, line);
						ApplyColor(false);
					}
					else
						WriteAligned(AlignString(editedInstance.PropertyCapsules[i].ToString(), maxLength, aligned, truncate), aligned, line);
				ClearLine(TitleHeight + 1); ClearLine(Console.WindowHeight - 2); ApplyColor();
				if (top != 0)
					WriteAligned($" ↑  ↑  ↑  +{top}", aligned, TitleHeight + 1);
				if (bottom() < editedInstance.PropertyCapsules.Count - 1)
					WriteAligned($" ↓  ↓  ↓  +{editedInstance.PropertyCapsules.Count - bottom() - 1}", aligned, Console.WindowHeight - 2);

				ConsoleKey action = Console.ReadKey(true).Key;
				if (KeyBundles["Back"].Contains(action)) break;
				else if (KeyBundles["Up"].Contains(action)) cursor--;
				else if (KeyBundles["Down"].Contains(action)) cursor++;
				else if (KeyBundles["Delete"].Contains(action))
				{ editedInstance.PropertyCapsules[cursor].Reset?.Invoke(); fullRefresh = true; }
				else if (KeyBundles["Select"].Contains(action))
				{ editedInstance.PropertyCapsules[cursor].Editor?.Invoke(TitleHeight + 2 + cursor - top); fullRefresh = true; }

				cursor = Math.Max(0, Math.Min(cursor, editedInstance.PropertyCapsules.Count - 1));
				if (cursor < top) top--;
				if (cursor > bottom()) top++;
			}
			RestoreColor();
		}
		public static T DisplayConstructor<T>((string, string, string)? header = null,
			(string, string, string)? footer = null, Alignement aligned = Alignement.Left,
			bool truncate = true) where T : IDisplayEditable<T>, new()
		{
			T newInstance = new T();
			DisplayEditor(newInstance, header, footer, aligned, truncate);
			return newInstance;
		}
	}

	struct PropertyCapsule
	{
		public string Label { get; set; }
		public Func<string> Get { get; set; }
		public Action? Reset { get; set; }
		public Action<int>? Editor { get; set; }

		public PropertyCapsule(string label = "", Func<string>? get = null, Action? reset = null, Action<int>? editor = null)
		{
			this.Label = label;
			this.Get = get ?? (() => "");
			this.Reset = reset;
			this.Editor = editor;
		}

		public override string ToString()
			=> this.Label + this.Get();
	}
	interface IDisplayEditable<TSelf>
	{
		List<PropertyCapsule> PropertyCapsules { get; }
	}
}