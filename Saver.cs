using System.Text;
using System.Text.Json;

namespace A3S5_TransConnect
{
	static class Saver
	{
		static public bool Save(object? obj, string path, bool ignoreErrors = false)
		{
			string jsonStr = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
			try
			{
				File.WriteAllText(path, jsonStr, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				if (ignoreErrors) return false;
				else throw ex;
			}
			return true;
		}
		static public T? Load<T>(string path, bool ignoreErrors = false)
		{
			string jsonStr;
			try
			{
				jsonStr = File.ReadAllText(path, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				if(ignoreErrors) return default(T);
				else throw ex;
			}
			return JsonSerializer.Deserialize<T>(jsonStr);
		}
	}
}