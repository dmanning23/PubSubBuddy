
namespace PubSubBuddy
{
	public interface IPulisher<T> where T : struct
	{
		void AddValue(string valueName, T value);

		void Subscribe(ISubscriber<T> sub);

		void UnSubscribe(ISubscriber<T> sub);

		void Publish(string valueName, T value);
	}
}
