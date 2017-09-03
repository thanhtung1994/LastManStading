using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAMO.Common {
    [Serializable]
    public class ExtendedList<T> : List<T> {
        #region PROPERTIES
        public bool IsEmpty { get { return this.Count <= 0; } }
        public T FirstItem { get { return this.GetFirstItem(); } }
        public T LastItem {
            get { return this.GetLastItem(); }
        }
        public T NextToLastItem {
            get { return this.GetNextToLastItem(); }
        }
        public T GetRandomItem
        {
            get { return this[UnityEngine.Random.Range(0, this.Count)]; }
        }
        #endregion
        #region CONSTRUCTORS
        public static ExtendedList<T> FromArray(Array array) {
            ExtendedList<T> result = new ExtendedList<T>();
            foreach (T item in array)
                result.Add(item);
            return result;
        }
        public static ExtendedList<T> FromList(List<T> list) {
            ExtendedList<T> result = new ExtendedList<T>();
            foreach (T item in list)
                result.Add(item);
            return result;
        }
        #endregion
        #region METHODS
        public T GetFirstItem() { return this.Count > 0 ? this[0] : default(T); }
        public T GetLastItem() {
            return (this.Count > 0) ? this[this.Count - 1] : default(T);
        }
        public T GetNextToLastItem() {
            return (this.Count > 1) ? this[this.Count - 2] : default(T);
        }
        public void RemoveFromIndex(int index) {
            if (index > this.Count) return;
            this.RemoveRange(index + 1, this.Count - index - 1);
        }
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector) {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(this.Count);
            foreach (T item in this)
                result.Add(keySelector(item), valueSelector(item));
            return result;
        }
        public ExtendedList<TNew> Parse<TNew>(Func<T, TNew> selector) where TNew : class {
            ExtendedList<TNew> result = new ExtendedList<TNew>();
            foreach (T item in this) {
                result.Add(selector(item));
            }
            return result;
        }
        public ExtendedList<TNew> ParseNullable<TNew>(Func<T, TNew> selector) {
            ExtendedList<TNew> result = new ExtendedList<TNew>();
            foreach (T item in this) {
                if (item == null) result.Add(default(TNew));
                else result.Add(selector(item));
            }
            return result;
        }
        public new ExtendedList<T> FindAll(Predicate<T> match) {
            return new ExtendedList<T>().AddRange(base.FindAll(match));
        }
        public new ExtendedList<T> Add(T item) {
            base.Add(item); return this;
        }
        public new ExtendedList<T> AddRange(IEnumerable<T> collection) {
            base.AddRange(collection); return this;
        }
        public new ExtendedList<T> Sort(Comparison<T> comparison) {
            base.Sort(comparison); return this;
        }
        public int GetIndex(Predicate<T> match) {
            if (Exists(match)) return FindIndex(match);
            else return -1;
        }
        #endregion
    }
}
