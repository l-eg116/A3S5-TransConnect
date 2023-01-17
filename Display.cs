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

		public static void DisplayTitle()
		{
			Console.SetCursorPosition(0, 0);
			if(Title.Length == 0) return;
			Console.BackgroundColor = TitleNegative ? TextColor : BackgroundColor;
			Console.ForegroundColor = TitleNegative ? BackgroundColor : TextColor;
			foreach (string line in Title.Split('\n'))
				Console.WriteLine(CenterString(line, Console.WindowWidth, true));
		}
		public static void DisplayHeader(string headerLeft = "", string headerCenter = "", string headerRight = "", bool negative = true, bool atCursor = false)
		{
			if(!atCursor) Console.SetCursorPosition(0, TitleHeight);
			Console.BackgroundColor = negative ? TextColor : BackgroundColor;
			Console.ForegroundColor = negative ? BackgroundColor : TextColor;
			string header = CenterString(headerCenter, Console.WindowWidth, true);
			header = header.Substring(0, header.Length - headerRight.Length) + headerRight;
			header = headerLeft + header.Substring(headerLeft.Length);
			Console.WriteLine(header);
		}
		public static void DisplayFooter(string footerLeft = "", string footerCenter = "", string footerRight = "", bool negative = true)
		{
			Console.SetCursorPosition(0, Console.WindowHeight - 1);
			Console.BackgroundColor = negative ? TextColor : BackgroundColor;
			Console.ForegroundColor = negative ? BackgroundColor : TextColor;
			string footer = CenterString(footerCenter, Console.WindowWidth, true);
			footer = footer.Substring(0, footer.Length - footerRight.Length) + footerRight;
			footer = footerLeft + footer.Substring(footerLeft.Length);
			Console.Write(footer);
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