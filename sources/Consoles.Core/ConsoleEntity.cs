﻿using System;

namespace Consoles.Core
{
	public class ConsoleEntity
	{
		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }
		public string ProcessPathArgs { get; set; }
		public string ProcessPath { get; set; }
		public PathType ProcessPathType { get; set; }
		public DateTime UTCCreation { get; set; }
	}
}
