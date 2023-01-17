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

		public static string CenterString(string str, int size, bool truncate = false)
		{
			int padding = (size - str.Length) / 2;
			int extra = (size - str.Length) % 2;
			if (truncate && padding <= 0) return str.Substring(-padding, size);
			return str.PadLeft(padding + extra + str.Length).PadRight(2 * padding + extra + str.Length);
		}
	}
}