namespace A3S5_TransConnect
{
	interface ISaveLoadable<T>
	{
		public bool Save(string path, bool supressErrors = false);
		public static T? Load(string path, bool supressErrors = false) => default(T);
	}

	interface ISaveLoadListable<T> : ISaveLoadable<T>
	{
		public static bool ListSave(string path, bool supressErrors = false) => false;
		public static List<T>? ListLoad(string path, bool supressErrors = false) => null;
	}
}