using System;
using System.Collections.Generic;

namespace Connected.DAL.Core
{
	public class ChangeTrackingManager
	{
		static ChangeTrackingManager _instance;
		
		public	Dictionary<EntityBase,List<string>> changeds=
			new Dictionary<EntityBase, List<string>>();

		private ChangeTrackingManager ()
		{
		}

		public static ChangeTrackingManager Instance()
		{
			//todo:lock
			if (_instance == null)
				_instance = new ChangeTrackingManager ();
			return _instance;

		}

		public void Add(EntityBase ent, string columnName)
		{
			if (!changeds.ContainsKey (ent)) {
				changeds.Add (ent, new List<string> (){ columnName });
			} else {
				if (!changeds [ent].Contains (columnName))
					changeds [ent].Add (columnName);
			}
		}
	}
}

