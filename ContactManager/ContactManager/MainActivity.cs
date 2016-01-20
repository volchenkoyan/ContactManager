using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views; 
using Android.Util;
using System.Collections.Generic;
using System.Text;

namespace ContactManager
{
	[Activity (Label = "Contact Manager", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{ 
		Database sqldb;
		List<Contact> contacts = new List<Contact>();
		ListView list_contacts;
		//List<string> names_for_delete;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);
			sqldb = new Database ("contact_db");
			list_contacts = FindViewById<ListView> (Resource.Id.list_contacts);
			list_contacts.Adapter = new ContactAdapter(this,contacts);
			list_contacts.ChoiceMode = ChoiceMode.Multiple;
			LoadContacts ();
		 }

		public void LoadContacts()
		{
			contacts.Clear ();
			Android.Database.ICursor sqldb_cursor = sqldb.GetRecordCursor();
			while(sqldb_cursor.MoveToNext ()) {
				contacts.Add(new Contact(sqldb_cursor.GetString (sqldb_cursor.GetColumnIndex ("FirstName")) + " "+ sqldb_cursor.GetString (sqldb_cursor.GetColumnIndex ("LastName")),sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("Phone"))));
			}

			list_contacts = FindViewById<ListView> (Resource.Id.list_contacts);
			list_contacts.Adapter = new ContactAdapter(this,contacts);
			list_contacts.ChoiceMode = ChoiceMode.Multiple;
		}

		void OnSelection ()
		{
			var checked_items = FindViewById<ListView> (Resource.Id.list_contacts).CheckedItemPositions;
			for (int i = 0; i < checked_items.Size (); i++) {
				var temp = list_contacts.GetItemAtPosition(checked_items.KeyAt(i)).Cast<Contact>();
				var t_name = temp.name.Split(new char [] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
				sqldb.DeleteRecord (t_name [0], t_name [1]);
			}
			LoadContacts ();
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu_main, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Resource.Id.action_add_contact) {
				StartActivity (typeof(AddUser));
				return true;
			}
			if (item.ItemId == Resource.Id.action_delete_contact) {
				OnSelection();
			}
			return base.OnOptionsItemSelected(item);
		}
			
		}

	public class ContactAdapter : BaseAdapter<Contact>
	{
		List<Contact> contacts;
		Activity context;
		public ContactAdapter(Activity context, List<Contact> contacts) : base()
		{
			this.context = context;
			this.contacts = contacts;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Contact this[int position]
		{
			get { return contacts [position]; }
		}

		public override int Count
		{
			get { return contacts.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var contact = contacts [position];
			View view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate (Resource.Layout.listview, null);

			Contact item = this [position];

			view.FindViewById<TextView> (Resource.Id.label).Text = item.name;
			view.FindViewById<TextView> (Resource.Id.phone).Text = item.phone;
			return view;
		}
	}

	public static class ObjectHelper
	{
		public static T Cast<T>(this Java.Lang.Object obj) where T : class
		{
			var PropertyInfo = obj.GetType ().GetProperty ("Instance");
			return PropertyInfo == null ? null : PropertyInfo.GetValue (obj, null) as T;
		}
	}

}


