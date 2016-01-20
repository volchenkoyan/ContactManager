
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ContactManager
{
	[Activity (Label = "AddUser")]			
	public class AddUser : Activity
	{
		EditText name, lastname, phone;
		Button add_contact;
		ImageView add_photo;
		Database sqldb;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			sqldb = new Database ("contact_db");

			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.AddUser);
			name = FindViewById<EditText> (Resource.Id.editName);
			lastname = FindViewById<EditText> (Resource.Id.editLastName);
			phone = FindViewById<EditText> (Resource.Id.editPhone);
			add_contact = FindViewById<Button> (Resource.Id.button1);

			add_contact.Click += delegate {
				sqldb.AddRecord(name.Text,lastname.Text,phone.Text," ");
				Android.Widget.Toast.MakeText(this, sqldb.Message, Android.Widget.ToastLength.Long).Show ();
				StartActivity(typeof(MainActivity));
			};
				
		}

	}
		
}

