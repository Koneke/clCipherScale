namespace clCipherScale
{
	using System;
	using System.IO;
	using System.Linq;

	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Usage: clCipherScale <scale> <plaintext>");
				return;
			}

			var scales = File.ReadAllLines(@"scales.txt")
				.Select(x => x.Split(';'))
				.Select(x => new Scale(x[0], x[1].Split(',')))
				.ToDictionary(x => x.Name.ToLower());

			var scale = args[0].ToLower();
			var plain = string.Join("", args.Skip(1));

			if (!scales.ContainsKey(scale))
			{
				Console.WriteLine($"Couldn't find scale \"{scale}\", available scales are:");
				Console.WriteLine(string.Join(", ", scales.Keys));
				return;
			}

			Generate(scales[scale], plain);
		}

		private static void Generate(Scale scale, string plain)
		{
			Console.WriteLine(
				string.Join(" ", plain
					.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c))
					.Select(char.ToLower)
					.Select(c => char.IsWhiteSpace(c) ? " " : scale.Notes[(c - 'a') % scale.Notes.Length])));
		}
	}
}

public class Scale
{
	public string Name { get; set; }
	public string[] Notes;

	public Scale(string name, string[] notes)
	{
		Name = name;
		Notes = notes;
	}
}