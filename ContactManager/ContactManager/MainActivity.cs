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

			List<string> names_for_delete = new List <string>();
			//List<string> contacts = new List<string>();

			Android.Database.ICursor sqldb_cursor = sqldb.GetRecordCursor();
			while(sqldb_cursor.MoveToNext ()) {
				//contacts.Add(sqldb_cursor.GetString (sqldb_cursor.GetColumnIndex ("FirstName")) + " "+ sqldb_cursor.GetString (sqldb_cursor.GetColumnIndex ("LastName")));
				contacts.Add(new Contact(sqldb_cursor.GetString (sqldb_cursor.GetColumnIndex ("FirstName")) + " "+ sqldb_cursor.GetString (sqldb_cursor.GetColumnIndex ("LastName")),sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("Phone"))));
				//names_for_delete.Add(sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("FirstName")));
			}


			list_contacts = FindViewById<ListView> (Resource.Id.list_contacts);
		
			list_contacts.Adapter = new ContactAdapter(this,contacts);
			list_contacts.ChoiceMode = ChoiceMode.Multiple;
				}

		/*void OnSelection ()
		{
			//Dictionary<string,object> checked_names = (Dictionary<string,object>) list_contacts.Adapter.GetItem(FindViewById<ListView>(Resource.Id.list_contacts).GetItemIdAtPosition);
			var checked_items = FindViewById<ListView> (Resource.Id.list_contacts).CheckedItemPositions;
						for (var i = 0; i < checked_items.Size (); i++) {
				//checked_names [i] = list_contacts.GetItemAtPosition((int)checked_items.KeyAt(i)).ToString();
				//var temp = list_contacts.GetItemAtPosition(checked_items.KeyAt (i)).GetType().GetProperties () [2].ToString();
				//var kek = (Contact)list_contacts.Adapter.GetItem(checked_items.KeyAt(i));
				//object val = list_contacts.GetItemAtPosition(checked_items.KeyAt(i));
				//result = contacts.Find (val);
				Android.Widget.Toast.MakeText (this, temp, Android.Widget.ToastLength.Long).Show ();
			}

		}*/

		

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			//return base.OnCreateOptionsMenu (menu);
			MenuInflater.Inflate(Resource.Menu.menu_main, menu);
			//base.OnCreateOptionsMenu (menu, menuInflater);
			//return true;
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			//switch (item.ItemId) {
			//case Resource.Id.action_add_contact:
			if (item.ItemId == Resource.Id.action_add_contact) {
				StartActivity (typeof(AddUser));
				//Android.Widget.Toast.MakeText (this, "ok", Android.Widget.ToastLength.Short).Show ();
				return true;
			}
			if (item.ItemId == Resource.Id.action_delete_contact) {
				OnSelection();
			}
			
			//default:
			//	throw new Java.Lang.IllegalArgumentException ();
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

}


