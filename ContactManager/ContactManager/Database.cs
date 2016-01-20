﻿using System;
using System.IO;
//using SQLite;
using Android.Database.Sqlite;

namespace ContactManager
{
	public class Database
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public Database ()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public Database(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				sqldb_available = false;
				CreateDatabase(sqldb_name);
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		public bool DatabaseAvailable
		{
			get{ return sqldb_available; }
			set{ sqldb_available = value; }
		}

		public string Message
		{
			get{ return sqldb_message; }
			set{ sqldb_message = value; }
		}

		public void CreateDatabase(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				string sqldb_path = Path.Combine(sqldb_location,sqldb_name);
				bool sqldb_exists = File.Exists(sqldb_path);
				if(!sqldb_exists)
				{
					
					sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path,null);
					sqldb_query = "CREATE TABLE IF NOT EXISTS Contact (_id INTEGER PRIMARY KEY AUTOINCREMENT, FirstName VARCHAR, LastName VARCHAR, " +
						"Phone VARCHAR, Photo VARCHAR);";
					sqldb.ExecSQL(sqldb_query);
					sqldb_message = "Database:"+sqldb_name+" created";
				}
				else
				{
					sqldb = SQLiteDatabase.OpenDatabase(sqldb_path,null,DatabaseOpenFlags.OpenReadwrite);
					sqldb_message = "Database:"+sqldb_name+" opened";
				}
				sqldb_available=true;
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}   

		public void AddRecord(string sFirstName, string sLastName, string sPhone, string sPhoto)
		{
			try
			{
				sqldb_query = "INSERT INTO Contact (FirstName, LastName, Phone, Photo) VALUES ('"+sFirstName+"','"+sLastName+"','"+sPhone+"','"+sPhoto+"');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		public void DeleteRecord(string sFirstName, string sLastName)
		{
			try
			{
				sqldb_query = "DELETE FROM Contact WHERE _id = (SELECT _id FROM Contact WHERE FirstName = '"+sFirstName+"' AND LastName = '"+sLastName+"');";
				sqldb.ExecSQL(sqldb_query);
				//sqldb_message = "Record "+iId+" deleted";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		public Android.Database.ICursor GetRecordCursor()
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT * FROM Contact ORDER BY FirstName;";
				//sqldb_query = "SELECT * FROM Contact;";
				sqldb_cursor = sqldb.RawQuery(sqldb_query,null);
				if(!(sqldb_cursor!=null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}
			
	}
}


