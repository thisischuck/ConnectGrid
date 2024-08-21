using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("CurrentLevel")]
	public class ES3UserType_GameManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameManager() : base(typeof(GameManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (GameManager)obj;
			
			writer.WriteProperty("CurrentLevel", instance.CurrentLevel, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (GameManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "CurrentLevel":
						instance.CurrentLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_GameManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameManagerArray() : base(typeof(GameManager[]), ES3UserType_GameManager.Instance)
		{
			Instance = this;
		}
	}
}