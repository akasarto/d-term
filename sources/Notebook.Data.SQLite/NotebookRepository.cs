using Dapper;
using Microsoft.Data.Sqlite;
using Notebook.Core;
using System.Collections.Generic;
using System.Linq;

namespace Notebook.Data.SQLite
{
	public class NotebookRepository : INotebookRepository
	{
		public List<Note> GetAll()
		{
			using (var connection = new SqliteConnection("Data Source=dTerm.db;Version=3;"))
			{
				var query = connection.Query<Note>(
					sql: "SELECT * FROM Notes"
				);

				return query.ToList();
				//using (var command = connection.CreateCommand())
				//{
				//	connection.Open();
				//	command.CommandText = "select name from from sqlite_master";
				//	using (var reader = command.ExecuteReader())
				//	{
				//		while (reader.Read())
				//		{
				//			Console.WriteLine(Convert.String(reader["name"]));
				//		}
				//	}
				//}
			}
			//using (var ctx = new NotebookContext())
			//{
			//	return ctx.Notes.ToList();
			//}
		}
	}
}
