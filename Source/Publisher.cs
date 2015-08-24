using System;
using System.Collections.Generic;

namespace PubSubBuddy
{
	public class Publisher<T> : IPulisher<T> where T : struct
	{
		#region Properties

		/// <summary>
		/// A list of all the subscribers mapped to the value they are listening for
		/// </summary>
		private Dictionary<string, EventHandler<PubSubEventArgs<T>>> SubscriberDictionaries { get; set; }

		/// <summary>
		/// Store all the current values of the system.
		/// </summary>
		private Dictionary<string, T> CurrentValues { get; set; }

		/// <summary>
		/// Thing for thread safety
		/// </summary>
		private readonly object _lock = new object();

		/// <summary>
		/// an example of the 'zero' value of the type to be published. 
		/// This would be something like Vector2D(0,0) or Color.White
		/// </summary>
		readonly private T _zeroValue;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor
		/// </summary>
		public Publisher(T zeroValue)
		{
			SubscriberDictionaries = new Dictionary<string, EventHandler<PubSubEventArgs<T>>>();
			CurrentValues = new Dictionary<string, T>();
			_zeroValue = zeroValue;
		}

		/// <summary>
		/// Add a starter value to the system.
		/// </summary>
		/// <param name="valueName"></param>
		/// <param name="value"></param>
		public void AddValue(string valueName, T value)
		{
			if (!CurrentValues.ContainsKey(valueName))
			{
				CurrentValues.Add(valueName, value);
			}
			else
			{
				CurrentValues[valueName] = value;
			}
		}

		/// <summary>
		/// Sign up a subscriber to a value's events
		/// </summary>
		/// <param name="sub"></param>
		public void Subscribe(ISubscriber<T> sub)
		{
			lock (_lock)
			{
				if (!SubscriberDictionaries.ContainsKey(sub.ValueName))
				{
					//Add the subscriber to listen for value changes.
					SubscriberDictionaries.Add(sub.ValueName, null);
					SubscriberDictionaries[sub.ValueName] += sub.OnValueChange;
				}
				else
				{
					SubscriberDictionaries[sub.ValueName] += sub.OnValueChange;
				}

				//Check if there is a default value yet.
				if (!CurrentValues.ContainsKey(sub.ValueName))
				{
					CurrentValues.Add(sub.ValueName, _zeroValue);
				}

				//Set the value of the subscriber to the default via the built-in publish mechanism.
				sub.OnValueChange(this, new PubSubEventArgs<T>(_zeroValue, CurrentValues[sub.ValueName]));
			}
		}

		/// <summary>
		/// Unsubscribe a sub from its value's events.
		/// </summary>
		/// <param name="sub"></param>
		public void UnSubscribe(ISubscriber<T> sub)
		{
			lock (_lock)
			{
				if (SubscriberDictionaries.ContainsKey(sub.ValueName))
				{
					SubscriberDictionaries[sub.ValueName] -= sub.OnValueChange;
				}
			}
		}

		/// <summary>
		/// fire out the pub sub engine to notify all listeners of a changed value
		/// </summary>
		/// <param name="valueName"></param>
		/// <param name="value"></param>
		public void Publish(string valueName, T value)
		{
			//find the old value
			var oldValue = CurrentValues[valueName];

			//store the new value
			CurrentValues[valueName] = value;

			//holla at anybody who listening
			var handler = SubscriberDictionaries[valueName];
			if (null != handler)
			{
				handler(this, new PubSubEventArgs<T>(oldValue, value));
			}
		}

		#endregion //Methods
	}
}