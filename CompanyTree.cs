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
					if (emp.HasSubordinates())
						emp.Subordinates.ForEach(Counter);
				}

				if (!this.IsEmpty()) Counter(this.Root);
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
		public List<(string, Employee)> MakeTree()
		{
			const string IBRANCH = "│   ";
			const string TBRANCH = "├───";
			const string LBRANCH = "└───";
			const string SPACER = "·   ";
			List<(string, Employee)> tree = new List<(string, Employee)>();
			void Renderer(Employee employee, string stack = "", string branch = "")
			{
				tree.Add(($"{stack}{branch}{employee.PrettyString()}", employee));
				stack += branch == "" ? "" : branch == TBRANCH ? IBRANCH : SPACER;
				if (employee.HasSubordinates())
				{
					for (int i = 0; i < employee.Subordinates.Count - 1; i++)
						Renderer(employee.Subordinates[i], stack, TBRANCH);
					Renderer(employee.Subordinates[^1], stack, LBRANCH);
				}
			}

			if (!this.IsEmpty()) Renderer(this.Root, "", "");
			return tree;
		}
	}
}