using System;

namespace Shoe.Data
{
	[Serializable]
	public class SaveGameDescription
	{
		public string Filename { get; set; }
		public string PlayerName { get; set; }
		public string PlayerLevel { get; set; }
		public string PlayerClass { get; set; }
		public string Region { get; set; }
		public string Description { get; set; }

		public SaveGameDescription()
		{
			Filename = "";
			PlayerName = "Unknown";
			PlayerLevel = "0";
			PlayerClass = "Unknown";
			Region = "Unknown";
			Description = "Empty";
		}
	}
}
