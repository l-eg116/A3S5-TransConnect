namespace A3S5_TransConnect
{
	class CompanyTree
	{
		public string Name { get; set; }
		public Employee? Root { get; init; }
		public int Count
		{
			get
			{
				int count = 0;
				void Counter(Employee emp)
				{
					count++;
					if(emp.HasSubordinates())
						emp.Subordinates.ForEach(Counter);
				}

				if(!this.IsEmpty()) Counter(this.Root);
				return count;
			}
		}
		public int Height
		{
			get
			{
				int height = 0;
				void Counter(Employee emp, int currentHeight)
				{
					if (!emp.HasSubordinates()) height = Math.Max(height, currentHeight);
					else
						foreach (Employee child in emp.Subordinates)
							Counter(child, currentHeight + 1);
				}

				if (!this.IsEmpty()) Counter(this.Root, 1);
				return height;
			}
		}

		public CompanyTree() : this(default, default)
		{ }
		public CompanyTree(string name = "Unnamed Company", Employee? root = null)
		{
			this.Name = name;
			this.Root = root;
		}

		public override string ToString()
			=> $"{this.Name} | {this.Count} employee(s) on {this.Height} levels";

		public bool IsEmpty()
			=> this.Root is null;
	}
}