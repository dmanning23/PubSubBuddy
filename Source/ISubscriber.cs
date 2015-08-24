
namespace PubSubBuddy
{
	public interface ISubscriber<T> where T : struct
	{
		T Value { get; }

		string ValueName { get; }

		void OnValueChange(object obj, PubSubEventArgs<T> e);

		void Unsubscribe();
	}
}
