using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public enum Change
    {
        ADD, REMOVE, EDIT
    }

    public class UpdateEventArgs<T> : EventArgs
    {
        public T What { get; private set; }
        public Change How { get; private set; }
        public UpdateEventArgs(Change how, T what) { How = how; What = what; }
    }

    public delegate void UpdateHandler<T>(IUpdateable<T> sender, UpdateEventArgs<T> args);

    public interface IUpdateable<T>
    {
        event UpdateHandler<T> OnUpdate;
    }

    public interface IConcurrentUpdatebleDictionary<THash, TKey, TValue> : IUpdateable<TValue>, IDictionary<TKey, TValue>
    {
        KeyValuePair<TKey, TValue> this[THash hash] { get; set; }
        KeyValuePair<TKey, TValue> this[int index] { get; }
        KeyValuePair<TKey, TValue>[] GetViewBetween(int lower, int upper);
    }

    public interface IConcurrentUpdatableList<T> : IList<T>, IUpdateable<T>
    {
    }
}
