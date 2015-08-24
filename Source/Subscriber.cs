
namespace PubSubBuddy
{
	/// <summary>
	/// An object that contains a value and listens for changes to that value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Subscriber<T> : ISubscriber<T> where T : struct
	{
		#region Fields

		/// <summary>
		/// The curent value of this object
		/// </summary>
		private T _value;

		/// <summary>
		/// The name of teh value this dude listens for
		/// </summary>
		private string _valueName;

		/// <summary>
		/// The publisher this dude listens to
		/// </summary>
		private readonly IPulisher<T> _pub; 

		#endregion //Fields

		#region Properties

		/// <summary>
		/// Get the current value of this dude
		/// </summary>
		public T Value
		{
			get
			{
				return _value;
			}
		}

		/// <summary>
		/// Muck around with the name of the value this dude listens for
		/// </summary>
		public string ValueName
		{
			get
			{
				return _valueName;
			}
			set
			{
				//first unsubscribe from the old value
				Unsubscribe();

				//now grba the new value and subscribe for its changes
				_valueName = value;
				_pub.Subscribe(this);
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pub">the publisher this dude listens to</param>
		public Subscriber(IPulisher<T> pub)
		{
			_pub = pub;
		}

		/// <summary>
		/// Method that gets fired off when the value is changed
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="e"></param>
		public void OnValueChange(object obj, PubSubEventArgs<T> e)
		{
			//just grab the new value
			_value = e.NewValue;
		}

		/// <summary>
		/// stop this dude from listening to the pubsub bus
		/// </summary>
		public void Unsubscribe()
		{
			_pub.UnSubscribe(this);
		}

		#endregion //Methods
	}
}