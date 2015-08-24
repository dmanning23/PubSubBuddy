using System;

namespace PubSubBuddy
{
	/// <summary>
	/// custom event argument that includes the previous and new value
	/// </summary>
	public class PubSubEventArgs<T> : EventArgs where T : struct
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public PubSubEventArgs(T oldvalue, T newValue)
		{
			OldValue = oldvalue;
			NewValue = newValue;
		}

		/// <summary>
		/// The previous value
		/// </summary>
		public T OldValue { get; set; }

		/// <summary>
		/// the new value we just changed to
		/// </summary>
		public T NewValue { get; set; }
	}

}