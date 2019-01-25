using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindBadCharacters
{
	class CharacterMetadata
	{
		public int Line { get; set; }
		public int Index { get; set; }
		public string LineContent { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			string[] files = Directory.GetFiles(@"C:\Office\dev\", "*.cs", new EnumerationOptions() { RecurseSubdirectories = true });
			foreach(var file in files)
			{
				if (file.Contains("test", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				var content = File.ReadAllText(file);
				List<CharacterMetadata> result = hasBadCharacters(content);
				if (result.Count > 0)
				{
					Console.WriteLine($"File {file} has invalid characters!!");
					Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
					Console.ReadLine();
				}
				Console.WriteLine($"File {file} is good");
			}
		}

		public static List<CharacterMetadata> hasBadCharacters(string input)
		{
			var result = new List<CharacterMetadata>();
			var lines = input.Split('\n');
			for(int lineIndex = 0; lineIndex < lines.Count(); lineIndex++)
			{
				var line = lines[lineIndex];
				for(int charIndex = 0; charIndex < line.Length; charIndex++)
				{
					var myChar = line[charIndex];
					if (!char.IsLetterOrDigit(myChar) && !char.IsPunctuation(myChar) && !char.IsWhiteSpace(myChar) && !char.IsSymbol(myChar))
					{
						result.Add(new CharacterMetadata()
						{
							Index = charIndex,
							Line = lineIndex,
							LineContent = line,
						});
					}
				}
			}
			return result;
		}
	}
}
