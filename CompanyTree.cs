namespace A3S5_TransConnect
{
	class CompanyTree : IDisplaySelector<Employee>, IDisplayEditable<CompanyTree>
	{
		public string Name { get; set; }
		public Employee? Root { get; set; }
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
		public Employee? FindSuperior(Employee employee)
		{
			Employee? superior = null;
			void Finder(Employee? e)
			{
				if (e?.Subordinates?.Contains(employee) ?? false) superior = e;
				else e?.Subordinates?.ForEach(Finder);
			}

			Finder(this.Root);
			return superior;
		}

		public List<(string, Employee)> InstanceSelector()
			=> this.MakeTree();

		private void PC_RemoveEmployee(Employee employee)
		{
			if (employee.HasSubordinates())
				switch (Display.DisplaySimpleSelector(
					new List<string> { " ¤ Delete subordinate(s)", " ↑ Give subordinate(s) to superior", " → Give subordinate(s) to other employee" },
					(" What will happen to this employee's subordinate(s)", "", "")))
				{
					case 0: break;
					case 1: employee.TransferSubordinates(FindSuperior(employee) ?? new Employee()); break;
					case 2:
						PC_MoveEmployee(employee);
						employee.TransferSubordinates(FindSuperior(employee) ?? new Employee());
						break;
					default: return;
				}

			FindSuperior(employee)?.Subordinates?.Remove(employee);
		}
		private void PC_MoveEmployee(Employee? employee)
		{
			if (employee is null) return;
			Employee? exSuperior = FindSuperior(employee);
			exSuperior?.Subordinates?.Remove(employee);
			Employee? newSuperior = Display.DisplayInstanceSelector(this, (" Select new superior", "", ""));
			(newSuperior ?? exSuperior)?.AddSubordinate(employee);
		}
		private void PC_AddEmployee()
		{
			Employee newCommer = Display.DisplayConstructor<Employee>((" Creating new employee", "", ""));
			if (this.IsEmpty()) this.Root = newCommer;
			else Display.DisplayInstanceSelector(this, (" Choose new employee's superior", "", ""))?.AddSubordinate(newCommer);
		}
		public List<PropertyCapsule> PropertyCapsules()
		{
			List<PropertyCapsule> propertyCapsules = new List<PropertyCapsule>()
				{ new PropertyCapsule("> Company has ", () => $"{this.Count} employee(s) on {this.Height} level(s) <") };
			propertyCapsules.Add(new PropertyCapsule(" + Add new employee", null, null, _ => PC_AddEmployee()));
			if (!this.IsEmpty())
				propertyCapsules.Add(new PropertyCapsule(" → Move an employee", null, null,
				_ => PC_MoveEmployee(Display.DisplayInstanceSelector(this, (" Select employee to move", "", "")))));
			foreach ((string, Employee) branch in this.MakeTree())
				propertyCapsules.Add(new PropertyCapsule(branch.Item1, null,
					() => PC_RemoveEmployee(branch.Item2),
					_ => Display.DisplayEditor<Employee>(branch.Item2, (" Editing Company employee", "", ""))));

			return propertyCapsules;
		}
	}
}