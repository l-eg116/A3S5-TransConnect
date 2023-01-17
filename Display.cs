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
			string text = obj is null ? "" : obj.ToString() + "";
			if (line < 0) line = Console.CursorTop;
			switch (aligned)
			{
				case (Alignement.Left): Console.SetCursorPosition(0, line); break;
				case (Alignement.Center): Console.SetCursorPosition((Console.BufferWidth - text.Length) / 2, line); break;
				case (Alignement.Right): Console.SetCursorPosition(Console.BufferWidth - text.Length, line); break;
			}
			Console.Write(text);
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

		public static void DisplayText(IEnumerable<string> lines,
		(string, string, string)? header = null, (string, string, string)? footer = null,
		Alignement aligned = Alignement.Left, bool truncate = true)
		{
			if (header is null) header = ("", "", "");
			(string, string, string) header_ = ((string, string, string))header;
			if (footer is null) footer = ("", "", "");
			(string, string, string) footer_ = ((string, string, string))footer;
			int maxLength = 0;
			foreach (string line in lines) maxLength = Math.Max(maxLength, line.Length);
			if (truncate) maxLength = Math.Min(maxLength, Console.WindowWidth);

			PrintTitle();
			PrintHeader(header_.Item1, header_.Item2, header_.Item3);
			PrintFooter(footer_.Item1, footer_.Item2, footer_.Item3);

			ApplyColor();
			int i = TitleHeight + 1;
			foreach (string line in lines)
			{
				WriteAligned(AlignString(line, maxLength, aligned, truncate), aligned, i++);
				if (truncate && i >= Console.WindowHeight - 1) break;
			}
			RestoreColor();
		}
	}
}