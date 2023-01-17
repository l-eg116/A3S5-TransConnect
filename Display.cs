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

		public static void PrintTitle()
		{
			Console.SetCursorPosition(0, 0);
			if(Title.Length == 0) return;
			Console.BackgroundColor = TitleNegative ? TextColor : BackgroundColor;
			Console.ForegroundColor = TitleNegative ? BackgroundColor : TextColor;
			foreach (string line in Title.Split('\n'))
				Console.WriteLine(CenterString(line, Console.WindowWidth, true));
		} 
		public static void PrintHeader(string headerLeft = "", string headerCenter = "", string headerRight = "", bool negative = true, bool atCursor = false)
		{
			if(!atCursor) Console.SetCursorPosition(0, TitleHeight);
			Console.BackgroundColor = negative ? TextColor : BackgroundColor;
			Console.ForegroundColor = negative ? BackgroundColor : TextColor;
			string header = CenterString(headerCenter, Console.WindowWidth, true);
			header = header.Substring(0, header.Length - headerRight.Length) + headerRight;
			header = headerLeft + header.Substring(headerLeft.Length);
			Console.WriteLine(header);
		}
		public static void PrintFooter(string footerLeft = "", string footerCenter = "", string footerRight = "", bool negative = true)
		{
			Console.SetCursorPosition(0, Console.WindowHeight - 1);
			Console.BackgroundColor = negative ? TextColor : BackgroundColor;
			Console.ForegroundColor = negative ? BackgroundColor : TextColor;
			string footer = CenterString(footerCenter, Console.WindowWidth, true);
			footer = footer.Substring(0, footer.Length - footerRight.Length) + footerRight;
			footer = footerLeft + footer.Substring(footerLeft.Length);
			Console.Write(footer);
		}

		public static void DisplayText(IEnumerable<string> lines, (string, string, string)? header = null, (string, string, string)? footer = null, bool truncate = true)
		{
			if (header is null) header = ("", "", "");
			(string, string, string) header_ = ((string, string, string))header;
			if (footer is null) footer = ("", "", "");
			(string, string, string) footer_ = ((string, string, string))footer;
			int maxLength = 0;
			foreach(string line in lines) maxLength = Math.Max(maxLength, line.Length);
			if(truncate) maxLength = Math.Min(maxLength, Console.WindowWidth);

			PrintTitle();
			PrintHeader(header_.Item1, header_.Item2, header_.Item3);
			PrintFooter(footer_.Item1, footer_.Item2, footer_.Item3);

			Console.BackgroundColor = BackgroundColor;
			Console.ForegroundColor = TextColor;
			int i = TitleHeight + 1;
			foreach(string line in lines)
			{
				Console.SetCursorPosition(0, i++);
				Console.Write(line.PadRight(maxLength).Substring(0, maxLength));
				if(truncate && i >= Console.WindowHeight - 1) break;
			}
		}

		public static string CenterString(string str, int size, bool truncate = false)
		{
			int padding = (size - str.Length) / 2;
			int extra = (size - str.Length) % 2;
			if (truncate && padding <= 0) return str.Substring(-padding, size);
			return str.PadLeft(padding + extra + str.Length).PadRight(2 * padding + extra + str.Length);
		}
	}
}